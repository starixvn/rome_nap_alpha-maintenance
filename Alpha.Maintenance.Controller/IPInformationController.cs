namespace Alpha.Maintenance.Controller
{
    using System.Threading.Tasks;
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

        public IPInformationController(IPInformationService ipInformationService)
        {
            this.ipInformationService = ipInformationService ?? new IPInformationServiceImpl();
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.IPInformation, Policy = Constant.Policy.Read)]
        public ActionResult Index() => View();

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.IPInformation, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> GetIPInfo(string ip)
            => Json(new { IPInfos = await ipInformationService.GetIPInfos(ip) }, JsonRequestBehavior.AllowGet);
    }
}