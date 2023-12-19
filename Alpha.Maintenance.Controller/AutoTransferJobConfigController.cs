using System.Web.Mvc;
using Alpha.Maintenance.Core.Constants;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Service.Impl.AutoTransferJobConfig;
using Alpha.Maintenance.Service.Interface.AutoTransferJobConfig;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;
using System.Linq;

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
        public ActionResult Index(string site, string env)
        {
            var model = AutoTransferEnvironmentConstants.AllEnvironments.Select(e => service.GetAutoTransferJobConfig(site, e));

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
