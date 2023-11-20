//
//  AIHelpUnity.m
//  AIHelpSupportSDK
//
//  Created by AIHelp on 2020/8/15.
//  Copyright © 2020 AIHelp. All rights reserved.
//
#import "AIHelpUnity.h"
#import <AIHelpSupportSDK/AIHelpSupportSDK.h>
#import <Foundation/Foundation.h>

#if defined(__cplusplus)
extern "C" {
#endif
    
    NSString* charToNSString (const char* string)
    {
        if (string){
            return [NSString stringWithUTF8String: string];
        }else{
            return [NSString stringWithUTF8String: ""];
        }
    }

    const char* NSStringToChar (NSString* string)
    {
        if (string){
            return string.UTF8String;
        }else{
            return @"".UTF8String;
        }
    }

    
    void unity_init (const char* apiKey, const char* domainName, const char* appId) {
        NSString *_apiKey = charToNSString(apiKey);
        NSString *_domainName = charToNSString(domainName);
        NSString *_appId = charToNSString(appId);
        [AIHelpSupportSDK initWithApiKey:_apiKey domainName:_domainName appId:_appId];
    }

    void unity_initLan (const char* apiKey, const char* domainName, const char* appId, const char* language) {
        NSString *_apiKey = charToNSString(apiKey);
        NSString *_domainName = charToNSString(domainName);
        NSString *_appId = charToNSString(appId);
        NSString *_language = charToNSString(language);
        [AIHelpSupportSDK initWithApiKey:_apiKey domainName:_domainName appId:_appId language:_language];
    }
    
    void unity_setOnInitializedCallback (AISupportInitCallBack callBack) {
        [AIHelpSupportSDK setOnInitializedCallback:callBack];
    }

    bool unity_show(const char* entranceId, const char* welcomeMessage) {
        AIHelpApiConfigBuilder *configBuilder = [[AIHelpApiConfigBuilder alloc] init];
        configBuilder.entranceId = charToNSString(entranceId);
        configBuilder.welcomeMessage = charToNSString(welcomeMessage);
        return [AIHelpSupportSDK showWithApiConfig:configBuilder.build];
    }

    void unity_showSingleFAQ(const char* faqId, int conversationMoment) {
        AIHelpFAQShowConversationMoment moment = AIHelpFAQShowConversationMomentNever;
        switch(conversationMoment){
            case 1:
                moment = AIHelpFAQShowConversationMomentNever;
                break;
            case 2:
                moment = AIHelpFAQShowConversationMomentAlways;
                break;
            case 3:
                moment = AIHelpFAQShowConversationMomentOnlyInAnswerPage;
                break;
            case 4:
                moment = AIHelpFAQShowConversationMomentAfterMarkingUnhelpful;
                break;
            default:
                break;    
        }
        [AIHelpSupportSDK showSingleFAQ:charToNSString(faqId) showConversationMoment:moment];
    }

    void unity_updateUserInfo (const char* userId, const char* userName, const char* serverId, const char* userTags, const char* customData, bool isSyncCrmInfo) {
        
        NSString *_userTags = charToNSString(userTags);
        NSString *_customData = charToNSString(customData);
        
        AIHelpUserConfigBuilder *userBuilder = [[AIHelpUserConfigBuilder alloc] init];
        userBuilder.userId = charToNSString(userId);
        userBuilder.userName = charToNSString(userName);
        userBuilder.serverId = charToNSString(serverId);
        userBuilder.isSyncCrmInfo = isSyncCrmInfo;
                
        if ([_userTags componentsSeparatedByString:@","]) {
            userBuilder.userTags = [_userTags componentsSeparatedByString:@","];
        }
        if (_customData && _customData.length) {
            NSData *jsonData = [_customData dataUsingEncoding:NSUTF8StringEncoding];
            NSError *error;
            NSDictionary *dic = [NSJSONSerialization JSONObjectWithData:jsonData options:NSJSONReadingMutableContainers error:&error];
            if (dic) {
                userBuilder.customData = dic;
            }
        }
        [AIHelpSupportSDK updateUserInfo:userBuilder.build];
    }

    void unity_resetUserInfo () {
        [AIHelpSupportSDK resetUserInfo];
    }

    void unity_setNetworkCheckHostAddress (const char*address,  AISupportPingCallBack callBack) {
        [AIHelpSupportSDK setNetworkCheckHostAddress:charToNSString(address) callback:callBack];
    }

    void unity_startUnreadMessageCountPolling (AISupportMessageCallBack callBack) {
        [AIHelpSupportSDK startUnreadMessageCountPolling:callBack];
    }

    void unity_updateSDKLanguage (const char* language) {
        [AIHelpSupportSDK updateSDKLanguage:charToNSString(language)];
    }

    void unity_setUploadLogPath (const char* path) {
        [AIHelpSupportSDK setUploadLogPath:charToNSString(path)];
    }
    
    void unity_setPushTokenAndPlatform (const char* pushToken, int pushPlatform) {
        AIHelpTokenPlatform ePlatform;
        switch (pushPlatform) {
            case 1:
                ePlatform = AIHelpTokenPlatformAPNS;
                break;
            case 2:
                ePlatform = AIHelpTokenPlatformFirebase;
                break;
            case 3:
                ePlatform = AIHelpTokenPlatformJpush;
                break;
            case 4:
                ePlatform = AIHelpTokenPlatformGeTui;
                break;
            case 6:
                ePlatform = AIHelpTokenPlatformHUAWEI;
                break;
            case 7:
                ePlatform = AIHelpTokenPlatformOneSignal;
                break;
            default:
                ePlatform = AIHelpTokenPlatformAPNS;
                break;
        }
        [AIHelpSupportSDK setPushToken:charToNSString(pushToken) pushPlatform:ePlatform];
    }

    const char* unity_getSDKVersion () {
        NSString *version = [AIHelpSupportSDK getSDKVersion];
        return strdup(NSStringToChar(version));
    }

    bool unity_isAIHelpShowing () {
        return [AIHelpSupportSDK isAIHelpShowing];
    }
    
    void unity_enableLogging (bool enable) {
        [AIHelpSupportSDK enableLogging:enable];
    }

    void unity_setSDKInterfaceOrientationMask (int interfaceOrientationMask) {
        [AIHelpSupportSDK setSDKInterfaceOrientationMask:interfaceOrientationMask];
    }
    
    void unity_setSDKAppearanceMode (int mode) {
        [AIHelpSupportSDK setSDKAppearanceMode:mode];
    }

    void unity_setSDKEdgeInsets (float top, float bottom, bool enable) {
        [AIHelpSupportSDK setSDKEdgeInsetsWithTop:top bottom:bottom enable:enable];
    }

    void unity_setSDKEdgeColor (float red, float green, float blue, float alpha) {
        [AIHelpSupportSDK setSDKEdgeColorWithRed:red green:green blue:blue alpha:alpha];
    }
    
    void unity_showUrl (const char* url) {
        [AIHelpSupportSDK showUrl:charToNSString(url)];
    }

    void unity_additionalSupportFor(int countryOrRegion) {
        AIHelpPublishCountryOrRegion tmpCountryOrRegion = AIHelpCN;
        if (countryOrRegion == 1) {
            tmpCountryOrRegion = AIHelpCN;
        }
        if (countryOrRegion == 2) {
            tmpCountryOrRegion = AIHelpIN;
        }
        [AIHelpSupportSDK additionalSupportFor:tmpCountryOrRegion];
    }

    void unity_setOnSpecificFormSubmittedCallback (AISupportIsSpecificFormCallBack callBack) {
        [AIHelpSupportSDK setOnSpecificFormSubmittedCallback:callBack];
    }

    void unity_setOnSessionOpenCallback(AISupportOpenSDKCallBack callBack) {
        [AIHelpSupportSDK setOnAIHelpSessionOpenCallback:callBack];
    }
    void unity_setOnSessionCloseCallback(AISupportCloseSDKCallBack callBack) {
        [AIHelpSupportSDK setOnAIHelpSessionCloseCallback:callBack];
    }

    void unity_setOnSpecificUrlClickedCallback(AISupportSpecificUrlClickedCallBack callBack)
    {
        [AIHelpSupportSDK setOnSpecificUrlClickedCallback:callBack];
    }

    void unity_close() {
        [AIHelpSupportSDK close];
    }
    
#if defined(__cplusplus)
}
#endif
