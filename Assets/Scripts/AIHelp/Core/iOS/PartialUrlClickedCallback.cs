using System;
using System.Runtime.InteropServices;
using AOT;

namespace AIHelp
{
#if UNITY_IOS
    public partial class AIHelpiOSCore
    {
        static event AIHelpDelegate.OnSpecificUrlClickedCallback _iOSSpecificUrlClickedCallback;

        [MonoPInvokeCallback(typeof(AIHelpDelegate.OnSpecificUrlClickedCallback))]
        private static void iOSSpecificUrlClicked(string url)
        {
            _iOSSpecificUrlClickedCallback?.Invoke(url);
        }
    }
#endif
}
