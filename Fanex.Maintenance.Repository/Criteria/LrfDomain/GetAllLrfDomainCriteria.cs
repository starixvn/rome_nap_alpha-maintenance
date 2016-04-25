// --------------------------------------------------------------------------
// <copyright file="GetAllLrfDomainCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Forrest.Nguyen" date="2013.10.23">Created</change>
// </history>

namespace Fanex.Maintenance.Repository.Criteria.LrfDomain
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get All LRF Domain Criteria.
    /// </summary>
    public class GetAllLrfDomainCriteria : CriteriaBase
    {
        public override string GetSettingKey()
        {
            return "Lrf_DomainSel";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
