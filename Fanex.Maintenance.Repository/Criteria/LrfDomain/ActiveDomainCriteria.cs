// --------------------------------------------------------------------------
// <copyright file="ActiveDomainCriteria.cs" company="Nexcel Solutions Vietnam">
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
    /// Update LRF Domain criteria.
    /// </summary>
    public class ActiveDomainCriteria : CriteriaBase
    {
        public int Id { get; set; }

        public override string GetSettingKey()
        {
            return "Lrf_ActiveDomainUpd";
        }

        public override bool IsValid()
        {
            return true;
        }  
    }
}
