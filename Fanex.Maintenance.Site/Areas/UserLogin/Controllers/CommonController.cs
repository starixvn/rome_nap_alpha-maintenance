// --------------------------------------------------------------------------
// <copyright file="CommonController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.1.4">Created</change>
// </history>

namespace Fanex.NAP.Hub.Areas.UserLogin.Controllers
{
    using System.Web.Mvc;
    using System.Web.Security;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;

    /// <summary>
    /// Common controller.
    /// </summary>
    public class CommonController : Controller
    {
        private IMaintenanceService _maintenanceService;

        public CommonController()
            : this(null)
        {
        }

        public CommonController(IMaintenanceService maintenanceService)
        {
            _maintenanceService = maintenanceService ?? new MaintenanceService();
        }

        /// <summary>
        /// Offline page.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult Offline()
        {
            var downTime = _maintenanceService.GetUnderMaintenanceTime();

            if (!downTime.IsUnderMaintenanceTime)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            ViewBag.UmtFrom = downTime.From;
            ViewBag.UmtTo = downTime.To;

            return View();
        }

        // GET: /UserControl/Common/
        public ActionResult Unauthorize()
        {
            return View();
        }

        /// <summary>
        /// Error page.
        /// </summary>
        /// <param name="errorPath">The error path.</param>
        /// <returns>Action result.</returns>
        public ActionResult Error(string errorPath)
        {
            if (!string.IsNullOrEmpty(errorPath))
            {
                return RedirectToAction("Error");
            }

            return View();
        }

        /// <summary>
        /// Not found page.
        /// </summary>
        /// <param name="errorPath">The error path.</param>
        /// <param name="url">The request URL.</param>
        /// <returns>Action result.</returns>
        public ActionResult NotFound(string errorPath, string url)
        {
            if (!string.IsNullOrEmpty(errorPath))
            {
                ViewBag.RequestedUrl = errorPath;

                return RedirectToAction("NotFound", new { url = errorPath });
            }

            ViewBag.RequestedUrl = url;

            return View();
        }
    }
}