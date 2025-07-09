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

extern "C" void unity_initiailize (const char* domainName, const char* appId, const char* language);
extern "C" void unity_registerAsyncEventListener (int eventType, AISupportAsyncEventListener listener);
extern "C" void unity_unregisterAsyncEventListener (int eventType);

extern "C" bool unity_show(const char* entranceId, const char* welcomeMessage);
extern "C" void unity_showSingleFAQ(const char* faqId, int conversationMoment);

extern "C" void unity_login (const char* userId, const char* userName, const char* serverId, const char* userTags, const char* customData);
extern "C" void unity_logout ();
extern "C" void unity_updateUserInfo (const char* userName, const char* serverId, const char* userTags, const char* customData);
extern "C" void unity_resetUserInfo ();

extern "C" void unity_fetchUnreadMessageCount ();
extern "C" void unity_fetchUnreadTaskCount ();

extern "C" void unity_updateSDKLanguage (const char* language);
extern "C" void unity_setUploadLogPath (const char* path);
extern "C" void unity_setPushTokenAndPlatform (const char* pushToken, int pushPlatform);
extern "C" const char* unity_getSDKVersion ();
extern "C" bool unity_isAIHelpShowing ();
extern "C" void unity_enableLogging (bool enable);
extern "C" void unity_showUrl (const char* url);
extern "C" void unity_additionalSupportFor(int countryOrRegion);

extern "C" void unity_close();

#endif
