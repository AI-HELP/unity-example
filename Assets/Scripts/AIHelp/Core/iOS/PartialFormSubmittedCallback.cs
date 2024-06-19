using System;
using System.Runtime.InteropServices;
using AOT;

namespace AIHelp
{
#if UNITY_IOS
    public partial class AIHelpiOSCore
    {
        static event AIHelpDelegate.OnSpecificFormSubmittedCallback _iOSSpecificFormCallback;

        [MonoPInvokeCallback(typeof(AIHelpDelegate.OnSpecificFormSubmittedCallback))]
        private static void iOSSpecificFormSubmit()
        {
            _iOSSpecificFormCallback?.Invoke();
        }
    }
#endif
}
