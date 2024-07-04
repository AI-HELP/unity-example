using System;
using UnityEngine;
namespace AIHelp
{
    public class AIHelpCore
    {
        private IAIHelpCore helpCore;
        private static AIHelpCore sInstance;

        private AIHelpCore()
        {

#if UNITY_ANDROID
            if (Application.platform == RuntimePlatform.Android) {
                helpCore = new AIHelpAndroidCore();
            }
#endif

#if UNITY_IOS
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                helpCore = new AIHelpiOSCore();
            }
#endif
        }

        public static AIHelpCore getInstance()
        {
            if (sInstance == null)
            {
                sInstance = new AIHelpCore();
            }
            return sInstance;
        }

        public void Initialize(string domain, string appId, string language)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.Initialize(domain, appId, language);
        }

        public void Initialize(string domain, string appId)
        {
            Initialize(domain, appId, "");
        }

        public bool Show(string entranceId)
        {
            if (!IsHelpCorePrepared()) return false;
            return helpCore.Show(entranceId);
        }

        public bool Show(ApiConfig apiConfig)
        {
            if (!IsHelpCorePrepared()) return false;
            return helpCore.Show(apiConfig);
        }

        public void ShowSingleFAQ(string faqId, ConversationMoment moment)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.ShowSingleFAQ(faqId, moment);
        }

        public void Login(LoginConfig loginConfig)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.Login(loginConfig);
        }

        public void Logout()
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.Logout();
        }

        public void UpdateUserInfo(UserConfig userConfig)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.UpdateUserInfo(userConfig);
        }

        public void ResetUserInfo()
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.ResetUserInfo();
        }

        public void UpdateSDKLanguage(string language)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.UpdateSDKLanguage(language);
        }

        public void SetUploadLogPath(string path)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.SetUploadLogPath(path);
        }

        public void SetPushTokenAndPlatform(string pushToken, PushPlatform platform)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.SetPushTokenAndPlatform(pushToken, platform);
        }

        public void FetchUnreadMessageCount()
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.FetchUnreadMessageCount();
        }

        public string GetSDKVersion()
        {
            if (!IsHelpCorePrepared()) return "";
            return helpCore.GetSDKVersion();
        }

        public bool IsAIHelpShowing()
        {
            if (!IsHelpCorePrepared()) return false;
            return helpCore.IsAIHelpShowing();
        }

        public void EnableLogging(bool enable)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.EnableLogging(enable);
        }

        public void ShowUrl(string url)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.ShowUrl(url);
        }

        public void AdditionalSupportFor(PublishCountryOrRegion countryOrRegion)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.AdditionalSupportFor(countryOrRegion);
        }

        public void RegisterAsyncEventListener(AIHelp.EventType eventType, AIHelpDelegate.AsyncEventListener listener)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.RegisterAsyncEventListener(eventType, listener);
        }

        public void UnregisterAsyncEventListener(AIHelp.EventType eventType)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.UnregisterAsyncEventListener(eventType);
        }

        public void Close()
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.Close();
        }

#if UNITY_IOS

        public void SetRequestedOrientation(int requestedOrientation)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.SetRequestedOrientation(requestedOrientation);
        }

        public void SetSDKAppearanceMode(int mode)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.SetSDKAppearanceMode(mode);
        }

#endif

        private bool IsHelpCorePrepared()
        {
            return helpCore != null;
        }

    }

}