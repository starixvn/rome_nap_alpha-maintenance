using Fanex.Data.Repository;

namespace Alpha.Maintenance.Repository.Criteria.Chatbot
{
    public class GetBotCriteria : CriteriaBase
    {
        public GetBotCriteria(int botId)
        {
            BotId = botId;
        }

        public int BotId { get; }

        public override string GetSettingKey() => "Age_Chatbot_BotSelAll";

        public override bool IsValid() => BotId > 0;
    }
}
