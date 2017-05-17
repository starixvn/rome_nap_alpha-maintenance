namespace Alpha.Maintenance.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Announcements.Models;
    using Announcements.Specs;
    using Core.Model;

    public interface AnnouncementService
    {
        Announcement GetAnnouncement(int announcementId);

        IEnumerable<Announcement> GetAnnouncements(DateTime date, string environment, string system);

        IEnumerable<ISpecification<IUser>> GetAllSpecifications();
    }
}