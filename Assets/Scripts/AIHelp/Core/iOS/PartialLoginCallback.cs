using System;
using System.Runtime.InteropServices;
using AOT;

namespace AIHelp
{
#if UNITY_IOS
    public partial class AIHelpiOSCore
    {
        static event AIHelpDelegate.OnLoginResultCallback _iOSLoginCallback;

        [MonoPInvokeCallback(typeof(AIHelpDelegate.OnLoginResultCallback))]
        private static void iOSLoginCallback(int code, string message)
        {
            _iOSLoginCallback?.Invoke(code, message);
        }
    }
#endif
}
