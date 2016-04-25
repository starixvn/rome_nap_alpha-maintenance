// --------------------------------------------------------------------------
// <copyright file="SiteViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.12.10">Created</change>
// </history>

namespace Fanex.Maintenance.ViewModel.Connection
{
    using System.ComponentModel.DataAnnotations;
    using Fanex.NAP.Core.Web;

    /// <summary>
    /// The site view model.
    /// </summary>
    public class SiteViewModel
    {
        private string _name = string.Empty;

        public int Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 5,
           ErrorMessageResourceName = "InputLengthValidationMsg", ErrorMessageResourceType = typeof(ViewResources))]
        public string Name
        {
            get { return _name != null ? _name.Trim() : string.Empty; }
            set { _name = value; }
        }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
