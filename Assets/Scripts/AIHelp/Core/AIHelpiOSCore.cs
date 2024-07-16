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

        public void RegisterAsyncEventListener(AIHelp.EventType eventType, AIHelpDelegate.AsyncEventListener listener)
        {
            eventListeners[eventType] = listener;
            unity_registerAsyncEventListener(eventType, OCAsyncEventListener);
        }

        public void UnregisterAsyncEventListener(AIHelp.EventType eventType)
        {
            unity_unregisterAsyncEventListener(eventType);
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
            var userConfig = loginConfig.UserConfig;
            if (userConfig == null) {
                userConfig = new UserConfig.Builder().Build();
            }
            unity_login(loginConfig.UserId, userConfig.UserName, userConfig.ServerId, userConfig.UserTags, userConfig.CustomData, loginConfig.EnterpriseAuth); 
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

        public void FetchUnreadMessageCount()
        {
            unity_fetchUnreadMessageCount();
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