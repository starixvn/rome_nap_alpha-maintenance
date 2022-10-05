namespace Alpha.Maintenance.Repository.Criteria.Banners
{
    using System;
    using System.Collections;

    public class EditBannerCriteria : SaveBannerCriteria
    {
        public EditBannerCriteria(
            int bannerId,
            string title,
            bool isActive,
            DateTime releasedDate,
            DateTime expirationDate,
            string environments,
            string sites,
            int userId,
            Hashtable banners) : base(
                title,
                isActive,
                releasedDate,
                expirationDate,
                environments,
                sites,
                userId,
                banners)
        {
            inputParams.Add(nameof(bannerId), bannerId);
        }

        public override string GetSettingKey()
            => "Age_Banner_Update";
    }
}
