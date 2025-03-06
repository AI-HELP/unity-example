using System;

namespace AIHelp
{
    public interface IAIHelpCore
    {
        void Initialize(string domain, string appId, string language);

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

        void FetchUnreadMessageCount();
        void FetchUnreadTaskCount();

        string GetSDKVersion();
        bool IsAIHelpShowing();
        void EnableLogging(bool enable);
        void ShowUrl(string url);
        void AdditionalSupportFor(PublishCountryOrRegion countryOrRegion);

        void RegisterAsyncEventListener(AIHelp.EventType eventType, AIHelpDelegate.AsyncEventListener listener);
        void UnregisterAsyncEventListener(AIHelp.EventType eventType);

        void Close();
        void Uninstall();

    }

}