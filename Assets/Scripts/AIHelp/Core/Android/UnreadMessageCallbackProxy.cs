using UnityEngine;
using System;

namespace AIHelp
{
#if UNITY_ANDROID
        public class UnreadMessageCallbackProxy : AndroidJavaProxy
        {
            private readonly AIHelpDelegate.OnMessageCountArrivedCallback msgCountCallback;

            public UnreadMessageCallbackProxy(AIHelpDelegate.OnMessageCountArrivedCallback callback) : base("net.aihelp.ui.listener.OnMessageCountArrivedCallback")
            {
                this.msgCountCallback = callback;
            }

            void onMessageCountArrived(int msgCount)
            {
                msgCountCallback(msgCount);
            }
        } 
#endif
}