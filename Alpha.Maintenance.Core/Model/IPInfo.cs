namespace Alpha.Maintenance.Core.Model
{
    public class IPInfo
    {
        public IPInfo(
            string countryCode,
            string countryName,
            string region,
            string city)
        {
            CountryCode = countryCode;
            CountryName = countryName;
            Region = region;
            City = city;
        }

        public string CountryCode { get; }

        public string CountryName { get; }

        public string Region { get; }

        public string City { get; }
    }
}
