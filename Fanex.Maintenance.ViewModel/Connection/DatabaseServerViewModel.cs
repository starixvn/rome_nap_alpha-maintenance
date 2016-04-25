// --------------------------------------------------------------------------
// <copyright file="DatabaseServerViewModel.cs" company="Nexcel Solutions Vietnam">
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
    /// The database server view model.
    /// </summary>
    public class DatabaseServerViewModel
    {
        private string _name = string.Empty;
        private string _serverIp = string.Empty;
        private string _description = string.Empty;

        public int Id { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 5,
           ErrorMessageResourceName = "InputLengthValidationMsg", ErrorMessageResourceType = typeof(ViewResources))]
        public string Name
        {
            get { return _name != null ? _name.Trim() : string.Empty; }
            set { _name = value; }
        }

        [Required]
        [StringLength(150, MinimumLength = 10,
           ErrorMessageResourceName = "InputLengthValidationMsg", ErrorMessageResourceType = typeof(ViewResources))]
        public string ServerIP
        {
            get { return _serverIp != null ? _serverIp.Trim() : string.Empty; }
            set { _serverIp = value; }
        }

        public string Description
        {
            get { return _description != null ? _description.Trim() : string.Empty; }
            set { _description = value; }
        }

        public bool IsActive { get; set; }
    }
}
