
using System;
namespace AIHelp
{

    public enum PushPlatform
    {
        APNS = 1, FIREBASE = 2, JIGUANG = 3, GETUI = 4
    }

    public enum ConversationMoment
    {
        NEVER = 1001, ALWAYS = 1002, ONLY_IN_ANSWER_PAGE = 1003, AFTER_MARKING_UNHELPFUL = 1004
    }

    public enum ConversationIntent
    {
        BOT_SUPPORT = 1, HUMAN_SUPPORT = 2
    }

    public class ConversationConfig
    {

        private int conversationIntent;
        private bool alwaysShowHumanSupportButtonInBotPage;
        private string welcomeMessage;
        private string storyNode;

        public class Builder
        {
            private int conversationIntent = (int)ConversationIntent.BOT_SUPPORT;
            private bool alwaysShowHumanSupportButtonInBotPage;
            private string welcomeMessage;
            private string storyNode;

            public Builder SetConversationIntent(ConversationIntent conversationIntent)
            {
                this.conversationIntent = (int)conversationIntent;
                return this;
            }

            public Builder SetAlwaysShowHumanSupportButtonInBotPage(bool alwaysShowHumanSupportButtonInBotPage)
            {
                this.alwaysShowHumanSupportButtonInBotPage = alwaysShowHumanSupportButtonInBotPage;
                return this;
            }

            public Builder setWelcomeMessage(string welcomeMessage)
            {
                this.welcomeMessage = welcomeMessage;
                return this;
            }

            public Builder SetStoryNode(string storyNode)
            {
                this.storyNode = storyNode;
                return this;
            }

            public ConversationConfig build()
            {
                return new ConversationConfig(conversationIntent, alwaysShowHumanSupportButtonInBotPage, welcomeMessage, storyNode);
            }
        }

        public int GetConversationIntent()
        {
            return conversationIntent;
        }

        public bool IsAlwaysShowHumanSupportButtonInBotPage()
        {
            return alwaysShowHumanSupportButtonInBotPage;
        }

        public string GetWelcomeMessage()
        {
            return welcomeMessage;
        }

        public string GetStoryNode()
        {
            return storyNode;
        }

        private ConversationConfig(int conversationIntent, bool alwaysShowHumanSupportButtonInBotPage, string welcomeMessage, string storyNode)
        {
            this.conversationIntent = conversationIntent;
            this.alwaysShowHumanSupportButtonInBotPage = alwaysShowHumanSupportButtonInBotPage;
            this.welcomeMessage = welcomeMessage;
            this.storyNode = storyNode;
        }

    }



    public class FaqConfig
    {

        private int showConversationMoment;
        private ConversationConfig conversationConfig;

        public class Builder
        {

            private int showConversationMoment = (int)ConversationMoment.NEVER;
            private ConversationConfig conversationConfig = new ConversationConfig.Builder().build();

            public Builder SetShowConversationMoment(ConversationMoment showConversationMoment)
            {
                this.showConversationMoment = (int)showConversationMoment;
                return this;
            }

            public Builder SetConversationConfig(ConversationConfig conversationConfig)
            {
                if (conversationConfig != null)
                {
                    this.conversationConfig = conversationConfig;
                }
                return this;
            }

            public FaqConfig build()
            {
                return new FaqConfig(showConversationMoment, conversationConfig);
            }
        }

        public int GetShowConversationMoment()
        {
            return showConversationMoment;
        }

        public ConversationConfig GetConversationConfig()
        {
            return conversationConfig;
        }

        private FaqConfig(int showConversationMoment, ConversationConfig conversationConfig)
        {
            this.showConversationMoment = showConversationMoment;
            this.conversationConfig = conversationConfig;
        }

    }

    public class OperationConfig
    {

        private int selectIndex;
        private string conversationTitle;
        private ConversationConfig conversationConfig;

        public class Builder
        {

            private int selectIndex = 10000;
            private string conversationTitle = "HELP";
            private ConversationConfig conversationConfig = new ConversationConfig.Builder().build();

            public Builder setSelectIndex(int selectIndex)
            {
                if (selectIndex < 0) selectIndex = 10000;
                this.selectIndex = selectIndex;
                return this;
            }

            public Builder SetConversationTitle(string conversationTitle)
            {
                this.conversationTitle = conversationTitle;
                return this;
            }

            public Builder SetConversationConfig(ConversationConfig conversationConfig)
            {
                this.conversationConfig = conversationConfig;
                return this;
            }

            public OperationConfig build()
            {
                return new OperationConfig(selectIndex, conversationTitle, conversationConfig);
            }
        }


        public int GetSelectIndex()
        {
            return selectIndex;
        }

        public string GetConversationTitle()
        {
            return conversationTitle;
        }

        public ConversationConfig GetConversationConfig()
        {
            return conversationConfig;
        }

        private OperationConfig(int selectIndex, string conversationTitle, ConversationConfig conversationConfig)
        {
            this.selectIndex = selectIndex;
            this.conversationTitle = conversationTitle;
            this.conversationConfig = conversationConfig;
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
        private string pushToken;
        private PushPlatform pushPlatform;

        public class Builder
        {
            private string userId;
            private string userName = "anonymous";
            private string serverId = "-1";
            private string userTags = "";
            private string customData = "";
            private bool isSyncCrmInfo;
            private string pushToken = "";
            private PushPlatform pushPlatform = 0;

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

            public Builder SetPushToken(string pushToken)
            {
                this.pushToken = pushToken;
                return this;
            }

            public Builder setPushPlatform(PushPlatform pushPlatform)
            {
                this.pushPlatform = pushPlatform;
                return this;
            }

            public UserConfig build()
            {
                return new UserConfig(userId, userName, serverId, userTags, customData, isSyncCrmInfo, pushToken, pushPlatform);
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

        public string GetPushToken()
        {
            return pushToken;
        }

        public PushPlatform GetPushPlatform()
        {
            return pushPlatform;
        }

        private UserConfig(string userId, string userName, string serverId, string userTags, string customData, bool isSyncCrmInfo, string pushToken, PushPlatform pushPlatform)
        {
            this.userId = userId;
            this.userName = userName;
            this.serverId = serverId;
            this.userTags = userTags;
            this.customData = customData;
            this.isSyncCrmInfo = isSyncCrmInfo;
            this.pushToken = pushToken;
            this.pushPlatform = pushPlatform;
        }

    }
}