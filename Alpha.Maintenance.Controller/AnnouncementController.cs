namespace Alpha.Maintenance.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Announcements.Models;
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
            var model = new Announcement()
            {
               Environments = new List<string>() { "PRO", "STAGING", "UAT" },
               Systems = new List<string>() { "MBC", "123abc"}
            };

           return View(model);
        }

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
    }
}