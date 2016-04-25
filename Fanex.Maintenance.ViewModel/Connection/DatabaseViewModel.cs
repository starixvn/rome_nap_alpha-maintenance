// --------------------------------------------------------------------------
// <copyright file="DatabaseViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.12.5">Created</change>
// </history>

namespace Fanex.Maintenance.ViewModel.Connection
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.ConnectionString.Domain;
    using Fanex.NAP.Core.Web;

    /// <summary>
    /// The database view model.
    /// </summary>
    public class DatabaseViewModel
    {
        private string _name = string.Empty;
        private string _serverAlias = string.Empty;

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
        [StringLength(150, MinimumLength = 5,
           ErrorMessageResourceName = "InputLengthValidationMsg", ErrorMessageResourceType = typeof(ViewResources))]
        public string ServerAlias
        {
            get { return _serverAlias != null ? _serverAlias.Trim() : string.Empty; }
            set { _serverAlias = value; }
        }

        public bool IsActive { get; set; }

        [Required]
        public int DbServerId { get; set; }

        public List<DatabaseServer> DatabaseServers { get; set; }

        public List<SelectListItem> SelectListDatabaseServer
        {
            get
            {
                return DatabaseServers != null ? DatabaseServers.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Selected = x.Id == DbServerId
                }).ToList() : new List<SelectListItem>();
            }
        }
    }
}
