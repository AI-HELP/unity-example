
using System;
namespace AIHelp
{

    public enum PushPlatform
    {
        APNS = 1, FIREBASE = 2, JIGUANG = 3, GETUI = 4
    }

    public enum ConversationMoment
    {
        NEVER = 1, ALWAYS = 2, ONLY_IN_ANSWER_PAGE = 3, AFTER_MARKING_UNHELPFUL = 4
    }

    public enum PublishCountryOrRegion
    {
        CN = 1, IN = 2
    };

    public class ApiConfig
    {
        private string entranceId;
        private string welcomeMessage;

        public class Builder
        {
            private string entranceId;
            private string welcomeMessage;

            public Builder SetEntranceId(string entranceId)
            {
                this.entranceId = entranceId;
                return this;
            }

            public Builder SetWelcomeMessage(string welcomeMessage)
            {
                this.welcomeMessage = welcomeMessage;
                return this;
            }

            public ApiConfig build()
            {
                return new ApiConfig(entranceId, welcomeMessage);
            }

        }

        public string GetEntranceId()
        {
            return entranceId;
        }

        public string GetWelcomeMessage()
        {
            return welcomeMessage;
        }

        private ApiConfig(string entranceId, string welcomeMessage)
        {
            this.entranceId = entranceId;
            this.welcomeMessage = welcomeMessage;
        }

    }

    public class UserConfig
    {

        private string userId;
        private string userName;
        private string serverId = "-1";
        private string userTags;
        private string customData;
        private bool isSyncCrmInfo;

        public class Builder
        {
            private string userId;
            private string userName = "anonymous";
            private string serverId = "-1";
            private string userTags = "";
            private string customData = "";
            private bool isSyncCrmInfo;

            public Builder SetUserId(string userId)
            {
                this.userId = userId;
                return this;
            }

            public Builder SetUserName(string userName)
            {
                this.userName = userName;
                return this;
            }

            public Builder SetServerId(string serverId)
            {
                this.serverId = serverId;
                return this;
            }

            public Builder SetUserTags(string userTags)
            {
                this.userTags = userTags;
                return this;
            }

            public Builder SetCustomData(string customDataJsonstring)
            {
                this.customData = customDataJsonstring;
                return this;
            }

            public Builder SetSyncCrmInfo(bool syncCrmInfo)
            {
                isSyncCrmInfo = syncCrmInfo;
                return this;
            }

            public UserConfig build()
            {
                return new UserConfig(userId, userName, serverId, userTags, customData, isSyncCrmInfo);
            }

        }

        public bool GetWhetherSyncCrmInfo()
        {
            return isSyncCrmInfo;
        }

        public string GetUserId()
        {
            return userId;
        }

        public string GetUserName()
        {
            return userName;
        }

        public string GetServerId()
        {
            return serverId;
        }

        public string GetUserTags()
        {
            return userTags;
        }

        public string GetCustomData()
        {
            return customData;
        }

        private UserConfig(string userId, string userName, string serverId, string userTags, string customData, bool isSyncCrmInfo)
        {
            this.userId = userId;
            this.userName = userName;
            this.serverId = serverId;
            this.userTags = userTags;
            this.customData = customData;
            this.isSyncCrmInfo = isSyncCrmInfo;
        }

    }
}