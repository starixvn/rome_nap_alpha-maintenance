namespace Alpha.Maintenance.Service.Impl.Announcement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Announcements.Models;
    using Announcements.Services;
    using Announcements.Specs;
    using Core.Configurations;
    using Core.Model;
    using Fanex.Caching;
    using Interface.Announcement;
    using Newtonsoft.Json;
    using Repository.Criteria.Announcements;
    using Repository.Impl;

    public class AnnouncementServiceImpl : AnnouncementService
    {
        private readonly SpecificationFactory specificationFactory;
        private readonly AnnouncementRepository announcementRepository;

        private readonly IDictionary<string, Func<dynamic>> getValueRangeFunction;
        private const string SpecificationsCacheKey = "Alpha.Maintenance.Service.Impl.AnnouncementServiceImpl.AllSpecifications";

        private IEnumerable<string> languages { get; set; }
        private IEnumerable<string> offeredSpecifications { get; set; }

        public AnnouncementServiceImpl(
            SpecificationFactory specificationFactory = null,
            AnnouncementRepository announcementRepository = null,
            IEnumerable<string> languages = null,
            IEnumerable<string> offeredSpecifications = null)
        {
            this.specificationFactory = specificationFactory ?? new SpecificationFactoryImpl();
            this.announcementRepository = announcementRepository ?? new AnnouncementRepositoryImpl();
            this.languages = languages ?? new List<string>{
                                                            "en-US",
                                                            "zh-CN",
                                                            "zh-TW",
                                                            "ja-JP",
                                                            "ko-KR",
                                                            "th-TH",
                                                            "vi-VN"
                                                        };

            this.offeredSpecifications = offeredSpecifications ?? AppSettings.AnnouncementConfig.OfferedSpecifications;

            getValueRangeFunction = new Dictionary<string, Func<dynamic>>
            {
                // Add more initial data function here
            };
        }

        public IEnumerable<ISpecification<IParam>> GetOfferedSpecifications()
        {
            var factorySpecs = GetFactorySpecifications();

            var offeredSpecs = FiterOfferedSpecs(factorySpecs, offeredSpecifications);

            foreach (var spec in offeredSpecs)
            {
                if (getValueRangeFunction.ContainsKey(spec.Id))
                {
                    spec.ValueRange = getValueRangeFunction[spec.Id]();
                }
            }

            return offeredSpecs;
        }

        public IEnumerable<Announcement> GetAnnouncements(SelectAnnouncementsCriteria criteria)
                => announcementRepository.GetAnnouncements(criteria);

        public Result InsertOrUpdateAnnouncement(Announcement announcement, ActionType Action)
                => announcementRepository.InsertOrUpdateAnnouncement(
                    new InsertOrUpdateAnnouncementsCriteria
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

        public Announcement GetAnnouncementById(SelectAnnouncementByIdCriteria criteria)
        {
            var announcement = announcementRepository.GetAnnouncementById(criteria);
            announcement.Contents = CorrectContent(announcement.Contents, languages);

            return announcement;
        }

        public Announcement GetDefaultAnnouncement()
           => new Announcement
           {
               AnnouncementType = AnnouncementType.Popup,
               FromDate = DateTime.Now,
               ToDate = DateTime.Now,
               Ordinal = 1,
               Environments = new List<string> { "Local" },
               Systems = new List<string> { "IBCBet" },
               Contents = languages.Select(language => new MessageContent
                           {
                               LanguageKey = language,
                               Text = string.Empty
                           }),
               IsActive = true,
               Specifications = new List<ISpecification<IParam>>()
           };

        internal virtual IEnumerable<MessageContent> CorrectContent(
            IEnumerable<MessageContent> contents,
            IEnumerable<string> languages)
                => languages.Select(
                        language =>
                        {
                            var existedContent = contents.Single(content => content.LanguageKey.Equals(language, StringComparison.OrdinalIgnoreCase));

                            return existedContent ?? new MessageContent
                            {
                                LanguageKey = language,
                                Text = string.Empty,
                                Url = string.Empty,
                                IsDisplayingByUrl = false
                            };
                        });


        private IEnumerable<ISpecification<IParam>> FiterOfferedSpecs(
            IEnumerable<ISpecification<IParam>> factorySpecs,
            IEnumerable<string> offeredSpecifications)
                => factorySpecs.Where(spec =>
                    {
                        return offeredSpecifications.Any(existedSpec => existedSpec.Equals(spec.Name));
                    });

        private IEnumerable<ISpecification<IParam>> GetFactorySpecifications()
                => ConfiguredCacheService
                    .GetOrSet(
                    SpecificationsCacheKey,
                    () => specificationFactory.LoadSpecifications(),
                    new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromHours(24)));

    }
}