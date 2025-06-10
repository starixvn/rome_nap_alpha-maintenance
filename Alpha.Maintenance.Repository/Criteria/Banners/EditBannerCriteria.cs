namespace Alpha.Maintenance.Repository.Criteria.Banners
{
    public class EditBannerCriteria : SaveBannerCriteria
    {
        public int BannerId { get; set; }

        public override string GetSettingKey()
            => IsDBStandby ? "Age_Banner_Update_Standby" : "Age_Banner_Update";

        public override bool IsValid()
            => true;
    }
}
