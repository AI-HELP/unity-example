// We need this one for importing our IOS functions
using System.Runtime.InteropServices;
using AOT;
using System;
namespace AIHelp
{

#if UNITY_IOS
    public partial class AIHelpiOSCore : IAIHelpCore
    {

        public void Initialize(string domain, string appId, string language = "")
        {
            unity_initiailize(domain, appId, language);
        }

        public void SetOnAIHelpInitializedCallback(AIHelpDelegate.OnAIHelpInitializedCallback callback)
        {
            _iOSInitCallback = callback;
            unity_setOnInitializedCallback(iOSInitCallback);
        }

        public void SetOnAIHelpInitializedAsyncCallback(AIHelpDelegate.OnAIHelpInitializedAsyncCallback callback)
        {
            _iOSInitAsyncCallback = callback;
            unity_setOnInitializedAsyncCallback(iOSInitCallbackAsync);
        }

        public bool Show(string entranceId) 
        {
            return unity_show(entranceId, "");
        }

        public bool Show(ApiConfig apiConfig) 
        {
            return unity_show(apiConfig.EntranceId, apiConfig.WelcomeMessage);
        }

        public void ShowSingleFAQ(string faqId, ConversationMoment moment)
        {
            unity_showSingleFAQ(faqId, (int)moment);
        }

        public void Login(LoginConfig loginConfig) {
            _iOSEnterpriseAuthCallback = loginConfig.OnEnterpriseAuthCallback;
            _iOSLoginCallback = loginConfig.OnLoginResultCallback;
            UserConfig userConfig = loginConfig.UserConfig;
            
            if (_iOSEnterpriseAuthCallback == null && _iOSLoginCallback == null) {
                unity_login(loginConfig.UserId, userConfig.UserName, userConfig.ServerId, userConfig.UserTags, userConfig.CustomData, null, null); 
            } else if (_iOSEnterpriseAuthCallback == null) {
                unity_login(loginConfig.UserId, userConfig.UserName, userConfig.ServerId, userConfig.UserTags, userConfig.CustomData, null, iOSLoginCallback); 
            } else if (_iOSLoginCallback == null) {
                unity_login(loginConfig.UserId, userConfig.UserName, userConfig.ServerId, userConfig.UserTags, userConfig.CustomData, iOSEnterpriseAuthCallback, null); 
            } else {
                unity_login(loginConfig.UserId, userConfig.UserName, userConfig.ServerId, userConfig.UserTags, userConfig.CustomData, iOSEnterpriseAuthCallback, iOSLoginCallback);
            }

        }

        public void Logout() {
            unity_logout();
        }

        public void UpdateUserInfo(UserConfig userConfig)
        {
            unity_updateUserInfo(userConfig.UserName, userConfig.ServerId, userConfig.UserTags, userConfig.CustomData);
        }

        public void ResetUserInfo()
        {
            unity_resetUserInfo();
        }

        public void UpdateSDKLanguage(string language)
        {
            unity_updateSDKLanguage(language);
        }

        public void SetUploadLogPath(string path)
        {
            unity_setUploadLogPath(path);
        }

        public void SetPushTokenAndPlatform(string pushToken, PushPlatform platform)
        {
            unity_setPushTokenAndPlatform(pushToken, getPushPlatform(platform));
        }

        public void StartUnreadMessageCountPolling(AIHelpDelegate.OnMessageCountArrivedCallback callback)
        {
            _iOSPollingMessageCountCallback = callback;
            unity_startUnreadMessageCountPolling(iOSOnPollingMessageCountArrivedMethod);
        }

        public void FetchUnreadMessageCount(AIHelpDelegate.OnMessageCountArrivedCallback callback)
        {
            _iOSFetchMessageCountCallback = callback;
            unity_fetchUnreadMessageCount(iOSOnFetchMessageCountArrivedMethod);
        }

        public string GetSDKVersion()
        {
            return unity_getSDKVersion();
        }

        public bool IsAIHelpShowing()
        {
            return unity_isAIHelpShowing();
        }

        public void EnableLogging(bool enable)
        {
            unity_enableLogging(enable);
        }

        public void ShowUrl(string url)
        {
            unity_showUrl(url);
        }

        public void AdditionalSupportFor(PublishCountryOrRegion countryOrRegion)
        {
            unity_additionalSupportFor(countryOrRegion);
        }

        public void SetOnSpecificFormSubmittedCallback(AIHelpDelegate.OnSpecificFormSubmittedCallback callback)
        {
            _iOSSpecificFormCallback = callback;
            unity_setOnSpecificFormSubmittedCallback(iOSSpecificFormSubmit);
        }

        public void SetOnAIHelpSessionOpenCallback(AIHelpDelegate.OnAIHelpSessionOpenCallback callback)
        {
            _iOSSessionOpenCallback = callback;
            unity_setOnSessionOpenCallback(iOSSessionOpenSubmit);
        }

        public void SetOnAIHelpSessionCloseCallback(AIHelpDelegate.OnAIHelpSessionCloseCallback callback)
        {
            _iOSSessionCloseCallback = callback;
            unity_setOnSessionCloseCallback(iOSSessionCloseSubmit);
        }

        public void SetOnSpecificUrlClickedCallback(AIHelpDelegate.OnSpecificUrlClickedCallback callback)
        {
            _iOSSpecificUrlClickedCallback = callback;
            unity_setOnSpecificUrlClickedCallback(iOSSpecificUrlClicked);
        }

        public void Close() {
            unity_close();
        }

        public void SetRequestedOrientation(int requestedOrientation)
        {
            unity_setSDKInterfaceOrientationMask(requestedOrientation);
        }

        public void SetSDKAppearanceMode(int mode)
        {
            unity_setSDKInterfaceOrientationMask(mode);
        }

        private int getPushPlatform(PushPlatform platform){
            int tempPlatform = 2;
            if (platform == PushPlatform.APNS)
            {
                tempPlatform = 1;
            }
            else if (platform == PushPlatform.FIREBASE)
            {
                tempPlatform = 2;
            }
            else if (platform == PushPlatform.JIGUANG)
            {
                tempPlatform = 3;
            }
            else if (platform == PushPlatform.GETUI)
            {
                tempPlatform = 4;
            }
            else if (platform == PushPlatform.HUAWEI)
            {
                tempPlatform = 6;
            }
            else if (platform == PushPlatform.ONE_SIGNAL)
            {
                tempPlatform = 7;
            }
            return tempPlatform;
        }

    }
#endif
}