
using System;
namespace AIHelp
{
    public class LoginConfig
    {
        public string UserId { get; }
        public UserConfig UserConfig { get; }
        public bool EnterpriseAuth { get; }

        private LoginConfig(Builder builder)
        {
            UserId = builder.UserId;
            UserConfig = builder.UserConfig;
            EnterpriseAuth = builder.EnterpriseAuth;
        }

        public class Builder
        {
            public string UserId { get; private set; }
            public UserConfig UserConfig { get; private set; }
            public bool EnterpriseAuth { get; private set; }

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

            public Builder SetEnterpriseAuth(bool enterpriseAuth)
            {
                EnterpriseAuth = enterpriseAuth;
                return this;
            }

            public LoginConfig Build()
            {
                return new LoginConfig(this);
            }
        }
    }
}