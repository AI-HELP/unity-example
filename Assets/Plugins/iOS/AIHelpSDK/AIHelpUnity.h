//
//  AIHelpUnity.h
//  AIHelpSupportSDK
//
//  Created by AIHelp on 2020/8/15.
//  Copyright © 2020 AIHelp. All rights reserved.
//

#import <AIHelpSupportSDK/AIHelpSupportSDK.h>

#ifndef AIHelpUnity_h
#define AIHelpUnity_h

extern "C" void unity_init (const char* apiKey, const char* domainName, const char* appId);
extern "C" void unity_initLan (const char* apiKey, const char* domainName, const char* appId, const char* language);
extern "C" void unity_setOnInitializedCallback (AISupportInitCallBack callBack);

extern "C" void unity_showConversation ();
extern "C" void unity_showConversationConfig (int conversationIntent, bool alwaysShowHumanSupportButtonInBotPage, const char* welcomeMessage, const char* storyNode);

extern "C" void unity_showAllFAQSections ();
extern "C" void unity_showAllFAQSectionsConfig (int conversationMoment, int conversationIntent, bool alwaysShowHumanSupportButtonInBotPage, const char* storyNode, const char* welcomeMessage);
extern "C" void unity_showFAQSection (const char* sectionId);
extern "C" void unity_showFAQSectionConfig (const char* sectionId, int conversationMoment, int conversationIntent, bool alwaysShowHumanSupportButtonInBotPage, const char* storyNode, const char* welcomeMessage);
extern "C" void unity_showSingleFAQ (const char* faqId);
extern "C" void unity_showSingleFAQConfig (const char* faqId, int conversationMoment, int conversationIntent, bool alwaysShowHumanSupportButtonInBotPage, const char* storyNode, const char* welcomeMessage);

extern "C" void unity_showOperation ();
extern "C" void unity_showOperationConfig (int selectIndex, const char* conversationTitle, int conversationIntent, bool alwaysShowHumanSupportButtonInBotPage, const char* storyNode, const char* welcomeMessage);

extern "C" void unity_updateUserInfo (const char* userId, const char* userName, const char* serverId, const char* userTags, const char* customData, bool isSyncCrmInfo, const char* pushToken, int pushPlatform);
extern "C" void unity_resetUserInfo ();

extern "C" void unity_setNetworkCheckHostAddress (const char*address,  AISupportPingCallBack callBack);
extern "C" void unity_startUnreadMessageCountPolling (AISupportMessageCallBack callBack);
extern "C" void unity_updateSDKLanguage (const char* language);
extern "C" void unity_setUploadLogPath (const char* path);
extern "C" void unity_setPushTokenAndPlatform (const char* pushToken, int pushPlatform);
extern "C" const char* unity_getSDKVersion ();
extern "C" bool unity_isAIHelpShowing ();
extern "C" void unity_enableLogging (bool enable);
extern "C" void unity_setSDKInterfaceOrientationMask (int interfaceOrientationMask);
extern "C" void unity_setSDKAppearanceMode (int mode);
extern "C" void unity_setSDKEdgeInsets (float top, float bottom, bool enable);
extern "C" void unity_setSDKEdgeColor (float red, float green, float blue, float alpha);
extern "C" void unity_showUrl (const char* url);
extern "C" void unity_uninstall ();
extern "C" void unity_additionalSupportFor(int countryOrRegion);
extern "C" void unity_setOnSpecificFormSubmittedCallback (AISupportIsSpecificFormCallBack callBack);
extern "C" void unity_setOnSessionOpenCallback(AISupportOpenSDKCallBack callBack);
extern "C" void unity_setOnSessionCloseCallback(AISupportCloseSDKCallBack callBack);
extern "C" void unity_setOnOperationUnreadChangedCallback(AISupportOperationUnReadCallBack callBack);
extern "C" void unity_setOnSpecificUrlClickedCallback(AISupportSpecificUrlClickedCallBack callBack);

#endif
