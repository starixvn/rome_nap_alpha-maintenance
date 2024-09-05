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
            return View(new LicenseeAdministrationViewModel() { Url = "LicenseeAdministration/Index", Payload = payload });
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public async Task<ActionResult> Index(LicenseeAdministrationViewModel viewModel)
        {
            var response = await SendPostRequest("api/deposit/players", viewModel.Payload);
            return Json(new { statusCode = response.Item1, result = response.Item2 }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public ActionResult CreateShadowAccount()
        {
            string payload = "{\"realCustID\": 2147483647,\"agentID\": 2147483647,\"isDebug\": true}";
            return View(new LicenseeAdministrationViewModel() { Url = "LicenseeAdministration/CreateShadowAccount", Payload = payload });
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public async Task<ActionResult> CreateShadowAccount(LicenseeAdministrationViewModel viewModel)
        {
            var response = await SendPostRequest("api/deposit/shadowaccounts", viewModel.Payload);
            return Json(new { statusCode = response.Item1, result = response.Item2 }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public ActionResult CreateDummyPlayer()
        {
            string payload = "{\"agentId\": 0,\"creator\": 0,\"ip\": \"127.0.1\"}";
            return View(new LicenseeAdministrationViewModel() { Url = "LicenseeAdministration/CreateDummyPlayer", Payload = payload });
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public async Task<ActionResult> CreateDummyPlayer(LicenseeAdministrationViewModel viewModel)
        {
            var response = await SendPostRequest($"api/site-builder/dummy-members", viewModel.Payload);
            return Json(new { statusCode = response.Item1, result = response.Item2 }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public ActionResult CreateLicensee()
        {
            string payload = "{";
            payload += "\"ProductBetSetting:\": [{\"ProductID\":0,\"Sporttype\":0,\"Bettype\":0,\"Currency\":0,\"MinBet\":0,\"MaxBet\":0,\"MPM\":0,\"MPB\":0,\"MPPM\":0}],";
            payload += "\"ProductStakeLevel\": [{ \"Currency\": 0,\"GameTypeID\": 0,\"MinStake\": 0,\"MaxStake\": 0}],";
            payload += "\"ProductComm\": [{ \"ProductID\": 0,\"Comm\": 0}],";
            payload += "\"Creator\":\"string\",\"ip\":\"string\",\"isCreateSub\":true,\"licenseeType\":\"string\",\"siteName\":\"string\",\"isChinaView\":true,\"siteDBGroupId\":0,\"specialPlatform\":0";
            payload += "}";
            return View(new LicenseeAdministrationViewModel { Url = "LicenseeAdministration/CreateLicensee", Payload = payload });
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public async Task<ActionResult> CreateLicensee(LicenseeAdministrationViewModel viewModel)
        {
            var response = await SendPostRequest("api/site-builder/licensees", viewModel.Payload);
            return Json(new { statusCode = response.Item1, result = response.Item2 }, JsonRequestBehavior.DenyGet);
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public ActionResult CreateCurrency()
        {
            string payload = "{";
            payload += "\"SiteName:\": \"string\",";
            payload += "\"ProductBetSetting:\": [{\"ProductID\":0,\"Sporttype\":0,\"Bettype\":0,\"Currency\":0,\"MinBet\":0,\"MaxBet\":0,\"MPM\":0,\"MPB\":0,\"MPPM\":0}],";
            payload += "\"ProductStakeLevel\": [{ \"Currency\": 0,\"GameTypeID\": 0,\"MinStake\": 0,\"MaxStake\": 0}],";
            payload += "\"ProductComm\": [{ \"ProductID\": 0,\"Comm\": 0}],";
            payload += "\"Creator\":\"string\",\"ip\":\"string\",\"isCreateSub\":true,\"licenseeType\":\"string\",\"siteName\":\"string\"";
            payload += "}";
            return View(new LicenseeAdministrationViewModel { Url = "LicenseeAdministration/CreateCurrency", Payload = payload });
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public async Task<ActionResult> CreateCurrency(LicenseeAdministrationViewModel viewModel)
        {
            var response = await SendPostRequest($"api/site-builder/currencies", viewModel.Payload);
            return Json(new { statusCode = response.Item1, result = response.Item2 }, JsonRequestBehavior.DenyGet);
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

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public ActionResult Reactive() => View();

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