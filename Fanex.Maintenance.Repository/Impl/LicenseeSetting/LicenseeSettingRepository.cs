// --------------------------------------------------------------------------
// <copyright file="LicenseeSettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Forrest.Nguyen" date="2013.10.23">Created</change>
// </history>

namespace Fanex.Maintenance.Repository.Impl.LicenseeSetting
{
    using Fanex.Data;
    using Fanex.Maintenance.Core.Model;
    using Fanex.Maintenance.Repository.Criteria.LrfDomain;
    using Fanex.Maintenance.Repository.Interface.LicenseeSetting;

    /// <summary>
    /// Licensee Setting Repository.
    /// </summary>
    public class LicenseeSettingRepository : BaseRepository<LrfDomain>, ILicenseeSettingRepository
    {
        /// <summary>
        /// Active Domain Up.
        /// </summary>
        /// <param name="criteria">Domain criteria.</param>
        public void ActiveDomain(ActiveDomainCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria);
            }
        }
    }
}
