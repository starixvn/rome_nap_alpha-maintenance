// --------------------------------------------------------------------------
// <copyright file="HomeController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.9.20">Created</change>
//   <change who="Ricky.Nguyen" date="2012.10.16">StyleCop</change>
// </history>

namespace Fanex.Maintenance.Controller
{
    using System.Web.Mvc;
    using Fanex.NAP.Core.Web.Attribute;

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
            return View();
        }
    }
}