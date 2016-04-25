// --------------------------------------------------------------------------
// <copyright file="PermissionService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.10.18">Created</change>
//   <change who="Ricky.Nguyen" date="2012.11.20">Update Check Permission</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Services
{
    using System.Collections.Generic;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Configuration;
    using Fanex.NAP.Core.Models.ValueObject;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Session;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PermissionService
    {
        private static ICurrentSession _currentSession;

        private static IUserService _userService;

        static PermissionService()
        {
            _userService = new UserService();
            _currentSession = new CurrentSession();
        }

        /// <summary>
        /// HasPermission: check that user has permission to access one menu link.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>
        /// Has Read Permission indicator.
        /// </returns>
        public static bool HasReadPermission(string functionName)
        {
            return HasPermission(functionName);
        }

        public static bool HasHostPermission(string functionName)
        {
            return HasPermission(functionName, Constant.Policy.Host);
        }

        public static bool HasPermission(string functionName, Constant.Policy functionPolicy = Constant.Policy.Read)
        {
            return _userService.IsHavingPermission(
                _currentSession.UserId,
                NapSettings.Settings.SiteId,
                functionName,
                functionPolicy);
        }
    }
}