using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace Wangz.IFly
{
    public class IFlyMSPCMN
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        public const string mscdll = "msc";
#elif UNITY_ANDROID
        public const string mscdll = "msc";
#elif UNITY_IOS
        public const string mscdll = "__Internal";
#endif

        [DllImport(mscdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MSPLogin(string usr, string pwd, string parameters);
        [DllImport(mscdll, CallingConvention = CallingConvention.Cdecl)]
        public static extern int MSPLogout();
    }
}