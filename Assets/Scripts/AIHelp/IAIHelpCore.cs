namespace AIHelp
{

    public class AIHelpDefine
    {

        public delegate void OnAIHelpInitializedCallback();

        public delegate void OnNetworkCheckResultCallback(string netLog);

        public delegate void OnMessageCountArrivedCallback(int msgCount);

        public delegate void OnSpecificFormSubmittedCallback();

        public delegate void OnAIHelpSessionOpenCallback();

        public delegate void OnAIHelpSessionCloseCallback();

    }

    public interface IAIHelpCore
    {
        void Init(string appKey, string domain, string appId, string language);
        void SetOnAIHelpInitializedCallback(AIHelpDefine.OnAIHelpInitializedCallback callback);
        
        bool Show(string entranceId);
        bool Show(ApiConfig apiConfig);

        void UpdateUserInfo(UserConfig userConfig);
        void ResetUserInfo();

        void UpdateSDKLanguage(string language);
        void SetUploadLogPath(string path);
        void SetPushTokenAndPlatform(string pushToken, PushPlatform platform);

        void SetNetworkCheckHostAddress(string address, AIHelpDefine.OnNetworkCheckResultCallback callback);
        void StartUnreadMessageCountPolling(AIHelpDefine.OnMessageCountArrivedCallback callback);

        string GetSDKVersion();
        bool IsAIHelpShowing();
        void EnableLogging(bool enable);
        void ShowUrl(string url);
        void AdditionalSupportFor(PublishCountryOrRegion countryOrRegion);
        void SetOnSpecificFormSubmittedCallback(AIHelpDefine.OnSpecificFormSubmittedCallback callback);
        void SetOnAIHelpSessionOpenCallback(AIHelpDefine.OnAIHelpSessionOpenCallback callback);
        void SetOnAIHelpSessionCloseCallback(AIHelpDefine.OnAIHelpSessionCloseCallback callback);

        void Close();

#if UNITY_IOS
        void SetRequestedOrientation(int requestedOrientation);     // ios only
        void SetSDKAppearanceMode(int mode);    // iOS only  0: follow the system  1: light mode  2： dark mode
        void SetSDKEdgeInsets(float top, float bottom, bool enable);    // iOS only
        void SetSDKEdgeColor(float red, float green, float blue, float alpha);      // iOS only
#endif

    }

}