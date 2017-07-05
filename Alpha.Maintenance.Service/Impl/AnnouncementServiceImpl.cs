namespace Alpha.Maintenance.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Announcements.Models;
    using Announcements.Services;
    using Announcements.Specs;
    using Core.Configurations;
    using Core.Model;
    using Interface;
    using Newtonsoft.Json;
    using Repository.Criteria.Announcements;
    using Repository.Impl;

    public class AnnouncementServiceImpl : AnnouncementService
    {
        private readonly SpecificationFactory specificationFactory;
        private readonly AnnouncementRepository announcementRepository;

        private readonly IDictionary<string, Func<dynamic>> getValueRangeFunction;

        public AnnouncementServiceImpl(
            SpecificationFactory specificationFactory = null,
            AnnouncementRepository announcementRepository = null)
        {
            this.specificationFactory = specificationFactory ?? new SpecificationFactoryImpl();
            this.announcementRepository = announcementRepository ?? new AnnouncementRepositoryImpl();

            getValueRangeFunction = new Dictionary<string, Func<dynamic>>
            {
                // Add more initial data function here
            };
        }

        public IEnumerable<ISpecification<IParam>> GetAllSpecifications()
        {
            var specs = specificationFactory.LoadSpecifications();

            foreach (var spec in specs)
            {
                if (getValueRangeFunction.ContainsKey(spec.Id))
                {
                    spec.ValueRange = getValueRangeFunction[spec.Id]();
                }
            }

            return specs;
        }

        public IEnumerable<Announcement> GetAnnouncements(DateTime date, string environment, string system)
                => GetAnnouncements(new GetAnnouncementsCriteria
                {
                    FromDate = date,
                    ToDate = date,
                    Environment = environment,
                    System = system
                });

        public IEnumerable<Announcement> GetAnnouncements(GetAnnouncementsCriteria criteria) 
                => announcementRepository.GetAnnouncements(criteria);

        public Result InsertAnnouncements(Announcement announcement, ActionType Action)
                => announcementRepository.InsertAnnouncement(
                    new InsertAnnouncementsCriteria
                    {
                        Action = Action,
                        Id = announcement.Id,
                        Code = announcement.Code,
                        Title = announcement.Title,
                        IsActive = announcement.IsActive,
                        AnnouncementType = (int)announcement.AnnouncementType,
                        FromDate = announcement.FromDate,
                        ToDate = announcement.ToDate,
                        Environments = String.Join(", ", announcement.Environments.ToArray()),
                        Systems = String.Join(", ", announcement.Systems.ToArray()),
                        Ordinal = announcement.Ordinal,
                        Contents = JsonConvert.SerializeObject(announcement.Contents),
                        Specifications = JsonConvert.SerializeObject(announcement.Specifications)
                    });

        public Announcement EditAnnouncement(EditAnnouncementsCriteria criteria)
        {
            var announcement = announcementRepository.EditAnnouncement(criteria);
            announcement.Contents = CorrectContent(announcement.Contents);

            return announcement;
        }

        private IEnumerable<MessageContent> CorrectContent(IEnumerable<MessageContent> contents)
                => AppSettings.AnnouncementConfig.Languages.Select(
                        language => new MessageContent
                        {
                            LanguageKey = language,
                            Text = contents?.FirstOrDefault(content => content.LanguageKey.Equals(language, StringComparison.OrdinalIgnoreCase))?.Text
                        });
    }
}