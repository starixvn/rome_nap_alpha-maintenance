namespace Alpha.Maintenance.Controller.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Announcements.Models;
    using Core;
    using Repository.Criteria.Announcements;

    public class ManageAnnouncementsViewModel
    {
        public DateTime FromDate { get; set; } = SystemTime.Today;

        public DateTime ToDate { get; set; } = SystemTime.Today;

        public string Environment { get; set; }

        public string System { get; set; }

        public FilterBy FilterBy { get; set; } = FilterBy.CreatedDate;

        public IEnumerable<Announcement> Announcements { get; set; }
    }
}
