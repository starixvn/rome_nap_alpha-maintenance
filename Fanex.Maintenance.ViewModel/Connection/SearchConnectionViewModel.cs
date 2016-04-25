// --------------------------------------------------------------------------
// <copyright file="SearchConnectionViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.12.9">Created</change>
// </history>

namespace Fanex.Maintenance.ViewModel.Connection
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.ConnectionString.Domain;

    /// <summary>
    /// View model for search connection.
    /// </summary>
    public class SearchConnectionViewModel
    {
        public SearchConnectionViewModel()
        {
            DbServerId = 0;
            HasEditPermission = false;
            HasSwitchConnectionPermission = false;
        }

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

        public List<Connection> Connections { get; set; }

        public bool HasEditPermission { get; set; }

        public bool HasSwitchConnectionPermission { get; set; }
    }
}
