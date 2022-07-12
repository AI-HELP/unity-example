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
    private string appKey = "";
    private string domain = "";
    private string appId = "";

    private void Awake()
    {
        AIHelpSupport.enableLogging(true);
        AIHelpSupport.Init(appKey, domain, appId,"en");
        AIHelpSupport.SetOnAIHelpInitializedCallback(OnAIHelpInitializedCallback);
        AIHelpSupport.SetOnSpecificFormSubmittedCallback(OnSpecificFormSubmittedCallback);
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

    void robotClick()
    {
        ConversationConfig.Builder conversationBuilder = new ConversationConfig.Builder();
        conversationBuilder.SetAlwaysShowHumanSupportButtonInBotPage(true);
        AIHelpSupport.ShowConversation(conversationBuilder.build());
    }

    void manualClick()
    {
        ConversationConfig.Builder conversationBuilder = new ConversationConfig.Builder();
        conversationBuilder.SetConversationIntent(ConversationIntent.HUMAN_SUPPORT);
        AIHelpSupport.ShowConversation(conversationBuilder.build());
    }

    void allSectionClick()
    {
        AIHelpSupport.ShowAllFAQSections();
    }
    void singleSectionClick()
    {
        AIHelpSupport.ShowFAQSection("SECTION ID");
    }

    void singleFAQClick()
    {
        AIHelpSupport.ShowSingleFAQ("FAQ ID");
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
        Console.Write("SDKVersionClick");
    }

    void showUrlClick()
    {
        AIHelpSupport.ShowUrl("https://www.aihelp.net");
    }

    void runAccelerationClick()
    {
        AIHelpSupport.AdditionalSupportFor(PublishCountryOrRegion.CN);
    }

    public void OnSpecificFormSubmittedCallback()
    {
        Console.Write("OnSpecificFormSubmittedCallback");
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
