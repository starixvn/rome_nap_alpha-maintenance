using Fanex.Data.Repository;

namespace Alpha.Maintenance.Repository.Criteria.Log
{
    public class GetLogMessageCriteria : CriteriaBase
    {
        private const string SettingKeyName = "Logging_SelMessage";

        public long LogId { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => SettingKeyName;
    }
}
