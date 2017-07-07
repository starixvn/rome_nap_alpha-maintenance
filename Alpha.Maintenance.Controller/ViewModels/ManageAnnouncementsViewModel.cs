using System;
using System.Collections.Generic;
using Alpha.Announcements.Models;

namespace Alpha.Maintenance.Controller.ViewModels
{
    public class ManageAnnouncementsViewModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Environment { get; set; }

        public string System { get; set; }

        public IEnumerable<Announcement> Announcements { get; set; }
    }
}
