namespace Alpha.Maintenance.Core.Model
{
    using System.Collections.Generic;

    public class GlobalSetting
    {
        private readonly IDictionary<string, string> settings;

        public GlobalSetting(IDictionary<string, string> settings)
                => this.settings = settings;

        public bool IsRunningJob(string env, string serviceName, string settingName)
            => GetBool(env, serviceName, settingName);

        public string GetLastId(string env, string serviceName)
            => GetString(env, serviceName, "LastId");

        public string GetLastCheckTime(string env, string serviceName)
            => GetString(env, serviceName, "LastCheckTime");

        public string GetLastScanSettingDate(string env, string serviceName)
            => GetString(env, serviceName, "LastScanSettingDate");

        public string GetSuperUUS(string serviceName)
            => GetString(string.Empty, serviceName, "SuperUUS");

        private bool GetBool(string env, string serviceName, string settingName)
            => "1" == GetString(env, serviceName, settingName);

        private string GetString(string env, string serviceName, string settingName)
            => settings[string.IsNullOrWhiteSpace(env)
                ? $"{serviceName}_{settingName}"
                : $"{env}-{serviceName}_{settingName}"];
    }
}
