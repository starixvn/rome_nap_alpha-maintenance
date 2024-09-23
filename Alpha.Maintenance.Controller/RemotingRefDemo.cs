using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

using Fanex.RemotingRef;
using RefClient.Factory;
using RefClient.Service.IService;
using RefDataModelStandard.Models.Request;
using PublicEnum.Language;
using RefClient.Service;

namespace Alpha.Maintenance.Controller
{
    [AllowAnonymous]
    public class RemotingRefDemoController : BaseController
    {

        private readonly IRemotingRefService remotingRefService;
        private readonly IBetRefClientService twRemotingRefService = RefClientFactory.GetBetRefClientService();
        private readonly IRefDataClient _twRefDataClient = RefClientFactory.GetRefDataClient();

        public RemotingRefDemoController() : this(null)
        {

        }
        public RemotingRefDemoController(IRemotingRefService remotingRefService)
        {
            this.remotingRefService = remotingRefService ?? new RemotingRefService();
        }

        [HttpGet]
        public ActionResult ResourceData()
            => View();

        [HttpPost]
        public ActionResult ResourceData(string refId, string resourceId, string languageCode = "en-US")
        {

            return View("ResourceData", (object)remotingRefService.GetResourceDataByRefIdAndResourceId(refId, resourceId, languageCode, string.Empty));
        }

        [HttpGet]
        public ActionResult GetTeamName3rd()
            => View();

        [HttpPost]
        public ActionResult GetTeamName3rd(string team3rdId, string languageCode = "en-US")
        {

            return View("GetTeamName3rd", (object)remotingRefService.GetTeamName3rd(team3rdId, languageCode));
        }

        [HttpGet]
        public ActionResult GetLeagueName3rd()
            => View();

        [HttpPost]
        public ActionResult GetLeagueName3rd(string league3rdId, string languageCode = "en-US")
        {

            return View("GetLeagueName3rd", (object)remotingRefService.GetLeagueName3rd(league3rdId, languageCode));
        }

        [HttpGet]
        public ActionResult GetLeagueName()
            => View(new RemotingRefViewModel());

        [HttpPost]
        public ActionResult GetLeagueName(string leagueId, string languageCode = "en-US")
        {
            var twName = _twRefDataClient.GetLeagueName(new GetLeagueNameModel
            {
                LeagueId = int.Parse(leagueId),
                Language = languageCode.ConvertCultureToEnum()
            }).ConfigureAwait(false).GetAwaiter().GetResult();
            var name = remotingRefService.GetLeagueName(leagueId, languageCode);

            return View("GetLeagueName", new RemotingRefViewModel(name, twName));
        }

        [HttpGet]
        public ActionResult GetTeamName()
            => View(new RemotingRefViewModel());

        [HttpPost]
        public ActionResult GetTeamName(string teamId, string languageCode = "en-US")
        {
            var twName = _twRefDataClient.GetTeamName(new GetTeamNameModel
            {
                TeamId = int.Parse(teamId),
                Language = languageCode.ConvertCultureToEnum()
            }).ConfigureAwait(false).GetAwaiter().GetResult();
            var name = remotingRefService.GetTeamName(teamId, languageCode);

            return View("GetTeamName", new RemotingRefViewModel(name, twName));
        }

        [HttpGet]
        public ActionResult GetBetTypeName()
           => View(new RemotingRefViewModel());

        [HttpPost]
        public ActionResult GetBetTypeName(string bettypeId, string resourceId, string languageCode = "en-US")
        {
            var name = string.Empty;
            if (string.IsNullOrEmpty(resourceId))
                name = remotingRefService.GetBetTypeName(bettypeId, languageCode);
            else
                name = remotingRefService.GetBetTypeName(bettypeId, languageCode, string.Empty, resourceId);
            var twName = twRemotingRefService.GetBetTypeName(GetBetTypeNameModel.CreateModel(int.Parse(bettypeId), languageCode.ConvertCultureToEnum(), resourceId));
            return View("GetBetTypeName", new RemotingRefViewModel(name, twName));
        }
    }

    public class RemotingRefViewModel
    {
        public RemotingRefViewModel()
        {
            vnSide = "";
            twSide = "";
        }

        public RemotingRefViewModel(string vn, string tw)
        {
            vnSide = vn;
            twSide = tw;
        }

        public string vnSide { get; set; }
        public string twSide { get; set; }
    }
}
