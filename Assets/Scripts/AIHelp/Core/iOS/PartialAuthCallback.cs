using System;
using System.Runtime.InteropServices;
using AOT;

namespace AIHelp
{
#if UNITY_IOS
    public partial class AIHelpiOSCore
    {
        static event AIHelpDelegate.OnEnterpriseAuthCallback _iOSEnterpriseAuthCallback;

        public static AIHelpEnterpriseAuthCallback GlobalCompletionDelegate { get; set; }

        public delegate void AIHelpEnterpriseTokenDelegate(IntPtr token);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void AIHelpEnterpriseAuthCallback(string result);

        [MonoPInvokeCallback(typeof(AIHelpEnterpriseTokenDelegate))] 
        private static void iOSEnterpriseAuthCallback(IntPtr completionPtr)
        {
            GlobalCompletionDelegate = Marshal.GetDelegateForFunctionPointer<AIHelpEnterpriseAuthCallback>(completionPtr);
            _iOSEnterpriseAuthCallback?.Invoke(async (token) =>
            {
                GlobalCompletionDelegate?.Invoke(token);
            });
        }
    }
#endif
}
