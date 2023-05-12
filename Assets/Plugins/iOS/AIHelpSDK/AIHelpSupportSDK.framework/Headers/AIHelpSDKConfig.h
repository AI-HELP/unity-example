//
//  AIHelpSDKConfig
//
//  Created by AIHelp.
//  Copyright © 2020 aihelp.net. All rights reserved.
//

#import <Foundation/Foundation.h>

typedef NS_ENUM(int,AIHelpTokenPlatform) {                    /* PushTokenPlatform enum*/
    AIHelpTokenPlatformAPNS               = 1,                // Apple APNS
    AIHelpTokenPlatformFirebase           = 2,                // firebase-FCM
    AIHelpTokenPlatformJpush              = 3,                // Jpush
    AIHelpTokenPlatformGeTui              = 4,                // GeTui
    AIHelpTokenPlatformHUAWEI             = 6,                // HUAWEI
    AIHelpTokenPlatformOneSignal          = 7,                // OneSignal
};

typedef NS_ENUM(int,AIHelpConversationIntent) {                    /* ConversationIntent enum */
    AIHelpConversationIntentBotSupport         = 1,                // ShowBot
    AIHelpConversationIntentHumanSupport       = 2,                // ShowHumanSupport
};

typedef NS_ENUM(int,AIHelpFAQShowConversationMoment) {             /* ConversationMoment enum, show ContactUs moment */
    AIHelpFAQShowConversationMomentNever            = 1,           // Never show
    AIHelpFAQShowConversationMomentAlways           = 2,           // Always show
    AIHelpFAQShowConversationMomentAfterMarkingUnhelpful = 3,      // Show after unhelpful with Faq detail
    AIHelpFAQShowConversationMomentOnlyInAnswerPage = 4,           // only in answer page show
};

typedef NS_ENUM(int,AIHelpPublishCountryOrRegion) {
    AIHelpCN = 1,
    AIHelpIN
};

typedef NS_ENUM(int, AIHelpFAQSupportEntrance) {
    AIHelpFAQSupportEntranceHomePage = 1,
    AIHelpFAQSupportEntranceQuestionList = 2,
    AIHelpFAQSupportEntranceAnswerPage = 3,
    AIHelpFAQSupportEntranceAfterMarkingUnhelpful = 4
};

#pragma mark - ECServiceUserConfig

@interface AIHelpUserConfig : NSObject
- (id) init NS_UNAVAILABLE;
@end

@interface AIHelpUserConfigBuilder : NSObject
@property (nonatomic, copy)NSString       *userId;        // default is unique deviceId
@property (nonatomic, copy)NSString       *userName;      // default is "anonymous"
@property (nonatomic, copy)NSString       *serverId;      // default is nil
@property (nonatomic, strong)NSArray        *userTags;      // If you assign this field with existing tags from aihelp admin dashboard, the tickets created by current user will take these tags by default.
@property (nonatomic, strong)NSDictionary   *customData;    // Set custom meta data you want to see in the aihelp admin dashboard.
@property (nonatomic, assign)BOOL           isSyncCrmInfo;  // If you set this to true, when you update current user's information, the sdk will sync user's information to you crm database.
- (AIHelpUserConfig *)build;
@end

#pragma mark - AIHelpApiConfig

@interface AIHelpApiConfig : NSObject
- (id) init NS_UNAVAILABLE;
@end

@interface AIHelpApiConfigBuilder : NSObject
@property (nonatomic, copy)NSString *entranceId;
@property (nonatomic, copy)NSString *welcomeMessage;
- (AIHelpApiConfig *)build;
@end
