using Fanex.Data.Repository;
namespace Alpha.Maintenance.Repository.Criteria.Chatbot
{
    public class UpdateBotSystemSettingCriteria : CriteriaBase
    {
        public UpdateBotSystemSettingCriteria(int id, string value, int creator, string ip)
        {
            Id = id;
            Value = value;
            Creator = creator;
            Ip = ip;
        }

        public int Id { get; }

        public string Value { get; }

        public int Creator { get; }

        public string Ip { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "NAP_Chatbot_BotSystemSettingUpd";
    }
}
