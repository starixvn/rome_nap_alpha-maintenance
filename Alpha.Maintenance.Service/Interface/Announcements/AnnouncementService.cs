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

        IEnumerable<Announcement> GetAnnouncements(CriteriaBase criteria);
        
        Result SaveAnnouncement(SaveAnnouncementsCriteria criteria);

        Announcement GetAnnouncementById(SelectAnnouncementByIdCriteria criteria);

        Announcement GetDefaultAnnouncement();
    }
}