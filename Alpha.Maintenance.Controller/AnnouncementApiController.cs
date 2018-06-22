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
        public IEnumerable<Announcement> GetAnnouncementsHistory(AnnouncementHistoryRequest request)
            => announcementService.GetAnnouncements(
                new SelectAnnouncementsCriteria
                {
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    System = request.System,
                    Environment = request.Environment,
                    IsActive = true,
                    FilterBy = FilterBy.ExpiredDate
                });
    }

    public class AnnouncementHistoryRequest
    {   
        public string Environment { get; set; }

        public string System { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }
    }
}