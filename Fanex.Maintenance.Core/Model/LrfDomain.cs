// --------------------------------------------------------------------------
// <copyright file="LrfDomain.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Forrest.Nguyen" date="2013.10.23">Created</change>
// </history>

namespace Fanex.Maintenance.Core.Model
{
    /// <summary>
    /// LRF Domain Class.
    /// </summary>
    public class LrfDomain
    {
        public int Id { get; set; }

        public string DomainName { get; set; }

        public bool IsActive { get; set; }
    }
}
