using System;

namespace AIHelp
{
    public interface IAIHelpCore
    {
        void Initialize(string domain, string appId, string language);
        void SetOnAIHelpInitializedCallback(AIHelpDelegate.OnAIHelpInitializedCallback callback);
        void SetOnAIHelpInitializedAsyncCallback(AIHelpDelegate.OnAIHelpInitializedAsyncCallback callback);

        bool Show(string entranceId);
        bool Show(ApiConfig apiConfig);
        void ShowSingleFAQ(string faqId, ConversationMoment moment);

        void Login(LoginConfig loginConfig);
        void Logout();
        void UpdateUserInfo(UserConfig userConfig);
        void ResetUserInfo();

        void UpdateSDKLanguage(string language);
        void SetUploadLogPath(string path);
        void SetPushTokenAndPlatform(string pushToken, PushPlatform platform);

        void StartUnreadMessageCountPolling(AIHelpDelegate.OnMessageCountArrivedCallback callback);
        void FetchUnreadMessageCount(AIHelpDelegate.OnMessageCountArrivedCallback callback);

        string GetSDKVersion();
        bool IsAIHelpShowing();
        void EnableLogging(bool enable);
        void ShowUrl(string url);
        void AdditionalSupportFor(PublishCountryOrRegion countryOrRegion);
        void SetOnSpecificFormSubmittedCallback(AIHelpDelegate.OnSpecificFormSubmittedCallback callback);
        void SetOnAIHelpSessionOpenCallback(AIHelpDelegate.OnAIHelpSessionOpenCallback callback);
        void SetOnAIHelpSessionCloseCallback(AIHelpDelegate.OnAIHelpSessionCloseCallback callback);
        void SetOnSpecificUrlClickedCallback(AIHelpDelegate.OnSpecificUrlClickedCallback callback);

        void Close();

#if UNITY_IOS
        void SetRequestedOrientation(int requestedOrientation);     // ios only
        void SetSDKAppearanceMode(int mode);    // iOS only  0: follow the system  1: light mode  2： dark mode
#endif

    }

}