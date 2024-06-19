
using System;
namespace AIHelp
{
    public class ApiConfig
    {
        public string EntranceId { get; }
        public string WelcomeMessage { get; }

        private ApiConfig(Builder builder)
        {
            EntranceId = builder.EntranceId;
            WelcomeMessage = builder.WelcomeMessage;
        }

        public class Builder
        {
            public string EntranceId { get; private set; }
            public string WelcomeMessage { get; private set; }

            public Builder SetEntranceId(string entranceId)
            {
                EntranceId = entranceId;
                return this;
            }

            public Builder SetWelcomeMessage(string welcomeMessage)
            {
                WelcomeMessage = welcomeMessage;
                return this;
            }

            public ApiConfig Build()
            {
                return new ApiConfig(this);
            }
        }
    }
}