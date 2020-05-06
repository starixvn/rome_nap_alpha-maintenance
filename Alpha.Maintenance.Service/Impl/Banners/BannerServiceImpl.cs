namespace Alpha.Maintenance.Service.Impl.Banners
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Alpha.Maintenance.Core;
    using Alpha.Maintenance.Core.Enums;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Repository.Criteria.Banners;
    using Alpha.Maintenance.Repository.Impl;
    using Alpha.Maintenance.Service.Interface.Banners;
    using Fanex.Data.Repository;

    public class BannerServiceImpl : BannerService
    {
        private readonly BannerRepository bannerRepository;

        public BannerServiceImpl(BannerRepository bannerRepository = null)
        {
            this.bannerRepository = bannerRepository ?? new BannerRepositoryImpl();
        }

        public IEnumerable<Banner> GetBanners(
            BannerFilterBy filterBy,
            DateTime fromDate,
            DateTime toDate,
            string environment,
            string site)
            => bannerRepository.GetBanners(
                new GetBannerCriteria(filterBy, fromDate, toDate, environment, site));

        public Tuple<Banner, IDictionary<string, string>, IDictionary<string, string>> GetBanner(int id)
        {
            var desktopBanners = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var mobileBanners = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var bannerForEdit = bannerRepository.GetBanner(new GetBannerByIdCriteria(id));
            var banner = bannerForEdit.Item1;
            var bannerDetails = bannerForEdit.Item2;

            if (bannerDetails != null)
            {
                foreach (var detail in bannerDetails)
                {
                    if (detail.ImageContent != null)
                    {
                        var bannerHash = detail.Type == BannerType.Desktop ? desktopBanners : mobileBanners;
                        bannerHash[detail.LanguageCode] = Convert.ToBase64String(detail.ImageContent);
                    }
                }
            }

            return new Tuple<Banner, IDictionary<string, string>, IDictionary<string, string>>(
                banner,
                desktopBanners,
                mobileBanners);
        }

        public Result AddBanner(
            string title,
            bool isActive,
            DateTime releasedDate,
            DateTime expirationDate,
            string environments,
            string sites,
            int userId,
            Hashtable banners,
            string mobileBgColor)
            => SaveBanner(new AddBannerCriteria(
                title,
                isActive,
                releasedDate,
                expirationDate,
                environments,
                sites,
                userId,
                banners,
                mobileBgColor));

        public Result EditBanner(
            int id,
            string title,
            bool isActive,
            DateTime releasedDate,
            DateTime expirationDate,
            string environments,
            string sites,
            int userId,
            Hashtable banners,
            string mobileBgColor)
            => SaveBanner(new EditBannerCriteria(
                id,
                title,
                isActive,
                releasedDate,
                expirationDate,
                environments,
                sites,
                userId,
                banners,
                mobileBgColor));

        private Result SaveBanner(ICriteria criteria)
        {
            var result = bannerRepository.ExecuteNonQuery(criteria);

            if (result.IsSuccessful)
            {
                result.Message = ViewResource.UploadedBannerSuccessfully;
            }

            return result;
        }
    }
}
