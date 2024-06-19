using UnityEngine;
using System;

namespace AIHelp
{
#if UNITY_ANDROID
    public class InitializeCallbackProxy : AndroidJavaProxy
    {
        private readonly AIHelpDelegate.OnAIHelpInitializedCallback initCallback;
        private readonly AIHelpDelegate.OnAIHelpInitializedAsyncCallback initAsyncCallback;

        public InitializeCallbackProxy(AIHelpDelegate.OnAIHelpInitializedCallback callback) : base("net.aihelp.ui.listener.OnAIHelpInitializedCallback")
        {
            this.initCallback = callback;
        }

        public InitializeCallbackProxy(AIHelpDelegate.OnAIHelpInitializedAsyncCallback callback) : base("net.aihelp.ui.listener.OnAIHelpInitializedAsyncCallback")
        {
            this.initAsyncCallback = callback;
        }

        void onAIHelpInitialized(bool isSuccess, string message)
        {
            this.initCallback(isSuccess, message);
        }

        void onAIHelpInitializedAsync(bool isSuccess, string message)
        {
            initAsyncCallback(isSuccess, message);
        }
    }
#endif
}