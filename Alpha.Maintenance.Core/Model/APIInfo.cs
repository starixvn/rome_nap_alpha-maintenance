namespace Alpha.Maintenance.Core.Model
{
    public class APIInfo
    {
        public APIInfo(
            string name,
            string apiLink,
            string webLink,
            string countryCodeKey,
            string countryNameKey,
            string regionKey,
            string cityKey)
        {
            Name = name;
            APILink = apiLink;
            WebLink = webLink;
            CountryCodeKey = countryCodeKey;
            CountryNameKey = countryNameKey;
            RegionKey = regionKey;
            CityKey = cityKey;
        }

        public string Name { get; }

        public string APILink { get; }

        public string WebLink { get; }

        public string CountryCodeKey { get; }

        public string CountryNameKey { get; }

        public string RegionKey { get; }

        public string CityKey { get; }
    }
}
