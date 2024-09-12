using Fanex.NAP.Core.Business;
using System.Web.Mvc;
using Fanex.NAP.Core.Web.Attribute;
using Alpha.Maintenance.Controller.ViewModels;
using System.Net.Http;
using System;
using Alpha.Maintenance.Core.Configurations;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;

namespace Alpha.Maintenance.Controller
{
    [NapSuperAuth]
    public class LicenseeAdministrationController : BaseController
    {
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public ActionResult Index()
        {
            string payload = "{\"userName\": \"string\",\"userID\": 2147483647,\"password\": \"string\",\"oddsType\": \"string\",\"agentId\": 2147483647,\"affiliateID\": 2147483647,\"country\": \"string\",\"subsidiaryID\": 2147483647,\"isDebug\": true}";
            return View(new LicenseeAdministrationViewModel() { Url = "api/deposit/players", Payload = payload });
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public async Task<ActionResult> Index(LicenseeAdministrationViewModel viewModel)
        {
            var response = await SendPostRequest(viewModel.Url, viewModel.Payload);
            return Json(new { statusCode = response.Item1, result = response.Item2 }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public ActionResult GetSamplePageLoad(int apiId)
        {
            LicenseeAdministrationViewModel viewModel = null;
            switch (apiId)
            {
                case 1:
                    viewModel = new LicenseeAdministrationViewModel() { Url = "api/deposit/players", Payload = CreatePlayerPayload() };
                    break;

                case 2:
                    viewModel = new LicenseeAdministrationViewModel() { Url = "api/deposit/shadowaccounts", Payload = CreateShadowAccountPayload() };
                    break;

                case 3:
                    viewModel = new LicenseeAdministrationViewModel { Url = "api/site-builder/licensees", Payload = CreateLicenseePayload() };
                    break;

                case 4:
                    viewModel = new LicenseeAdministrationViewModel { Url = "api/site-builder/currencies", Payload = CreateCurrencyPayload() };
                    break;

                case 5:
                    viewModel = new LicenseeAdministrationViewModel { Url = "api/site-builder/dummy-players", Payload = "{\"agentId\": 0,\"creator\": 0,\"ip\": \"127.0.1\"}" };
                    break;
            }

            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        private string CreatePlayerPayload()
            => "{\"userName\": \"string\",\"userID\": 2147483647,\"password\": \"string\",\"oddsType\": \"string\",\"agentId\": 2147483647,\"affiliateID\": 2147483647,\"country\": \"string\",\"subsidiaryID\": 2147483647,\"isDebug\": true}";

        private string CreateShadowAccountPayload()
        {
            return "{\"realCustID\": 2147483647,\"agentID\": 2147483647,\"isDebug\": true}";
        }

        private string CreateLicenseePayload()
        {
            string payload = "{";
            payload += "\"ProductBetSetting:\": [{\"ProductID\":0,\"Sporttype\":0,\"Bettype\":0,\"Currency\":0,\"MinBet\":0,\"MaxBet\":0,\"MPM\":0,\"MPB\":0,\"MPPM\":0}],";
            payload += "\"ProductStakeLevel\": [{ \"Currency\": 0,\"GameTypeID\": 0,\"MinStake\": 0,\"MaxStake\": 0}],";
            payload += "\"ProductComm\": [{ \"ProductID\": 0,\"Comm\": 0}],";
            payload += "\"Creator\":\"string\",\"ip\":\"string\",\"isCreateSub\":true,\"licenseeType\":\"string\",\"siteName\":\"string\",\"isChinaView\":true,\"siteDBGroupId\":0,\"specialPlatform\":0";
            payload += "}";

            return payload;
        }

        private string CreateCurrencyPayload()
        {
            string payload = "{";
            payload += "\"SiteName:\": \"string\",";
            payload += "\"ProductBetSetting:\": [{\"ProductID\":0,\"Sporttype\":0,\"Bettype\":0,\"Currency\":0,\"MinBet\":0,\"MaxBet\":0,\"MPM\":0,\"MPB\":0,\"MPPM\":0}],";
            payload += "\"ProductStakeLevel\": [{ \"Currency\": 0,\"GameTypeID\": 0,\"MinStake\": 0,\"MaxStake\": 0}],";
            payload += "\"ProductComm\": [{ \"ProductID\": 0,\"Comm\": 0}],";
            payload += "\"Creator\":\"string\",\"ip\":\"string\",\"isCreateSub\":true,\"licenseeType\":\"string\",\"siteName\":\"string\"";
            payload += "}";
            return payload;
        }

        private async Task<(HttpStatusCode, string)> SendPostRequest(string url, string payload)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppSettings.LicenseeAPIUrl);
                client.DefaultRequestHeaders.Add("ApiKey", AppSettings.LicenseeAPISecretKey);
                using (var content = new StringContent(JsonConvert.SerializeObject(JObject.Parse(payload)), Encoding.UTF8, "application/json"))
                {
                    using (var response = await client.PostAsync(url, content).ConfigureAwait(false))
                    {
                        return (response.StatusCode, await response.Content.ReadAsStringAsync());
                    }
                }
            }
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public async Task<ActionResult> Reactive(ReactiveDormantPlayerViewModel viewModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppSettings.LicenseeAPIUrl);
                client.DefaultRequestHeaders.Add("ApiKey", AppSettings.LicenseeAPISecretKey);
                var payload = new { viewModel.ReinitOldBalance, viewModel.SubsidiaryID };
                using (var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"))
                {
                    using (var response = await client.PutAsync($"api/deposit/players/{viewModel.CustId}", content).ConfigureAwait(false))
                    {
                        return Json(new { response.StatusCode, result = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.DenyGet);
                    }
                }
            }
        }
    }
}