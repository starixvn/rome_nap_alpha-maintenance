using Fanex.Data.Repository;

namespace Alpha.Maintenance.Repository.Criteria.Chatbot
{
    public class GetBotSystemSettingCriteria : CriteriaBase
    {
        public GetBotSystemSettingCriteria(string serviceName)
        {
            ServiceName = serviceName;
        }

        public string ServiceName { get; }

        public override string GetSettingKey() => "NAP_Chatbot_BotSystemSettingSel";

        public override bool IsValid()
            => true;
    }
}
