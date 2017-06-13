using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;

namespace Wangz.IFly
{
    public class IFlyMSPISR
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        public const string mscdll = "msc";
#elif UNITY_ANDROID
        public const string mscdll = "msc";
#elif UNITY_IOS
        public const string mscdll = "__Internal";
#endif

        [DllImport(mscdll)]
        public static extern string QISRSessionBegin(string grammarList, string parameters, ref int errorCode);
        [DllImport(mscdll)]
        public static extern int QISRAudioWrite(string sessionID, IntPtr waveData, int waveLen, int audioStatus, ref int epStatus, ref int recogStatus);
        [DllImport(mscdll)]
        public static extern string QISRGetResult(string sessionID, ref int rsltStatus, int waitTime, ref int errorCode);
        [DllImport(mscdll)]
        public static extern string QISRGetBinaryResult(string sessionID, ref int rsltLen, ref int rsltStatus, int waitTime, ref int errorCode);
        [DllImport(mscdll)]
        public static extern int QISRSessionEnd(string sessionID, string hints);
        [DllImport(mscdll)]
        public static extern int QISRGetParam(string sessionID, string paramName, string paramValue, ref int valueLen);
        [DllImport(mscdll)]
        public static extern int QISRSetParam(string sessionID, string paramName, string paramValue);
    }
}
