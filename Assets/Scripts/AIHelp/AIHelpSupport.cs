using System;
namespace AIHelp
{
    public class AIHelpSupport
    {
        public static void Initialize(string domain, string appId, string language)
        {
            AIHelpCore.getInstance().Initialize(domain, appId, language);
        }

        public static void Initialize(string domain, string appId)
        {
            Initialize(domain, appId, "");
        }

        public static void SetOnAIHelpInitializedCallback(AIHelpDelegate.OnAIHelpInitializedCallback callback)
        {
            AIHelpCore.getInstance().SetOnAIHelpInitializedCallback(callback);
        }

        public static void SetOnAIHelpInitializedAsyncCallback(AIHelpDelegate.OnAIHelpInitializedAsyncCallback callback)
        {
            AIHelpCore.getInstance().SetOnAIHelpInitializedAsyncCallback(callback);
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

        public static void Login(string userId)
        {
            Login(new LoginConfig.Builder().SetUserId(userId).Build());
        }

        public static void Login(LoginConfig loginConfig)
        {
            AIHelpCore.getInstance().Login(loginConfig);
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

        public static void StartUnreadMessageCountPolling(AIHelpDelegate.OnMessageCountArrivedCallback callback)
        {
            AIHelpCore.getInstance().StartUnreadMessageCountPolling(callback);
        }

        public static void FetchUnreadMessageCount(AIHelpDelegate.OnMessageCountArrivedCallback callback)
        {
            AIHelpCore.getInstance().FetchUnreadMessageCount(callback);
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

        public static void SetOnSpecificFormSubmittedCallback(AIHelpDelegate.OnSpecificFormSubmittedCallback callback)
        {
            AIHelpCore.getInstance().SetOnSpecificFormSubmittedCallback(callback);
        }

        public static void SetOnAIHelpSessionOpenCallback(AIHelpDelegate.OnAIHelpSessionOpenCallback callback)
        {
            AIHelpCore.getInstance().SetOnAIHelpSessionOpenCallback(callback);
        }

        public static void SetOnAIHelpSessionCloseCallback(AIHelpDelegate.OnAIHelpSessionCloseCallback callback)
        {
            AIHelpCore.getInstance().SetOnAIHelpSessionCloseCallback(callback);
        }

        public static void SetOnSpecificUrlClickedCallback(AIHelpDelegate.OnSpecificUrlClickedCallback callback)
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

#endif
    }
}