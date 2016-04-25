// --------------------------------------------------------------------------
// <copyright file="SSOController.cs" company="Nexcel Solutions Vietnam">
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
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;
    using Fanex.Common;
    using Fanex.NAP.Core.Models;
    using Fanex.NAP.Core.Models.ParameterObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Auth;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// SSO controller.
    /// </summary>
    public class SSOController : BaseController
    {
        #region members

        private readonly int _timeExipration = 30;
        private IUserService _userService;
        private ISecurityService _securityService;
        private ICurrentSession _currentSession;

        #endregion members

        #region ctors

        public SSOController()
            : this(null, null, null)
        {
        }

        public SSOController(
            IUserService userService,
            ISecurityService securityService,
            ICurrentSession currentSession)
        {
            _userService = userService ?? new UserService();
            _securityService = securityService ?? new SecurityService();
            _currentSession = currentSession ?? new CurrentSession();
        }

        #endregion ctors

        /// <summary>
        /// Index page.
        /// </summary>
        /// <param name="userId">User object ID.</param>
        /// <param name="onuserId">Online user object ID.</param>
        /// <param name="time">Time value.</param>
        /// <returns>Action result.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index(string userId, string onuserId, string time)
        {
            userId = Cryptography.CfsDeCrypt(userId.Replace(" ", "+"));
            onuserId = Cryptography.CfsDeCrypt(onuserId.Replace(" ", "+"));
            time = Cryptography.CfsDeCrypt(time.Replace(" ", "+"));
            string currentTime = _securityService.GetTime();

            var expiration = Convert.ToDateTime(currentTime) - Convert.ToDateTime(time);

            if (expiration.TotalSeconds <= _timeExipration)
            {
                // SSO check-in
                var criteria = new SSOLoginParameter
                {
                    UserId = Convert.ToInt32(userId),
                    SiteId = SiteId,
                    CheckOnUserId = Convert.ToInt32(onuserId),
                    OnUserId = 0,
                    IP = CurrentContext.IP,
                    NetSessionId = Session.SessionID,
                    LoginDate = DateTime.Now,
                    BrowserName = CurrentContext.BrowserName,
                    CountryCode = CurrentContext.CountryCode,
                    Domain = CurrentContext.DomainName,
                    Platform = CurrentContext.Platform
                };

                var onlineUserId = _securityService.SSOLogin(criteria);

                if (onlineUserId != 0)
                {
                    var user = _userService.Get(criteria.UserId) as User;

                    // saves user session & auth info
                    _currentSession.UserId = user.Id;
                    _currentSession.Username = user.Username;
                    _currentSession.OnUserId = onlineUserId;
                    _currentSession.SessionId = Session.SessionID.ToString();
                    _currentSession.Level = _userService.GetUserLevel(user.Id, this.SiteId);

                    if (this.SiteId != 0)
                    {
                        _currentSession.MenuItemList = _userService.GetMenuItem(
                                                                                user.Id,
                                                                                SiteId,
                                                                                Environment,
                                                                                true) as IList<MenuItem>;
                    }

                    CreateAuthInfo(user);

                    return Redirect(FormsAuthentication.DefaultUrl);
                }
            }

            return Redirect(FormsAuthentication.LoginUrl);
        }

        #region utilities

        /// <summary>
        /// Create authorize information.
        /// </summary>
        /// <param name="user">Current login user.</param>
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

        #endregion utilities
    }
}