using Fanex.Data.Repository;

namespace Alpha.Maintenance.Repository.Criteria.Chatbot
{
    public class GetAllBotCriteria : CriteriaBase
    {
        public GetAllBotCriteria(bool isTest, bool enabled)
        {
            IsTest = isTest;
            Status = enabled;
        }

        public bool IsTest { get; }

        public bool Status { get; }

        public override string GetSettingKey() => "Age_Chatbot_BotSelAll";

        public override bool IsValid() => true;
    }
}
