using UnityEngine;
using System;

namespace AIHelp
{
#if UNITY_ANDROID

    public class AIHelpAndroidCore : IAIHelpCore
    {

        private AndroidJavaClass javaSupport;
        private AndroidJavaObject currentActivity;

        public AIHelpAndroidCore()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            javaSupport = new AndroidJavaClass("net.aihelp.init.AIHelpSupport");
            currentActivity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        }

        public void Initialize(string domain, string appId, string language)
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("initialize", currentActivity, domain, appId, language);
            }
        }

        private AndroidJavaObject getLoginConfig(LoginConfig config)
        {
            AndroidJavaObject builder = new AndroidJavaObject("net.aihelp.config.LoginConfig$Builder");
            builder.Call<AndroidJavaObject>("setUserId", config.UserId);
            builder.Call<AndroidJavaObject>("setUserConfig", getUserConfig(config.UserConfig));
            builder.Call<AndroidJavaObject>("setEnterpriseAuth", config.EnterpriseAuth);
            return builder.Call<AndroidJavaObject>("build");
        }

        private AndroidJavaObject getApiConfig(ApiConfig config)
        {
            AndroidJavaObject builder = new AndroidJavaObject("net.aihelp.config.ApiConfig$Builder");
            return builder.Call<AndroidJavaObject>("build", config.EntranceId, config.WelcomeMessage);
        }

        private AndroidJavaObject getUserConfig(UserConfig config)
        {
            if(config == null) return null;
            AndroidJavaObject builder = new AndroidJavaObject("net.aihelp.config.UserConfig$Builder");
            builder.Call<AndroidJavaObject>("setUserName", config.UserName);
            builder.Call<AndroidJavaObject>("setServerId", config.ServerId);
            builder.Call<AndroidJavaObject>("setUserTags", config.UserTags);
            builder.Call<AndroidJavaObject>("setCustomData", config.CustomData);
            return builder.Call<AndroidJavaObject>("build");
        }

        private AndroidJavaObject getPublishCountryOrRegion(PublishCountryOrRegion countryOrRegion)
        {
            AndroidJavaClass clz = new AndroidJavaClass("net.aihelp.config.enums.PublishCountryOrRegion");
            return clz.CallStatic<AndroidJavaObject>("fromValue", (int)countryOrRegion);
        }

        private AndroidJavaObject getPushPlatform(PushPlatform platform)
        {
            AndroidJavaClass clz = new AndroidJavaClass("net.aihelp.config.enums.PushPlatform");
            return clz.CallStatic<AndroidJavaObject>("fromValue", (int)platform);
        }

        private AndroidJavaObject getShowConversationMoment(ConversationMoment conversationMoment)
        {
            AndroidJavaClass clz = new AndroidJavaClass("net.aihelp.config.enums.ShowConversationMoment");
            return clz.CallStatic<AndroidJavaObject>("fromValue", (int)conversationMoment);
        }

        private AndroidJavaObject getEventType(EventType eventType)
        {
            AndroidJavaClass clz = new AndroidJavaClass("net.aihelp.event.EventType");
            return clz.CallStatic<AndroidJavaObject>("fromValue", (int)eventType);
        }

        public bool Show(string entranceId)
        {
            if (javaSupport != null && currentActivity != null)
            {
                return javaSupport.CallStatic<bool>("show", entranceId);
            }
            return false;
        }

        public bool Show(ApiConfig apiConfig)
        {
            if (javaSupport != null && currentActivity != null)
            {
                return javaSupport.CallStatic<bool>("show", getApiConfig(apiConfig));
            }
            return false;
        }

        public void ShowSingleFAQ(string faqId, ConversationMoment conversationMoment) 
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("showSingleFAQ", faqId, getShowConversationMoment(conversationMoment));
            }
        }

        public void Login(LoginConfig loginConfig) 
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("login", getLoginConfig(loginConfig));
            } 
        }

        public void Logout() 
        {
            if (javaSupport != null && currentActivity != null)
            {
                javaSupport.CallStatic("logout");
            } 
        }

        public void UpdateUserInfo(UserConfig config)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("updateUserInfo", getUserConfig(config));
            }
        }

        public void ResetUserInfo()
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("resetUserInfo");
            }
        }

        public void UpdateSDKLanguage(string language)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("updateSDKLanguage", language);
            }
        }

        public void SetUploadLogPath(string logPath)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("setUploadLogPath", logPath);
            }
        }

        public void SetPushTokenAndPlatform(string logPath, PushPlatform pushPlatform)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("setPushTokenAndPlatform", logPath, getPushPlatform(pushPlatform));
            }
        }

        public void RegisterAsyncEventListener(AIHelp.EventType eventType, AIHelpDelegate.AsyncEventListener listener)
        {
            javaSupport.CallStatic("registerAsyncEventListener", getEventType(eventType), listener == null ? null : new AsyncEventListenerProxy(listener));
        }

        public void UnregisterAsyncEventListener(AIHelp.EventType eventType)
        {
            javaSupport.CallStatic("unregisterAsyncEventListener", getEventType(eventType));
        }

        public void FetchUnreadMessageCount()
        {
            javaSupport.CallStatic("fetchUnreadMessageCount");
        }

        public void FetchUnreadTaskCount()
        {
            javaSupport.CallStatic("fetchUnreadTaskCount");
        }

        public void ShowUrl(string url)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("showUrl", url);
            }
        }

        public void AdditionalSupportFor(PublishCountryOrRegion countryOrRegion)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("additionalSupportFor", getPublishCountryOrRegion(countryOrRegion));
            }
        }

        public string GetSDKVersion()
        {
            if (javaSupport != null)
            {
                return javaSupport.CallStatic<string>("getSDKVersion");
            }
            return "";
        }

        public bool IsAIHelpShowing()
        {
            if (javaSupport != null)
            {
                return javaSupport.CallStatic<bool>("isAIHelpShowing");
            }
            return false;
        }

        public void EnableLogging(bool isOpen)
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("enableLogging", isOpen);
            }
        }

        public void Close() 
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("close");
            }
        }

        public void Uninstall()
        {
            if (javaSupport != null)
            {
                javaSupport.CallStatic("uninstall");
            }
        }
    }
#endif
}