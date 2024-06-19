using System;

namespace AIHelp
{
    public class AIHelpDelegate
    {
        public delegate void OnAIHelpInitializedCallback(bool isSuccess, string message);

        public delegate void OnAIHelpInitializedAsyncCallback(bool isSuccess, string message);

        public delegate void OnEnterpriseAuthCallback(Action<string> completion);

        public delegate void OnLoginResultCallback(int code, string message);

        public delegate void OnNetworkCheckResultCallback(string netLog);

        public delegate void OnMessageCountArrivedCallback(int msgCount);

        public delegate void OnSpecificFormSubmittedCallback();

        public delegate void OnAIHelpSessionOpenCallback();

        public delegate void OnAIHelpSessionCloseCallback();

        public delegate void OnSpecificUrlClickedCallback(string url);

    }
}