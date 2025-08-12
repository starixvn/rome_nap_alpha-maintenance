using Alpha.Maintenance.Core.Configurations;

namespace Alpha.Maintenance.Core.Model
{
    public class ApiInfo
    {
        public ApiInfo(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public string APILink => AppSettings.GetConfigValue($"{Name}:ApiUrl");

        public string WebLink => AppSettings.GetConfigValue($"{Name}:WebUrl");

        public string CountryCodeKey => AppSettings.GetConfigValue($"{Name}:CountryCodeKey");

        public string CountryNameKey => AppSettings.GetConfigValue($"{Name}:CountryNameKey");

        public string RegionKey => AppSettings.GetConfigValue($"{Name}:RegionKey");

        public string CityKey => AppSettings.GetConfigValue($"{Name}:CityKey");
    }
}
