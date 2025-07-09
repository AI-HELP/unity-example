using System.Threading;
using System.Threading.Tasks;
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
    private string domain = "release.aihelp.net";
    // private string appId = "TryElva_platform_79453658-02b7-42fb-9384-8e8712539777";
    private string appId = "";

    private void Awake()
    {
        UnityMainThreadDispatcher.Initialize();
        AIHelpSupport.enableLogging(true);
        // AIHelpSupport.AdditionalSupportFor(PublishCountryOrRegion.CN);

#if UNITY_ANDROID
            if (Application.platform == RuntimePlatform.Android) {
                appId = "TryElva_platform_79453658-02b7-42fb-9384-8e8712539777";
            }
#endif
#if UNITY_IOS
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                appId = "TryElva_platform_09ebf7fa-8d45-4843-bd59-cfda3d8a8dc0";
            }
#endif
        AIHelpSupport.Initialize(domain, appId);
        RegisterAIHelpEventListener();
    }

    private void Start()
    {
        Dictionary<string, Action> dic = new Dictionary<string, Action>()
        {
            {"Canvas/customer", customerServiceClick},
            {"Canvas/helpcenter", helpCenterClick},
            {"Canvas/custom", customEntranceClick},
            {"Canvas/singleFAQ", singleFAQClick},
            {"Canvas/updateUserInfo", updateUserInfoClick},
            {"Canvas/updateSDKLanguage", updateSDKLanguageClick},
            {"Canvas/isHelpShow", isHelpShowClick},
            {"Canvas/unreadMeassage", unreadMeassageClick},
            {"Canvas/uploadLog", upLoadLogClick},
            {"Canvas/enableLogging", enableLoggingClick},
            {"Canvas/SDKVersion", SDKVersionClick},
            {"Canvas/showUrl", showUrlClick},
            {"Canvas/runAcceleration", runAccelerationClick},
        };

        dic.All(keyval =>
        {
            GameObject robotObj = GameObject.Find(keyval.Key);

            Button robotBtn = (Button)robotObj.GetComponent<Button>();

            robotBtn.onClick.AddListener(() => { keyval.Value(); });

            return true;
        });
    }

    void customerServiceClick()
    {
        AIHelpSupport.Show("test004");
    }

    void helpCenterClick()
    {
        AIHelpSupport.Show("test001");
    }

    void customEntranceClick()
    {
        AIHelpSupport.Show("THIS IS YOUR ENTRANCE ID");
    }

    void singleFAQClick()
    {
        AIHelpSupport.ShowSingleFAQ("THIS IS YOUR FAQ ID", ConversationMoment.AFTER_MARKING_UNHELPFUL);
    }

    void updateUserInfoClick()
    {
        UserConfig config = new UserConfig.Builder()
            .SetUserName("AIHelp")
            .SetUserTags("VIP1")
            .SetCustomData("{}")
            .Build();

        LoginConfig loginConfig = new LoginConfig.Builder()
            .SetUserId(GetRandomNumber())
            // .SetUserConfig(config)
            .Build();

        AIHelpSupport.Login(loginConfig);
    }

    void updateSDKLanguageClick()
    {
        AIHelpSupport.UpdateSDKLanguage("en");
    }

    void isHelpShowClick()
    {
        AIHelpSupport.IsAIHelpShowing();
        AIHelpSupport.FetchUnreadMessageCount();
    }

    private void printOnScreen(string message)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Text notifyMessage = GameObject.Find("Canvas/notifyMessage").GetComponent<Text>();
            notifyMessage.text = message;
        });
        Debug.LogError(message);
    }

    void unreadMeassageClick()
    {
        AIHelpSupport.FetchUnreadMessageCount();
        AIHelpSupport.FetchUnreadTaskCount();
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
        printOnScreen(AIHelpSupport.GetSDKVersion());
    }

    void showUrlClick()
    {
        AIHelpSupport.ShowUrl("https://www.aihelp.net");
    }

    void runAccelerationClick()
    {
        AIHelpSupport.AdditionalSupportFor(PublishCountryOrRegion.CN);
    }

    void RegisterAIHelpEventListener() {
        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.Initialization, (jsonEventData, ack) =>
        {
            printOnScreen("Unity Initialization " + jsonEventData);
        });

        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.UserLogin, (jsonEventData, ack) =>
        {
            printOnScreen("Unity Login " + jsonEventData);
        });

        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.EnterpriseAuth, async (jsonEventData, ack) =>
        {
            printOnScreen("Unity EnterpriseAuth " + jsonEventData);
            await Task.Delay(2000);
            ack("{\"token\":\"this is your async token\"}");
        });

        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.SessionOpen, (jsonEventData, ack) =>
        {
            printOnScreen("Unity SessionOpen " + jsonEventData);
        });

        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.SessionClose, (jsonEventData, ack) =>
        {
            printOnScreen("Unity SessionClose " + jsonEventData);
            AIHelpSupport.UnregisterAsyncEventListener(AIHelp.EventType.SessionOpen);
            AIHelpSupport.UnregisterAsyncEventListener(AIHelp.EventType.SessionClose);
        });

        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.MessageArrival, (jsonEventData, ack) =>
        {
            printOnScreen("Unity MessageArrival " + jsonEventData);
        });

        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.LogUpload, (jsonEventData, ack) =>
        {
            printOnScreen("Unity LogUpload " + jsonEventData);
            ack("{\"content\":\"this is your synchronous log\"}");
        });

        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.UrlClick, (jsonEventData, ack) =>
        {
            printOnScreen("Unity UrlClick " + jsonEventData);
        });

        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.UnreadTaskCount, (jsonEventData, ack) =>
        {
            printOnScreen("Unity UnreadTaskCount " + jsonEventData);
        });

        AIHelpSupport.RegisterAsyncEventListener(AIHelp.EventType.ConversationStart, (jsonEventData, ack) =>
        {
            printOnScreen("Unity ConversationStart " + jsonEventData);
        });
    }

    string GetRandomNumber() {
        System.Random random = new System.Random();
        return random.NextDouble() + "";
    }

}