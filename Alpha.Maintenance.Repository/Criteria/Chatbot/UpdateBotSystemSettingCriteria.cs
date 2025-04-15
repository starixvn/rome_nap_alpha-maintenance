using Fanex.Data.Repository;
namespace Alpha.Maintenance.Repository.Criteria.Chatbot
{
    public class UpdateBotSystemSettingCriteria : NonQueryCommand
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public int Creator { get; set; }

        public string Ip { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "NAP_Chatbot_BotSystemSettingUpd";

        public override bool IsValid()
            => true;
    }
}
