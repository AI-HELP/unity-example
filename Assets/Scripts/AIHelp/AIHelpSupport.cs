using System;
namespace AIHelp
{

    public class AIHelpSupport
    {

        public static void Init(string appKey, string domain, string appId, string language)
        {
            AIHelpCore.getInstance().Init(appKey, domain, appId, language);
        }

        public static void Init(string appKey, string domain, string appId)
        {
            Init(appKey, domain, appId, "");
        }

        public static void SetOnAIHelpInitializedCallback(AIHelpDefine.OnAIHelpInitializedCallback callback)
        {
            AIHelpCore.getInstance().SetOnAIHelpInitializedCallback(callback);
        }

        public static bool Show(string entranceId)
        {
            return AIHelpCore.getInstance().Show(entranceId);
        }

        public static bool Show(ApiConfig apiConfig)
        {
            return AIHelpCore.getInstance().Show(apiConfig);
        }

        public static void ShowSingleFAQ(string faqId, ConversationMoment moment)
        {
            AIHelpCore.getInstance().ShowSingleFAQ(faqId, moment);
        }

        public static void UpdateUserInfo(UserConfig userConfig)
        {
            AIHelpCore.getInstance().UpdateUserInfo(userConfig);
        }

        public static void ResetUserInfo()
        {
            AIHelpCore.getInstance().ResetUserInfo();
        }

        public static void UpdateSDKLanguage(string language)
        {
            AIHelpCore.getInstance().UpdateSDKLanguage(language);
        }

        public static void SetUploadLogPath(string path)
        {
            AIHelpCore.getInstance().SetUploadLogPath(path);
        }

        public static void SetPushTokenAndPlatform(string pushToken, PushPlatform platform)
        {
            AIHelpCore.getInstance().SetPushTokenAndPlatform(pushToken, platform);
        }

        public static void SetNetworkCheckHostAddress(string address)
        {
            SetNetworkCheckHostAddress(address, null);
        }

        public static void SetNetworkCheckHostAddress(string address, AIHelpDefine.OnNetworkCheckResultCallback callback)
        {
            AIHelpCore.getInstance().SetNetworkCheckHostAddress(address, callback);
        }

        public static void StartUnreadMessageCountPolling(AIHelpDefine.OnMessageCountArrivedCallback callback)
        {
            AIHelpCore.getInstance().StartUnreadMessageCountPolling(callback);
        }

        public static string GetSDKVersion()
        {
            return AIHelpCore.getInstance().GetSDKVersion();
        }

        public static bool IsAIHelpShowing()
        {
            return AIHelpCore.getInstance().IsAIHelpShowing();
        }

        public static void enableLogging(bool enable)
        {
            AIHelpCore.getInstance().EnableLogging(enable);
        }

        public static void ShowUrl(string url)
        {
            AIHelpCore.getInstance().ShowUrl(url);
        }

        public static void AdditionalSupportFor(PublishCountryOrRegion countryOrRegion)
        {
            AIHelpCore.getInstance().AdditionalSupportFor(countryOrRegion);
        }

        public static void SetOnSpecificFormSubmittedCallback(AIHelpDefine.OnSpecificFormSubmittedCallback callback)
        {
            AIHelpCore.getInstance().SetOnSpecificFormSubmittedCallback(callback);
        }

        public static void SetOnAIHelpSessionOpenCallback(AIHelpDefine.OnAIHelpSessionOpenCallback callback)
        {
            AIHelpCore.getInstance().SetOnAIHelpSessionOpenCallback(callback);
        }

        public static void SetOnAIHelpSessionCloseCallback(AIHelpDefine.OnAIHelpSessionCloseCallback callback)
        {
            AIHelpCore.getInstance().SetOnAIHelpSessionCloseCallback(callback);
        }

        public static void SetOnSpecificUrlClickedCallback(AIHelpDefine.OnSpecificUrlClickedCallback callback)
        {
            AIHelpCore.getInstance().SetOnSpecificUrlClickedCallback(callback);
        }

        public static void Close() {
            AIHelpCore.getInstance().Close();
        }

#if UNITY_IOS

        public static void SetRequestedOrientation(int requestedOrientation)
        {
            AIHelpCore.getInstance().SetRequestedOrientation(requestedOrientation);
        }

        public static void SetSDKAppearanceMode(int mode)
        {
            AIHelpCore.getInstance().SetSDKAppearanceMode(mode);
        }

        public static void SetSDKEdgeInsets(float top, float bottom, bool enable)
        {
            AIHelpCore.getInstance().SetSDKEdgeInsets(top, bottom, enable);
        }

        public static void SetSDKEdgeColor(float red, float green, float blue, float alpha)
        {
            AIHelpCore.getInstance().SetSDKEdgeColor(red, green, blue, alpha);
        }

#endif

    }

}