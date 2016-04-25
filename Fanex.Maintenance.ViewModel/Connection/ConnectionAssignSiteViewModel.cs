// --------------------------------------------------------------------------
// <copyright file="ConnectionAssignSiteViewModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.12.10">Created</change>
// </history>

namespace Fanex.Maintenance.ViewModel.Connection
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.ConnectionString.Domain;

    /// <summary>
    /// The connection assign site view model.
    /// </summary>
    public class ConnectionAssignSiteViewModel
    {
        private string _name = string.Empty;

        public int Id { get; set; }

        public string Name
        {
            get { return _name != null ? _name.Trim() : string.Empty; }
            set { _name = value; }
        }

        public List<Site> Sites { get; set; }

        public List<Site> SiteConnections { get; set; }

        public List<SelectListItem> SelectListSite
        {
            get
            {
                var list = new List<SelectListItem>();
                list = Sites.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name }).ToList();

                foreach (var item in SiteConnections)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Value == item.Id.ToString())
                        {
                            list[i].Selected = true;
                            break;
                        }
                    }
                }

                return list;
            }
        }

        public bool HasEditPermission { get; set; }
    }
}
