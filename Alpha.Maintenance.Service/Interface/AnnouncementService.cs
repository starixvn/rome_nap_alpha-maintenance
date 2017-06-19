namespace Alpha.Maintenance.Service.Interface
{
    using System;
    using System.Collections.Generic;
    using Announcements.Models;
    using Announcements.Specs;
    using Core.Model;
    using Repository.Criteria.Announcements;

    public interface AnnouncementService
    {
        Announcement GetAnnouncement(int announcementId);

        IEnumerable<Announcement> GetAnnouncements(DateTime date, string environment, string system);

        IEnumerable<ISpecification<IUser>> GetAllSpecifications();

        IEnumerable<Announcement> GetAnnouncements(GetAnnouncementsCriteria criteria);

        Result InsertAnnouncements(Announcement announcement, ActionType Action);

        Announcement EditAnnouncement(EditAnnouncementsCriteria criteria);
    }
}