namespace Alpha.Maintenance.Service.Impl.IPInformation
{
    using System.Collections.Generic;
    using System.Net;
    using Core.Model;
    using Interface.IPInformation;
    using Newtonsoft.Json.Linq;
    using Repository.Criteria.IPInformation;
    using Repository.Impl;

    public class IPInformationServiceImpl : IPInformationService
    {
        private const string NoInformationCharacter = "-";
        private static readonly IList<APIInfo> GeolocationAPIs = new List<APIInfo>
        {
            new APIInfo("feegeoip.net", "http://freegeoip.net/json/", "country_code", "country_name", "region_name", "city"),
            new APIInfo("ip-api.com", "http://ip-api.com/json/", "countryCode", "country", "regionName", "city"),
            new APIInfo("IPLocate", "https://www.iplocate.io/api/lookup/", "country_code", "country", "subdivision", "city"),
            new APIInfo("eXTReMe-IP-Lookup.com", "https://extreme-ip-lookup.com/json/", "countryCode", "country", "region", "city")
        };
        private readonly IPInformationRepository ipInformactionRepository;

        public IPInformationServiceImpl()
            : this(null)
        {
        }

        public IPInformationServiceImpl(IPInformationRepository ipInformactionRepository = null)
        {
            this.ipInformactionRepository = ipInformactionRepository ?? new IPInformationRepositoryImpl();
        }

        public IPInfo GetIPInfoFromDB(string ip)
            => ipInformactionRepository.GetIPInfo(new GetIPInformationCriteria(ip));

        public IList<IPInfo> GetIPInfoFromAPIs(string ip)
        {
            var result = new List<IPInfo>();

            using (var webClient = new WebClient())
            {
                foreach (var api in GeolocationAPIs)
                {
                    var responseString = webClient.DownloadString(api.Link + ip);
                    var jsonObject = JObject.Parse(responseString);

                    var ipInfo = GetIPInfoFromAPI(api, jsonObject);
                    result.Add(ipInfo);
                }
            }

            return result;
        }

        private IPInfo GetIPInfoFromAPI(APIInfo apiInfo, JObject jsonObject)
            => new IPInfo(
                dataSource: apiInfo.Name,
                countryCode: !string.IsNullOrEmpty(jsonObject[apiInfo.CountryCodeKey]?.ToString()) ? jsonObject[apiInfo.CountryCodeKey].ToString() : NoInformationCharacter,
                countryName: !string.IsNullOrEmpty(jsonObject[apiInfo.CountryNameKey]?.ToString()) ? jsonObject[apiInfo.CountryNameKey].ToString().ToUpperInvariant() : NoInformationCharacter,
                region: !string.IsNullOrEmpty(jsonObject[apiInfo.RegionKey]?.ToString()) ? jsonObject[apiInfo.RegionKey].ToString().ToUpperInvariant() : NoInformationCharacter,
                city: !string.IsNullOrEmpty(jsonObject[apiInfo.CityKey]?.ToString()) ? jsonObject[apiInfo.CityKey].ToString().ToUpperInvariant() : NoInformationCharacter);
    }
}
