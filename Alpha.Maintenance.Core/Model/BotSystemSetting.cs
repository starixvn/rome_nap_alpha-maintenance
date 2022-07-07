namespace Alpha.Maintenance.Core.Model
{
    public class BotSystemSetting
    {
        public BotSystemSetting(int id, string serviceName, string settingName, string value)
        {
            Id = id;
            ServiceName = serviceName;
            SettingName = settingName;
            Value = value;
        }

        public int Id { get; }

        public string ServiceName { get; }

        public string SettingName { get; }

        public string Value { get; }
    }
}
