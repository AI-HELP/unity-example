using UnityEngine;
namespace AIHelp
{
#if UNITY_ANDROID

    public class AIHelpAndroidCore : IAIHelpCore
    {

        private AndroidJavaClass javaSupport;
        private AndroidJavaObject currentActivity;

        public AIHelpAndroidCore()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            javaSupport = new AndroidJavaClass("net.aihelp.init.AIHelpSupport");
            currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }

        public void Init(string appKey, string domain, string appId, string language)
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("init", currentActivity, appKey, domain, appId, language);
            }
        }

        private AndroidJavaObject getSupportConfig(ConversationConfig config)
        {
            AndroidJavaObject builder = new AndroidJavaObject("net.aihelp.config.ConversationConfig$Builder");
            return builder.Call<AndroidJavaObject>("build", config.GetConversationIntent(),
            config.IsAlwaysShowHumanSupportButtonInBotPage(), config.GetWelcomeMessage(), config.GetStoryNode());
        }

        private AndroidJavaObject getFaqConfig(FaqConfig config)
        {
            AndroidJavaObject builder = new AndroidJavaObject("net.aihelp.config.FaqConfig$Builder");
            return builder.Call<AndroidJavaObject>("build", config.GetShowConversationMoment(), getSupportConfig(config.GetConversationConfig()));
        }

        private AndroidJavaObject getOperationConfig(OperationConfig config)
        {
            AndroidJavaObject builder = new AndroidJavaObject("net.aihelp.config.OperationConfig$Builder");
            builder.Call<AndroidJavaObject>("setSelectIndex", config.GetSelectIndex());
            builder.Call<AndroidJavaObject>("setConversationTitle", config.GetConversationTitle());
            builder.Call<AndroidJavaObject>("setConversationConfig", getSupportConfig(config.GetConversationConfig()));
            return builder.Call<AndroidJavaObject>("build");
        }

        private AndroidJavaObject getUserConfig(UserConfig config)
        {
            AndroidJavaObject builder = new AndroidJavaObject("net.aihelp.config.UserConfig$Builder");
            builder.Call<AndroidJavaObject>("setUserId", config.GetUserId());
            builder.Call<AndroidJavaObject>("setUserName", config.GetUserName());
            builder.Call<AndroidJavaObject>("setServerId", config.GetServerId());
            builder.Call<AndroidJavaObject>("setUserTags", config.GetUserTags());
            builder.Call<AndroidJavaObject>("setCustomData", config.GetCustomData());
            builder.Call<AndroidJavaObject>("setSyncCrmInfo", config.GetWhetherSyncCrmInfo());
            builder.Call<AndroidJavaObject>("setPushToken", config.GetPushToken());
            builder.Call<AndroidJavaObject>("setPushPlatform", getPushPlatform(config.GetPushPlatform()));
            return builder.Call<AndroidJavaObject>("build");
        }

        private AndroidJavaObject getPublishCountryOrRegion(PublishCountryOrRegion countryOrRegion)
        {
            AndroidJavaClass clz = new AndroidJavaClass("net.aihelp.config.enums.PublishCountryOrRegion");
            return clz.CallStatic<AndroidJavaObject>("fromValue", (int)countryOrRegion);
        }

        private AndroidJavaObject getPushPlatform(PushPlatform platform)
        {
            AndroidJavaClass clz = new AndroidJavaClass("net.aihelp.config.enums.PushPlatform");
            return clz.CallStatic<AndroidJavaObject>("fromValue", (int)platform);
        }

        public void ShowConversation()
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("showConversation");
            }
        }

        public void ShowConversation(ConversationConfig config)
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("showConversation", getSupportConfig(config));
            }
        }

        public void ShowAllFAQSections()
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("showAllFAQSections");
            }
        }

        public void ShowAllFAQSections(FaqConfig config)
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("showAllFAQSections", getFaqConfig(config));
            }
        }

        public void ShowFAQSection(string sectionId)
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("showFAQSection", sectionId);
            }
        }

        public void ShowFAQSection(string sectionId, FaqConfig config)
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("showFAQSection", sectionId, getFaqConfig(config));
            }
        }


        public void ShowSingleFAQ(string faqId)
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("showSingleFAQ", faqId);
            }
        }

        public void ShowSingleFAQ(string faqId, FaqConfig config)
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("showSingleFAQ", faqId, getFaqConfig(config));
            }
        }

        public void ShowOperation()
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("showOperation");
            }
        }

        public void ShowOperation(OperationConfig config)
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("showOperation", getOperationConfig(config));
            }
        }

        public void UpdateUserInfo(UserConfig config)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("updateUserInfo", getUserConfig(config));
            }
        }

        public void ResetUserInfo()
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("resetUserInfo");
            }
        }

        public void UpdateSDKLanguage(string language)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("updateSDKLanguage", language);
            }
        }

        public void SetUploadLogPath(string logPath)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("setUploadLogPath", logPath);
            }
        }

        public void SetPushTokenAndPlatform(string logPath, PushPlatform pushPlatform)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("setPushTokenAndPlatform", logPath, getPushPlatform(pushPlatform));
            }
        }


        private class ListenerAdapter : AndroidJavaProxy
        {

            private readonly AIHelpDefine.OnAIHelpInitializedCallback initCallback;
            private readonly AIHelpDefine.OnMessageCountArrivedCallback msgCountCallback;
            private readonly AIHelpDefine.OnNetworkCheckResultCallback netCheckCallback;
            private readonly AIHelpDefine.OnSpecificFormSubmittedCallback submittedCallback;
            private readonly AIHelpDefine.OnAIHelpSessionOpenCallback sessionOpenCallback;
            private readonly AIHelpDefine.OnAIHelpSessionCloseCallback sessionCloseCallback;
            private readonly AIHelpDefine.OnOperationUnreadChangedCallback unreadChangedCallback;
			private readonly AIHelpDefine.OnSpecificUrlClickedCallback urlClickedCallback;

            public ListenerAdapter(AIHelpDefine.OnAIHelpInitializedCallback callback) : base("net.aihelp.ui.listener.OnAIHelpInitializedCallback")
            {
                this.initCallback = callback;
            }

            public ListenerAdapter(AIHelpDefine.OnMessageCountArrivedCallback callback) : base("net.aihelp.ui.listener.OnMessageCountArrivedCallback")
            {
                this.msgCountCallback = callback;
            }

            public ListenerAdapter(AIHelpDefine.OnNetworkCheckResultCallback callback) : base("net.aihelp.ui.listener.OnNetworkCheckResultCallback")
            {
                this.netCheckCallback = callback;
            }

            public ListenerAdapter(AIHelpDefine.OnSpecificFormSubmittedCallback callback) : base("net.aihelp.ui.listener.OnSpecificFormSubmittedCallback")
            {
                this.submittedCallback = callback;
            }

            public ListenerAdapter(AIHelpDefine.OnAIHelpSessionOpenCallback callback) : base("net.aihelp.ui.listener.OnAIHelpSessionOpenCallback")
            {
                this.sessionOpenCallback = callback;
            }

            public ListenerAdapter(AIHelpDefine.OnAIHelpSessionCloseCallback callback) : base("net.aihelp.ui.listener.OnAIHelpSessionCloseCallback")
            {
                this.sessionCloseCallback = callback;
            }

            public ListenerAdapter(AIHelpDefine.OnOperationUnreadChangedCallback callback) : base("net.aihelp.ui.listener.OnOperationUnreadChangedCallback")
            {
                this.unreadChangedCallback = callback;
            }

            public ListenerAdapter(AIHelpDefine.OnSpecificUrlClickedCallback callback) : base("net.aihelp.ui.listener.OnSpecificUrlClickedCallback")
            {
                this.urlClickedCallback = callback;
            }

            void onAIHelpInitialized()
            {
                initCallback();
            }

            void onMessageCountArrived(int msgCount)
            {
                msgCountCallback(msgCount);
            }

            void onNetworkCheckResult(string netLog)
            {
                netCheckCallback(netLog);
            }

            void onFormSubmitted()
            {
                submittedCallback();
            }

            void onAIHelpSessionOpened()
            {
                sessionOpenCallback();
            }

            void onAIHelpSessionClosed()
            {
                sessionCloseCallback();
            }

            void onOperationUnreadChanged(bool hasUnreadArticles)
            {
                unreadChangedCallback(hasUnreadArticles);
            }

            void onSpecificUrlClicked(string url)
            {
                urlClickedCallback(url);
            }

        }

        public void SetOnAIHelpInitializedCallback(AIHelpDefine.OnAIHelpInitializedCallback listener)
        {
            javaSupport.CallStatic("setOnAIHelpInitializedCallback", listener == null ? null : new ListenerAdapter(listener));
        }

        public void SetNetworkCheckHostAddress(string address, AIHelpDefine.OnNetworkCheckResultCallback listener)
        {
            javaSupport.CallStatic("setNetworkCheckHostAddress", address, listener == null ? null : new ListenerAdapter(listener));
        }

        public void StartUnreadMessageCountPolling(AIHelpDefine.OnMessageCountArrivedCallback listener)
        {
            javaSupport.CallStatic("startUnreadMessageCountPolling", listener == null ? null : new ListenerAdapter(listener));
        }

        public void SetOnSpecificFormSubmittedCallback(AIHelpDefine.OnSpecificFormSubmittedCallback listener)
        {
            javaSupport.CallStatic("setOnSpecificFormSubmittedCallback", listener == null ? null : new ListenerAdapter(listener));
        }

        public void SetOnAIHelpSessionOpenCallback(AIHelpDefine.OnAIHelpSessionOpenCallback listener)
        {
            javaSupport.CallStatic("setOnAIHelpSessionOpenCallback", listener == null ? null : new ListenerAdapter(listener));
        }

        public void SetOnAIHelpSessionCloseCallback(AIHelpDefine.OnAIHelpSessionCloseCallback listener)
        {
            javaSupport.CallStatic("setOnAIHelpSessionCloseCallback", listener == null ? null : new ListenerAdapter(listener));
        }

        public void SetOnOperationUnreadChangedCallback(AIHelpDefine.OnOperationUnreadChangedCallback listener)
        {
            javaSupport.CallStatic("setOnOperationUnreadChangedCallback", listener == null ? null : new ListenerAdapter(listener));
        }

        public void SetOnSpecificUrlClickedCallback(AIHelpDefine.OnSpecificUrlClickedCallback listener)
        {
            javaSupport.CallStatic("setOnSpecificUrlClickedCallback", listener == null ? null : new ListenerAdapter(listener));
        }

        public void ShowUrl(string url)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("showUrl", url);
            }
        }

        public void AdditionalSupportFor(PublishCountryOrRegion countryOrRegion)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("additionalSupportFor", getPublishCountryOrRegion(countryOrRegion));
            }
        }

        public string GetSDKVersion()
        {
            if (javaSupport != null)
            {
                return javaSupport.CallStatic<string>("getSDKVersion");
            }
            return "";
        }

        public bool IsAIHelpShowing()
        {
            if (javaSupport != null)
            {
                return javaSupport.CallStatic<bool>("isAIHelpShowing");
            }
            return false;
        }

        public void EnableLogging(bool isOpen)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("enableLogging", isOpen);
            }
        }

    }
#endif
}