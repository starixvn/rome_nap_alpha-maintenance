namespace Alpha.Maintenance.Repository.Criteria.Banners
{
    public class AddBannerCriteria : SaveBannerCriteria
    {
        public override string GetSettingKey()
            => IsDBStandby ? "Age_Banner_Insert_Standby" : "Age_Banner_Insert";

        public override bool IsValid()
            => true;
    }
}
