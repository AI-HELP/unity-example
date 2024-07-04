using UnityEngine;
using System;

namespace AIHelp
{
#if UNITY_ANDROID
    public class AsyncEventListenerProxy : AndroidJavaProxy
    {
        private readonly AIHelpDelegate.AsyncEventListener eventListener;
        private AndroidJavaObject ackRef;

        public AsyncEventListenerProxy(AIHelpDelegate.AsyncEventListener eventListener) : base("net.aihelp.event.AsyncEventListener")
        {
            this.eventListener = eventListener;
        }
        
        void onAsyncEventReceived(string jsonEventData, AndroidJavaObject ack)
        {
            ackRef = ack;
            var jniThread = System.Threading.Thread.CurrentThread;
            
            Action<string> acknowledge = jsonAckData =>
            {
                bool isAttached = false;
                bool isAsync = System.Threading.Thread.CurrentThread != jniThread;
                try
                {
                    // Attach the current thread to the JNI environment
                    if (AndroidJNI.AttachCurrentThread() == (int)IntPtr.Zero)
                    {
                        isAttached = true;
                    }
                    if (ackRef != null)
                    {
                        ackRef.Call("acknowledge", jsonAckData);
                    }
                }
                catch (Exception e)
                {
                    // Handle any exceptions that may occur during the JNI call
                    Debug.LogError("Exception during JNI call: " + e.Message);
                }
                finally
                {
                    if (isAsync && isAttached)
                    {
                        // Ensure we detach the current thread from the JNI environment if it was attached
                        AndroidJNI.DetachCurrentThread();
                    }
                }
            };
            eventListener(jsonEventData, acknowledge);
        }
    }
#endif
}