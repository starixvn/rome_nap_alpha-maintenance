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
        public JsonResult InsertAnnouncement(string announcement, ActionType action, HttpPostedFileBase file)
        {
            var result = AssignValueOfUserIdsFileSpec(file, 
                                                    JsonConvert.DeserializeObject<Announcement>(
                                                    announcement,
                                                    new SpecificationConverter()));

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
            var offeredSpecifications = announcementService.GetOfferedSpecifications(AppSettings.AnnouncementConfig.OfferedSpecifications);

            var model = criteria.Id == null ? 
                announcementService.GetDefaultAnnouncement(AppSettings.AnnouncementConfig.Languages) : 
                announcementService.GetAnnouncementById(criteria);

            var viewModel = new ComposeAnnouncementViewModel(
                                model, 
                                criteria.Id == null ? ActionType.Create : ActionType.Update, 
                                offeredSpecifications,
                                BuildSelectedListByFilterFromAppSetting(model.Environments, AppSettings.AnnouncementConfig.Environments),
                                BuildSelectedListByFilterFromAppSetting(model.Systems, AppSettings.AnnouncementConfig.Systems));

            return View(viewModel);
        }

        private IEnumerable<SelectListItem> BuildSelectedListByFilterFromAppSetting(
            IEnumerable<string> valueRange, 
            IEnumerable<string> appSettingValues) 
                => appSettingValues.Select(
                        value => new SelectListItem
                        {
                            Text = value,
                            Value = value,
                            Selected = valueRange.Contains(value)
                        });


        private Announcement AssignValueOfUserIdsFileSpec(HttpPostedFileBase file, Announcement announcement)
        {
            if (file != null && file.ContentLength != 0)
            {
                string fileString = string.Empty;

                using (BinaryReader reader = new BinaryReader(file.InputStream))
                {
                    byte[] binaryData = reader.ReadBytes(file.ContentLength);
                    fileString = System.Text.Encoding.UTF8.GetString(binaryData);
                }

                char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                var data = fileString.Split(delimiterChars, StringSplitOptions.None).Select(x => int.Parse(x)).ToList();

                announcement.Specifications.Where(spec => spec.Name.Equals("User Ids File")).Select(spec =>
                {
                    return new UserIdFileSpecification(data);
                }).FirstOrDefault();
            }

            return announcement;
        }
    }
}