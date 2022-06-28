namespace Alpha.Maintenance.Repository.Criteria.Chatbot
{
    public class UpdateBotCriteria : CreateBotCriteria
    {
        public UpdateBotCriteria(
            int botId,
            string username,
            string displayName,
            string system,
            string accessToken,
            bool status,
            bool isTest,
            int creator,
            string ip)
            : base(username, displayName, system, accessToken, status, isTest, creator, ip)
        {
            BotId = botId;
        }

        public int BotId { get; }

        public override string GetSettingKey() => "NAP_Chatbot_BotUpd";

        public override bool IsValid() => BotId > 0;
    }
}
