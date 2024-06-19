using UnityEngine;
using System;

namespace AIHelp
{
#if UNITY_ANDROID
    public class LoginCallbackProxy : AndroidJavaProxy
    {
        private readonly AIHelpDelegate.OnLoginResultCallback loginCallback;
        
        public LoginCallbackProxy(AIHelpDelegate.OnLoginResultCallback callback) : base("net.aihelp.ui.listener.OnLoginResultCallback")
        {
            this.loginCallback = callback;
        }
        
        void onLoginResult(int code, string message)
        {
            this.loginCallback(code, message);
        }
    }
#endif
}