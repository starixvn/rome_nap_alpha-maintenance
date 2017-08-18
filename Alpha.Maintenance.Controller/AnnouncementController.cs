namespace Alpha.Maintenance.Controller
{
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using Announcements.Models;
    using Core;
    using Core.Model;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Bson;
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
                    IsActive = demand.DeserializeAnnouncement.IsActive,
                    AnnouncementType = (int)demand.DeserializeAnnouncement.AnnouncementType,
                    CreatedDate = SystemTime.Today,
                    FromDate = demand.DeserializeAnnouncement.FromDate,
                    ToDate = demand.DeserializeAnnouncement.ToDate,
                    Environments = demand.IsCopy ? string.Empty : demand.Environments,
                    Systems = demand.Systems,
                    Ordinal = demand.DeserializeAnnouncement.Ordinal,
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
                      IsActive = false,
                      AnnouncementType = (int)demand.DeserializeAnnouncement.AnnouncementType,
                      FromDate = demand.DeserializeAnnouncement.FromDate,
                      ToDate = demand.DeserializeAnnouncement.ToDate,
                      Environments = demand.Environments,
                      Systems = demand.Systems,
                      Ordinal = demand.DeserializeAnnouncement.Ordinal,
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
        public FileStreamResult Export(SelectAnnouncementByIdCriteria criteria)
        {
            var model = announcementService.GetAnnouncementById(criteria);
            var byteArray = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(model));
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", $"{model.Title}.txt");
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.AnnouncementManagement, Policy = Constant.Policy.Host)]
        public JsonResult Import(HttpPostedFileBase file)
        {
            string announcement = string.Empty;
            using (BinaryReader b = new BinaryReader(file.InputStream))
            {
                byte[] binData = b.ReadBytes(file.ContentLength);
                announcement = Encoding.UTF8.GetString(binData);
            }

            var demand = new SaveAnnouncementDemand(announcement);

            return new JsonNetResult
            {
                Data = announcementService.SaveAnnouncement(new InsertAnnouncementsCriteria
                {
                    Code = demand.DeserializeAnnouncement.Code,
                    Title = $"Import of {demand.DeserializeAnnouncement.Title}",
                    IsActive = false,
                    AnnouncementType = (int)demand.DeserializeAnnouncement.AnnouncementType,
                    CreatedDate = SystemTime.Today,
                    FromDate = demand.DeserializeAnnouncement.FromDate,
                    ToDate = demand.DeserializeAnnouncement.ToDate,
                    Environments = string.Empty,
                    Systems = demand.Systems,
                    Ordinal = demand.DeserializeAnnouncement.Ordinal,
                    Contents = demand.EncodedContents,
                    Specifications = JsonConvert.SerializeObject(demand.DeserializeAnnouncement.Specifications),
                    CreatorId = CurrentSession.UserId,
                    CreatorName = CurrentSession.Username
                }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}