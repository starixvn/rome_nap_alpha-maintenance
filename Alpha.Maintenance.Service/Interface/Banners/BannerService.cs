namespace Alpha.Maintenance.Service.Interface.Banners
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Enums;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Repository.Criteria.Banners;

    public interface BannerService
    {
        IEnumerable<Banner> GetBanners(
            BannerFilterBy filterBy,
            DateTime fromDate,
            DateTime toDate,
            string environment,
            string site);

        Tuple<Banner, IDictionary<string, string>, IDictionary<string, string>> GetBanner(int id);

        Result AddBanner(
            AddBannerCriteria criteria,
            Hashtable banners);

        Result EditBanner(
            EditBannerCriteria criteria,
            Hashtable banners);
    }
}
