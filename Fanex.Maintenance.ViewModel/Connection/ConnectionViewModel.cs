// --------------------------------------------------------------------------
// <copyright file="ConnectionViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.12.6">Created</change>
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
    /// The connection view model.
    /// </summary>
    public class ConnectionViewModel
    {
        private string _name = string.Empty;
        private string _login = string.Empty;
        private string _password = string.Empty;
        private string _freeText = string.Empty;

        public int Id { get; set; }
        
        public string Name
        {
            get { return _name != null ? _name.Trim() : string.Empty; }
            set { _name = value; }
        }

        [Required]
        public int DatabaseId { get; set; }

        [Required]
        public int DbServerId { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 5,
           ErrorMessageResourceName = "InputLengthValidationMsg", ErrorMessageResourceType = typeof(ViewResources))]
        public string LoginName
        {
            get { return _login != null ? _login.Trim() : string.Empty; }
            set { _login = value; }
        }

        public string Password
        {
            get { return _password != null ? _password.Trim() : string.Empty; }
            set { _password = value; }
        }

        public string FreeText
        {
            get { return _freeText != null ? _freeText.Trim() : string.Empty; }
            set { _freeText = value; }
        }

        public bool IsActive { get; set; }

        public List<Database> Databases { get; set; }

        public List<DatabaseServer> DatabaseServers { get; set; }

        public List<SelectListItem> SelectListDatabase
        {
            get
            {
                return Databases != null ? Databases.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Selected = x.Id == DatabaseId
                }).ToList() : new List<SelectListItem>();
            }
        }

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
