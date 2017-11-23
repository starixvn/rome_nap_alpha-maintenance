namespace Alpha.Maintenance.Controller
{
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Service.Impl.IPInformation;
    using Service.Interface.IPInformation;

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
        public ActionResult Index() => View();

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.IPInformation, Policy = Constant.Policy.Read)]
        public ActionResult GetIPInfoFromDB(string ip)
            => Json(new { IPInfo = ipInformationService.GetIPInfoFromDB(ip) }, JsonRequestBehavior.AllowGet);

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.IPInformation, Policy = Constant.Policy.Read)]
        public ActionResult GetIPInfoFromAPIs(string ip)
            => Json(new { ListIPInfo = ipInformationService.GetIPInfoFromAPIs(ip) }, JsonRequestBehavior.AllowGet);
    }
}
