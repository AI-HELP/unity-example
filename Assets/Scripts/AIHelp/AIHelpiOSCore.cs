// We need this one for importing our IOS functions
using System.Runtime.InteropServices;
using AOT;
using System;
namespace AIHelp
{

#if UNITY_IOS

    public class AIHelpiOSCore : IAIHelpCore
    {

        static event AIHelpDefine.OnAIHelpInitializedCallback _iOSInitCallback;
        static event AIHelpDefine.OnNetworkCheckResultCallback _iOSNetworkCheckCallback;
        static event AIHelpDefine.OnMessageCountArrivedCallback _iOSPollingMessageCountCallback;
        static event AIHelpDefine.OnMessageCountArrivedCallback _iOSFetchMessageCountCallback;
        static event AIHelpDefine.OnSpecificFormSubmittedCallback _iOSSpecificFormCallback;
        static event AIHelpDefine.OnAIHelpSessionOpenCallback _iOSSessionOpenCallback;
        static event AIHelpDefine.OnAIHelpSessionCloseCallback _iOSSessionCloseCallback;
        static event AIHelpDefine.OnSpecificUrlClickedCallback _iOSSpecificUrlClickedCallback;

        [DllImport("__Internal")]
        private static extern void unity_init(string apiKey, string domainName, string appId);

        [DllImport("__Internal")]
        private static extern void unity_initLan(string apiKey, string domainName, string appId, string language);

        public delegate void iOSOnAIHelpInitialized(bool isSuccess, string message);
        [MonoPInvokeCallback(typeof(iOSOnAIHelpInitialized))]
        private static void iOSInitCallback(bool isSuccess, string message)
        {
            if (_iOSInitCallback != null)
            {
                _iOSInitCallback(isSuccess, message);

            }
        }
        [DllImport("__Internal")]
        private static extern void unity_setOnInitializedCallback(iOSOnAIHelpInitialized callBack);

        [DllImport("__Internal")]
        private static extern bool unity_show(string entranceId, string welcomeMessage);

        [DllImport("__Internal")]
        private static extern void unity_showSingleFAQ(string faqId, int conversationMoment);

        [DllImport("__Internal")]
        private static extern void unity_updateUserInfo(string userId, string userName, string serverId, string userTags, string customData, bool isSyncCrmInfo);

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
        private static extern void unity_setSDKInterfaceOrientationMask(int interfaceOrientationMask);

        [DllImport("__Internal")]
        private static extern void unity_setSDKAppearanceMode(int mode);

        [DllImport("__Internal")]
        private static extern void unity_setSDKEdgeInsets(float top, float bottom, bool enable);

        [DllImport("__Internal")]
        private static extern void unity_setSDKEdgeColor(float red, float green, float blue, float alpha);


        public delegate void iOSOnNetworkCheckResult(string netLog);
        [MonoPInvokeCallback(typeof(iOSOnNetworkCheckResult))]
        private static void iOSNetworkCheckCallbackMethod(string netLog)
        {
            if (_iOSNetworkCheckCallback != null)
            {
                _iOSNetworkCheckCallback(netLog);

            }
        }
        [DllImport("__Internal")]
        private static extern void unity_setNetworkCheckHostAddress(string address, iOSOnNetworkCheckResult callback);


        public delegate void iOSOnPollingMessageCountArrived(int msgCount);
        [MonoPInvokeCallback(typeof(iOSOnPollingMessageCountArrived))]
        private static void iOSOnPollingMessageCountArrivedMethod(int msgCount)
        {
            if (_iOSPollingMessageCountCallback != null)
            {
                _iOSPollingMessageCountCallback(msgCount);
            }
        }

        public delegate void iOSOnFetchMessageCountArrived(int msgCount);
        [MonoPInvokeCallback(typeof(iOSOnFetchMessageCountArrived))]
        private static void iOSOnFetchMessageCountArrivedMethod(int msgCount)
        {
            if (_iOSFetchMessageCountCallback != null)
            {
                _iOSFetchMessageCountCallback(msgCount);
            }
        }

        [DllImport("__Internal")]
        private static extern void unity_startUnreadMessageCountPolling(iOSOnPollingMessageCountArrived callback);

        [DllImport("__Internal")]
        private static extern void unity_fetchUnreadMessageCount(iOSOnFetchMessageCountArrived callback);

        [DllImport("__Internal")]
        private static extern void unity_showUrl(string url);

        [DllImport("__Internal")]
        private static extern void unity_additionalSupportFor(PublishCountryOrRegion countryOrRegion);

        public delegate void iOSOnAIHelpSpecificFormSubmit();
        [MonoPInvokeCallback(typeof(iOSOnAIHelpSpecificFormSubmit))]
        private static void iOSSpecificFormSubmit()
        {
            if (_iOSSpecificFormCallback != null)
            {
                _iOSSpecificFormCallback();

            }
        }
        [DllImport("__Internal")]
        private static extern void unity_setOnSpecificFormSubmittedCallback(iOSOnAIHelpSpecificFormSubmit callBack);



        public delegate void iOSOnAIHelpSessionOpenSubmit();
        [MonoPInvokeCallback(typeof(iOSOnAIHelpSessionOpenSubmit))]
        private static void iOSSessionOpenSubmit()
        {
            if (_iOSSessionOpenCallback != null)
            {
                _iOSSessionOpenCallback();

            }
        }
        [DllImport("__Internal")]
        private static extern void unity_setOnSessionOpenCallback(iOSOnAIHelpSessionOpenSubmit callBack);


        public delegate void iOSOnAIHelpSessionCloseSubmit();
        [MonoPInvokeCallback(typeof(iOSOnAIHelpSessionCloseSubmit))]
        private static void iOSSessionCloseSubmit()
        {
            if (_iOSSessionCloseCallback != null)
            {
                _iOSSessionCloseCallback();

            }
        }
        [DllImport("__Internal")]
        private static extern void unity_setOnSessionCloseCallback(iOSOnAIHelpSessionCloseSubmit callBack);

        public delegate void iOSSpecificUrlClickedCallback(string url);
        [MonoPInvokeCallback(typeof(iOSSpecificUrlClickedCallback))]
        private static void iOSSpecificUrlClicked(string url)
        {
            if (_iOSSpecificUrlClickedCallback != null)
            {
                _iOSSpecificUrlClickedCallback(url);
            }
        }
        [DllImport("__Internal")]
        private static extern void unity_setOnSpecificUrlClickedCallback(iOSSpecificUrlClickedCallback callback);

        public void Init(string appKey, string domain, string appId)
        {
            unity_init(appKey, domain, appId);
        }

        public void Init(string appKey, string domain, string appId, string language)
        {
            unity_initLan(appKey, domain, appId, language);
        }

        public void SetOnAIHelpInitializedCallback(AIHelpDefine.OnAIHelpInitializedCallback callback)
        {
            _iOSInitCallback = callback;
            unity_setOnInitializedCallback(iOSInitCallback);
        }

        public bool Show(string entranceId) 
        {
            return unity_show(entranceId, "");
        }

        public bool Show(ApiConfig apiConfig) 
        {
            return unity_show(apiConfig.GetEntranceId(), apiConfig.GetWelcomeMessage());
        }

        public void ShowSingleFAQ(string faqId, ConversationMoment moment)
        {
            unity_showSingleFAQ(faqId, (int)moment);
        }

        public void UpdateUserInfo(UserConfig userConfig)
        {
            unity_updateUserInfo(userConfig.GetUserId(), userConfig.GetUserName(),
                                 userConfig.GetServerId(), userConfig.GetUserTags(),
                                 userConfig.GetCustomData(), userConfig.GetWhetherSyncCrmInfo());
        }

        public void ResetUserInfo()
        {
            unity_resetUserInfo();
        }

        public void UpdateSDKLanguage(string language)
        {
            unity_updateSDKLanguage(language);
        }

        public void SetRequestedOrientation(int requestedOrientation)
        {
            unity_setSDKInterfaceOrientationMask(requestedOrientation);
        }

        public void SetUploadLogPath(string path)
        {
            unity_setUploadLogPath(path);
        }

        public void SetPushTokenAndPlatform(string pushToken, PushPlatform platform)
        {
            unity_setPushTokenAndPlatform(pushToken, getPushPlatform(platform));
        }

        public void SetNetworkCheckHostAddress(string address, AIHelpDefine.OnNetworkCheckResultCallback callback)
        {
            _iOSNetworkCheckCallback = callback;
            unity_setNetworkCheckHostAddress(address, iOSNetworkCheckCallbackMethod);
        }

        public void StartUnreadMessageCountPolling(AIHelpDefine.OnMessageCountArrivedCallback callback)
        {
            _iOSPollingMessageCountCallback = callback;
            unity_startUnreadMessageCountPolling(iOSOnPollingMessageCountArrivedMethod);
        }

        public void FetchUnreadMessageCount(AIHelpDefine.OnMessageCountArrivedCallback callback)
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

        public void SetSDKAppearanceMode(int mode)
        {
            unity_setSDKAppearanceMode(mode);
        }

        public void SetSDKEdgeInsets(float top, float bottom, bool enable)
        {
            unity_setSDKEdgeInsets(top, bottom, enable);
        }

        public void SetSDKEdgeColor(float red, float green, float blue, float alpha)
        {
            unity_setSDKEdgeColor(red, green, blue, alpha);
        }

        public void ShowUrl(string url)
        {
            unity_showUrl(url);
        }

        public void AdditionalSupportFor(PublishCountryOrRegion countryOrRegion)
        {
            unity_additionalSupportFor(countryOrRegion);
        }

        public void SetOnSpecificFormSubmittedCallback(AIHelpDefine.OnSpecificFormSubmittedCallback callback)
        {
            _iOSSpecificFormCallback = callback;
            unity_setOnSpecificFormSubmittedCallback(iOSSpecificFormSubmit);
        }

        public void SetOnAIHelpSessionOpenCallback(AIHelpDefine.OnAIHelpSessionOpenCallback callback)
        {
            _iOSSessionOpenCallback = callback;
            unity_setOnSessionOpenCallback(iOSSessionOpenSubmit);
        }

        public void SetOnAIHelpSessionCloseCallback(AIHelpDefine.OnAIHelpSessionCloseCallback callback)
        {
            _iOSSessionCloseCallback = callback;
            unity_setOnSessionCloseCallback(iOSSessionCloseSubmit);
        }

        public void SetOnSpecificUrlClickedCallback(AIHelpDefine.OnSpecificUrlClickedCallback callback)
        {
            _iOSSpecificUrlClickedCallback = callback;
            unity_setOnSpecificUrlClickedCallback(iOSSpecificUrlClicked);
        }

        public void Close() {
            unity_close();
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