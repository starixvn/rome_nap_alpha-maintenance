// --------------------------------------------------------------------------
// <copyright file="BaseController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.1.4">Created</change>
// </history>

namespace Fanex.Maintenance.Controller
{
    using System;
    using System.Configuration;
    using System.Web.Mvc;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Context;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// Base controller.
    /// </summary>
    public class BaseController : Controller
    {
        protected readonly string Environment = NapSettings.Settings.Environment;
        protected readonly int SiteId = NapSettings.Settings.SiteId;
        protected readonly CurrentContext CurrentContext;
        protected readonly CurrentSession CurrentSession;
        private readonly IUserService _userService;
        private bool _issiteLogin = NapSettings.Settings.EnableDirectLogin;
        private int _showLogTimeZone = 7;

        public BaseController()
        {
            CurrentContext = new CurrentContext();
            CurrentSession = new CurrentSession();

            ViewFlags = new ViewFlags
            {
                IsReCheckin = NapSettings.Settings.EnableReCheckIn
            };

            _userService = new UserService();

            _showLogTimeZone = Convert.ToInt32(ConfigurationManager.AppSettings["GMTShowLog"]);
        }

        /// <summary>
        /// Gets a value indicating whether the current site allows users login directly.
        /// </summary>
        /// <value>0 : users must login through NAP Hub.</value>
        public bool IsSiteLogin
        {
            get
            {
                return _issiteLogin;
            }
        }

        public int ShowLogTimeZone
        {
            get
            {
                return _showLogTimeZone;
            }
        }

        protected ViewFlags ViewFlags
        {
            get
            {
                return ViewBag.ViewFlags;
            }

            set
            {
                ViewBag.ViewFlags = value;
            }
        }

        public PartialViewResult _Menu()
        {
            ViewBag.Menu = CurrentSession.MenuItemList;
            return PartialView("~/Views/Shared/_Menu.cshtml");
        }

        public bool HasRight(string functionKey, Constant.Policy policy)
        {
            int functionPolicy = _userService.GetPermission(CurrentSession.UserId, this.SiteId, functionKey);

            return functionPolicy >= (int)policy;
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }
    }
}