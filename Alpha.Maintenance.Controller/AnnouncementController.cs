namespace Alpha.Maintenance.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Announcements.Models;
    using Core.Configurations;
    using Core.Model;
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

        public JsonResult InsertAnnouncement(Announcement announcement, ActionType action)
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

        public ActionResult ComposeMessage(EditAnnouncementsCriteria criteria)
        {
            var model = criteria.Code == null ? 
                new Announcement
                {
                    AnnouncementType = AnnouncementType.HeaderPopup,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    Order = 1,
                    Environments = new List<string> {"Local"},
                    Systems = new List<string> { "MBC" },
                    Contents = AppSettings.AnnouncementConfig.Languages.Select(
                            language => new MessageContent
                            {
                                LanguageKey = language,
                                Text = string.Empty
                            })
                }
                : announcementService.EditAnnouncement(criteria);

            var viewModel = new ComposeAnnouncementViewModel(model);
           return View(viewModel);
        }
    }
}