// --------------------------------------------------------------------------
// <copyright file="UpdateMailGroupCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.MailStation.Criteria.MailGroup
{
    using Fanex.Data.Repository;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class UpdateMailGroupCriteria : CriteriaBase
    {
        public int MailGroupId { get; set; }

        public string MailGroupName { get; set; }

        public string Description { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_Group_UpdateMailGroup";
        }
    }
}
