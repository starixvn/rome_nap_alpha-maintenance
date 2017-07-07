namespace Alpha.Maintenance.Controller
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Announcements.Models;
    using Repository.Criteria.Announcements;
    using Requests;
    using Service.Impl;
    using Service.Interface;
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
        public IEnumerable<Announcement> GetAnnouncements(QueryAnnouncementRequest request)
                => announcementService.GetAnnouncements(new SelectAnnouncementsCriteria
                {
                    FromDate = request.Date,
                    ToDate = request.Date,
                    System = request.System,
                    Environment = request.Environment
                });
    }
}