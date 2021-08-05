using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using UnityEngine.UI;
using AIHelp;
public class TestBehaviourScript : MonoBehaviour
{
    private string appKey = "THIS IS YOUR APP KEY";
    private string domain = "a.aihelp.net";
    private string appId = "TryElva_platform_09ebf7fa-8d45-4843-bd59-cfda3d8a8dc0"; 

    private void Awake()
    {
     
        AIHelpSupport.AdditionalSupportFor(PublishCountryOrRegion.IN);
        AIHelpSupport.enableLogging(true);
        AIHelpSupport.Init(appKey, domain, appId,"en");
        AIHelpSupport.SetOnAIHelpInitializedCallback(OnAIHelpInitializedCallback);
        AIHelpSupport.SetOnSpecificFormSubmittedCallback(OnSpecificFormSubmittedCallback);
        AIHelpSupport.SetOnOperationUnreadChangedCallback(OnOperationUnreadChangedCallback);
        //AIHelpSupport.SetOnAIHelpSessionOpenCallback(OnOpenCallBack);
        //AIHelpSupport.SetOnAIHelpSessionCloseCallback(OnCloseCallBack);
    }

    private void Start()
    {

        Dictionary<string, Action> dic = new Dictionary<string, Action>() {
            { "Canvas/robot",robotClick },
            { "Canvas/manual",manualClick },
            { "Canvas/allSection",allSectionClick },
            { "Canvas/singleSection",singleSectionClick },
            { "Canvas/singleFAQ",singleFAQClick },
            { "Canvas/Operation",operationClick },
            { "Canvas/updateUserInfo",updateUserInfoClick },
            { "Canvas/updateSDKLanguage",updateSDKLanguageClick },
            { "Canvas/isHelpShow",isHelpShowClick },
            { "Canvas/unreadMeassage",unreadMeassageClick },
            { "Canvas/Push",pushClick },
            { "Canvas/netWorkCheck",netWorkCheckClick },
            { "Canvas/uploadLog",upLoadLogClick },
            { "Canvas/enableLogging",enableLoggingClick },
            { "Canvas/SDKVersion",SDKVersionClick },
            { "Canvas/showUrl",showUrlClick },
            { "Canvas/runAcceleration",runAccelerationClick }
        };

        dic.All(keyval=> {

            GameObject robotObj = GameObject.Find(keyval.Key);

            Button robotBtn = (Button)robotObj.GetComponent<Button>();

            robotBtn.onClick.AddListener(()=> { keyval.Value(); });

            return true;
        });
    }

    public void OnAIHelpInitializedCallback() {  
        Console.Write("AIHelp init success");
        
    }

    public void OnOperationUnreadChangedCallback(bool hasUnreadArticles) {
        Console.Write("测试_______回调");
        robotClick();
    }


    void robotClick()
    {
        ConversationConfig config = new ConversationConfig.Builder()
            .SetAlwaysShowHumanSupportButtonInBotPage(true)
            .SetConversationIntent(ConversationIntent.BOT_SUPPORT)
            .SetStoryNode("rate message")
            .build();

        AIHelpSupport.ShowConversation(config);
    }

    void manualClick()
    {
        ConversationConfig config = new ConversationConfig.Builder()
            .setWelcomeMessage("You can configure special welcome message for your end users at here.")
            .setWelcomeMessage(AIHelpSupport.GetSDKVersion())
            .SetAlwaysShowHumanSupportButtonInBotPage(false)
            .SetConversationIntent(ConversationIntent.HUMAN_SUPPORT)
            .SetStoryNode("")
            .build();
        AIHelpSupport.ShowConversation(config);

    }

    void allSectionClick()
    {
        FaqConfig.Builder faqBuilder = new FaqConfig.Builder();
        ConversationConfig.Builder conversationBuilder = new ConversationConfig.Builder();
        faqBuilder.SetShowConversationMoment(ConversationMoment.ALWAYS);
        conversationBuilder.SetAlwaysShowHumanSupportButtonInBotPage(true);
        conversationBuilder.setWelcomeMessage("");
        faqBuilder.SetConversationConfig(conversationBuilder.build());
        AIHelpSupport.ShowAllFAQSections(faqBuilder.build());

    }
    void singleSectionClick()
    {
        AIHelpSupport.ShowFAQSection("YOUR SECTIONID");
    }

    void singleFAQClick()
    {
       AIHelpSupport.ShowSingleFAQ("YOUR FAQID");
    }

    void operationClick()
    {
        
        ConversationConfig config = new ConversationConfig.Builder()
            .SetAlwaysShowHumanSupportButtonInBotPage(true)
            .SetConversationIntent(ConversationIntent.BOT_SUPPORT)
            .SetStoryNode("rate message")
            .build();
        OperationConfig.Builder opConfig = new OperationConfig.Builder();
        opConfig.SetConversationConfig(config);

        AIHelpSupport.ShowOperation(opConfig.build());
    }

    void updateUserInfoClick()
    {
        UserConfig config = new UserConfig.Builder()
            .SetUserId("123456789")
            .SetUserName("AIHelp")
            .SetUserTags("VIP1")
            .SetCustomData("{''}")
            .SetPushToken("pushToken")
            .SetPushPlatform(PushPlatform.JIGUANG)
            .SetSyncCrmInfo(true)
            .build();
        AIHelpSupport.UpdateUserInfo(config);
    }

    void updateSDKLanguageClick()
    {
        //AIHelpSupport.UpdateSDKLanguage("en");
        UserConfig config = new UserConfig.Builder()
            .SetUserId("123456789")
            .SetUserName("AIHelp")
            .SetUserTags("VIP1")
            .SetCustomData("")
            .build();
        AIHelpSupport.UpdateUserInfo(config);
    }

    void isHelpShowClick()
    {
        AIHelpSupport.IsAIHelpShowing();
    }

    void OnMessageCountArrivedCallback(int msgCount)
    {
        Console.Write("AIHelp you have " + msgCount + " unread messages");
    }

    void unreadMeassageClick()
    {
        AIHelpSupport.StartUnreadMessageCountPolling(OnMessageCountArrivedCallback);
    }

    void pushClick()
    {
        AIHelpSupport.SetPushTokenAndPlatform("TOKEN", PushPlatform.FIREBASE);
    }

    void netWorkCheckClick()
    {
        AIHelpSupport.SetNetworkCheckHostAddress("aihelp.net");
    }

    void upLoadLogClick()
    {
        AIHelpSupport.SetUploadLogPath("YOUR LOG PATH");
    }

    void enableLoggingClick()
    {
        AIHelpSupport.enableLogging(true);
    }
    
    void SDKVersionClick()
    {
        Console.Write("测试Test______SetOnOperationUnreadChangedCallback_________");
        
    }

    void showUrlClick()
    {
        AIHelpSupport.ShowUrl("https://www.baidu.com");
    }

    void runAccelerationClick()
    {
        AIHelpSupport.AdditionalSupportFor(PublishCountryOrRegion.CN);
    }

    public void OnSpecificFormSubmittedCallback()
    {
        Console.Write("______OnSpecificFormSubmittedCallback_________");
    }

    public void OnOpenCallBack()
    {
        Console.Write("AIHelp OnOpenCallBack");
    }

    public void OnCloseCallBack()
    {
        Console.Write("AIHelp OnCloseCallBack");
    }
}
