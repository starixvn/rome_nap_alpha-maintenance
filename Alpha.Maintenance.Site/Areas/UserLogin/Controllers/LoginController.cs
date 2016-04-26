// --------------------------------------------------------------------------
// <copyright file="LoginController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.8.28">Add properties _currentContext</change>
// </history>

namespace Fanex.NAP.Hub.Areas.UserLogin.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.Common.Extensions;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Models.ParameterObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;
    using ONEbook.UM.Services;
    using ONEbook.UM.Services.Impl;

    /// <summary>
    /// Login controller.
    /// </summary>
    public class LoginController : BaseController
    {
        #region members

        private IUserService _userService;
        private IGroupService _groupService;
        private ISiteService _siteService;
        private ICurrentSession _currentSession;
        private IMaintenanceService _maintenanceService;
        private ISecurityService _securityService;

        #endregion members

        #region ctors

        public LoginController()
            : this(null, null, null, null, null, null)
        {
        }

        public LoginController(
            IUserService userService,
            IGroupService groupService,
            ISiteService siteService,
            ICurrentSession currentSession,
            IMaintenanceService maintenanceService,
            ISecurityService securityService)
        {
            _userService = userService ?? new UserService();
            _groupService = groupService ?? new GroupService();
            _siteService = siteService ?? new SiteService();
            _currentSession = currentSession ?? new CurrentSession();
            _maintenanceService = maintenanceService ?? new MaintenanceService();
            _securityService = securityService ?? new SecurityService();
        }

        #endregion ctors

        #region sign in

        /// <summary>
        /// Index page.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            if (!NapSettings.Settings.IsNapHub && !this.EnableDirectLogin)
            {
                // if login-directly feature is disabled then redirect users to NAP Hub
                return Redirect(NapSettings.Settings.HubUrl);
            }

            var isUM = _maintenanceService.IsUnderMaintenance();

            if (isUM)
            {
                return RedirectToAction("Offline", "Common");
            }

            return View();
        }

        /// <summary>
        /// Index page.
        /// </summary>
        /// <param name="username">Login username.</param>
        /// <param name="password">Login password.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string username, string password)
        {
            if (this.EnableDirectLogin)
            {
                if (!username.IsNullOrTrimEmpty() && !password.IsNullOrTrimEmpty())
                {
                    var user = _userService.Get(username.Trim()) as User;

                    if (user != null)
                    {
                        password = Cryptography.CfsEnCode(password);

                        if (password == user.Password)
                        {
                            var siteLevelList = _userService.GetSiteLevel(user.Id, Environment);

                            if (siteLevelList.Where(x => x.Site.Id == this.SiteId).Any())
                            {
                                var group = user.Group == null ? null : _groupService.Get(user.Group.Id);

                                if ((group == null || group.IsActive)
                                    && siteLevelList.Where(x => x.Site.Id == this.SiteId && x.IsActive).Any())
                                {
                                    // check-in user
                                    var criteria = new LoginParameter
                                    {
                                        UserId = user.Id,
                                        SiteId = this.SiteId,
                                        NetSessionId = Session.SessionID,
                                        Ip = CurrentContext.IP,
                                        LoginDate = DateTime.Now,
                                        BrowserName = CurrentContext.BrowserName,
                                        CountryCode = CurrentContext.CountryCode,
                                        Domain = CurrentContext.DomainName,
                                        Platform = CurrentContext.Platform
                                    };

                                    var onlineUserId = _securityService.Login(criteria);

                                    int level = siteLevelList.Where(x => x.Site.Id == this.SiteId).FirstOrDefault().Level;

                                    // saves user session
                                    _currentSession.Level = level;
                                    _currentSession.UserId = user.Id;
                                    _currentSession.Username = username;
                                    _currentSession.OnUserId = onlineUserId;
                                    _currentSession.SessionId = Session.SessionID.ToString();

                                    CreateUserMenu(user);
                                    CreateAuthInfo(user);

                                    return Redirect(FormsAuthentication.DefaultUrl);
                                }
                                else
                                {
                                    ViewBag.Message = ViewResources.Your_account_was_suspended;
                                }
                            }
                            else
                            {
                                ViewBag.Message = ViewResources.You_have_no_authorization;
                            }
                        }
                        else
                        {
                            // wrong password
                            ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                        }
                    }
                    else
                    {
                        // user doesn't exist
                        ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                    }
                }
                else
                {
                    ViewBag.Message = ViewResources.Invalid_Username_or_Password;
                }
            }
            else
            {
                ViewBag.Message = ViewResources.You_have_no_authorization;
            }

            return View();
        }

        #endregion sign in

        #region sign out

        /// <summary>
        /// User log out.
        /// </summary>
        /// <returns>Action result.</returns>
        public ActionResult LogOut()
        {
            if (_currentSession != null && _currentSession.OnUserId != 0)
            {
                _securityService.Logout(_currentSession.OnUserId);
            }

            FormsAuthentication.SignOut();
            Response.Cookies.Clear();
            Session.Abandon();
            _currentSession.ClearAll();

            return Redirect(FormsAuthentication.LoginUrl);
        }

        #endregion sign out

        #region re check in

        /// <summary>
        /// User recheck login.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult ReCheckLogin()
        {
            var kickOutResult = "1";
            if (_currentSession == null || _currentSession.OnUserId == 0)
            {
                return Json(kickOutResult);
            }

            var isPassUM = _maintenanceService.IsPassUnderMaintenance();

            if (!isPassUM)
            {
                return Json("-1");
            }

            return Json("0");
        }

        #endregion re check in

        #region helpers

        /// <summary>
        /// Create user menu.
        /// </summary>
        /// <param name="user">User object.</param>
        private void CreateUserMenu(User user)
        {
            _currentSession.MenuItemList = (IList<MenuItem>)_userService.GetMenuItem(
                                                                                    user.Id,
                                                                                    SiteId,
                                                                                    Environment,
                                                                                    true);
        }

        /// <summary>
        /// Create authorization info.
        /// </summary>
        /// <param name="user">User object.</param>
        private void CreateAuthInfo(User user)
        {
            var currentDomain = CurrentContext.CurrentDomain;

            var authInfo = new AuthInfo();

            authInfo.UserId = user.Id;
            authInfo.OnUserId = this._currentSession.OnUserId;

            var authTicket = new FormsAuthenticationTicket(
                 1,
                user.Username,
                DateTime.Now,
                DateTime.Now.AddMinutes(20000),
                false,
                authInfo.ToXml(),
                currentDomain.Path);

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
            {
                HttpOnly = true,
                Path = currentDomain.Path,
                Name = FormsAuthentication.FormsCookieName
            };

            Response.Cookies.Add(authCookie);
        }

        #endregion helpers
    }
}