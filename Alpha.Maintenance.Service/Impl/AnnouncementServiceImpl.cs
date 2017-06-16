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
    using Fanex.Data.Repository;
    using Interface;
    using Newtonsoft.Json;
    using Repository.Criteria.Announcements;
    using Repository.Impl;

    public class AnnouncementServiceImpl : AnnouncementService
    {
        private readonly SpecificationFactory specificationFactory;
        private readonly AnnouncementRepository announcementRepository;
        private readonly IDynamicRepository dynamicRepository;

        private readonly IDictionary<string, Func<dynamic>> getValueRangeFunction;

        public AnnouncementServiceImpl(
            IDynamicRepository dynamicRepository = null,
            SpecificationFactory specificationFactory = null,
            AnnouncementRepository announcementRepository = null)
        {
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
            this.specificationFactory = specificationFactory ?? new SpecificationFactoryImpl();
            this.announcementRepository = announcementRepository ?? new AnnouncementRepositoryImpl();
            getValueRangeFunction = new Dictionary<string, Func<dynamic>>
            {
                [SiteSpecification.SpecificationId] = GetSites
                // Add more initial data function here
            };
        }

        public IEnumerable<ISpecification<IUser>> GetAllSpecifications()
        {
            var specs = specificationFactory.LoadSpecifications();

            foreach (var spec in specs)
            {
                if (getValueRangeFunction.ContainsKey(spec.Id))
                {
                    spec.ValueRange = getValueRangeFunction[spec.Id]();
                    spec.Value = new List<string>
                    {
                        "121212",
                        "3434343"
                    };
                }
            }

            return specs;
        }

        public Announcement GetAnnouncement(int announcementId)
        {
            throw new NotImplementedException();
        }

        private dynamic GetSites()
        {
            return announcementRepository.GetSites();
        }

        public IEnumerable<Announcement> GetAnnouncements(DateTime date, string environment, string system)
        {
            // Get from repository and filter
            return new List<Announcement>
            {
                new Announcement
                {
                    Id = 2323,
                    AnnouncementType = AnnouncementType.HeaderPopup,
                    Code = "3434343",
                    Title = "Announcement Title",
                    IsActive = true,
                    Specifications = GetAllSpecifications(),
                    Contents = new List<MessageContent>
                    {
                        new MessageContent
                        {
                            LanguageKey = "en-us",
                            Text = " asdasd asdad d asd asd"
                        }
                    }
                }
            };
        }

        public IEnumerable<Announcement> GetAnnouncements(GetAnnouncementsCriteria criteria)
        {
            var announcements = announcementRepository.GetAnnouncements(criteria);

            return announcements;
        }

        public Result InsertAnnouncements(Announcement announcement, string specifications)
        {
            return announcementRepository.InsertAnnouncement(
                    new InsertAnnouncementsCriteria
                    {
                        Id = announcement.Id,
                        Code = announcement.Code,
                        Title = announcement.Title,
                        IsActive = announcement.IsActive,
                        AnnouncementType = (int)announcement.AnnouncementType,
                        FromDate = announcement.FromDate,
                        ToDate = announcement.ToDate,
                        Environments = String.Join(", ", announcement.Environments.ToArray()),
                        Systems = String.Join(", ", announcement.Systems.ToArray()),
                        Order = announcement.Order,
                        Contents = JsonConvert.SerializeObject(announcement.Contents),
                        Specifications = specifications
                    });
        }

        public Announcement EditAnnouncement(EditAnnouncementsCriteria criteria)
        {
            var announcement = announcementRepository.EditAnnouncement(criteria);
            announcement.Contents = CorrectContent(announcement.Contents);
            return announcement;
        }

        public IEnumerable<MessageContent> CorrectContent(IEnumerable<MessageContent> contents)
        {
            IEnumerable<MessageContent> temp = AppSettings.AnnouncementConfig.Languages.Select(
                language => new MessageContent
                {
                    LanguageKey = language,
                    Text = contents?.FirstOrDefault(content => content.LanguageKey.Equals(language, StringComparison.OrdinalIgnoreCase))?.Text
                });

            return temp;
        }
    }
}