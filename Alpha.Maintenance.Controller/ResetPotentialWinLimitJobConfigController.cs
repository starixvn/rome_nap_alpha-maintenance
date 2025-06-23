using System.Web.Mvc;
using Alpha.Maintenance.Core.Model.ResetPotentialWinLimitJobConfig;
using Alpha.Maintenance.Service.Impl.ResetPotentialWinLimitJobConfig;
using Alpha.Maintenance.Service.Interface.ResetPotentialWinLimitJobConfig;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Alpha.Maintenance.Controller
{
    public class ResetPotentialWinLimitJobConfigController : BaseController
    {
        private readonly ResetPotentialWinLimitJobConfigService service;

        public ResetPotentialWinLimitJobConfigController()
            : this(null)
        {
        }

        public ResetPotentialWinLimitJobConfigController(ResetPotentialWinLimitJobConfigService service)
        {
            this.service = service ?? new ResetPotentialWinLimitJobConfigServiceImpl();
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.AdminTools, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            var setting = service.GetResetPotentialWinLimitJobConfig();
            return View("~/Views/ResetPotentialWinLimitJobConfig/Index.cshtml", setting);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.AdminTools, Policy = Constant.Policy.Host)]
        public ActionResult Update(ResetPotentialWinLimitJobSettingModel setting)
        {
            var result = service.Update(setting);

            return Json(result);
        }
    }
}
