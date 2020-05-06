namespace Alpha.Maintenance.Repository.Criteria.Banners
{
    using System;
    using Alpha.Maintenance.Core.Enums;
    using Fanex.Data.Repository;

    public class GetBannerCriteria : CriteriaBase
    {
        public GetBannerCriteria(
            BannerFilterBy filterBy,
            DateTime fromDate,
            DateTime toDate,
            string environment,
            string site)
        {
            FilterBy = filterBy;
            FromDate = fromDate;
            ToDate = toDate;
            Environments = environment ?? string.Empty;
            Systems = site ?? string.Empty;
        }

        public BannerFilterBy FilterBy { get; }

        public DateTime FromDate { get; }

        public DateTime ToDate { get; }

        public string Environments { get; }

        public string Systems { get; }

        public override string GetSettingKey()
            => "Age_Banner_Select";

        public override bool IsValid()
            => FromDate <= ToDate;
    }
}
