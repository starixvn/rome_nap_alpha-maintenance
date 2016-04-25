// --------------------------------------------------------------------------
// <copyright file="ILicenseeSettingRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Forrest.Nguyen" date="2013.10.23">Created</change>
// </history>

namespace Fanex.Maintenance.Repository.Interface.LicenseeSetting
{
    using Fanex.Maintenance.Repository.Criteria.LrfDomain;

    /// <summary>
    /// Licensee Setting Repository Interface.
    /// </summary>
    public interface ILicenseeSettingRepository
    {
        /// <summary>
        /// Active Domain Up.
        /// </summary>
        /// <param name="criteria">Domain criteria.</param>
        void ActiveDomain(ActiveDomainCriteria criteria);
    }
}
