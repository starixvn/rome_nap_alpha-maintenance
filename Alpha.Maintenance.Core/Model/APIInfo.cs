namespace Alpha.Maintenance.Core.Model
{
    public class APIInfo
    {
        public APIInfo(
            string name,
            string link,
            string countryCodeKey,
            string countryNameKey,
            string regionKey,
            string cityKey)
        {
            Name = name;
            Link = link;
            CountryCodeKey = countryCodeKey;
            CountryNameKey = countryNameKey;
            RegionKey = regionKey;
            CityKey = cityKey;
        }

        public string Name { get; }

        public string Link { get; }

        public string CountryCodeKey { get; }

        public string CountryNameKey { get; }

        public string RegionKey { get; }

        public string CityKey { get; }
    }
}
