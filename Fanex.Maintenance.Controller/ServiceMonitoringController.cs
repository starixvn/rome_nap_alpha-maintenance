// --------------------------------------------------------------------------
// <copyright file="ServiceMonitoringController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.truong" date="2014.6.25">Created</change>
// </history>

namespace Fanex.Maintenance.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Maintenance.Repository.Criteria.ServiceMonitoring;
    using Fanex.Maintenance.Service.Impl.ServiceMonitoring;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.ServiceManagement.Model;

    /// <summary>
    /// Log controller.
    /// </summary>
    [NapSuperAuth]
    public class ServiceMonitoringController : BaseController
    {
        public ServiceMonitoringController()
        {
        }

        /// <summary>
        /// Log list page.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ServiceMonitoring, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            return View("~/Views/ServiceMonitoring/Index.cshtml");
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>List services.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ServiceMonitoring, Policy = Constant.Policy.Read)]
        public ActionResult GetServices(ServiceMonitoringCriteria criteria)
        {
            ServiceManagement serviceManagement = new ServiceManagement(CurrentSession.UserId, criteria.IP);
            Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
            Response.Cache.SetMaxAge(new TimeSpan(0));
            var listServices = serviceManagement.GetServices(criteria.ServiceName, criteria.Status);
            string incomingEtag = Request.Headers["If-None-Match"];
            string newETag = serviceManagement.GetEtag(listServices);
            Response.Cache.SetETag(newETag);
            if (string.Compare(incomingEtag, newETag, StringComparison.Ordinal) == 0)
            {
                // we don't need return the data which the client already have
                Response.SuppressContent = true;
                Response.StatusCode = (int)HttpStatusCode.NotModified;
                return new EmptyResult();
            }

            return Json(listServices);
        }

        /// <summary>
        /// Server login.
        /// </summary>
        /// <param name="networkLogin">The network login.</param>
        /// <returns>JSON result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ServiceMonitoring, Policy = Constant.Policy.Read)]
        public ActionResult ServerLogin(NetworkLogin networkLogin)
        {
            ServiceManagement serviceManagement = new ServiceManagement(networkLogin);
            var result = serviceManagement.TestConnection();
            if (result)
            {
                UserManagement userManagement = new UserManagement();
                userManagement.UpdateNetworkLoginByIP(CurrentSession.UserId, networkLogin);
            }

            return Json(result);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ServiceMonitoring, Policy = Constant.Policy.Host)]
        public void StartService(ServiceMonitoringCriteria criteria)
        {
            ServiceManagement serviceManagement = new ServiceManagement(CurrentSession.UserId, criteria.IP);
            serviceManagement.StartService(criteria.ServiceName);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ServiceMonitoring, Policy = Constant.Policy.Host)]
        public void StopService(ServiceMonitoringCriteria criteria)
        {
            ServiceManagement serviceManagement = new ServiceManagement(CurrentSession.UserId, criteria.IP);
            serviceManagement.StopService(criteria.ServiceName);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ServiceMonitoring, Policy = Constant.Policy.Host)]
        public void RestartService(ServiceMonitoringCriteria criteria)
        {
            ServiceManagement serviceManagement = new ServiceManagement(CurrentSession.UserId, criteria.IP);
            serviceManagement.RestartService(criteria.ServiceName);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ServiceMonitoring, Policy = Constant.Policy.Host)]
        public ActionResult GetServiceConfigs(ServiceMonitoringCriteria criteria)
        {
            ServiceManagement serviceManagement = new ServiceManagement(CurrentSession.UserId, criteria.IP);
            return Json(serviceManagement.GetServiceConfigs(criteria.ServiceName));
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.ServiceMonitoring, Policy = Constant.Policy.Host)]
        public ActionResult EditServiceConfigFile(ServiceMonitoringCriteria criteria)
        {
            ServiceManagement serviceManagement = new ServiceManagement(CurrentSession.UserId, criteria.IP);
            ViewBag.xmlstring = serviceManagement.GetServiceConfigFile(criteria.ServiceName, criteria.ConfigFileName);
            return View("~/Views/ServiceMonitoring/XMLEditor.cshtml", criteria);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ServiceMonitoring, Policy = Constant.Policy.Host)]
        [ValidateInput(false)]
        public ActionResult SaveServiceConfigFile(ServiceMonitoringCriteria criteria)
        {
            ServiceManagement serviceManagement = new ServiceManagement(CurrentSession.UserId, criteria.IP);
            return Json(serviceManagement.SaveServiceConfigFile(criteria.ServiceName, criteria.ConfigFileName, criteria.FileText));
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ServiceMonitoring, Policy = Constant.Policy.Host)]
        public ActionResult GetListServerIP()
        {
            UserManagement userManagement = new UserManagement();
            return Json(userManagement.GetListServerIP(CurrentSession.UserId));
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ServiceMonitoring, Policy = Constant.Policy.Host)]
        public void UpdateNetworkLoginByIP(NetworkLogin networkLogin)
        {
            UserManagement userManagement = new UserManagement();
            userManagement.UpdateNetworkLoginByIP(CurrentSession.UserId, networkLogin);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ServiceMonitoring, Policy = Constant.Policy.Host)]
        public ActionResult GetListFavouriteServices()
        {
            UserManagement userManagement = new UserManagement();
            return Json(userManagement.GetListFavouriteServices(CurrentSession.UserId));
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ServiceMonitoring, Policy = Constant.Policy.Host)]
        public void UpdateListFavouriteServices(List<string> favouriteServiceList)
        {
            UserManagement userManagement = new UserManagement();
            userManagement.UpdateListFavouriteServices(CurrentSession.UserId, favouriteServiceList);
        }
    }
}