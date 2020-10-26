// We need this one for importing our IOS functions
using System.Runtime.InteropServices;
using AOT;
using static AIHelpDefine;

#if UNITY_IOS

public class AIHelpiOSCore : IAIHelpCore
{

    static event OnAIHelpInitializedCallback _iOSInitCallback;
    static event OnNetworkCheckResultCallback _iOSNetworkCheckCallback;
    static event OnMessageCountArrivedCallback _iOSMessageCountCallback;

    [DllImport("__Internal")]  
    private static extern void unity_init(string apiKey, string domainName, string appId);

    [DllImport("__Internal")]
    private static extern void unity_initLan(string apiKey, string domainName, string appId, string language);

    public delegate void iOSOnAIHelpInitialized();
    [MonoPInvokeCallback(typeof(iOSOnAIHelpInitialized))]
    private static void iOSInitCallback()
    {
        if (_iOSInitCallback != null)
        {
            _iOSInitCallback();

        }
    }
    [DllImport("__Internal")]
    private static extern void unity_setOnInitializedCallback(iOSOnAIHelpInitialized callBack);

    [DllImport("__Internal")]
    private static extern void unity_showConversation();

    [DllImport("__Internal")]
    private static extern void unity_showConversationConfig(int conversationIntent, bool alwaysShowHumanSupportButtonInBotPage, string welcomeMessage, string storyNode);

    [DllImport("__Internal")]
    private static extern void unity_showAllFAQSections();

    [DllImport("__Internal")]
    private static extern void unity_showAllFAQSectionsConfig(int conversationMoment, int conversationIntent, bool alwaysShowHumanSupportButtonInBotPage, string welcomeMessage);

    [DllImport("__Internal")]
    private static extern void unity_showFAQSection(string sectionId);

    [DllImport("__Internal")]
    private static extern void unity_showFAQSectionConfig(string sectionId, int conversationMoment, int conversationIntent, bool alwaysShowHumanSupportButtonInBotPage, string welcomeMessage);

    [DllImport("__Internal")]
    private static extern void unity_showSingleFAQ(string faqId);

    [DllImport("__Internal")]
    private static extern void unity_showSingleFAQConfig(string faqId, int conversationMoment, int conversationIntent, bool contactUsAlwaysOnline, string welcomeMessage);

    [DllImport("__Internal")]
    private static extern void unity_showOperation();

    [DllImport("__Internal")]
    private static extern void unity_showOperationConfig(int selectIndex, string conversationTitle, int conversationIntent, bool contactUsAlwaysOnline, string welcomeMessage);

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


    public delegate void iOSOnMessageCountArrived(int msgCount);
    [MonoPInvokeCallback(typeof(iOSOnMessageCountArrived))]
    private static void iOSOnMessageCountArrivedMethod(int msgCount)
    {
        if (_iOSMessageCountCallback != null)
        {
            _iOSMessageCountCallback(msgCount);

        }
    }
    [DllImport("__Internal")]
    private static extern void unity_startUnreadMessageCountPolling(iOSOnMessageCountArrived callback);


    public void Init(string appKey, string domain, string appId){
        unity_init(appKey, domain, appId);
    }

    public void Init(string appKey, string domain, string appId, string language){
        unity_initLan(appKey, domain, appId, language);
    }

    public void SetOnAIHelpInitializedCallback(OnAIHelpInitializedCallback callback)
    {
        _iOSInitCallback = callback;
        unity_setOnInitializedCallback(iOSInitCallback);
    }

    public void ShowConversation()
    {
        unity_showConversation();
    }

    public void ShowConversation(ConversationConfig config)
    {
        unity_showConversationConfig(config.GetConversationIntent(), config.IsAlwaysShowHumanSupportButtonInBotPage(), config.GetWelcomeMessage(), config.GetStoryNode());
    }

    public void ShowAllFAQSections()
    {
        unity_showAllFAQSections();
    }

    public void ShowAllFAQSections(FaqConfig config)
    {
        unity_showAllFAQSectionsConfig(config.GetShowConversationMoment(), 
                             config.GetConversationConfig().GetConversationIntent(), 
                             config.GetConversationConfig().IsAlwaysShowHumanSupportButtonInBotPage(), 
                             config.GetConversationConfig().GetWelcomeMessage());
    }

    public void ShowFAQSection(string sectionId)
    {
        unity_showFAQSection(sectionId);
    }

    public void ShowFAQSection(string sectionId, FaqConfig config)
    {
        unity_showFAQSectionConfig(sectionId, config.GetShowConversationMoment(),
                             config.GetConversationConfig().GetConversationIntent(),
                             config.GetConversationConfig().IsAlwaysShowHumanSupportButtonInBotPage(),
                             config.GetConversationConfig().GetWelcomeMessage());
    }

    public void ShowSingleFAQ(string faqId)
    {
        unity_showSingleFAQ(faqId);
    }

    public void ShowSingleFAQ(string faqId, FaqConfig config)
    {
        unity_showSingleFAQConfig(faqId, config.GetShowConversationMoment(),
                             config.GetConversationConfig().GetConversationIntent(),
                             config.GetConversationConfig().IsAlwaysShowHumanSupportButtonInBotPage(),
                             config.GetConversationConfig().GetWelcomeMessage());
    }

    public void ShowOperation()
    {
        unity_showOperation();
    }

    public void ShowOperation(OperationConfig config)
    {
        unity_showOperationConfig(config.GetSelectIndex(), 
                                  config.GetConversationTitle(), 
                                  config.GetConversationConfig().GetConversationIntent(),
                                  config.GetConversationConfig().IsAlwaysShowHumanSupportButtonInBotPage(),
                                  config.GetConversationConfig().GetWelcomeMessage());
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
        int tempPlatform = 1;
        if (platform == PushPlatform.APNS)
        {
            tempPlatform = 1;
        }else if (platform == PushPlatform.FIREBASE)
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
        unity_setPushTokenAndPlatform(pushToken, tempPlatform);
    }

    public void SetNetworkCheckHostAddress(string address, OnNetworkCheckResultCallback callback)
    {
        _iOSNetworkCheckCallback = callback;
        unity_setNetworkCheckHostAddress(address, iOSNetworkCheckCallbackMethod);
    }

    public void StartUnreadMessageCountPolling(OnMessageCountArrivedCallback callback)
    {
        _iOSMessageCountCallback = callback;
        unity_startUnreadMessageCountPolling(iOSOnMessageCountArrivedMethod);
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
}

#endif