using Alpha.Maintenance.Controller.ViewModels;
using Alpha.Maintenance.Service.Impl;
using Alpha.Maintenance.Service.Interface;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Alpha.Maintenance.Controller
{
    public enum Sites : int
    {
        MBCAgencySite = 3100000,
        IndoAgencySite = 3200000,
        _789yAgencySite = 3300000,
        _777AgencySite = 3500000,
        AccountSite = 2000000,
        LRF = 2000001,
        OWA = 2000002,
        Aff12Bet = 2000003,
        AffLiveBet365 = 2000004,
        RMS = 2000005,
        DW = 2000006,
        OddsFeed = 2000007,
        CRM = 2000008,
        _789y = 4100100,
        Test = 9999999
    }

    public class UMTokenController : BaseController
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
        public async Task<ActionResult> Index()
        {
            var viewModel = new TokenViewModel
            {
                NewTokens = await umTokenService.GetTokens()
            };
            return View(viewModel);
        }
    }
}