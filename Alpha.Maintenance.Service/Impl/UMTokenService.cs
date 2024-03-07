using Alpha.Maintenance.Core.Configurations;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Service.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Alpha.Maintenance.Service.Impl
{
    public class UMTokenService : IUMTokenService
    {
        private readonly string SecretKey = "9yBNYA5Fnv2V58+pKLvYpFwV5BMkIh9NNTxPQk6Sbss=";

        private readonly IDictionary<string, int> Sites = new Dictionary<string, int>
        {
            { "MBCAgencySite", 3100000 },
            { "IndoAgencySite", 3200000 },
            {"_789yAgencySite", 3300000 },
            {"_777AgencySite" , 3500000 },
            {"AccountSite", 2000000 },
            { "LRF", 2000001 },
            { "OWA", 2000002 },
            {"Aff12Bet", 2000003},
            {"AffLiveBet365", 2000004 },
            {"RMS" , 2000005 },
            {"DW" , 2000006},
            {"OddsFeed", 2000007},
            {"CRM", 2000008 },
            {"_789y", 4100100},
            {"Test" , 9999999 },
        };

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

        public IDictionary<string, SiteToken> GetOldTokens()
        {
            IDictionary<string, SiteToken> tokens = new Dictionary<string, SiteToken>() { };

            foreach (var site in Sites)
            {
                tokens.Add(site.Key, new SiteToken { SiteId = site.Value, Token = GenerateToken(site.Value) });
            }

            return tokens;
        }

        private string GenerateToken(int siteId)
        {
            var today = DateTime.UtcNow.ToShortDateString();
            var key = string.Format("{0}_{1}_{2}", siteId, today, SecretKey);

            using (MD5 md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(key));

                return HttpServerUtility.UrlTokenEncode(hash);
            }
        }
    }
}