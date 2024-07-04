using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using System.Collections.Generic;

namespace AIHelp
{
#if UNITY_IOS
    public partial class AIHelpiOSCore
    {
        private static Dictionary<AIHelp.EventType, AIHelpDelegate.AsyncEventListener> eventListeners = 
                new Dictionary<AIHelp.EventType, AIHelpDelegate.AsyncEventListener>();

        // 在 cs 注册的回调方法
        static event AIHelpDelegate.AsyncEventListener csAsyncEventListener;

        // oc 传过来的 ack 回调，缓存下来异步使用
        public static Acknowledgement AckDelegate { get; set; }

        // 声明一个符合 ack 回调的代理，用来持有 ack 回调指针的时候使用
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void Acknowledgement(string ackJsonData);

        // OC 回调方法签名 - event, ack
        public delegate void AIHelpAsyncEventListener(string jsonEventData, IntPtr acknowledgePtr);

        [MonoPInvokeCallback(typeof(AIHelpAsyncEventListener))] 
        private static void OCAsyncEventListener(string jsonEventData, IntPtr acknowledgePtr)
        {
            AIHelp.EventType eventType = (AIHelp.EventType)SimpleJsonParser.ExtractEventTypeFromJson(jsonEventData);
            if (eventListeners.TryGetValue(eventType, out var listener))
            {
                Action<string> acknowledge = jsonAckData => {
                    AckDelegate = Marshal.GetDelegateForFunctionPointer<Acknowledgement>(acknowledgePtr);
                    AckDelegate?.Invoke(jsonAckData);
                };

                listener?.Invoke(jsonEventData, acknowledge);
            }
        }
    }
#endif
}
