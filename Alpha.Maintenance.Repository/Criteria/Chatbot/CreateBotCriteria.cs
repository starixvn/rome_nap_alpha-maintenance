using Fanex.Data.Repository;

namespace Alpha.Maintenance.Repository.Criteria.Chatbot
{
    public class CreateBotCriteria : NonQueryCommand
    {
        public string Username { get; set; }

        public string DisplayName { get; set; }

        public string System { get; set; }

        public string AccessToken { get; set; }

        public bool Status { get; set; }

        public bool IsTest { get; set; }

        public int Creator { get; set; }

        public string IP { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "NAP_Chatbot_BotIns";

        public override bool IsValid() => true;
    }
}
