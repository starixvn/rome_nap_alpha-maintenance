namespace Alpha.Maintenance.Controller
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Announcements.Models;
    using Service.Impl;
    using Service.Interface;
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
            var announcements = announcementService.GetAllAnnouncements();

            var model = new Announcement()
            {
               Environments = new List<string>() { "PRO", "STAGING", "UAT" },
               Systems = new List<string>() { "MBC", "123abc"}
            };


           return View(model);
        }

        public ActionResult ShowAnnouncements()
        {
            var announcements = announcementService.GetAllAnnouncements();

            return View(announcements);
        }
    }
}