namespace Alpha.Maintenance.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Announcements.Client.Helpers;
    using Announcements.Models;
    using Announcements.Specs;
    using Core.Configurations;
    using Core.Model;
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

        public JsonResult InsertAnnouncement(string announcement, ActionType action)
        {
          var result =  JsonConvert.DeserializeObject<Announcement>(
                    announcement,
                    new SpecificationConverter());

            return new JsonNetResult
            {
                Data = announcementService.InsertAnnouncements(result, action),
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
            var specifications = announcementService.GetAllSpecifications();

            var model = criteria.Id == null ?
                new Announcement
                {
                    AnnouncementType = AnnouncementType.HeaderPopup,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    Order = 1,
                    Environments = new List<string> { "Local" },
                    Systems = new List<string> { "MBC" },
                    Contents = AppSettings.AnnouncementConfig.Languages.Select(
                            language => new MessageContent
                            {
                                LanguageKey = language,
                                Text = string.Empty
                            }),
                    Specifications = new List<ISpecification<IUser>>
                    {
                        specifications.First()
                    }
                }
                : announcementService.EditAnnouncement(criteria);

            var viewModel = new ComposeAnnouncementViewModel(model, criteria.Id == null ? ActionType.Create : ActionType.Update, specifications);

            return View(viewModel);
        }

        public ActionResult SpecificationSetting(EditAnnouncementsCriteria criteria)
        {
           var specs = @"[
                 {
                        'Value':[
                           'Site1',
                           'Site4'
                    ],
                    'Sites':[
                       'Site1',
                       'Site4'
                    ],
                    'Id':'6edc73e0-aa87-4757-96e7-e5ffea0ce06a',
                    'Name':'Sites',
                    'Combinable':true,
                    'TypeName':'Alpha.Announcements.Specs.SiteSpecification, Alpha.Announcements, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null',
                    'ConjunctionOperator':0,
                    'SpecificationOperator':1,
                    'ValueRange':null
                 },
                 {  
                    'Value':[
                       3,
                       4
                    ],
                    'Levels':[
                       3,
                       4
                    ],
                    'ValueRange':{
                       '0':'All',
                       '2':'Agent',
                       '3':'Master',
                       '4':'Super'
                    },
                    'Id':'e6cacf4d-37d8-4031-a17c-8b02600d4c3f',
                    'Name':'User Level',
                    'Combinable':true,
                    'TypeName':'Alpha.Announcements.Specs.UserLevelSpecification, Alpha.Announcements, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null',
                    'ConjunctionOperator':2,
                    'SpecificationOperator':1
                 }
              ]";

            var model = criteria.Id == null ?
                new Announcement
                {
                    AnnouncementType = AnnouncementType.HeaderPopup,
                    FromDate = DateTime.Now,
                    ToDate = DateTime.Now,
                    Order = 1,
                    Environments = new List<string> { "Local" },
                    Systems = new List<string> { "MBC" },
                    Contents = AppSettings.AnnouncementConfig.Languages.Select(
                            language => new MessageContent
                            {
                                LanguageKey = language,
                                Text = string.Empty
                            }),
                    Specifications = JsonConvert.DeserializeObject<IEnumerable<ISpecification<IUser>>>(
                    specs,
                    new SpecificationConverter())
                }
                : announcementService.EditAnnouncement(criteria);

            var specifications = announcementService.GetAllSpecifications();

            var viewModel = new SpecificationSettingViewModel(model, specifications);

            return View(viewModel);
        }
    }
}