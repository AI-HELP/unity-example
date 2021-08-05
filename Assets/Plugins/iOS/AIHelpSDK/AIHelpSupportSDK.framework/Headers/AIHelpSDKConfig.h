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
@property(nonatomic,copy)NSString *pushToken;
@property(nonatomic,assign)AIHelpTokenPlatform pushPlatform;
- (AIHelpUserConfig *)build;
@end


#pragma mark - ECServiceConversationConfig

@interface AIHelpConversationConfig : NSObject
- (id) init NS_UNAVAILABLE;
@end

@interface AIHelpConversationConfigBuilder : NSObject
@property (nonatomic, assign)AIHelpConversationIntent conversationIntent;    // show elva bot page or show conversation page
@property (nonatomic, assign)BOOL alwaysShowHumanSupportButtonInBotPage;         // default is NO.if you set ture,user can always see the contact us button
@property (nonatomic, copy)NSString *welcomeMessage;           // default is http://aihelp.net/dashboard setting. you can show different welcome msg by different users with this param, It has a higher priority.
@property (nonatomic, copy)NSString *storyNode;                   // set specific story node for specific scene. With this api call, you can show different stories in different scenes. the story node's User Say content you configured in aihelp admin dashboard.(http://aihelp.net/dashboard)
- (AIHelpConversationConfig *)build;
@end


#pragma mark - ECServiceFAQConfig

@interface AIHelpFAQConfig : NSObject
- (id) init NS_UNAVAILABLE;
@end

@interface AIHelpFAQConfigBuilder : NSObject
@property (nonatomic, assign)AIHelpFAQShowConversationMoment showConversationMoment;      // see enum -> ElvaFAQShowConversationMoment
@property (nonatomic, strong)AIHelpConversationConfig *conversationConfig;           // see config -> ECServiceConversationConfig
- (AIHelpFAQConfig *)build;
@end


#pragma mark - ECServiceOperationConfig

@interface AIHelpOperationConfig : NSObject
- (id) init NS_UNAVAILABLE;
@end

@interface AIHelpOperationConfigBuilder : NSObject
@property (nonatomic, assign)int selectIndex;                                  // default is elva tab.  If you set a negative index, or index larger than the total tab counts, the selected tab will still be elva.
@property (nonatomic, copy)NSString *conversationTitle;                        // default is "HELP", you can change the operation conversation bot title
@property (nonatomic, strong)AIHelpConversationConfig *conversationConfig;            // see config -> ECServiceConversationConfig
- (AIHelpOperationConfig *)build;
@end
