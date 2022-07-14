namespace Alpha.Maintenance.Core.Model
{
    public class GlobalSettingEntity
    {
        public GlobalSettingEntity()
        {
        }

        public GlobalSettingEntity(
            string serviceName,
            string settingName,
            string settingValue)
        {
            ServiceName = serviceName;
            SettingName = settingName;
            SettingValue = settingValue;
        }

        public string ServiceName { get; set; }

        public string SettingName { get; set; }

        public string SettingValue { get; set; }
    }
}
