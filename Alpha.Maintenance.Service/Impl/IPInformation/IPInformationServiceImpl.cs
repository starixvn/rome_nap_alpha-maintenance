using System.Collections.Generic;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Service.Interface.IPInformation;
using Alpha.Maintenance.Repository.Criteria.IPInformation;
using Alpha.Maintenance.Repository.Impl;
using Alpha.Maintenance.Core.Configurations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net;

namespace Alpha.Maintenance.Service.Impl.IPInformation
{
    public class IPInformationServiceImpl : IPInformationService
    {
        private readonly IPInformationRepository ipInformationRepository;
        private readonly HttpClient client;

        public IPInformationServiceImpl()
            : this(null)
        {
            var proxyUri = ConfigurationManager.AppSettings["Proxy"];
            client = string.IsNullOrEmpty(proxyUri)
                ? new HttpClient()
                : new HttpClient(new HttpClientHandler { Proxy = new WebProxy(proxyUri), UseProxy = true });
        }

        public IPInformationServiceImpl(IPInformationRepository ipInformationRepository = null)
        {
            this.ipInformationRepository = ipInformationRepository ?? new IPInformationRepositoryImpl();
        }

        public async Task<IEnumerable<IPInfo>> GetIPInfos(string ip)
        {
            var result = new List<IPInfo>
            {
                ipInformationRepository.GetIPInfo(new GetIPInformationCriteria(ip))
            };

            var apis = GetGeolocationAPIs();
            foreach(var api in apis)
            {
                var apiParts = api.APILink.Split('?');
                var url = apiParts[0] + ip + (apiParts.Length > 1 ? "?" + apiParts[1] : "");
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(responseBody);
                result.Add(new IPInfo(
                    api.Name, 
                    data[api.CountryCodeKey].ToString(), 
                    data[api.CountryNameKey].ToString(),
                    data[api.RegionKey].ToString(),
                    data[api.CityKey].ToString(), 
                    url));
            }

            return result;
        }

        private static IEnumerable<APIInfo> GetGeolocationAPIs()
            => AppSettings.APINames.Select(name => new APIInfo(name));
    }
}