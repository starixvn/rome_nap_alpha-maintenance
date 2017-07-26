namespace Alpha.Maintenance.Service.Interface.Announcement
{
    using System.Collections.Generic;
    using Announcements.Models;
    using Announcements.Specs;
    using Core.Model;
    using Fanex.Data.Repository;
    using Repository.Criteria.Announcements;

    public interface AnnouncementService
    {
        IEnumerable<ISpecification<IParam>> GetOfferedSpecifications();

        IEnumerable<Announcement> GetAnnouncements(ICriteria criteria);

        Result InsertAnnouncement(ICriteria criteria);

        Result UpdateAnnouncement(ICriteria criteria);
        Announcement GetAnnouncementById(SelectAnnouncementByIdCriteria criteria);

        Announcement GetDefaultAnnouncement();
    }
}