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

        public static void RegisterAsyncEventListener(AIHelp.EventType eventType, AIHelpDelegate.AsyncEventListener listener)
        {
            AIHelpCore.getInstance().RegisterAsyncEventListener(eventType, listener);
        }

        public static void UnregisterAsyncEventListener(AIHelp.EventType eventType)
        {
            AIHelpCore.getInstance().UnregisterAsyncEventListener(eventType);
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

        public static void FetchUnreadMessageCount()
        {
            AIHelpCore.getInstance().FetchUnreadMessageCount();
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