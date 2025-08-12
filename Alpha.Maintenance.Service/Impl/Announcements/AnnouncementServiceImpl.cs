namespace Alpha.Maintenance.Service.Impl.Announcement
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Alpha.Maintenance.Repository.DTOs;
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

    public class AnnouncementServiceImpl : AnnouncementService
    {
        private readonly BaseAnnouncementService baseAnnouncementService;
        private readonly IDynamicRepository dynamicRepository;

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
            IDynamicRepository dynamicRepository = null,
            BaseAnnouncementService baseAnnouncementService = null,
            IEnumerable<string> languages = null,
            IEnumerable<string> excludedSpecifications = null,
            bool? getAnnouncementsByJson = null)
        {
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
            this.getAnnouncementsByJson = getAnnouncementsByJson ?? AppSettings.AnnouncementConfig.GetAnnouncementsByJson;

            getValueRangeFunction = new Dictionary<string, Func<dynamic>>
            {
                // Add more initial data function here
            };

            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public IEnumerable<ISpecification<IParam>> GetOfferedSpecifications()
        {
            var factorySpecs = baseAnnouncementService.GetFactorySpecifications();

            var offeredSpecs = baseAnnouncementService.FiterOfferedSpecs(factorySpecs, excludedSpecifications)
                .Where(spec => getValueRangeFunction.ContainsKey(spec.Id))
                .Select(spec =>
                {
                    spec.ValueRange = getValueRangeFunction[spec.Id]();
                    return spec;
                });

            return offeredSpecs;
        }

        public IEnumerable<Announcement> GetAnnouncements(CriteriaBase criteria)
        {
            var announcements = getAnnouncementsByJson == true ?
                GetAnnouncementsByJsonFile() : FetchAnnouncements(criteria);

            return announcements.Select(announcement =>
            {
                announcement.Specifications = baseAnnouncementService.FiterOfferedSpecs(announcement.Specifications, excludedSpecifications);
                return announcement;
            })
            .OrderBy(announcement => announcement.ReleasedDate);
        }

        public Announcement GetAnnouncementById(SelectAnnouncementByIdCriteria criteria)
        {
            var announcementDto = dynamicRepository.Get<AnnouncementDTO>(criteria);
            var announcement = MapToAnnouncement(announcementDto);
            announcement.Contents = baseAnnouncementService.CorrectContent(announcement.Contents, languages);
            announcement.Specifications = baseAnnouncementService.FiterOfferedSpecs(announcement.Specifications, excludedSpecifications);

            return announcement;
        }

        public Result SaveAnnouncement(SaveAnnouncementsCriteria criteria)
        {
            dynamicRepository.Execute(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrCode == 0
                            ? AnnouncementResources.SaveSuccessfully
                            : criteria.ErrMsg
            };
        }

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
            using (StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, JsonPath)))
            {
                return JsonConvert.DeserializeObject<List<Announcement>>(reader.ReadToEnd(), new SpecificationConverter());
            }
        }

        private IEnumerable<Announcement> FetchAnnouncements(CriteriaBase criteria)
        {
            return dynamicRepository.Fetch<AnnouncementDTO>(criteria).Select(dto => MapToAnnouncement(dto));
        }

        private Announcement MapToAnnouncement(AnnouncementDTO announcementDTO)
        {
            return new Announcement
            {
                Id = announcementDTO.Id,
                ReleasedDate = announcementDTO.FromDate,
                ExpiredDate = announcementDTO.ToDate,
                Environments = announcementDTO.Environments.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                        .Select(environment => environment.Trim()),
                Systems = announcementDTO.Systems.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                    .Select(system => system.Trim()),
                Contents = JsonConvert.DeserializeObject<IEnumerable<MessageContent>>(Encoding.UTF8.GetString(Convert.FromBase64String(announcementDTO.Contents))),
                Specifications = JsonConvert.DeserializeObject<IEnumerable<ISpecification<IParam>>>(
                            announcementDTO.Specifications,
                            new SpecificationConverter()),
                IsPopup = announcementDTO.AnnouncementType == "3",
                Code = announcementDTO.Code,
                Title = announcementDTO.Title,
                CreatedDate = announcementDTO.CreatedDate,
                CreatorId = announcementDTO.CreatorId,
                CreatorName = announcementDTO.CreatorName,
                LastEditDate = announcementDTO.LastEditDate,
                LastEditorId = announcementDTO.LastEditorId,
                LastEditorName = announcementDTO.LastEditorName,
                IsActive = announcementDTO.IsActive
            };
        }
    }
}