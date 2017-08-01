namespace Alpha.Maintenance.Controller
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Announcements.Client.Helpers;
    using Announcements.Models;
    using Core;
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

        public AnnouncementController(
            AnnouncementService announcementService,
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

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.AnnouncementManagement, Policy = Constant.Policy.Host)]
        public JsonResult SaveAnnouncement(SaveAnnouncementDemand demand)
        {
            var announcement = JsonConvert.DeserializeObject<Announcement>(
                                                    demand.Announcement,
                                                    new SpecificationConverter());

            var contentsBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(announcement.Contents));
            var encodedContents = Convert.ToBase64String(contentsBytes);

            var systems = String.Join(", ", announcement.Systems.ToArray());
            var environments = String.Join(", ", announcement.Environments.ToArray());

            var data = demand.Action.Equals(ActionType.Update) ?
               announcementService.UpdateAnnouncement(new UpdateAnnouncementsCriteria
               {
                   Id = announcement.Id,
                   Code = announcement.Code,
                   Title = announcement.Title,
                   IsActive = announcement.IsActive,
                   AnnouncementType = (int)announcement.AnnouncementType,
                   FromDate = announcement.FromDate,
                   ToDate = announcement.ToDate,
                   Environments = environments,
                   Systems = systems,
                   Ordinal = announcement.Ordinal,
                   Contents = encodedContents,
                   Specifications = JsonConvert.SerializeObject(announcement.Specifications),
                   LastEditDate = SystemTime.Today,
                   LastEditorId = CurrentSession.UserId,
                   LastEditorName = CurrentSession.Username
               }) :
                 announcementService.InsertAnnouncement(new InsertAnnouncementsCriteria
                 {
                     Id = announcement.Id,
                     Code = announcement.Code,
                     Title = announcement.Title,
                     IsActive = announcement.IsActive,
                     AnnouncementType = (int)announcement.AnnouncementType,
                     CreatedDate = SystemTime.Today,
                     FromDate = announcement.FromDate,
                     ToDate = announcement.ToDate,
                     Environments = environments,
                     Systems = systems,
                     Ordinal = announcement.Ordinal,
                     Contents = encodedContents,
                     Specifications = JsonConvert.SerializeObject(announcement.Specifications),
                     CreatorId = CurrentSession.UserId,
                     CreatorName = CurrentSession.Username
                 });

            return new JsonNetResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}