namespace Alpha.Maintenance.Service.Impl.Announcement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Announcements.Models;
    using Announcements.Services;
    using Announcements.Specs;
    using Core;
    using Core.Configurations;
    using Core.Model;
    using Fanex.Data.Repository;
    using Interface.Announcement;
    using Repository.Criteria.Announcements;
    using Repository.Impl;

    public class AnnouncementServiceImpl : AnnouncementService
    {
        private readonly BaseAnnouncementService baseAnnouncementService;
        private readonly AnnouncementRepository announcementRepository;

        private readonly IDictionary<string, Func<dynamic>> getValueRangeFunction;

        private readonly int DefaultAnnoucementOrdinal = 30;

        private readonly IEnumerable<string> languages;

        private readonly IEnumerable<string> offeredSpecifications;

        private readonly IEnumerable<string> environments;

        private readonly IEnumerable<string> systems;

        public AnnouncementServiceImpl()
            : this(null, null, null, null, null)
        {
        }

        public AnnouncementServiceImpl(
            SpecificationFactory specificationFactory = null,
            AnnouncementRepository announcementRepository = null,
            BaseAnnouncementService baseAnnouncementService = null,
            IEnumerable<string> languages = null,
            IEnumerable<string> offeredSpecifications = null,
            IEnumerable<string> environments = null,
            IEnumerable<string> systems = null)
        {
            this.announcementRepository = announcementRepository ?? new AnnouncementRepositoryImpl();
            this.baseAnnouncementService = baseAnnouncementService ?? new BaseAnnouncementServiceImpl(specificationFactory);

            this.languages = languages ?? new List<string>{
                                                            "en-US",
                                                            "vi-VN",
                                                            "zh-CN",
                                                            "zh-TW",
                                                            "th-TH",
                                                            "ko-KR",
                                                            "ja-JP"
                                                        };

            this.offeredSpecifications = offeredSpecifications ?? AppSettings.AnnouncementConfig.OfferedSpecifications;
            this.environments = environments ?? AppSettings.AnnouncementConfig.Environments;
            this.systems = systems ?? AppSettings.AnnouncementConfig.Systems;

            getValueRangeFunction = new Dictionary<string, Func<dynamic>>
            {
                // Add more initial data function here
            };
        }

        public IEnumerable<ISpecification<IParam>> GetOfferedSpecifications()
        {
            var factorySpecs = baseAnnouncementService.GetFactorySpecifications();

            var offeredSpecs = baseAnnouncementService.FiterOfferedSpecs(factorySpecs, offeredSpecifications);

            foreach (var spec in offeredSpecs)
            {
                if (getValueRangeFunction.ContainsKey(spec.Id))
                {
                    spec.ValueRange = getValueRangeFunction[spec.Id]();
                }
            }

            return offeredSpecs;
        }

        public IEnumerable<Announcement> GetAnnouncements(ICriteria criteria)
            => announcementRepository.GetAnnouncements(criteria)
                .OrderBy(announcement => announcement.Ordinal)
                .ThenBy(announcement => announcement.CreatedDate);

        public Result SaveAnnouncement(ICriteria criteria)
            => announcementRepository.SaveAnnouncement(criteria);

        public Announcement GetAnnouncementById(SelectAnnouncementByIdCriteria criteria)
        {
            var announcement = announcementRepository.GetAnnouncementById(criteria);
            announcement.Contents = baseAnnouncementService.CorrectContent(announcement.Contents, languages);

            return announcement;
        }

        public Announcement GetDefaultAnnouncement()
           => new Announcement
           {
               AnnouncementType = AnnouncementType.Popup,
               FromDate = SystemTime.Today,
               ToDate = SystemTime.Today,
               Ordinal = DefaultAnnoucementOrdinal,
               Environments = environments,
               Systems = systems,
               Contents = languages.Select(language => new MessageContent
                           {
                               LanguageKey = language,
                               Text = string.Empty
                           }),
               IsActive = false,
               Specifications = new List<ISpecification<IParam>>()
           };
    }
}