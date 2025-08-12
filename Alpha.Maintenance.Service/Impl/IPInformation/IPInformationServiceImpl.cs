using System.Collections.Generic;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Service.Interface.IPInformation;
using Alpha.Maintenance.Repository.Criteria.IPInformation;
using Alpha.Maintenance.Core.Configurations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Net;
using Fanex.Logging;
using Fanex.Data.Repository;

namespace Alpha.Maintenance.Service.Impl.IPInformation
{
    public class IPInformationServiceImpl : IPInformationService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly HttpClient client;

        public IPInformationServiceImpl()
            : this(null)
        {
            var proxyUri = ConfigurationManager.AppSettings["Proxy"];
            Logger.Log.Info(proxyUri);
            client = string.IsNullOrEmpty(proxyUri)
                ? new HttpClient()
                : new HttpClient(new HttpClientHandler { Proxy = new WebProxy(proxyUri), UseProxy = true }, true);
        }

        public IPInformationServiceImpl(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public async Task<IEnumerable<IPInfo>> GetIPInfos(string ip)
        {
            var result = (await dynamicRepository.FetchAsync<IPInfo>(new GetIPInformationCriteria(ip)))
                .Select(info =>
                {
                    info.DataSource = "IPDB";
                    return info;
                })
                .ToList();

            var apis = GetGeolocationAPIs();
            foreach(var api in apis)
            {
                var apiParts = api.APILink.Split('?');
                var url = apiParts[0] + ip + (apiParts.Length > 1 ? "?" + apiParts[1] : "");
                Logger.Log.Info(url);
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var data = JObject.Parse(responseBody);
                if(data[api.CountryCodeKey] != null && data[api.CountryNameKey] != null && data[api.RegionKey] != null && data[api.CityKey] != null)
                {
                    result.Add(new IPInfo
                    {
                        DataSource = api.Name,
                        Url = url,
                        CountryCode = data[api.CountryCodeKey].ToString(),
                        CountryName = data[api.CountryNameKey].ToString(),
                        Region = data[api.RegionKey].ToString(),
                        City = data[api.CityKey].ToString()
                    });
                }
                
            }

            return result;
        }

        private static IEnumerable<ApiInfo> GetGeolocationAPIs()
            => AppSettings.APINames.Select(name => new ApiInfo(name));
    }
}