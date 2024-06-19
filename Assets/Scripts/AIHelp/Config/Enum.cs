
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

}