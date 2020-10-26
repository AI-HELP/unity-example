using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;
using UnityEngine.UI;

public class TestBehaviourScript : MonoBehaviour
{
    private string appKey = "THIS IS YOUR APP KEY";
    private string domain = "THIS IS YOUR DOMAIN";
    private string appId = "THIS IS YOUR APP ID"; 

    private void Awake()
    {
        AIHelpSupport.Init(appKey, domain, appId);
        AIHelpSupport.SetOnAIHelpInitializedCallback(OnAIHelpInitializedCallback);
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
            { "Canvas/SDKVersion",SDKVersionClick }
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

    void robotClick()
    {
        ConversationConfig config = new ConversationConfig.Builder()
            .setWelcomeMessage("You can configure special welcome message for your end users at here.")
            .SetAlwaysShowHumanSupportButtonInBotPage(true)
            .SetConversationIntent(ConversationIntent.BOT_SUPPORT)
            .SetStoryNode("")
            .build();

        AIHelpSupport.ShowConversation(config);
    }

    void manualClick()
    {
        ConversationConfig config = new ConversationConfig.Builder()
            .setWelcomeMessage("You can configure special welcome message for your end users at here.")
            .SetAlwaysShowHumanSupportButtonInBotPage(false)
            .SetConversationIntent(ConversationIntent.HUMAN_SUPPORT)
            .SetStoryNode("")
            .build();
        AIHelpSupport.ShowConversation();
    }

    void allSectionClick()
    {
        AIHelpSupport.ShowAllFAQSections();

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
        AIHelpSupport.ShowOperation();
    }

    void updateUserInfoClick()
    {
        UserConfig config = new UserConfig.Builder()
            .SetUserId("123456789")
            .SetUserName("AIHelp")
            .SetUserTags("VIP1")
            .SetCustomData("")
            .build();
        AIHelpSupport.UpdateUserInfo(config);
    }

    void updateSDKLanguageClick()
    {
        AIHelpSupport.UpdateSDKLanguage("en");
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
        AIHelpSupport.GetSDKVersion();
    }

}
