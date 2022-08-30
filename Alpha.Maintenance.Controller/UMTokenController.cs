using Alpha.Maintenance.Service.Impl;
using Alpha.Maintenance.Service.Interface;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Alpha.Maintenance.Controller
{
    public class UMTokenController: BaseController
    {
        private readonly IUMTokenService umTokenService;

        public UMTokenController()
            : this(null)
        {
        }

        public UMTokenController(IUMTokenService umTokenService)
        {
            this.umTokenService = umTokenService ?? new UMTokenService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.UMToken, Policy = Constant.Policy.Read)]
        public async Task<ActionResult> Index() => View(await umTokenService.GetTokens());
    }
}
