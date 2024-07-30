//
//  AIHelpSupportSDK
//
//  Created by AIHelp.
//  Copyright © 2020 aihelp.net. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <AIHelpSupportSDK/AIHelpSDKConfig.h>
#import <UIKit/UIKit.h>

@interface AIHelpSupportSDK : NSObject

/**
 * Initialize AIHelp sdk
 *
 * When initializing AIHelp you must pass these three tokens. You initialize AIHelp by adding the following lines in the implementation file for your app delegate, ideally at the top of application:didFinishLaunchingWithOptions
 * @param domainName This is your domain name without any http:// or forward slashes
 * @param appId  This is the unique ID assigned to your app
 */
+ (void)initializeWithDomainName:(NSString *)domainName appId:(NSString *)appId;

/**
 * Initialize AIHelp sdk
 *
 * When initializing AIHelp you must pass these three tokens. You initialize AIHelp by adding the following lines in the implementation file for your app delegate, ideally at the top of application:didFinishLaunchingWithOptions
 * @param domainName This is your domain name without any http:// or forward slashes
 * @param appId  This is the unique ID assigned to your app
 * @param language  This is your expected init language
 */
+ (void)initializeWithDomainName:(NSString *)domainName appId:(NSString *)appId language:(NSString *)language;

/**
 * Logs in a user using their user ID.
 * * @param userId the unique identifier of the user to log in
 */
+ (void)loginWithUserId:(NSString *)userId;

/**
 * Logs in a user using a LoginConfig object.
 * <p>
 * Checkout {@link LoginConfig} for more information.
 *
 * @param config the configuration object containing login details.
 * @see LoginConfig
 */
+ (void)login:(AIHelpLoginConfig *)config;

/**
 * Show the AIHelp conversation screen.
 *
 * If you want to custom your welcome message, please check next method for more information.
 */
+ (BOOL)showWithEntranceId:(NSString *)entranceId;

/**
 Show the AIHelp conversation screen.
 */
+ (BOOL)showWithApiConfig:(AIHelpApiConfig *)apiConfig;

+ (void)showSingleFAQ:(NSString *)faqId showConversationMoment:(AIHelpFAQShowConversationMoment)showConversationMoment;

/**
 * Update a user's profile via UserConfig.
 *
 * Please check ECServiceUserConfigfor more detail information.
 * @param config configs which contains all information about a user.
 */
+ (void)updateUserInfo:(AIHelpUserConfig *)config;

/**
 * Clear the values set to a user, reset the userId to deviceId, userName to 'anonymous'.
 */
+ (void)resetUserInfo;

/**
 * Change the SDK language. By default, the device's prefered language is used.
 *
 * The call will fail in the following cases :
 * 1. If a AIHelp session is already active at the time of invocation
 * 2. Language code is incorrect
 * 3. Corresponding localization file is not found
 * @param sdkLanguage the string representing the language code. For example, use 'fr' for French.
 */
+ (void)updateSDKLanguage:(NSString*)sdkLanguage;

/**
 * Set log path for uploading.
 *
 * In order to serve your customers well, you can upload customer-related-logs when tickets are created or
 * specific forms are submitted.
 * @param path the absolute path of log, which will be uploaded when needed
 */
+ (void)setUploadLogPath:(NSString*)path;

/**
 * set the pushToken and platform to enable push notifications.
 *
 * To enable push notifications in the Helpshift iOS SDK, set the Push Notifications’ deviceToken using this method inside your application:didRegisterForRemoteNotificationsWithDeviceToken application delegate.
 * NOTE: You must get the specific push sdk in your App BEFORE this invocation.
 *
 * @param pushToken    the pushToken received from the push notification servers.
 * @param pushPlatform the specific push platform, please check ElvaTokenPlatform  for more information.
 */
+ (void)setPushToken:(NSString*)pushToken pushPlatform:(AIHelpTokenPlatform)pushPlatform;

/**
 * Get current AIHelp SDK version
 * @return AIHelp SDK version
 */
+ (NSString*)getSDKVersion;

/**
 * Whether AIHelp session is visible to users
 * @return whether sdk is active
 */
+ (BOOL)isAIHelpShowing;

/**
 * Whether to print logs.
 *
 * It only works in Debug mode
 * @param enable YES/NO
 */
+ (void)enableLogging:(BOOL)enable;

/**
 * The preferred screen orientation sdk would like to run in.
 *
 * NOTE: The SDK direction must be included in the program direction Settings, otherwise the setting will fail
 * @param interfaceOrientationMask please refer to the UIInterfaceOrientationMask API
 */
+ (void)setSDKInterfaceOrientationMask:(UIInterfaceOrientationMask)interfaceOrientationMask;
/**
 * Unregisters an event listener for a specific event type.
 *
 * @param eventType The type of event to stop listening for.
 */
+ (void)registerAsyncListener:(AISupportAsyncEventListener)asyncEventListener
                    eventType:(AIHelpEventType)eventType;

+ (void)unregisterAsyncListenerWithEvent:(AIHelpEventType)eventType;

/**
 * Fetch unread message count proactively
 */
+ (void)fetchUnreadMessageCount;

+ (void)fetchUnreadTaskCount;

/**
 * Set the SDK display mode
 *
 * Default following system
 * @param mode
            0: follow the system
            1: light mode
            2: dark mode
 */
+ (void)setSDKAppearanceMode:(int)mode;

/**
 * AIHelp provide additional support for some country or regions.
 *
 * @param countryOrRegion ISO country code, please check https://www.iso.org/obp/ui/#search to learn more.
 */
+ (void)additionalSupportFor:(AIHelpPublishCountryOrRegion)countryOrRegion;

+ (void)showUrl:(NSString *)url;

+ (void)setKeyWindow:(UIWindow *)keyWin;

+ (void)setSDKEdgeInsetsWithTop:(float)top bottom:(float)bottom enable:(BOOL)enable;
+ (void)setSDKEdgeColorWithRed:(float)red green:(float)green blue:(float)blue alpha:(float)alpha;

+ (void)close;

+ (void)logout;

@end
