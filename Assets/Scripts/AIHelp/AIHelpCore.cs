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

        //internal void SetOnAIHelpInitializedCallback(AIHelpDefine.OnAIHelpInitializedCallback callback)
        //{
        //    throw new NotImplementedException();
        //}

        public static AIHelpCore getInstance()
        {
            if (sInstance == null)
            {
                sInstance = new AIHelpCore();
            }
            return sInstance;
        }

        public void Init(string appKey, string domain, string appId, string language)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.Init(appKey, domain, appId, language);
        }

        public void Init(string appKey, string domain, string appId)
        {
            Init(appKey, domain, appId, "");
        }

        public void SetOnAIHelpInitializedCallback(AIHelpDefine.OnAIHelpInitializedCallback callback)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.SetOnAIHelpInitializedCallback(callback);
        }

        public bool Show(string entranceId) {
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

        public void SetNetworkCheckHostAddress(string address)
        {
            SetNetworkCheckHostAddress(address, null);
        }

        public void SetNetworkCheckHostAddress(string address, AIHelpDefine.OnNetworkCheckResultCallback callback)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.SetNetworkCheckHostAddress(address, callback);
        }

        public void StartUnreadMessageCountPolling(AIHelpDefine.OnMessageCountArrivedCallback callback)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.StartUnreadMessageCountPolling(callback);
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

        public void SetOnSpecificFormSubmittedCallback(AIHelpDefine.OnSpecificFormSubmittedCallback callback)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.SetOnSpecificFormSubmittedCallback(callback);
        }

        public void SetOnAIHelpSessionOpenCallback(AIHelpDefine.OnAIHelpSessionOpenCallback callback)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.SetOnAIHelpSessionOpenCallback(callback);
        }

        public void SetOnAIHelpSessionCloseCallback(AIHelpDefine.OnAIHelpSessionCloseCallback callback)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.SetOnAIHelpSessionCloseCallback(callback);
        }

        public void Close() {
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

        public void SetSDKEdgeInsets(float top, float bottom, bool enable)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.SetSDKEdgeInsets(top, bottom, enable);
        }

        public void SetSDKEdgeColor(float red, float green, float blue, float alpha)
        {
            if (!IsHelpCorePrepared()) return;
            helpCore.SetSDKEdgeColor(red, green, blue, alpha);
        }

#endif

        private bool IsHelpCorePrepared()
        {
            return helpCore != null;
        }

    }

}