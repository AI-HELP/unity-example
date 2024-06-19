using UnityEngine;
using System;

namespace AIHelp
{
#if UNITY_ANDROID
    public class SessionChangeCallbackProxy : AndroidJavaProxy
    {
        private readonly AIHelpDelegate.OnAIHelpSessionOpenCallback sessionOpenCallback;
        private readonly AIHelpDelegate.OnAIHelpSessionCloseCallback sessionCloseCallback;

        public SessionChangeCallbackProxy(AIHelpDelegate.OnAIHelpSessionOpenCallback callback) : base("net.aihelp.ui.listener.OnAIHelpSessionOpenCallback")
        {
            this.sessionOpenCallback = callback;
        }

        public SessionChangeCallbackProxy(AIHelpDelegate.OnAIHelpSessionCloseCallback callback) : base("net.aihelp.ui.listener.OnAIHelpSessionCloseCallback")
        {
            this.sessionCloseCallback = callback;
        }

        void onAIHelpSessionOpened()
        {
            sessionOpenCallback();
        }

        void onAIHelpSessionClosed()
        {
            sessionCloseCallback();
        }
    }
#endif
}