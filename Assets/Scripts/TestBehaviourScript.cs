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
    private string appKey = "THIS IS YOUR APP KEY";
    private string domain = "THIS IS YOUR DOMAIN";
    private string appId = "THIS IS YOUR APP ID";


    private void Awake()
    {
        AIHelpSupport.enableLogging(true);
        AIHelpSupport.AdditionalSupportFor(PublishCountryOrRegion.CN);
        AIHelpSupport.Init(appKey, domain, appId);
        AIHelpSupport.SetOnAIHelpInitializedCallback(OnAIHelpInitializedCallback);
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
            {"Canvas/netWorkCheck", netWorkCheckClick},
            {"Canvas/uploadLog", upLoadLogClick},
            {"Canvas/enableLogging", enableLoggingClick},
            {"Canvas/SDKVersion", SDKVersionClick},
            {"Canvas/showUrl", showUrlClick},
            {"Canvas/runAcceleration", runAccelerationClick},
        };

        dic.All(keyval =>
        {
            GameObject robotObj = GameObject.Find(keyval.Key);

            Button robotBtn = (Button) robotObj.GetComponent<Button>();

            robotBtn.onClick.AddListener(() => { keyval.Value(); });

            return true;
        });
    }

    public void OnAIHelpInitializedCallback(bool isSuccess, string message)
    {
        printOnScreen("init isSuccess " + isSuccess + ", " + message);
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
            .SetUserId("123456789")
            .SetUserName("AIHelp")
            .SetUserTags("VIP1")
            .SetCustomData("{''}")
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
        AIHelpSupport.FetchUnreadMessageCount(OnFetchedMessageCountArrivedCallback);
    }

    void OnFetchedMessageCountArrivedCallback(int msgCount)
    {
        printOnScreen("You have " + msgCount + " unread messages obtained by fetching");
    }

    void OnMessageCountArrivedCallback(int msgCount)
    {
        printOnScreen("You have " + msgCount + " unread messages obtained by polling");
    }

    private void printOnScreen(string message)
    {
        Text notifyMessage = GameObject.Find("Canvas/notifyMessage").GetComponent<Text>();
        notifyMessage.text = message;
    }

    void unreadMeassageClick()
    {
        AIHelpSupport.StartUnreadMessageCountPolling(OnMessageCountArrivedCallback);
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

    public void OnSpecificFormSubmittedCallback()
    {
        printOnScreen("OnSpecificFormSubmittedCallback");
    }

    public void OnOpenCallBack()
    {
        printOnScreen("AIHelp OnOpenCallBack");
    }

    public void OnCloseCallBack()
    {
        printOnScreen("AIHelp OnCloseCallBack");
    }

    public void OnSpecialUrlClickedCallBack(string url)
    {
        printOnScreen("AIHelp OnSpecialUrlClickedCallBack: " + url);
    }
}