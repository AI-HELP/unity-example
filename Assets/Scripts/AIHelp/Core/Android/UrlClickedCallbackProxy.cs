using UnityEngine;
using System;

namespace AIHelp
{
#if UNITY_ANDROID
    public class UrlClickedCallbackProxy : AndroidJavaProxy
    {
        private readonly AIHelpDelegate.OnSpecificUrlClickedCallback urlClickedCallback;

        public UrlClickedCallbackProxy(AIHelpDelegate.OnSpecificUrlClickedCallback callback) : base("net.aihelp.ui.listener.OnSpecificUrlClickedCallback")
        {
            this.urlClickedCallback = callback;
        }
        
        void onSpecificUrlClicked(string url)
        {
            urlClickedCallback(url);
        }
    }
#endif
}