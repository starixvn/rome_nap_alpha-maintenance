// --------------------------------------------------------------------------
// <copyright file="LicenseeSettingController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Forrest.Nguyen" date="2013.10.23">Created</change>
// </history>

namespace Fanex.Maintenance.Controller
{
    using System.Web.Mvc;
    using Fanex.Maintenance.Service.Impl.LicenseeSetting;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Licensee Setting Controller.
    /// </summary>
    [NapSuperAuth]
    public class LicenseeSettingController : BaseController
    {
        #region Member

        /// <summary>
        /// Licensee Setting Service.
        /// </summary>
        private readonly LicenseeSettingService _licenseeSettingService;

        #endregion Member

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the LicenseeSettingController class.
        /// </summary>
        public LicenseeSettingController()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the LicenseeSettingController class.
        /// </summary>
        /// <param name="licenseeSettingService">Licensee Setting Service.</param>
        public LicenseeSettingController(LicenseeSettingService licenseeSettingService)
        {
            _licenseeSettingService = new LicenseeSettingService();
        }

        #endregion Ctor

        #region Action

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.LrfDomain, Policy = Constant.Policy.Read)]
        public ActionResult LrfDomain()
        {
            var domains = _licenseeSettingService.GetAll();
            ViewBag.HasEdit = HasRight(SiteFunctionKeys.LrfDomain, Constant.Policy.Host);
            return View(domains);
        }

        [HttpPost]
        public ActionResult LrfDomain(int id)
        {
            _licenseeSettingService.ActiveDomain(id);
            var domains = _licenseeSettingService.GetAll();
            ViewBag.success = true;
            ViewBag.HasEdit = HasRight(SiteFunctionKeys.LrfDomain, Constant.Policy.Host);

            return View(domains);
        }

        #endregion Action

        #region Ajax

        #endregion Ajax

        #region Private

        #endregion Private
    }
}