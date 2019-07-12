namespace Alpha.Maintenance.Core.Model
{
    public class IPInfo
    {
        public IPInfo(
            string dataSource,
            string countryCode,
            string countryName,
            string region,
            string city)
        {
            DataSource = dataSource;
            CountryCode = countryCode;
            CountryName = countryName;
            Region = region;
            City = city;
        }

        public string DataSource { get; }

        public string CountryCode { get; }

        public string CountryName { get; }

        public string Region { get; }

        public string City { get; }
    }
}