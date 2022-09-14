namespace Alpha.Maintenance.Repository.Criteria.WinLossMonitor
{
    using Fanex.Data.Repository;

    public class UpdateGlobalSettingCriteria : CriteriaBase
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

        public string ServiceName { get; }

        public string SettingName { get; }

        public string SettingValue { get; }

        [SpParam(Ignored = true)]
        public string DBAlias { get; }

        public override string GetSettingKey()
            => string.IsNullOrWhiteSpace(DBAlias)
            ? "Acc_WLMonitored_Service_GlobalSettingUpd"
            : $"{DBAlias}_Acc_WLMonitored_Service_GlobalSettingUpd";
    }
}
