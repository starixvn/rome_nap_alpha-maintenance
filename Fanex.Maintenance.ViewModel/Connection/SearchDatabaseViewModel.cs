// --------------------------------------------------------------------------
// <copyright file="SearchDatabaseViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.12.5">Created</change>
// </history>

namespace Fanex.Maintenance.ViewModel.Connection
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.ConnectionString.Domain;

    /// <summary>
    /// The search database view model.
    /// </summary>
    public class SearchDatabaseViewModel
    {
        public SearchDatabaseViewModel()
        {
            DbServerId = 0;
            HasEditPermission = false;
        }

        public int DbServerId { get; set; }

        public List<DatabaseServer> DatabaseServers { get; set; }

        public List<SelectListItem> SelectListDatabaseServer
        {
            get
            {
                return DatabaseServers.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name, Selected = x.Id == DbServerId }).ToList();
            }
        }

        public List<Database> Database { get; set; }

        public bool HasEditPermission { get; set; }
    }
}
