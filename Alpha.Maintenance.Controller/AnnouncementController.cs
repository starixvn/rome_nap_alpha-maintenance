namespace Alpha.Maintenance.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Alpha.Maintenance.Service.Impl;
    using Service.Interface;
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

        public JsonResult GetSpecifications()
        {
            return new JsonNetResult
            {
                Data = announcementService.GetAllSpecifications(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}