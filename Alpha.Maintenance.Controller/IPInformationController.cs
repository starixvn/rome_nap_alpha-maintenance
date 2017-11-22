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

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.IPInformation, Policy = Constant.Policy.Read)]
        public ActionResult Index(string ip)
            => Json(new { IPInfo = ipInformationService.GetIPInformation(ip) });

    }
}
