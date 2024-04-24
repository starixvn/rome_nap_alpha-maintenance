using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Alpha.Maintenance.Core.Configurations;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Service.Interface;
using Newtonsoft.Json;

namespace Alpha.Maintenance.Service.Impl
{
    public class UMTokenService : IUMTokenService
    {
        public async Task<IDictionary<string, SiteToken>> GetTokens()
        {
            IDictionary<string, SiteToken> tokens = null;

            using (var client = new HttpClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.BaseAddress = new Uri(AppSettings.ONEBookUMUri);
                var response = await client.GetAsync("Token/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    tokens = JsonConvert.DeserializeObject<IDictionary<string, SiteToken>>(content);
                }
            }

            return tokens;
        }

        public async Task<IDictionary<string, SiteToken>> GetOldTokens()
        {
            IDictionary<string, SiteToken> tokens = null;

            using (var client = new HttpClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.BaseAddress = new Uri(AppSettings.OldONEBookUMUri);
                var response = await client.GetAsync("Token/GetAll");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    tokens = JsonConvert.DeserializeObject<IDictionary<string, SiteToken>>(content);
                }
            }

            return tokens;
        }
    }
}