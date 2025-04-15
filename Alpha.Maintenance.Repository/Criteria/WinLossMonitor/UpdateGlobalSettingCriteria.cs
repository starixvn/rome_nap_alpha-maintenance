namespace Alpha.Maintenance.Repository.Criteria.WinLossMonitor
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UpdateGlobalSettingCriteria : NonQueryCommand
    {
        public UpdateGlobalSettingCriteria(
            string serviceName,
            string settingName,
            string settingValue,
            string dbAlias)
        {
            ServiceName = serviceName;
            SettingName = settingName;
            SettingValue = settingValue;
            DBAlias = dbAlias;
        }

        public string ServiceName { get; set; }

        public string SettingName { get; set; }

        public string SettingValue { get; set; }

        [SpParam(Ignored = true)]
        public string DBAlias { get; set; }

        public override string GetSettingKey()
            => string.IsNullOrWhiteSpace(DBAlias)
            ? "Acc_WLMonitored_Service_GlobalSettingUpd"
            : $"{DBAlias}_Acc_WLMonitored_Service_GlobalSettingUpd";

        public override bool IsValid()
            => true;
    }
}
