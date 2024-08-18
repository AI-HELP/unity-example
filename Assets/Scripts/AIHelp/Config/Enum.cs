
using System;
namespace AIHelp
{

    public enum PushPlatform
    {
        APNS = 1, FIREBASE = 2, JIGUANG = 3, GETUI = 4, HUAWEI = 6, ONE_SIGNAL = 7
    }

    public enum ConversationMoment
    {
        NEVER = 1, ALWAYS = 2, ONLY_IN_ANSWER_PAGE = 3, AFTER_MARKING_UNHELPFUL = 4
    }

    public enum PublishCountryOrRegion
    {
        CN = 1, IN = 2
    };

    public enum EventType
    {
        /**
         * Event for SDK initialization
         */
        Initialization,

        /**
         * Event for user login
         */
        UserLogin,

        /**
         * Event for enterprise authentication
         */
        EnterpriseAuth,

        /**
         * Event for opening or closing a session (window)
         */
        SessionOpen,
        SessionClose,

        /**
         * Event for message arrival
         */
        MessageArrival,

        /**
         * Event for log upload
         */
        LogUpload,

        /**
         * Event for URL click
         */
        UrlClick,

        /**
         * Event for unread task count 
         */
        UnreadTaskCount,

        /**
         * Event for conversation start, along with the user's first message 
         */
        ConversationStart,
    }

}