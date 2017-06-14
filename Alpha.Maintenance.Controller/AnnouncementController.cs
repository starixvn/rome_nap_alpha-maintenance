namespace Alpha.Maintenance.Controller
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Announcements.Models;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Repository.Criteria.Announcements;
    using Service.Impl;
    using Service.Interface;
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
        //[NapAuth(FunctionKey = SiteFunctionKeys.Announcement, Policy = Constant.Policy.Read)]
        public ActionResult ShowAnnouncements(GetAnnouncementsCriteria criteria)
        {
            var viewModel = new AnnouncementViewModel
            {
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                Environment = criteria.Environment,
                Announcements = announcementService.GetAnnouncements(criteria)
            };

            return View(viewModel);
        }

        public JsonResult InsertAnnouncement(Announcement announcement)
        {
            return new JsonNetResult
            {
                Data = announcementService.InsertAnnouncements(announcement),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetSpecifications()
        {
            return new JsonNetResult
            {
                Data = announcementService.GetAllSpecifications(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public ActionResult ComposeMessage()
        {
           return View();
        }

        public ActionResult EditMessage(EditAnnouncementsCriteria criteria)
        {
            var announcement = announcementService.EditAnnouncement(criteria);
            return View(announcement);
        }
    }
}