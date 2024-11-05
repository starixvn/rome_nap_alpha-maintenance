// --------------------------------------------------------------------------
// <copyright file="Global.asax.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.8.30">Created</change>
// </history>

namespace Alpha.Maintenance.Site
{
    using System;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using App_Start;
    using Fanex.Caching;
    using Fanex.Data.Configuration;
    using Fanex.Logging;
    using Fanex.Logging.RabbitMQ;
    using Fanex.RemotingRef;
    using ONEbook.UM;
    using ONEbook.UM.AspNetMVC.Filters;
    using ONEbook.UM.Configurations;
    using ONEbook.UM.Helpers;
    using ONEbook.UM.NETFramework;
    using ONEbook.UM.Services;
    using ONEbook.UM.Transport;
    using RefClient.Factory;
    using RefDataModelStandard.Models;

    /// <summary>
    /// Note: For instructions on enabling IIS6 or IIS7 classic mode, visit http://go.microsoft.com/?LinkId=9394801 .
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Application start.
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var appSettings = ConfigurationManager.AppSettings;
            LogManager
                .SetDefaultLogCategory(appSettings["Fanex.Logging:DefaultCategory"])
                .Use(new RabbitMQLogging(appSettings["Fanex.Logging:RabbitMQConnectionStrings"]));

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            DbSettingProviderManager.Current.Start();

            var umConfig = new UMClientConfiguration().Initialize();
            UMClientManager
                .UseConfig(umConfig)
                .UseCacheService(new UMCacheService(new CacheService()))
                .UseRestClient(new UMRestClient(umConfig))
                .UseHttpContext(new HttpContextHelper());

            RefDataTypeContainer
                .ConnectTo(new Uri(appSettings["RemoteRefURL"]))
                .FromApplication("M7 site");

            //RefClientFactory.InitializeConfig(new ClientConfigModel
            //{
            //    RefDataWebApiUrl = new RefDataWebApiUrl
            //    {
            //        Url = appSettings["TW.RemotingRef.API"]
            //    },
            //    RefDataLocalRedisConfiguration = new RedisConfiguration
            //    {
            //        Host = appSettings["TW.RemotingRef.Redis"].Split(',').ToList(),
            //        DatabaseId = int.Parse(appSettings["TW.RemotingRef.DatabaseId"]),
            //    }
            //});

            GlobalFilters.Filters.Add(new CheckUMFilterAttribute(
                new MaintenanceService(UMClientManager.RestClient, UMClientManager.CacheService, UMClientManager.UMClientConfigs, UMClientManager.HttpContext), null));
        }

        /// <summary>
        /// Application error event handler.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event arguments.</param>
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        protected void Application_Error(object sender, EventArgs e)
        {
            // Logging
            if (!Request.IsLocal)
            {
                // Remove sensitive log data
                var formVariable = string.Empty;
                int length = Request.Form.Count;

                for (int i = 0; i < length; i++)
                {
                    if (!Request.Form.Keys[i].Equals("password", StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (i != 0)
                        {
                            formVariable += "&";
                        }

                        formVariable += string.Format("{0}={1}", Request.Form.Keys[i], Request.Form[Request.Form.Keys[i]]);
                    }
                }

                Exception ex = Server.GetLastError().GetBaseException();

                // Don't write log for HTTP exception
                if (ex is HttpException)
                {
                    return;
                }

                // Get base exception data
                string message = ex.Message +
                    "\r\nType: " + ex.GetType().ToString() +
                    "\r\nSource: " + ex.Source +
                    "\r\nDomain: " + HttpContext.Current.Request.Headers["Host"] +
                    "\r\nRequest URL: " + HttpContext.Current.Request.Url.AbsolutePath +
                    "\r\nForm Variable: " + formVariable +
                    "\r\nQuery String: " + Request.QueryString.ToString() +
                    "\r\nTarget Site: " + ex.TargetSite +
                    "\r\nStack Trace: " + ex.StackTrace;

                // Get additional exception data
                SqlException sqlException = ex as SqlException;

                if (sqlException == null && ex.InnerException != null)
                {
                    sqlException = ex.InnerException as SqlException;
                }

                if (sqlException != null)
                {
                    message += "\r\nProcedure: " + sqlException.Procedure +
                        "\r\nLine Number: " + sqlException.LineNumber +
                        "\r\nServer: " + sqlException.Server;
                }

                LogService.Error(message);
            }
        }
    }
}