using Fanex.Data.Repository;

namespace Alpha.Maintenance.Repository.Criteria.Chatbot
{
    public class CreateBotCriteria : CriteriaBase
    {
        public CreateBotCriteria(
            string username,
            string displayName,
            string system,
            string accessToken,
            bool status,
            bool isTest,
            int creator,
            string ip)
        {
            Username = username;
            DisplayName = displayName;
            System = system;
            AccessToken = accessToken;
            Status = status;
            IsTest = isTest;
            Creator = creator;
            IP = ip;
        }

        public string Username { get; }

        public string DisplayName { get; }

        public string System { get; }

        public string AccessToken { get; }

        public bool Status { get; }

        public bool IsTest { get; }

        public int Creator { get; }

        public string IP { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "NAP_Chatbot_BotIns";

        public override bool IsValid() => true;
    }
}
