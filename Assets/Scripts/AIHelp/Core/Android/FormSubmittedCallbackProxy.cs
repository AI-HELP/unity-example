using UnityEngine;
using System;

namespace AIHelp
{
#if UNITY_ANDROID
    public class FormSubmittedCallbackProxy : AndroidJavaProxy
    {
        private readonly AIHelpDelegate.OnSpecificFormSubmittedCallback submittedCallback;

        public FormSubmittedCallbackProxy(AIHelpDelegate.OnSpecificFormSubmittedCallback callback) : base("net.aihelp.ui.listener.OnSpecificFormSubmittedCallback")
        {
            this.submittedCallback = callback;
        }

        void onFormSubmitted()
        {
            submittedCallback();
        }
    }
#endif
}