namespace Alpha.Maintenance.Controller.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Announcements.Models;
    using Core;
    using Core.Extension;
    using Repository.Criteria.Announcements;

    public class ManageAnnouncementsViewModel
    {
        public DateTime FromDate { get; set; } = SystemTime.Today;

        public DateTime ToDate { get; set; } = SystemTime.Today;

        public string Environment { get; set; }

        public string System { get; set; }

        public FilterBy SearchBy { get; set; } = FilterBy.CreatedDate;

        public IEnumerable<Announcement> Announcements { get; set; }       

        public string FormatExpireDate(AnnouncementType? announcementType, DateTime expiredDate)
            => announcementType == AnnouncementType.EmergencyPopup ? expiredDate.ToShortDate() : string.Empty;
    }
}
