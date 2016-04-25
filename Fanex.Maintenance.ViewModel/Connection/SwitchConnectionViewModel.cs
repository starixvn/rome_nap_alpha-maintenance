// --------------------------------------------------------------------------
// <copyright file="SwitchConnectionViewModel.cs" company="Nexcel Solutions Vietnam">
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
    /// Switch connection view model.
    /// </summary>
    public class SwitchConnectionViewModel
    {
        public int FromDbServerId { get; set; }

        public int ToDbServerId { get; set; }

        public List<DatabaseServer> DatabaseServers { get; set; }

        public List<SelectListItem> SelectListDatabaseServer
        {
            get
            {
                return DatabaseServers.Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                    Selected = x.Id == FromDbServerId
                }).ToList();
            }
        }

        public List<Connection> Connections { get; set; }
    }
}
