// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.9.20">Created</change>
//   <change who="Ricky.Nguyen" date="2012.10.16">StyleCop</change>
// </history>

namespace Alpha.Maintenance.Controller
{
    using Fanex.NAP.Core.Web.Attribute;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    /// <summary>
    /// Home Controller.
    /// </summary>
    [NapSuperAuth]
    public class HomeController : BaseController
    {
        /// <summary>
        /// Index Page.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult Index()
        {
            if (CurrentSession.MenuItemList == null)
            {
                return View();
            }

            var result = new List<string>();
            var lvl0MenuItemList = CurrentSession.MenuItemList.Where(m => m.Parent == null).OrderBy(m => m.DisplayOrder);

            foreach (var menu in lvl0MenuItemList)
            {
                var lvl1MenuItemList = CurrentSession.MenuItemList.Where(x => x.Parent != null && x.Parent.Id == menu.Id)
                    .OrderBy(x => x.DisplayOrder);
                if (lvl1MenuItemList.Any())
                {
                    result = Enumerable.Union(result, lvl1MenuItemList.Select(subMenuItem => subMenuItem.ValidUrl)).ToList();
                }
                else
                {
                    result.Add(menu.ValidUrl);
                }
            }

            var firstHref = result.FirstOrDefault(href => !string.IsNullOrWhiteSpace(href));
            if (firstHref == null)
            {
                return View();
            }

            return Redirect(firstHref);
        }
    }
}