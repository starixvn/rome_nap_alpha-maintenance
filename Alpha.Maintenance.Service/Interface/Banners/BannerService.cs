namespace Alpha.Maintenance.Service.Interface.Banners
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Enums;
    using Alpha.Maintenance.Core.Model;

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
            string title,
            bool isActive,
            DateTime releasedDate,
            DateTime expirationDate,
            string environments,
            string sites,
            int userId,
            Hashtable banners,
            string mobileBgColor);

        Result EditBanner(
            int id,
            string title,
            bool isActive,
            DateTime releasedDate,
            DateTime expirationDate,
            string environments,
            string sites,
            int userId,
            Hashtable banners,
            string mobileBgColor);
    }
}
