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

namespace Alpha.Maintenance.Controller
{
    [NapSuperAuth]
    public class LicenseeAdministrationController : BaseController
    {
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public ActionResult Index() => View();

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.LicenseeAdministration, Policy = Constant.Policy.Host)]
        public async Task<ActionResult> Index(LicenseeAdministrationViewModel viewModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(AppSettings.LicenseeAPIUrl);
                client.DefaultRequestHeaders.Add("ApiKey", AppSettings.LicenseeAPISecretKey);
                using (var content = new StringContent(JsonConvert.SerializeObject(JObject.Parse(viewModel.Payload)), Encoding.UTF8, "application/json"))
                {
                    using (var response = await client.PostAsync(viewModel.Url, content).ConfigureAwait(false))
                    {
                        return Json(new { response.StatusCode, result = await response.Content.ReadAsStringAsync() }, JsonRequestBehavior.DenyGet);
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