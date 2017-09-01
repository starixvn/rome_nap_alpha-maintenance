namespace Alpha.Maintenance.Service.Interface.Announcement
{
    using System.Collections.Generic;
    using Announcements.Models;
    using Announcements.Specs;

    public interface BaseAnnouncementService
    {
        IEnumerable<MessageContent> CorrectContent(
            IEnumerable<MessageContent> contents,
            IEnumerable<string> languages);

        IEnumerable<ISpecification<IParam>> FiterOfferedSpecs(
            IEnumerable<ISpecification<IParam>> factorySpecs,
            IEnumerable<string> excludedSpecifications);

        IEnumerable<ISpecification<IParam>> GetFactorySpecifications();
    }
}