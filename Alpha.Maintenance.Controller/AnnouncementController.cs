namespace Alpha.Maintenance.Controller
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Announcements.Models;
    using Core;
    using Core.Helpers;
    using Core.Model;
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
        private readonly bool preventedActive = false;
        private readonly string emptyEnvironments = string.Empty;

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
            bool? isSiteLogin,
            int? siteId)
            : base(currentContext,
                  currentSession,
                  userService,
                  enableReCheckIn,
                  showLogTimeZone,
                  isSiteLogin,
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
                    SearchBy = criteria.FilterBy,
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
        public JsonResult InsertAnnouncement(SaveAnnouncementDemand demand)
            => new JsonNetResult
            {
                Data = announcementService.SaveAnnouncement(new InsertAnnouncementsCriteria
                {
                    Code = demand.DeserializeAnnouncement.Code,
                    Title = demand.IsCopy ? $"Copy of {demand.DeserializeAnnouncement.Title}"
                                            : demand.DeserializeAnnouncement.Title,
                    IsActive = demand.IsCopy ? preventedActive : demand.DeserializeAnnouncement.IsActive,
                    IsPopup = demand.IsPopup,
                    CreatedDate = SystemTime.Today,
                    ReleasedDate = demand.DeserializeAnnouncement.ReleasedDate,
                    ExpiredDate = demand.IsPopup ? demand.DeserializeAnnouncement.ExpiredDate : demand.DeserializeAnnouncement.ReleasedDate,
                    Environments = demand.IsCopy ? emptyEnvironments : demand.Environments,
                    Systems = demand.Systems,
                    Contents = demand.EncodedContents,
                    Specifications = JsonConvert.SerializeObject(demand.DeserializeAnnouncement.Specifications),
                    CreatorId = CurrentSession.UserId,
                    CreatorName = CurrentSession.Username
                }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.AnnouncementManagement, Policy = Constant.Policy.Host)]
        public JsonResult UpdateAnnouncement(SaveAnnouncementDemand demand)
              => new JsonNetResult
              {
                  Data = announcementService.SaveAnnouncement(new UpdateAnnouncementsCriteria
                  {
                      Id = demand.DeserializeAnnouncement.Id,
                      Code = demand.DeserializeAnnouncement.Code,
                      Title = demand.DeserializeAnnouncement.Title,
                      IsActive = demand.DeserializeAnnouncement.IsActive,
                      IsPopup = demand.IsPopup,
                      ReleasedDate = demand.DeserializeAnnouncement.ReleasedDate,
                      ExpiredDate = demand.IsPopup ? demand.DeserializeAnnouncement.ExpiredDate : demand.DeserializeAnnouncement.ReleasedDate,
                      Environments = demand.Environments,
                      Systems = demand.Systems,
                      Contents = demand.EncodedContents,
                      Specifications = JsonConvert.SerializeObject(demand.DeserializeAnnouncement.Specifications),
                      LastEditDate = SystemTime.Today,
                      LastEditorId = CurrentSession.UserId,
                      LastEditorName = CurrentSession.Username
                  }),
                  JsonRequestBehavior = JsonRequestBehavior.AllowGet
              };

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.AnnouncementManagement, Policy = Constant.Policy.Host)]
        public FileStreamResult ExportAnAnnouncement(SelectAnnouncementByIdCriteria criteria)
        {
            var model = announcementService.GetAnnouncementById(criteria);
            return ExportToTxtFile(new List<Announcement> { model }, $"{model.Title}.txt");
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.AnnouncementManagement, Policy = Constant.Policy.Host)]
        public FileStreamResult ExportAllAnnouncements(SelectAnnouncementsCriteria criteria)
            => ExportToTxtFile(announcementService.GetAnnouncements(criteria), "announcements.txt");

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.AnnouncementManagement, Policy = Constant.Policy.Host)]
        public JsonResult Import(HttpPostedFileBase file)
        {
            string announcementsJson = string.Empty;
            using (BinaryReader b = new BinaryReader(file.InputStream))
            {
                byte[] binData = b.ReadBytes(file.ContentLength);
                announcementsJson = Encoding.Unicode.GetString(binData);
            }

            var announcements = JsonConvert.DeserializeObject<List<Announcement>>(announcementsJson, new SpecificationConverter());

            var resuts = new List<Result>();

            foreach (var announcement in announcements)
            {
                resuts.Add(announcementService.SaveAnnouncement(new InsertAnnouncementsCriteria
                {
                    Code = announcement.Code,
                    Title = $"Import of {announcement.Title}",
                    IsActive = preventedActive,
                    IsPopup = announcement.IsPopup,
                    CreatedDate = SystemTime.Today,
                    ReleasedDate = announcement.ReleasedDate,
                    ExpiredDate = announcement.ExpiredDate,
                    Environments = emptyEnvironments,
                    Systems = String.Join(", ", announcement.Systems.ToArray()),
                    Contents = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(announcement.Contents))),
                    Specifications = JsonConvert.SerializeObject(announcement.Specifications),
                    CreatorId = CurrentSession.UserId,
                    CreatorName = CurrentSession.Username
                }));

            }

            return new JsonNetResult
            {
                Data = resuts,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        private FileStreamResult ExportToTxtFile(IEnumerable<Announcement> announcements, string fileName)
        {
            var byteArray = Encoding.Unicode.GetBytes(JsonConvert.SerializeObject(announcements));
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", fileName);
        }
    }
}