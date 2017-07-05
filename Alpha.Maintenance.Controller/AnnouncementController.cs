namespace Alpha.Maintenance.Controller
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Announcements.Client.Helpers;
    using Announcements.Models;
    using Announcements.Specs;
    using Core.Configurations;
    using Core.Model;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Newtonsoft.Json;
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
                => View(new AnnouncementViewModel
                {
                    FromDate = criteria.FromDate,
                    ToDate = criteria.ToDate,
                    Environment = criteria.Environment,
                    System = criteria.System,
                    Announcements = announcementService.GetAnnouncements(criteria)
                });

        [HttpPost]
        //[NapAuth(FunctionKey = SiteFunctionKeys.Announcement, Policy = Constant.Policy.Host)]
        public JsonResult InsertAnnouncement(string announcement, ActionType action, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength != 0)
            {
                string fileString = string.Empty;

                using (BinaryReader b = new BinaryReader(file.InputStream))
                {
                    byte[] binData = b.ReadBytes(file.ContentLength);
                    fileString = System.Text.Encoding.UTF8.GetString(binData);
                }

                string[] data = new string[] { };
                char[] delimiterChars = { ' ', ',', '.', ':', '\t' };

                data = fileString.Split(delimiterChars, StringSplitOptions.None);
            }

            var result = JsonConvert.DeserializeObject<Announcement>(
                announcement,
                new SpecificationConverter());

            return new JsonNetResult
            {
                Data = announcementService.InsertAnnouncements(result, action),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetSpecifications()
                => new JsonNetResult
                {
                    Data = announcementService.GetAllSpecifications(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

        public ActionResult ComposeMessage(EditAnnouncementsCriteria criteria)
        {
            var allSpecifications = announcementService.GetAllSpecifications();

            var model = criteria.Id == null ?
                new Announcement
                {
                    AnnouncementType = AnnouncementType.HeaderPopup,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    Ordinal = 1,
                    Environments = new List<string> { "Local" },
                    Systems = new List<string> { "MBC" },
                    Contents = AppSettings.AnnouncementConfig.Languages.Select(
                            language => new MessageContent
                            {
                                LanguageKey = language,
                                Text = string.Empty
                            }),
                    Specifications = new List<ISpecification<IParam>>()
                } : announcementService.EditAnnouncement(criteria);

            var viewModel = new ComposeAnnouncementViewModel(model, criteria.Id == null ? ActionType.Create : ActionType.Update, allSpecifications);

            return View(viewModel);
        }
    }
}