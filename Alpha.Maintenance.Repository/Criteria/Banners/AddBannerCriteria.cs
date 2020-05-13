namespace Alpha.Maintenance.Repository.Criteria.Banners
{
    using System;
    using System.Collections;

    public class AddBannerCriteria : SaveBannerCriteria
    {
        public AddBannerCriteria(
            string title,
            bool isActive,
            DateTime releasedDate,
            DateTime expirationDate,
            string environments,
            string sites,
            int userId,
            Hashtable banners,
            string mobileBgColor) : base(
              title,
              isActive,
              releasedDate,
              expirationDate,
              environments,
              sites,
              userId,
              banners,
              mobileBgColor)
        {
        }

        public override string GetSettingKey()
            => "Age_Banner_Insert";
    }
}
