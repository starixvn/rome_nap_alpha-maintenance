using System.Web.Mvc;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Service.Impl.AutoTransferJobConfig;
using Alpha.Maintenance.Service.Interface.AutoTransferJobConfig;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Alpha.Maintenance.Controller
{
    public class AutoTransferJobConfigController : BaseController
    {
        private readonly AutoTransferJobConfigService service;

        public AutoTransferJobConfigController()
            : this(null)
        {
        }

        internal AutoTransferJobConfigController(AutoTransferJobConfigService service)
        {
            this.service = service ?? new AutoTransferJobConfigServiceImpl();
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.AdminTools, Policy = Constant.Policy.Read)]
        public ActionResult Index(string site)
        {
            var model = service.GetAutoTransferJobConfig(site);

            return View("~/Views/AutoTransferJobConfig/Index.cshtml", model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.AdminTools, Policy = Constant.Policy.Host)]
        public ActionResult Update(AutoTransferJobConfigModel config)
        {
            var result = service.Update(config);

            return Json(result);
        }
    }
}
