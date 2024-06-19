using UnityEngine;
using System;

namespace AIHelp
{
#if UNITY_ANDROID
    public class AuthCallbackProxy : AndroidJavaProxy {
           
        private AIHelpDelegate.OnEnterpriseAuthCallback authCallback;
        private AndroidJavaObject callbackObjectGlobalRef;

        public AuthCallbackProxy(AIHelpDelegate.OnEnterpriseAuthCallback callback) : base("net.aihelp.ui.listener.OnEnterpriseAuthCallback")
        {
            this.authCallback = callback;
        }

        void onEnterpriseAuth(AndroidJavaObject callback)
        {
            callbackObjectGlobalRef = callback;

            Action<string> completionCallback = token =>
            {
                bool isAttached = false;
                try
                {
                    // Attach the current thread to the JNI environment
                    if (AndroidJNI.AttachCurrentThread() == (int)IntPtr.Zero)
                    {
                        isAttached = true;
                    }

                    if (callbackObjectGlobalRef != null)
                    {
                        callbackObjectGlobalRef.Call("onAuthReady", token);
                    }
                }
                catch (Exception e)
                {
                    // Handle any exceptions that may occur during the JNI call
                    Debug.LogError("Exception during JNI call: " + e.Message);
                }
                finally
                {
                    if (isAttached)
                    {
                        // Ensure we detach the current thread from the JNI environment if it was attached
                        AndroidJNI.DetachCurrentThread();
                    }
                }
            };

            // Assuming authCallback is a method that takes a callback and invokes it later
            authCallback(completionCallback);
        }

    }
#endif
}