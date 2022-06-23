using Fanex.Data.Repository;

namespace Alpha.Maintenance.Repository.Criteria.Chatbot
{
    public class CreateBotCriteria : CriteriaBase
    {
        public CreateBotCriteria(
            string displayName,
            string username,
            string accessToken,
            bool status,
            bool isTest,
            int creator,
            string ip)
        {
            DisplayName = displayName;
            Username = username;
            AccessToken = accessToken;
            Status = status;
            IsTest = isTest;
            Creator = creator;
            IP = ip;
        }

        public string DisplayName { get; }

        public string Username { get; }

        public string AccessToken { get; }

        public bool Status { get; }

        public bool IsTest { get; }

        public int Creator { get; }

        public string IP { get; }

        public override string GetSettingKey() => "NAP_Chatbot_BotIns";

        public override bool IsValid() => true;
    }
}
