namespace Alpha.Maintenance.Controller
{
    using System.Web.Mvc;
    using Announcements.Client.Helpers;
    using Announcements.Models;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using Newtonsoft.Json;
    using Repository.Criteria.Announcements;
    using Request;
    using Service.Impl.Announcement;
    using Service.Interface.Announcement;
    using ViewModels;
    using Web;

    public class AnnouncementController : BaseController
    {
        private readonly AnnouncementService announcementService;

        public AnnouncementController()
            : this(null, null, null, null, null, null, null, null)
        {
        }

        public AnnouncementController(AnnouncementService announcementService,
            ICurrentContext currentContext,
            ICurrentSession currentSession,
            IUserService userService, 
            bool? enableReCheckIn,
            int? showLogTimeZone, 
            bool? issiteLogin, 
            int? siteId)
            : base(currentContext, 
                  currentSession, 
                  userService, 
                  enableReCheckIn, 
                  showLogTimeZone, 
                  issiteLogin, 
                  siteId)
        {
            this.announcementService = announcementService ?? new AnnouncementServiceImpl();
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.AnnouncementManagement, Policy = Constant.Policy.Read)]
        public ActionResult ManageAnnouncements(SelectAnnouncementsCriteria criteria)
                => View(new ManageAnnouncementsViewModel
                {
                    FromDate = criteria.FromDate,
                    ToDate = criteria.ToDate,
                    Environment = criteria.Environment,
                    System = criteria.System,
                    Announcements = announcementService.GetAnnouncements(criteria)
                });

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.AnnouncementManagement, Policy = Constant.Policy.Host)]
        public JsonResult InsertOrUpdateAnnouncement(InsertOrUpdateAnnouncementDemand demand)
        {
            var result = JsonConvert.DeserializeObject<Announcement>(
                                                    demand.Announcement,
                                                    new SpecificationConverter());

            return new JsonNetResult
            {
                Data = announcementService.InsertOrUpdateAnnouncement(result, demand.Action),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.AnnouncementManagement, Policy = Constant.Policy.Read)]
        public ActionResult ComposeMessage(SelectAnnouncementByIdCriteria criteria)
        {
            var offeredSpecifications = announcementService.GetOfferedSpecifications();

            var model = criteria.Id.Equals(0) ? 
                announcementService.GetDefaultAnnouncement() : 
                announcementService.GetAnnouncementById(criteria);

            return View(new ComposeAnnouncementViewModel(
                                model,
                                criteria.Id.Equals(0) ? ActionType.Create : ActionType.Update,
                                offeredSpecifications));
        }
    }
}