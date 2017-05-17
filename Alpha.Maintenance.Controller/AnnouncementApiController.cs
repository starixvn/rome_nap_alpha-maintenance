namespace Alpha.Maintenance.Controller
{
    using System.Collections.Generic;
    using System.Web.Http;
    using Announcements.Models;
    using Core.Model;
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
        {
            return announcementService.GetAnnouncements(request.Date, request.Environment, request.System);
        }
    }
}