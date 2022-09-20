namespace Alpha.Maintenance.Repository.Criteria.Banners
{
    using System;
    using System.Collections;
    using Fanex.Data.Repository;

    public abstract class SaveBannerCriteria : CriteriaBase
    {
        protected readonly Hashtable inputParams;

        protected SaveBannerCriteria(
            string bannerName,
            bool isActive,
            DateTime releasedDate,
            DateTime expirationDate,
            string environments,
            string systems,
            int creator,
            Hashtable banners)
        {
            inputParams = new Hashtable
            {
                [nameof(bannerName)] = bannerName,
                [nameof(isActive)] = isActive,
                [nameof(releasedDate)] = releasedDate,
                [nameof(expirationDate)] = expirationDate,
                [nameof(environments)] = environments,
                [nameof(systems)] = systems,
                [nameof(creator)] = creator,
                ["ErrCode"] = 0,
                ["ErrMsg"] = string.Empty
            };

            foreach (var key in banners.Keys)
            {
                inputParams[key] = banners[key];
            }
        }

        public override Hashtable GetSpParams()
            => inputParams;
    }
}
