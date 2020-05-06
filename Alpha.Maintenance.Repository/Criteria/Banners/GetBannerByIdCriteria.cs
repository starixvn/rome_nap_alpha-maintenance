namespace Alpha.Maintenance.Repository.Criteria.Banners
{
    using Fanex.Data.Repository;

    public class GetBannerByIdCriteria : CriteriaBase
    {
        public GetBannerByIdCriteria(int id)
        {
            BannerId = id;
        }

        public int BannerId { get; }

        public override string GetSettingKey()
            => "Age_Banner_SelectById";

        public override bool IsValid()
            => BannerId > 0;
    }
}
