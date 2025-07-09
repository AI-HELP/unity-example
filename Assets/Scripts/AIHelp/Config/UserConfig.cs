
using System;
namespace AIHelp
{
    public class UserConfig
    {
        public string UserName { get; }
        public string ServerId { get; }
        public string UserTags { get; }
        public string CustomData { get; }

        private UserConfig(Builder builder)
        {
            UserName = builder.UserName;
            ServerId = builder.ServerId;
            UserTags = builder.UserTags;
            CustomData = builder.CustomData;
        }

        public class Builder
        {
            public string UserName { get; private set; } = "anonymous";
            public string ServerId { get; private set; } = "-1";
            public string UserTags { get; private set; } = string.Empty;
            public string CustomData { get; private set; } = string.Empty;

            public Builder SetUserName(string userName)
            {
                UserName = userName;
                return this;
            }

            public Builder SetServerId(string serverId)
            {
                ServerId = serverId;
                return this;
            }

            public Builder SetUserTags(string userTags)
            {
                UserTags = userTags;
                return this;
            }

            public Builder SetCustomData(string customDataJsonstring)
            {
                CustomData = customDataJsonstring;
                return this;
            }

            public UserConfig Build()
            {
                return new UserConfig(this);
            }
        }
    }
}