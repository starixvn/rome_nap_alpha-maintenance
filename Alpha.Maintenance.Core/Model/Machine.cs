// --------------------------------------------------------------------------
// <copyright file="Machine.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Jason.Ly" date="2013.8.7">Created</change>
// </history>

namespace Alpha.Maintenance.Core.Model
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Machine model.
    /// </summary>
    public class Machine
    {
        private const int MaxTimeZone = 12;
        private const int MinTimeZone = -12;

        public int MachineId { get; set; }

        [Required]
        public string MachineName { get; set; }

        public bool IsProduction { get; set; }

        [Range(MinTimeZone, MaxTimeZone)]
        public short TimeZone { get; set; }

        public string MachineIP { get; set; }
    }
}