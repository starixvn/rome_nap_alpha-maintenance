namespace Alpha.Maintenance.Controller
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Announcements.Models;
    using Core;
    using Repository.Criteria.Announcements;
    using Service.Impl.Announcement;
    using Service.Interface.Announcement;
    using Web;

    [HmacAuthorizationFilter]
    public class AnnouncementApiController : ApiController
    {
        private readonly AnnouncementService announcementService;

        public AnnouncementApiController()
            : this(null)
        {
        }

        public AnnouncementApiController(AnnouncementService announcementService)
        {
            this.announcementService = announcementService ?? new AnnouncementServiceImpl();
        }

        [HttpPost]
        public IEnumerable<Announcement> GetAnnouncements(string environment, string system)
        {
           var result = announcementService.GetAnnouncements(new SelectAnnouncementsCriteria
            {
                FromDate = SystemTime.Today,
                ToDate = SystemTime.Today,
                System = system,
                Environment = environment,
                IsActive = true,
                FilterBy = FilterBy.ExpiredDate
            });

            return result;
        }
    }
}