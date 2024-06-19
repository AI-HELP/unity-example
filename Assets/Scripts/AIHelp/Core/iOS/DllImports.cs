using System;
using System.Runtime.InteropServices;
using AOT;

namespace AIHelp
{
    #if UNITY_IOS
    public partial class AIHelpiOSCore
    {
        [DllImport("__Internal")]
        private static extern void unity_initiailize(string domainName, string appId, string language);

        [DllImport("__Internal")]
        private static extern void unity_setOnInitializedCallback(AIHelpDelegate.OnAIHelpInitializedCallback callBack);

        [DllImport("__Internal")]
        private static extern void unity_setOnInitializedAsyncCallback(AIHelpDelegate.OnAIHelpInitializedAsyncCallback callBack);

        [DllImport("__Internal")]
        private static extern void unity_login(string userId, string userName, string serverId, string userTags, string customData, 
            AIHelpEnterpriseTokenDelegate authCallBack, AIHelpDelegate.OnLoginResultCallback loginCallback);

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
        private static extern bool unity_show(string entranceId, string welcomeMessage);

        [DllImport("__Internal")]
        private static extern void unity_showSingleFAQ(string faqId, int conversationMoment);

        [DllImport("__Internal")]
        private static extern void unity_startUnreadMessageCountPolling(AIHelpDelegate.OnMessageCountArrivedCallback callback);

        [DllImport("__Internal")]
        private static extern void unity_fetchUnreadMessageCount(AIHelpDelegate.OnMessageCountArrivedCallback callback);

        [DllImport("__Internal")]
        private static extern void unity_showUrl(string url);

        [DllImport("__Internal")]
        private static extern void unity_additionalSupportFor(PublishCountryOrRegion countryOrRegion);

        [DllImport("__Internal")]
        private static extern void unity_setOnSpecificFormSubmittedCallback(AIHelpDelegate.OnSpecificFormSubmittedCallback callBack);

        [DllImport("__Internal")]
        private static extern void unity_setOnSessionOpenCallback(AIHelpDelegate.OnAIHelpSessionOpenCallback callBack);

        [DllImport("__Internal")]
        private static extern void unity_setOnSessionCloseCallback(AIHelpDelegate.OnAIHelpSessionCloseCallback callBack);

        [DllImport("__Internal")]
        private static extern void unity_setOnSpecificUrlClickedCallback(AIHelpDelegate.OnSpecificUrlClickedCallback callback);

        [DllImport("__Internal")]
        private static extern void unity_setSDKAppearanceMode(int mode);

        [DllImport("__Internal")]
        private static extern void unity_setSDKInterfaceOrientationMask(int interfaceOrientationMask);
    }
    #endif
}
