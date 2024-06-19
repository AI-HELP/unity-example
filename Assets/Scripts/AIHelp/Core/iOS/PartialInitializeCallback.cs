using System;
using System.Runtime.InteropServices;
using AOT;

namespace AIHelp
{
#if UNITY_IOS
    public partial class AIHelpiOSCore
    {
        static event AIHelpDelegate.OnAIHelpInitializedCallback _iOSInitCallback;
        static event AIHelpDelegate.OnAIHelpInitializedAsyncCallback _iOSInitAsyncCallback;

        [MonoPInvokeCallback(typeof(AIHelpDelegate.OnAIHelpInitializedCallback))]
        private static void iOSInitCallback(bool isSuccess, string message)
        {
            _iOSInitCallback?.Invoke(isSuccess, message);
        }

        [MonoPInvokeCallback(typeof(AIHelpDelegate.OnAIHelpInitializedAsyncCallback))]
        private static void iOSInitCallbackAsync(bool isSuccess, string message)
        {
            _iOSInitAsyncCallback?.Invoke(isSuccess, message);
        }
    }
#endif
}
