namespace Alpha.Maintenance.Service.Impl.Announcement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Announcements.Models;
    using Announcements.Services;
    using Announcements.Specs;
    using Core.Configurations;
    using Core.Model;
    using Interface.Announcement;
    using Newtonsoft.Json;
    using Repository.Criteria.Announcements;
    using Repository.Impl;

    public class AnnouncementServiceImpl : AnnouncementService
    {
        private readonly BaseAnnouncementService baseAnnouncementService;
        private readonly AnnouncementRepository announcementRepository;

        private readonly IDictionary<string, Func<dynamic>> getValueRangeFunction;

        private IEnumerable<string> languages { get; set; }

        private IEnumerable<string> offeredSpecifications { get; set; }

        public AnnouncementServiceImpl()
            : this(null, null, null, null, null)
        {
        }

        public AnnouncementServiceImpl(
            SpecificationFactory specificationFactory = null,
            AnnouncementRepository announcementRepository = null,
            BaseAnnouncementService baseAnnouncementService = null,
            IEnumerable<string> languages = null,
            IEnumerable<string> offeredSpecifications = null)
        {
            this.announcementRepository = announcementRepository ?? new AnnouncementRepositoryImpl();
            this.baseAnnouncementService = baseAnnouncementService ?? new BaseAnnouncementServiceImpl(specificationFactory);

            this.languages = languages ?? new List<string>{
                                                            "en-US",
                                                            "vi-VN",
                                                            "th-TH",
                                                            "zh-CN",
                                                            "zh-TW",
                                                            "ko-KR",
                                                            "ja-JP"
                                                        };

            this.offeredSpecifications = offeredSpecifications ?? AppSettings.AnnouncementConfig.OfferedSpecifications;

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

        public IEnumerable<Announcement> GetAnnouncements(SelectAnnouncementsCriteria criteria)
                => announcementRepository.GetAnnouncements(criteria);

        public Result InsertOrUpdateAnnouncement(Announcement announcement, ActionType Action)
        {
            var contentsBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(announcement.Contents));
            var encodedContents = Convert.ToBase64String(contentsBytes);

            return announcementRepository.InsertOrUpdateAnnouncement(
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
                        Contents = encodedContents,
                        Specifications = JsonConvert.SerializeObject(announcement.Specifications)
                    });
        }

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
               IsActive = false,
               Specifications = new List<ISpecification<IParam>>()
           };
    }
}