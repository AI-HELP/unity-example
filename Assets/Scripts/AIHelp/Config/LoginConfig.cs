
using System;
namespace AIHelp
{
    public class LoginConfig
    {
        public string UserId { get; }
        public UserConfig UserConfig { get; }

        private LoginConfig(Builder builder)
        {
            UserId = builder.UserId;
            UserConfig = builder.UserConfig;
        }

        public class Builder
        {
            public string UserId { get; private set; }
            public UserConfig UserConfig { get; private set; }

            public Builder SetUserId(string userId)
            {
                UserId = userId;
                return this;
            }

            public Builder SetUserConfig(UserConfig userConfig)
            {
                UserConfig = userConfig;
                return this;
            }

            public LoginConfig Build()
            {
                return new LoginConfig(this);
            }
        }
    }
}