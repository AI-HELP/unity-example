using System;
using System.Runtime.InteropServices;
using AOT;

namespace AIHelp
{
#if UNITY_IOS
    public partial class AIHelpiOSCore
    {
        static event AIHelpDelegate.OnAIHelpSessionOpenCallback _iOSSessionOpenCallback;
        static event AIHelpDelegate.OnAIHelpSessionCloseCallback _iOSSessionCloseCallback;

        [MonoPInvokeCallback(typeof(AIHelpDelegate.OnAIHelpSessionOpenCallback))]
        private static void iOSSessionOpenSubmit()
        {
            _iOSSessionOpenCallback?.Invoke();
        }

        [MonoPInvokeCallback(typeof(AIHelpDelegate.OnAIHelpSessionCloseCallback))]
        private static void iOSSessionCloseSubmit()
        {
            _iOSSessionCloseCallback?.Invoke();
        }
    }
#endif
}
