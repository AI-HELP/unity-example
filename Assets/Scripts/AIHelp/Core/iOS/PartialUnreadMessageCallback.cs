using System.Runtime.InteropServices;
using System;
using AOT;

namespace AIHelp
{
#if UNITY_IOS
    public partial class AIHelpiOSCore
    {
        static event AIHelpDelegate.OnMessageCountArrivedCallback _iOSPollingMessageCountCallback;
        static event AIHelpDelegate.OnMessageCountArrivedCallback _iOSFetchMessageCountCallback;

        [MonoPInvokeCallback(typeof(AIHelpDelegate.OnMessageCountArrivedCallback))]
        private static void iOSOnPollingMessageCountArrivedMethod(int msgCount)
        {
            _iOSPollingMessageCountCallback?.Invoke(msgCount);
        }

        [MonoPInvokeCallback(typeof(AIHelpDelegate.OnMessageCountArrivedCallback))]
        private static void iOSOnFetchMessageCountArrivedMethod(int msgCount)
        {
            _iOSFetchMessageCountCallback?.Invoke(msgCount);
        }
    }
#endif
}
