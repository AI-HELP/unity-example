
// using System;
// namespace AIHelp
// {

//     public enum PushPlatform
//     {
//         APNS = 1, FIREBASE = 2, JIGUANG = 3, GETUI = 4, HUAWEI = 6, ONE_SIGNAL = 7
//     }

//     public enum ConversationMoment
//     {
//         NEVER = 1, ALWAYS = 2, ONLY_IN_ANSWER_PAGE = 3, AFTER_MARKING_UNHELPFUL = 4
//     }

//     public enum PublishCountryOrRegion
//     {
//         CN = 1, IN = 2
//     };


//     public class LoginConfig
//     {
//         public string UserId { get; }
//         public UserConfig UserConfig { get; }
//         public AIHelpDelegate.OnEnterpriseAuthCallback AuthCallback { get; }

//         public AIHelpDelegate.OnLoginResultCallback LoginCallback { get; }

//         private LoginConfig(Builder builder)
//         {
//             UserId = builder.UserId;
//             UserConfig = builder.UserConfig;
//             AuthCallback = builder.AuthCallback;
//             LoginCallback = builder.LoginCallback;
//         }

//         public class Builder
//         {
//             public string UserId { get; private set; }
//             public UserConfig UserConfig { get; private set; }
//             public AIHelpDelegate.OnEnterpriseAuthCallback AuthCallback { get; private set; }
//             public AIHelpDelegate.OnLoginResultCallback LoginCallback { get; private set; }

//             public Builder SetUserId(string userId)
//             {
//                 UserId = userId;
//                 return this;
//             }

//             public Builder SetUserConfig(UserConfig userConfig)
//             {
//                 UserConfig = userConfig;
//                 return this;
//             }

//             public Builder SetOnEnterpriseAuthCallback(AIHelpDelegate.OnEnterpriseAuthCallback authCallback)
//             {
//                 AuthCallback = authCallback;
//                 return this;
//             }

//             public Builder SetOnLoginResultCallback(AIHelpDelegate.OnLoginResultCallback loginCallback)
//             {
//                 LoginCallback = loginCallback;
//                 return this;
//             }

//             public LoginConfig Build()
//             {
//                 return new LoginConfig(this);
//             }
//         }
//     }


//     public class ApiConfig
//     {
//         public string EntranceId { get; }
//         public string WelcomeMessage { get; }

//         private ApiConfig(Builder builder)
//         {
//             EntranceId = builder.EntranceId;
//             WelcomeMessage = builder.WelcomeMessage;
//         }

//         public class Builder
//         {
//             public string EntranceId { get; private set; }
//             public string WelcomeMessage { get; private set; }

//             public Builder SetEntranceId(string entranceId)
//             {
//                 EntranceId = entranceId;
//                 return this;
//             }

//             public Builder SetWelcomeMessage(string welcomeMessage)
//             {
//                 WelcomeMessage = welcomeMessage;
//                 return this;
//             }

//             public ApiConfig Build()
//             {
//                 return new ApiConfig(this);
//             }
//         }
//     }

//     public class UserConfig
//     {
//         public string UserName { get; }
//         public string ServerId { get; }
//         public string UserTags { get; }
//         public string CustomData { get; }

//         private UserConfig(Builder builder)
//         {
//             UserName = builder.UserName;
//             ServerId = builder.ServerId;
//             UserTags = builder.UserTags;
//             CustomData = builder.CustomData;
//         }

//         public class Builder
//         {
//             public string UserName { get; private set; } = "anonymous";
//             public string ServerId { get; private set; } = "-1";
//             public string UserTags { get; private set; } = string.Empty;
//             public string CustomData { get; private set; } = string.Empty;

//             public Builder SetUserName(string userName)
//             {
//                 UserName = userName;
//                 return this;
//             }

//             public Builder SetServerId(string serverId)
//             {
//                 ServerId = serverId;
//                 return this;
//             }

//             public Builder SetUserTags(string userTags)
//             {
//                 UserTags = userTags;
//                 return this;
//             }

//             public Builder SetCustomData(string customDataJsonstring)
//             {
//                 CustomData = customDataJsonstring;
//                 return this;
//             }

//             public UserConfig Build()
//             {
//                 return new UserConfig(this);
//             }
//         }
//     }

// }