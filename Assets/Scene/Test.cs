using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Wangz.IFly;

public class Test : MonoBehaviour
{
    public IFlyBase m_ifly;
    public Button m_start;
    public Button m_stop;
    public Text m_result;

    // Use this for initialization
    void Start()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        gameObject.AddComponent<IFlyWin>();
#elif UNITY_ANDROID
        gameObject.AddComponent<IFlyAndroid>();
#elif UNITY_IOS
        gameObject.AddComponent<IFlyIOS>();
#endif
        m_start.onClick.AddListener(() =>
        {
            m_ifly.StartSpeech();
        });
        m_stop.onClick.AddListener(() =>
        {
            m_ifly.StopSpeech();
        });
        m_ifly.OnResultEvent += (result) =>
        {
            m_result.text = result;
        };
    }
}
