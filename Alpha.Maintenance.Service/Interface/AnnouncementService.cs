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
        IEnumerable<ISpecification<IParam>> GetOfferedSpecifications(IEnumerable<string> offeredSpecifications);

        IEnumerable<Announcement> GetAnnouncements(SelectAnnouncementsCriteria criteria);

        Result InsertOrUpdateAnnouncement(Announcement announcement, ActionType Action);

        Announcement GetAnnouncementById(SelectAnnouncementByIdCriteria criteria);

        Announcement GetDefaultAnnouncement(IEnumerable<string> languages);
    }
}