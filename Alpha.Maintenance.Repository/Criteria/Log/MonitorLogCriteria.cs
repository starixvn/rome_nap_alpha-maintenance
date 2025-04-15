using Fanex.Data.Repository;

namespace Alpha.Maintenance.Repository.Criteria.Log
{
    public class MonitorLogCriteria : NonQueryCommand
    {
        private const string SettingKeyName = "Logging_Monitor";

        public string LogIds { get; set; }
        public int? MonitoringUserId { get; set; }
        public bool IsMonitored { get; set; }

        public override bool IsValid() => true;

        public override string GetSettingKey() => SettingKeyName;
    }
}
