namespace Alpha.Maintenance.Repository.Criteria.Chatbot
{
    public class UpdateBotCriteria : CreateBotCriteria
    {
        public int BotId { get; set; }

        public override string GetSettingKey() => "NAP_Chatbot_BotUpd";

        public override bool IsValid() => BotId > 0;
    }
}
