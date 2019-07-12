namespace Alpha.Maintenance.Controller
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Core.Configurations;
    using Core.Model;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Service.Impl.IPInformation;
    using Service.Interface.IPInformation;
    using ViewModels;

    [NapSuperAuth]
    public class IPInformationController : BaseController
    {
        private readonly IPInformationService ipInformationService;

        public IPInformationController()
            : this(null)
        {
        }

        public IPInformationController(IPInformationService ipInformationService = null)
        {
            this.ipInformationService = ipInformationService ?? new IPInformationServiceImpl();
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.IPInformation, Policy = Constant.Policy.Read)]
        public ActionResult Index() => View(new IPInformationViewModel(CreateGeolocationAPIs()));

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.IPInformation, Policy = Constant.Policy.Read)]
        public ActionResult GetIPInfo(string ip)
            => Json(new { IPInfo = ipInformationService.GetIPInfo(ip) }, JsonRequestBehavior.AllowGet);

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.IPInformation, Policy = Constant.Policy.Read)]
        public ActionResult GetIPInfoFromTableMini(string ip)
            => Json(new { IPInfo = ipInformationService.GetIPInfoFromTableMini(ip) }, JsonRequestBehavior.AllowGet);

        private static IEnumerable<APIInfo> CreateGeolocationAPIs()
            => AppSettings.APINames.Select(name => new APIInfo(name));
    }
}