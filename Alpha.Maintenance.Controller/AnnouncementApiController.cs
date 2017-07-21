namespace Alpha.Maintenance.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http;
    using Announcements.Models;
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
                => announcementService.GetAnnouncements(new SelectAnnouncementsCriteria
                {
                    FromDate = DateTime.Today,
                    ToDate = DateTime.Today,
                    System = system,
                    Environment = environment,
                    IsActive = true,
                    FilterBy = FilterBy.ExpiredDate
                });
    }
}