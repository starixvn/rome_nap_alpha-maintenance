namespace Alpha.Maintenance.Core.Model
{
    public class IPInfo
    {
        public IPInfo(
            string dataSource,
            string webLink,
            string countryCode,
            string countryName,
            string region,
            string city)
        {
            DataSource = dataSource;
            WebLink = webLink;
            CountryCode = countryCode;
            CountryName = countryName;
            Region = region;
            City = city;
        }

        public string DataSource { get; }

        public string WebLink { get; }

        public string CountryCode { get; }

        public string CountryName { get; }

        public string Region { get; }

        public string City { get; }
    }
}
