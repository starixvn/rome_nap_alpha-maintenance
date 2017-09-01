namespace Alpha.Maintenance.Service.Impl.Announcement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Announcements.Models;
    using Announcements.Services;
    using Announcements.Specs;
    using Fanex.Caching;
    using Interface.Announcement;

    public class BaseAnnouncementServiceImpl : BaseAnnouncementService
    {
        private const string SpecificationsCacheKey = "Alpha.Maintenance.Service.Impl.AnnouncementServiceImpl.AllSpecifications";
        private readonly SpecificationFactory specificationFactory;
        private const int SpecificationFactoryCacheTime = 1;

        public BaseAnnouncementServiceImpl(SpecificationFactory specificationFactory = null)
        {
            this.specificationFactory = specificationFactory ?? new SpecificationFactoryImpl();
        }

        public IEnumerable<MessageContent> CorrectContent(
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


        public IEnumerable<ISpecification<IParam>> FiterOfferedSpecs(
            IEnumerable<ISpecification<IParam>> factorySpecs,
            IEnumerable<string> excludedSpecifications)
                => factorySpecs.Where(spec => !excludedSpecifications.Any(existedSpec => existedSpec.Equals(spec.Name)));

        public IEnumerable<ISpecification<IParam>> GetFactorySpecifications()
            => ConfiguredCacheService
                    .GetOrSet(
                    SpecificationsCacheKey,
                    () => specificationFactory.LoadSpecifications(),
                    new CacheItemOptions().SetAbsoluteExpiration(TimeSpan.FromHours(SpecificationFactoryCacheTime)));

    }
}