namespace Alpha.Maintenance.Service.Impl.Announcement
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Announcements.Models;
    using Announcements.Services;
    using Announcements.Specs;
    using Core;
    using Core.Configurations;
    using Core.Helpers;
    using Core.Model;
    using Fanex.Data.Repository;
    using Interface.Announcement;
    using Newtonsoft.Json;
    using Repository.Criteria.Announcements;
    using Repository.Impl;

    public class AnnouncementServiceImpl : AnnouncementService
    {
        private readonly BaseAnnouncementService baseAnnouncementService;
        private readonly AnnouncementRepository announcementRepository;

        private readonly IDictionary<string, Func<dynamic>> getValueRangeFunction;
        private readonly string JsonPath = "App_Data\\announcementData.json";

        private readonly IEnumerable<string> languages;
        private readonly IEnumerable<string> excludedSpecifications;
        private readonly bool? getAnnouncementsByJson;

        public AnnouncementServiceImpl()
            : this(null)
        {
        }

        public AnnouncementServiceImpl(
            SpecificationFactory specificationFactory,
            AnnouncementRepository announcementRepository = null,
            BaseAnnouncementService baseAnnouncementService = null,
            IEnumerable<string> languages = null,
            IEnumerable<string> excludedSpecifications = null,
            bool? getAnnouncementsByJson = null)
        {
            this.announcementRepository = announcementRepository ?? new AnnouncementRepositoryImpl();
            this.baseAnnouncementService = baseAnnouncementService ?? new BaseAnnouncementServiceImpl(specificationFactory);

            this.languages = languages ?? new List<string>{
                                                            "en-US",
                                                            "vi-VN",
                                                            "zh-CN",
                                                            "zh-TW",
                                                            "zh-Hans",
                                                            "th-TH",
                                                            "ko-KR",
                                                            "ja-JP"
                                                        };

            this.excludedSpecifications = excludedSpecifications ?? AppSettings.AnnouncementConfig.ExcludedSpecifications;
            this.getAnnouncementsByJson = getAnnouncementsByJson == null ?
                AppSettings.AnnouncementConfig.GetAnnouncementsByJson : getAnnouncementsByJson;

            getValueRangeFunction = new Dictionary<string, Func<dynamic>>
            {
                // Add more initial data function here
            };
        }

        public IEnumerable<ISpecification<IParam>> GetOfferedSpecifications()
        {
            var factorySpecs = baseAnnouncementService.GetFactorySpecifications();

            var offeredSpecs = baseAnnouncementService.FiterOfferedSpecs(factorySpecs, excludedSpecifications);

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
        {
            var announcements = getAnnouncementsByJson == true ?
                GetAnnouncementsByJsonFile() : announcementRepository.GetAnnouncements(criteria);

            return announcements.Select(announcement =>
            {
                announcement.Specifications = baseAnnouncementService.FiterOfferedSpecs(announcement.Specifications, excludedSpecifications);
                return announcement;
            })
            .OrderBy(announcement => announcement.ReleasedDate);
        }

        public Announcement GetAnnouncementById(SelectAnnouncementByIdCriteria criteria)
        {
            var announcement = announcementRepository.GetAnnouncementById(criteria);
            announcement.Contents = baseAnnouncementService.CorrectContent(announcement.Contents, languages);
            announcement.Specifications = baseAnnouncementService.FiterOfferedSpecs(announcement.Specifications, excludedSpecifications);

            return announcement;
        }

        public Result SaveAnnouncement(ICriteria criteria)
            => announcementRepository.SaveAnnouncement(criteria);

        public Announcement GetDefaultAnnouncement()
           => new Announcement
           {
               ReleasedDate = SystemTime.Today,
               ExpiredDate = SystemTime.Today,
               Environments = Enumerable.Empty<string>(),
               Systems = Enumerable.Empty<string>(),
               Contents = languages.Select(language => new MessageContent
               {
                   LanguageKey = language,
                   Text = string.Empty
               }),
               IsActive = false,
               Specifications = new List<ISpecification<IParam>>()
           };

        private IEnumerable<Announcement> GetAnnouncementsByJsonFile()
        {
            using (StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory.ToString(), JsonPath)))
            {
                return JsonConvert.DeserializeObject<List<Announcement>>(reader.ReadToEnd(), new SpecificationConverter());
            }
        }
    }
}