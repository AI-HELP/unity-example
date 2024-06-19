
using System;
namespace AIHelp
{
    public class LoginConfig
    {
        public string UserId { get; }
        public UserConfig UserConfig { get; }
        public AIHelpDelegate.OnEnterpriseAuthCallback OnEnterpriseAuthCallback { get; }
        public AIHelpDelegate.OnLoginResultCallback OnLoginResultCallback { get; }

        private LoginConfig(Builder builder)
        {
            UserId = builder.UserId;
            UserConfig = builder.UserConfig;
            OnEnterpriseAuthCallback = builder.OnEnterpriseAuthCallback;
            OnLoginResultCallback = builder.OnLoginResultCallback;
        }

        public class Builder
        {
            public string UserId { get; private set; }
            public UserConfig UserConfig { get; private set; }
            public AIHelpDelegate.OnEnterpriseAuthCallback OnEnterpriseAuthCallback { get; private set; }
            public AIHelpDelegate.OnLoginResultCallback OnLoginResultCallback { get; private set; }

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

            public Builder SetOnEnterpriseAuthCallback(AIHelpDelegate.OnEnterpriseAuthCallback authCallback)
            {
                OnEnterpriseAuthCallback = authCallback;
                return this;
            }

            public Builder SetOnLoginResultCallback(AIHelpDelegate.OnLoginResultCallback loginCallback)
            {
                OnLoginResultCallback = loginCallback;
                return this;
            }

            public LoginConfig Build()
            {
                return new LoginConfig(this);
            }
        }
    }
}