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
                    result.Add(GetIPInfoFromAPI(webClient, api, ip));
                }
            }

            return result;
        }

        private static IPInfo GetIPInfoFromAPI(WebClient webClient, APIInfo apiInfo, string ip)
        {
            var responseString = webClient.DownloadString(apiInfo.Link + ip);
            var jObject = JObject.Parse(responseString);

            return new IPInfo(
                dataSource: apiInfo.Name,
                countryCode: GetValueFromJson(jObject, apiInfo.CountryCodeKey),
                countryName: GetValueFromJson(jObject, apiInfo.CountryNameKey),
                region: GetValueFromJson(jObject, apiInfo.RegionKey),
                city: GetValueFromJson(jObject, apiInfo.CityKey));
        }

        private static string GetValueFromJson(JObject jObject, string key)
        {
            var value = jObject[key]?.ToString();

            return string.IsNullOrEmpty(value) ? "-" : value.ToUpperInvariant();
        }
    }
}
