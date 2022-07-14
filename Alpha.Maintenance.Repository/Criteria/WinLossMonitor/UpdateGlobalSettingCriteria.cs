namespace Alpha.Maintenance.Repository.Criteria.WinLossMonitor
{
    using Fanex.Data.Repository;

    public class UpdateGlobalSettingCriteria : CriteriaBase
    {
        public UpdateGlobalSettingCriteria(
            string serviceName,
            string settingName,
            string settingValue)
        {
            ServiceName = serviceName;
            SettingName = settingName;
            SettingValue = settingValue;
        }

        public string ServiceName { get; }

        public string SettingName { get; }

        public string SettingValue { get; }

        public override string GetSettingKey()
            => "Acc_WLMonitored_Service_GlobalSettingUpd";
    }
}
