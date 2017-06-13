using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

namespace Wangz.IFly
{
    public class IFlyBase : MonoBehaviour
    {
        public string m_appid;
        private const string SessionBeginParams = "sub = iat, ptt = 0, domain = iat, language = en_us, accent = mandarin, sample_rate = 16000, result_type = plain, result_encoding = utf8";
        private bool m_initState;
        #region Speech Callback
        public event Action<string> OnErrorEvent;
        public event Action OnBeginEvent;
        public event Action OnEndEvent;
        public event Action<string> OnResultEvent;
        #endregion

        #region Unity Method
        private AudioSource m_audioPlay;
        private AudioClip m_audioClip;
        private bool m_isListening;
        private IntPtr m_sessionId;

        void Awake()
        {
            m_audioPlay = gameObject.AddComponent<AudioSource>();
        }

        void Start()
        {
            Init();
        }

        void OnDestroy()
        {
            if (m_initState)
            {
                int ret = MSCDLL.MSPLogout();
                Debug.Log("MSPLogout : " + ret);
            }
        }
        #endregion

        #region iFly Method
        public virtual void Init()
        {
            int errorCode = (int)Errors.MSP_SUCCESS;
            string login_params = string.Format("appid = {0}, work_dir = .", m_appid); // 登录参数，appid与msc库绑定,请勿随意改动

            /* 用户登录 */
            errorCode = MSCDLL.MSPLogin(null, null, login_params); //第一个参数是用户名，第二个参数是密码，均传NULL即可，第三个参数是登录参数	
            if ((int)Errors.MSP_SUCCESS != errorCode)
            {
                OnError(string.Format("MSPLogin failed , Error code {0}", errorCode));
                MSCDLL.MSPLogout(); //退出登录
                m_initState = false;
                return;
            }

            Debug.Log("Init Succ");
            m_initState = true;
        }

        public virtual void StartSpeech()
        {
            int errorcode = (int)Errors.MSP_SUCCESS;

            if (m_isListening)
            {
                Debug.Log("Speech Recognizer Is Listening!!");
            }

            m_sessionId = MSCDLL.QISRSessionBegin(null, SessionBeginParams, ref errorcode); //听写不需要语法，第一个参数为NULL

            if ((int)Errors.MSP_SUCCESS != errorcode)
            {
                OnError(string.Format("QISRSessionBegin failed! error code : {0}", errorcode));
                return;
            }

            //m_audioClip = Microphone.Start(null, true, 3, 16000);
            m_audioClip = Resources.Load<AudioClip>("test");

            OnBegin();
            m_isListening = true;
        }

        public virtual bool isListening()
        {
            return m_isListening;
        }

        public virtual void StopSpeech()
        {
            if (m_isListening)
            {
                //Microphone.End(null);
                StartCoroutine("WaitResult");
            }
        }

        private IEnumerator WaitResult()
        {
            int errorCode = (int)Errors.MSP_SUCCESS;
            var audioState = AudioStatus.MSP_AUDIO_SAMPLE_FIRST;
            var epState = EpStatus.MSP_EP_LOOKING_FOR_SPEECH;
            var recState = RecogStatus.MSP_REC_STATUS_SUCCESS;

            var bytes = IFlyUtils.ConvertClipToBytes(m_audioClip);
            errorCode = MSCDLL.QISRAudioWrite(Marshal.PtrToStringAnsi(m_sessionId), bytes, (uint)bytes.Length, audioState, ref epState, ref recState);
            if ((int)Errors.MSP_SUCCESS != errorCode)
            {
                OnError(string.Format("write LAST_SAMPLE failed: {0}", errorCode));
            }
            else
            {
                errorCode = MSCDLL.QISRAudioWrite(Marshal.PtrToStringAnsi(m_sessionId), null, 0, AudioStatus.MSP_AUDIO_SAMPLE_LAST, ref epState, ref recState);
                if ((int)Errors.MSP_SUCCESS != errorCode)
                {
                    OnError(string.Format("write LAST_SAMPLE failed: {0}", errorCode));
                }
                else
                {
                    IntPtr resultPtr = IntPtr.Zero;
                    string result = "";
                    while (recState != RecogStatus.MSP_REC_STATUS_COMPLETE)
                    {
                        resultPtr = MSCDLL.QISRGetResult(Marshal.PtrToStringAnsi(m_sessionId), ref recState, 0, ref errorCode);
                        if ((int)Errors.MSP_SUCCESS != errorCode)
                        {
                            OnError(string.Format("QISRGetResult failed! error code: {0}", errorCode));
                            break;
                        }
                        Debug.Log("Get Result : " + Marshal.PtrToStringAnsi(resultPtr));
                        result += Marshal.PtrToStringAnsi(resultPtr);
                        yield return 0;
                    }
                    if (errorCode == (int)Errors.MSP_SUCCESS)
                    {
                        OnResult(result);
                        MSCDLL.QISRSessionEnd(Marshal.PtrToStringAnsi(m_sessionId), "normal");
                        OnEnd();
                        Clear();
                    }
                }
            }
        }

        public virtual void CancelSpeech()
        {
            Microphone.End(null);
            Clear();
        }

        private void Clear()
        {
            m_audioClip = null;
            m_sessionId = IntPtr.Zero;
            m_isListening = false;
        }

        private void OnError(string error)
        {
            Debug.Log("OnSpeechError : " + error);
            if (m_sessionId != IntPtr.Zero)
            {
                MSCDLL.QISRSessionEnd(Marshal.PtrToStringAnsi(m_sessionId), null);
                Clear();
            }
            if (OnErrorEvent != null)
                OnErrorEvent(error);
        }

        private void OnBegin()
        {
            Debug.Log("OnSpeechBegin");
            if (OnBeginEvent != null)
                OnBeginEvent();
        }

        private void OnEnd()
        {
            Debug.Log("OnSpeechEnd");
            if (OnEndEvent != null)
                OnEndEvent();
        }

        private void OnResult(string result)
        {
            Debug.Log("OnSpeechEnd : " + result);
            if (OnResultEvent != null)
                OnResultEvent(result);
        }
        #endregion

        #region Audio Method
        private static bool m_isPlaying;

        public virtual void Play(string filepath)
        {
            StartCoroutine("YieldLoadAudio", filepath);
        }

        private IEnumerator YieldLoadAudio(string filePath)
        {
            var www = new WWW("file:///" + filePath);
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                m_audioPlay.clip = www.audioClip;
                m_audioPlay.Play();
                m_isPlaying = true;
            }
            www.Dispose();
        }

        public virtual bool IsPlaying()
        {
            return m_isPlaying;
        }

        public virtual void StopPlay()
        {
            m_audioPlay.Stop();
            m_audioPlay.clip = null;
            m_isPlaying = false;
        }
        #endregion
    }
}
