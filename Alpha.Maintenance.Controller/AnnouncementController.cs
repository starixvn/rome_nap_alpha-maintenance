namespace Alpha.Maintenance.Controller
{
    using System.Web.Mvc;
    using Announcements.Client.Helpers;
    using Announcements.Models;
    using Core.Model;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Newtonsoft.Json;
    using Repository.Criteria.Announcements;
    using Service.Impl.Announcement;
    using Service.Interface.Announcement;
    using ViewModels;
    using Web;

    public class AnnouncementController : BaseController
    {
        private readonly AnnouncementService announcementService;
       
        public AnnouncementController()
            : this(null)
        {
        }

        public AnnouncementController(AnnouncementService announcementService)
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
        public JsonResult InsertOrUpdateAnnouncement(string announcement, ActionType action)
        {
            var result = JsonConvert.DeserializeObject<Announcement>(
                                                    announcement,
                                                    new SpecificationConverter());

            return new JsonNetResult
            {
                Data = announcementService.InsertOrUpdateAnnouncement(result, action),
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

            var viewModel = new ComposeAnnouncementViewModel(
                                model, 
                                criteria.Id.Equals(0) ? ActionType.Create : ActionType.Update, 
                                offeredSpecifications);

            return View(viewModel);
        }
    }
}