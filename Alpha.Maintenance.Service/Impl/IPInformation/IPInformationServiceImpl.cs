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
            new APIInfo("freegeoip.net"),
            new APIInfo("ip-api.com"),
            new APIInfo("eXTReMe-IP-Lookup.com")
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
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                foreach (var api in GeolocationAPIs)
                {
                    result.Add(GetIPInfoFromAPI(webClient, api, ip));
                }
            }

            return result;
        }

        private static IPInfo GetIPInfoFromAPI(WebClient webClient, APIInfo apiInfo, string ip)
        {
            var responseString = webClient.DownloadString(apiInfo.APILink + ip);
            var jObject = JObject.Parse(responseString);

            var webLink = !string.IsNullOrEmpty(apiInfo.WebLink) ? apiInfo.WebLink + ip : null;

            return new IPInfo(
                dataSource: apiInfo.Name,
                webLink: webLink,
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
