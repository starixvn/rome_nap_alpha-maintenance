namespace Alpha.Maintenance.Service.Impl.Banners
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;
    using Alpha.Maintenance.Core;
    using Alpha.Maintenance.Core.Enums;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Repository.Criteria.Banners;
    using Alpha.Maintenance.Service.Interface.Banners;
    using Fanex.Data.Repository;
    using Fanex.Logging;

    public class BannerServiceImpl : BannerService
    {
        private readonly IDynamicRepository dynamicRepository;

        public BannerServiceImpl(IDynamicRepository dynamicRepository = null)
        {
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public IEnumerable<Banner> GetBanners(
            BannerFilterBy filterBy,
            DateTime fromDate,
            DateTime toDate,
            string environment,
            string site)
            => dynamicRepository.Fetch<Banner>(
                new GetBannerCriteria(filterBy, fromDate, toDate, environment, site)).ToList();

        public Tuple<Banner, IDictionary<string, string>, IDictionary<string, string>> GetBanner(int id)
        {
            var desktopBanners = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var mobileBanners = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            var (banners, bannerDetails) = dynamicRepository.FetchMultiple<Banner, BannerDetail>(new GetBannerByIdCriteria(id));
            var banner = banners.FirstOrDefault();

            if (bannerDetails != null)
            {
                foreach (var detail in bannerDetails)
                {
                    if (detail.ImageContent != null)
                    {
                        var bannerHash =  (BannerType)detail.Type == BannerType.Desktop ? desktopBanners : mobileBanners;
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
            Hashtable banners)
        {
            var criteria = new AddBannerCriteria
            {
                BannerName = title,
                IsActive = isActive,
                ReleasedDate = releasedDate,
                ExpirationDate = expirationDate,
                Environments = environments,
                Systems = sites,
                Creator = userId
            };

            MapHashToType(banners, ref criteria);

            return SaveBanner(criteria);
        }

        public Result EditBanner(
            int id,
            string title,
            bool isActive,
            DateTime releasedDate,
            DateTime expirationDate,
            string environments,
            string sites,
            int userId,
            Hashtable banners)
        {
            var criteria = new EditBannerCriteria
            {
                BannerId = id,
                BannerName = title,
                IsActive = isActive,
                ReleasedDate = releasedDate,
                ExpirationDate = expirationDate,
                Environments = environments,
                Systems = sites,
                Creator = userId
            };

            MapHashToType(banners, ref criteria);

            return SaveBanner(criteria);
        }

        private Result SaveBanner(SaveBannerCriteria criteria)
        {
            dynamicRepository.Execute(criteria);

            var result = new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };

            if (result.IsSuccessful)
            {
                criteria.IsDBStandby = true;
                result.Message = ViewResource.UploadedBannerSuccessfully;
                SaveBannerOnDBStandby(criteria);
            }

            return result;
        }

        private void SaveBannerOnDBStandby(SaveBannerCriteria criteria)
        {
            try
            {
                dynamicRepository.Execute(criteria);
            }
            catch (Exception ex)
            {
                LogService.Error(ex.Message, ex);
            }
        }


        private static T MapHashToType<T>(Hashtable hashtable, ref T type)
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            var caseInsensitiveHash = new Dictionary<string, object>(
            hashtable.Cast<DictionaryEntry>()
                    .ToDictionary(x => x.Key.ToString(), x => x.Value),
            StringComparer.OrdinalIgnoreCase);

            foreach (PropertyInfo property in properties)
            {
                if(caseInsensitiveHash.TryGetValue(property.Name, out object value) && value != null)
                {
                    if (property.PropertyType != value.GetType())
                    {
                        value = Convert.ChangeType(value, property.PropertyType);
                    }
                    property.SetValue(type, value);
                } 
            }

            return type;
        }

    }
}
