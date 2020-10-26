using System;
using UnityEngine;
using static AIHelpDefine;

public class AIHelpCore{

    private IAIHelpCore helpCore;
    private static AIHelpCore sInstance;

    private AIHelpCore() {

        #if UNITY_ANDROID
            if (Application.platform == RuntimePlatform.Android) {
                helpCore = new AIHelpAndroidCore();
            }
        #endif
        
        #if UNITY_IOS
            if (Application.platform == RuntimePlatform.IPhonePlayer) {
                helpCore = new AIHelpiOSCore();
            }
        #endif
    }

    public static AIHelpCore getInstance() {
        if(sInstance == null) {
            sInstance = new AIHelpCore();
        }
        return sInstance;
    }

    public void Init(string appKey, string domain, string appId, string language) {
        if (!IsHelpCorePrepared()) return;
        helpCore.Init(appId, domain, appId, language);
    }

    public void Init(string appKey, string domain, string appId) {
        Init(appKey, domain, appId, "");
    }

    public void SetOnAIHelpInitializedCallback(OnAIHelpInitializedCallback callback)
    {
        if (!IsHelpCorePrepared()) return;
        helpCore.SetOnAIHelpInitializedCallback(callback);
    }

    public void ShowConversation() {
        if (!IsHelpCorePrepared()) return;
        helpCore.ShowConversation();
    }

    public void ShowConversation(ConversationConfig config) {
        if (!IsHelpCorePrepared()) return;
        helpCore.ShowConversation(config);
    }

    public void ShowAllFAQSections() {
        if (!IsHelpCorePrepared()) return;
        helpCore.ShowAllFAQSections();
    }

    public void ShowAllFAQSections(FaqConfig config) {
        if (!IsHelpCorePrepared()) return;
        helpCore.ShowAllFAQSections(config);
    }

    public void ShowFAQSection(string sectionId) {
        if (!IsHelpCorePrepared()) return;
        helpCore.ShowFAQSection(sectionId);
    }

    public void ShowFAQSection(string sectionId, FaqConfig config) {
        if (!IsHelpCorePrepared()) return;
        helpCore.ShowFAQSection(sectionId, config);
    }

    public void ShowSingleFAQ(string faqId) {
        if (!IsHelpCorePrepared()) return;
        helpCore.ShowSingleFAQ(faqId);
    }

    public void ShowSingleFAQ(string faqId, FaqConfig config) {
        if (!IsHelpCorePrepared()) return;
        helpCore.ShowSingleFAQ(faqId, config);
    }

    public void ShowOperation() {
        if (!IsHelpCorePrepared()) return;
        helpCore.ShowOperation();
    }

    public void ShowOperation(OperationConfig config) {
        if (!IsHelpCorePrepared()) return;
        helpCore.ShowOperation(config);
    }

    public void UpdateUserInfo(UserConfig userConfig) {
        if (!IsHelpCorePrepared()) return;
        helpCore.UpdateUserInfo(userConfig);
    }

    public void ResetUserInfo() {
        if (!IsHelpCorePrepared()) return;
        helpCore.ResetUserInfo();
    }

    public void UpdateSDKLanguage(string language) {
        if (!IsHelpCorePrepared()) return;
        helpCore.UpdateSDKLanguage(language);
    }

    public void SetUploadLogPath(string path) {
        if (!IsHelpCorePrepared()) return;
        helpCore.SetUploadLogPath(path);
    }

    public void SetPushTokenAndPlatform(string pushToken, PushPlatform platform) { 
        if (!IsHelpCorePrepared()) return;
        helpCore.SetPushTokenAndPlatform(pushToken, platform);
    }

    public void SetNetworkCheckHostAddress(string address) {
        SetNetworkCheckHostAddress(address, null);
    }

    public void SetNetworkCheckHostAddress(string address, OnNetworkCheckResultCallback callback) {
        if (!IsHelpCorePrepared()) return;
        helpCore.SetNetworkCheckHostAddress(address, callback);
    }

    public void StartUnreadMessageCountPolling(OnMessageCountArrivedCallback callback) {
        if (!IsHelpCorePrepared()) return;
        helpCore.StartUnreadMessageCountPolling(callback);
    }

    public string GetSDKVersion() {
        if (!IsHelpCorePrepared()) return "";
        return helpCore.GetSDKVersion();
    }

    public bool IsAIHelpShowing() {
        if (!IsHelpCorePrepared()) return false;
        return helpCore.IsAIHelpShowing();
    }

    public void EnableLogging(bool enable) {
        if (!IsHelpCorePrepared()) return;
        helpCore.EnableLogging(enable);
    }

#if UNITY_IOS

    public void SetRequestedOrientation(int requestedOrientation)
    {
        if (!IsHelpCorePrepared()) return;
        helpCore.SetRequestedOrientation(requestedOrientation);
    }

    public void SetSDKAppearanceMode(int mode)
    {
        if (!IsHelpCorePrepared()) return;
        helpCore.SetSDKAppearanceMode(mode);
    }

    public void SetSDKEdgeInsets(float top, float bottom, bool enable)
    {
        if (!IsHelpCorePrepared()) return;
        helpCore.SetSDKEdgeInsets(top, bottom, enable);
    }

    public void SetSDKEdgeColor(float red, float green, float blue, float alpha)
    {
        if (!IsHelpCorePrepared()) return;
        helpCore.SetSDKEdgeColor(red, green, blue, alpha);
    }

#endif

    private bool IsHelpCorePrepared() {
        return helpCore != null;
    }

}

