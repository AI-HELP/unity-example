using System;
using System.Runtime.InteropServices;
using AOT;

namespace AIHelp
{
    #if UNITY_IOS
    public partial class AIHelpiOSCore
    {
        [DllImport("__Internal")]
        private static extern void unity_initialize(string domainName, string appId, string language);

        [DllImport("__Internal")]
        private static extern void unity_registerAsyncEventListener(AIHelp.EventType eventType, AIHelpAsyncEventListener listener);

        [DllImport("__Internal")]
        private static extern void unity_unregisterAsyncEventListener(AIHelp.EventType eventType);

        [DllImport("__Internal")]
        private static extern void unity_login(string userId, string userName, string serverId, string userTags, string customData, bool isEnterpriseAuth);

        [DllImport("__Internal")]
        private static extern void unity_logout();

        [DllImport("__Internal")]
        private static extern void unity_updateUserInfo(string userName, string serverId, string userTags, string customData);

        [DllImport("__Internal")]
        private static extern void unity_resetUserInfo();

        [DllImport("__Internal")]
        private static extern void unity_updateSDKLanguage(string language);

        [DllImport("__Internal")]
        private static extern void unity_setUploadLogPath(string path);

        [DllImport("__Internal")]
        private static extern void unity_setPushTokenAndPlatform(string pushToken, int pushPlatform);

        [DllImport("__Internal")]
        private static extern string unity_getSDKVersion();

        [DllImport("__Internal")]
        private static extern bool unity_isAIHelpShowing();

        [DllImport("__Internal")]
        private static extern void unity_enableLogging(bool enable);

        [DllImport("__Internal")]
        private static extern void unity_close();

        [DllImport("__Internal")]
        private static extern void unity_uninstall();

        [DllImport("__Internal")]
        private static extern bool unity_show(string entranceId, string welcomeMessage);

        [DllImport("__Internal")]
        private static extern void unity_showSingleFAQ(string faqId, int conversationMoment);

        [DllImport("__Internal")]
        private static extern void unity_fetchUnreadMessageCount();

        [DllImport("__Internal")]
        private static extern void unity_fetchUnreadTaskCount();

        [DllImport("__Internal")]
        private static extern void unity_showUrl(string url);

        [DllImport("__Internal")]
        private static extern void unity_additionalSupportFor(PublishCountryOrRegion countryOrRegion);

    }
    #endif
}
