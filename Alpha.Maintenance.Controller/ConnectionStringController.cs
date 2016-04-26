// --------------------------------------------------------------------------
// <copyright file="ConnectionStringController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2013.1.21">Created</change>
// </history>

namespace Fanex.Maintenance.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.ConnectionString.Core.Service.Impl;
    using Fanex.ConnectionString.Core.Service.Interface;
    using Fanex.ConnectionString.Domain;
    using Fanex.ConnectionString.Model;
    using Fanex.Maintenance.ViewModel.Connection;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web;
    using Fanex.NAP.Core.Web.Attribute;
    using Fanex.NAP.Core.Command;

    /// <summary>
    /// Connection string controller.
    /// </summary>
    [NapSuperAuth]
    public class ConnectionStringController : BaseController
    {
        #region Member

        private readonly IDatabaseServerService _databaseServerService;
        private readonly ISiteService _siteService;
        private readonly IConnectionService _connectionService;
        private readonly ILogService _logService;
        private readonly IDatabaseService _databaseService;

        #endregion Member

        #region Ctor

        public ConnectionStringController()
            : this(null, null, null, null, null)
        {
        }

        public ConnectionStringController(
            IDatabaseServerService databaseServerService,
            ISiteService siteService,
            IConnectionService connectionService,
            ILogService logServer,
            IDatabaseService aliasDatabaseService)
        {
            _databaseServerService = databaseServerService ?? new DatabaseServerService();
            _siteService = siteService ?? new SiteService();
            _connectionService = connectionService ?? new ConnectionService();
            _logService = logServer ?? new LogService();
            _databaseService = aliasDatabaseService ?? new DatabaseService();
        }

        #endregion Ctor

        #region Connection String

        /// <summary>
        /// Connections view.
        /// </summary>
        /// <param name="model">The search connection view model.</param>
        /// <returns>Connections list view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageConnection, Policy = Constant.Policy.Read)]
        public ActionResult Connection(SearchConnectionViewModel model)
        {
            model = model ?? new SearchConnectionViewModel();
            model.HasEditPermission = HasRight(SiteFunctionKeys.ManageConnection, Constant.Policy.Host);
            model.HasSwitchConnectionPermission = HasRight(SiteFunctionKeys.SwitchConnection, Constant.Policy.Host);
            model.DatabaseServers = _databaseServerService.GetAll(isActive: true).ToList();
            model.Connections = _connectionService.GetAll(model.DbServerId).ToList();

            return View(model);
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>Create the connection view with default value.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageConnection, Policy = Constant.Policy.Host)]
        public ActionResult CreateConnection()
        {
            List<DatabaseServer> databaseServers = _databaseServerService.GetDatabaseServerHasDatabases().ToList();
            DatabaseServer firstDatabaseServer = databaseServers.FirstOrDefault();
            List<Database> databases = _databaseService.GetByDbServerId(firstDatabaseServer.Id).ToList();
            Database firstDatabase = databases.FirstOrDefault();

            var model = new ConnectionViewModel
            {
                DatabaseServers = databaseServers,
                Databases = databases,
                DatabaseId = firstDatabase.Id
            };

            return View(model);
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <param name="model">The connection view model.</param>
        /// <returns>Connection list view.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageConnection, Policy = Constant.Policy.Host)]
        public ActionResult CreateConnection(ConnectionViewModel model)
        {
            List<Connection> connections = _connectionService.GetAll().ToList();
            Database database = _databaseService.GetById(model.DatabaseId);

            model.DatabaseServers = _databaseServerService.GetDatabaseServerHasDatabases().ToList();
            model.Databases = _databaseService.GetByDbServerId(model.DbServerId).ToList();
            model.Name = string.Format("{0}.{1}.{2}", database.ServerAlias, database.Name, model.LoginName);

            Connection tempConnection = connections.Where(x => string.Equals(x.Name, model.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (!ReferenceEquals(tempConnection, null))
            {
                ModelState.AddModelError("Name", string.Format(ViewResources.DuplicateMsg, tempConnection.Name));

                return View(model);
            }

            if (ModelState.IsValid)
            {
                var connection = new Connection
                {
                    DatabaseServer = new DatabaseServer { Id = model.DbServerId },
                    Database = new Database { Id = model.DatabaseId },
                    FreeText = model.FreeText,
                    IsActive = model.IsActive,
                    LoginName = model.LoginName,
                    Name = model.Name,
                    Password = model.Password
                };

                CommandResult command = (CommandResult)_connectionService.Insert(connection);

                ViewBag.Success = command.IsSuccessful;
            }

            return View(model);
        }

        /// <summary>
        /// Edits the connection.
        /// </summary>
        /// <param name="id">The connection identifier.</param>
        /// <returns>Edit connection view with current value.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageConnection, Policy = Constant.Policy.Host)]
        public ActionResult EditConnection(int id)
        {
            Connection connection = _connectionService.GetById(id);

            var model = new ConnectionViewModel
            {
                DatabaseServers = _databaseServerService.GetDatabaseServerHasDatabases().ToList(),
                Databases = _databaseService.GetByDbServerId(connection.DatabaseServer.Id).ToList(),
                DatabaseId = connection.Database.Id,
                DbServerId = connection.DatabaseServer.Id,
                FreeText = connection.FreeText,
                Id = connection.Id,
                IsActive = connection.IsActive,
                LoginName = connection.LoginName,
                Name = connection.Name,
                Password = connection.Password
            };

            return View(model);
        }

        /// <summary>
        /// Edits the connection.
        /// </summary>
        /// <param name="model">The connection view model.</param>
        /// <returns>Connection list view.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageConnection, Policy = Constant.Policy.Host)]
        public ActionResult EditConnection(ConnectionViewModel model)
        {
            List<Connection> listConnection = _connectionService.GetAll().ToList();
            Database database = _databaseService.GetById(model.DatabaseId);
            var name = string.Format("{0}.{1}.{2}", database.ServerAlias, database.Name, model.LoginName);

            model.DatabaseServers = _databaseServerService.GetDatabaseServerHasDatabases().ToList();
            model.Databases = _databaseService.GetByDbServerId(model.DbServerId).ToList();

            var tempConnection = listConnection.Where(x => string.Equals(x.Name, name, StringComparison.OrdinalIgnoreCase) && x.Id != model.Id).FirstOrDefault();

            if (!ReferenceEquals(tempConnection, null))
            {
                ModelState.AddModelError("Name", string.Format(ViewResources.DuplicateMsg, tempConnection.Name));

                return View(model);
            }

            if (ModelState.IsValid)
            {
                var connection = new Connection
                {
                    DatabaseServer = new DatabaseServer { Id = model.DbServerId },
                    Database = new Database { Id = model.DatabaseId },
                    FreeText = model.FreeText,
                    IsActive = model.IsActive,
                    LoginName = model.LoginName,
                    Name = name,
                    Password = model.Password,
                    Id = model.Id
                };

                CommandResult command = (CommandResult)_connectionService.Update(connection);

                ViewBag.Success = command.IsSuccessful;
            }

            return View(model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageConnection, Policy = Constant.Policy.Host)]
        public ActionResult UpdateStatusConnection(int id, bool isActive)
        {
            if (Request.IsAjaxRequest())
            {
                CommandResult command = (CommandResult)_connectionService.UpdateStatus(id, isActive);

                return Json(command.IsSuccessful.ToString().ToLower());
            }

            return Json(null);
        }

        /// <summary>
        /// Switches the database server of list connection.
        /// </summary>
        /// <param name="model">The switch database server model.</param>
        /// <returns>Connection list view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.SwitchConnection, Policy = Constant.Policy.Host)]
        public ActionResult SwitchConnection(SwitchConnectionViewModel model)
        {
            List<DatabaseServer> databaseServers = _databaseServerService.GetAll(isActive: true).ToList();

            if (ReferenceEquals(model, null) || model.FromDbServerId == 0)
            {
                model = new SwitchConnectionViewModel
                {
                    FromDbServerId = 0,
                    DatabaseServers = databaseServers
                };

                return View(model);
            }

            model.DatabaseServers = databaseServers;
            model.Connections = _connectionService.GetAll(model.FromDbServerId).ToList();

            return View(model);
        }

        /// <summary>
        /// Switch the connection database server.
        /// </summary>
        /// <param name="connectionIds">The connection ids.</param>
        /// <param name="toDbServerId">To database server identifier.</param>
        /// <returns>Connection list view.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.SwitchConnection, Policy = Constant.Policy.Host)]
        public ActionResult SwitchConnectionDbServer(string connectionIds, int toDbServerId)
        {
            if (Request.IsAjaxRequest())
            {
                if (!string.IsNullOrWhiteSpace(connectionIds) && toDbServerId != 0)
                {
                    CommandResult command = (CommandResult)_connectionService.SwitchDbServer(connectionIds, toDbServerId);

                    return Json(command);
                }
            }

            return Json(null);
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageConnection, Policy = Constant.Policy.Read)]
        public ActionResult GetDatabaseByDbServerId(int databaseServerId)
        {
            if (Request.IsAjaxRequest())
            {
                List<Database> databases = _databaseService.GetByDbServerId(databaseServerId).ToList();

                return Json(databases, JsonRequestBehavior.AllowGet);
            }

            return Json(null);
        }

        /// <summary>
        /// Connections the assign site.
        /// </summary>
        /// <param name="id">The connection identifier.</param>
        /// <returns>Site list view.</returns>
        [HttpGet]
        public ActionResult ConnectionAssignSite(int id)
        {
            Connection connection = _connectionService.GetById(id);

            var model = new ConnectionAssignSiteViewModel
            {
                HasEditPermission = HasRight(SiteFunctionKeys.ManageConnection, Constant.Policy.Host),
                Id = connection.Id,
                Name = connection.Name,
                Sites = _siteService.GetAll(isActive: true).ToList(),
                SiteConnections = _siteService.GetByConnectionId(id).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageConnection, Policy = Constant.Policy.Host)]
        public ActionResult ConnectionAssignSite(int connectionId, int siteId)
        {
            if (Request.IsAjaxRequest())
            {
                CommandResult command = (CommandResult)_connectionService.AssignSite(connectionId, siteId);

                return Json(command.IsSuccessful.ToString().ToLower());
            }

            return Json(null);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageConnection, Policy = Constant.Policy.Host)]
        public ActionResult ConnectionRemoveSite(int connectionId, int siteId)
        {
            if (Request.IsAjaxRequest())
            {
                CommandResult command = (CommandResult)_connectionService.RemoveSite(connectionId, siteId);

                return Json(command.IsSuccessful.ToString().ToLower());
            }

            return Json(null);
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageConnection, Policy = Constant.Policy.Read)]
        public ActionResult GetConnectionBySite(int siteId)
        {
            List<Connection> connections = _connectionService.GetBySiteId(siteId).ToList();

            return View(connections);
        }

        #endregion

        #region Manage Site

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageSite, Policy = Constant.Policy.Read)]
        public ActionResult Site(SearchSiteViewModel model)
        {
            model = model ?? new SearchSiteViewModel();
            model.HasEditPermission = HasRight(SiteFunctionKeys.ManageSite, Constant.Policy.Host);
            model.Sites = _siteService.GetAll(model.Name).ToList();

            return View(model);
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageSite, Policy = Constant.Policy.Host)]
        public ActionResult CreateSite()
        {
            return View();
        }

        /// <summary>
        /// Creates the site.
        /// </summary>
        /// <param name="model">The site view model.</param>
        /// <returns>Site list view.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageSite, Policy = Constant.Policy.Host)]
        public ActionResult CreateSite(SiteViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<Site> sites = _siteService.GetAll().ToList();
                Site site = sites.Where(x => string.Equals(x.Name, model.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (!ReferenceEquals(site, null))
                {
                    ModelState.AddModelError("Name", string.Format(ViewResources.DuplicateMsg, site.Name));

                    return View(model);
                }

                CommandResult command = (CommandResult)_siteService.Insert(
                    new Site
                    {
                        Name = model.Name,
                        Description = model.Description,
                        IsActive = model.IsActive
                    });

                ViewBag.Success = command.IsSuccessful;
            }

            return View(model);
        }

        /// <summary>
        /// Edits the site.
        /// </summary>
        /// <param name="id">The site identifier.</param>
        /// <returns>Site edit view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageSite, Policy = Constant.Policy.Host)]
        public ActionResult EditSite(int id)
        {
            Site site = _siteService.GetById(id);

            var model = new SiteViewModel
            {
                Id = site.Id,
                Name = site.Name,
                Description = site.Description,
                IsActive = site.IsActive
            };

            return View(model);
        }

        /// <summary>
        /// Edits the site.
        /// </summary>
        /// <param name="model">The site view model.</param>
        /// <returns>Site list view.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageSite, Policy = Constant.Policy.Host)]
        public ActionResult EditSite(SiteViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<Site> sites = _siteService.GetAll().ToList();
                Site site = sites.Where(x => string.Equals(x.Name, model.Name, StringComparison.OrdinalIgnoreCase) && x.Id != model.Id).FirstOrDefault();

                if (!ReferenceEquals(site, null))
                {
                    ModelState.AddModelError("Name", string.Format(ViewResources.DuplicateMsg, site.Name));

                    return View(model);
                }

                CommandResult command = (CommandResult)_siteService.Update(
                    new Site
                    {
                        Id = model.Id,
                        Name = model.Name,
                        Description = model.Description,
                        IsActive = model.IsActive
                    });

                ViewBag.Success = command.IsSuccessful;
            }

            return View(model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageSite, Policy = Constant.Policy.Host)]
        public ActionResult UpdateStatusSite(int siteId, bool isActive)
        {
            if (Request.IsAjaxRequest())
            {
                CommandResult command = (CommandResult)_siteService.UpdateStatus(siteId, isActive);

                return Json(command.IsSuccessful);
            }

            return Json(null);
        }

        #endregion Site

        #region Database Server

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageDatabaseServer, Policy = Constant.Policy.Read)]
        public ActionResult DatabaseServer(SearchDatabaseServerViewModel model)
        {
            model = model ?? new SearchDatabaseServerViewModel();

            model.HasEditPermission = HasRight(SiteFunctionKeys.ManageDatabaseServer, Constant.Policy.Host);
            model.DatabaseServers = _databaseServerService.GetAll(model.Name).ToList();

            return View(model);
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageDatabaseServer, Policy = Constant.Policy.Host)]
        public ActionResult CreateDatabaseServer()
        {
            return View();
        }

        /// <summary>
        /// Creates the database server.
        /// </summary>
        /// <param name="model">The database server model.</param>
        /// <returns>Database server list view.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageDatabaseServer, Policy = Constant.Policy.Host)]
        public ActionResult CreateDatabaseServer(DatabaseServerViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<DatabaseServer> databaseServers = _databaseServerService.GetAll().ToList();
                DatabaseServer databaseServer = databaseServers.Where(x => string.Equals(x.Name, model.Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (!ReferenceEquals(databaseServer, null))
                {
                    ModelState.AddModelError("Name", string.Format(ViewResources.DuplicateMsg, databaseServer.Name));

                    return View(model);
                }

                CommandResult command = (CommandResult)_databaseServerService.Insert(
                    new DatabaseServer
                    {
                        Description = model.Description,
                        Id = model.Id,
                        IsActive = model.IsActive,
                        Name = model.Name,
                        ServerIP = model.ServerIP
                    });

                ViewBag.Success = command.IsSuccessful;
            }

            return View(model);
        }

        /// <summary>
        /// Edits the database server.
        /// </summary>
        /// <param name="id">The database server identifier.</param>
        /// <returns>Database server edit view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageDatabaseServer, Policy = Constant.Policy.Host)]
        public ActionResult EditDatabaseServer(int id)
        {
            DatabaseServer databaseServer = _databaseServerService.GetById(id);

            var model = new DatabaseServerViewModel
            {
                Id = databaseServer.Id,
                Description = databaseServer.Description,
                IsActive = databaseServer.IsActive,
                Name = databaseServer.Name,
                ServerIP = databaseServer.ServerIP
            };

            return View(model);
        }

        /// <summary>
        /// Edits the database server.
        /// </summary>
        /// <param name="model">The database server view model.</param>
        /// <returns>Database server list view.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageDatabaseServer, Policy = Constant.Policy.Host)]
        public ActionResult EditDatabaseServer(DatabaseServerViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<DatabaseServer> databaseServers = _databaseServerService.GetAll().ToList();
                DatabaseServer databaseServer = databaseServers.Where(x => string.Equals(x.Name, model.Name, StringComparison.OrdinalIgnoreCase)
                                                                            && x.Id != model.Id).FirstOrDefault();

                if (!ReferenceEquals(databaseServer, null))
                {
                    ModelState.AddModelError("Name", string.Format(ViewResources.DuplicateMsg, databaseServer.Name));

                    return View(model);
                }

                CommandResult command = (CommandResult)_databaseServerService.Update(
                    new DatabaseServer
                    {
                        Description = model.Description,
                        Id = model.Id,
                        IsActive = model.IsActive,
                        Name = model.Name,
                        ServerIP = model.ServerIP
                    });

                ViewBag.Success = command.IsSuccessful;
            }

            return View(model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageDatabaseServer, Policy = Constant.Policy.Host)]
        public ActionResult UpdateStatusDatabaseServer(int databaseServerId, bool isActive)
        {
            if (Request.IsAjaxRequest())
            {
                CommandResult command = (CommandResult)_databaseServerService.UpdateStatus(databaseServerId, isActive);

                return Json(command.IsSuccessful.ToString().ToLower());
            }

            return Json(null);
        }

        #endregion DB Server

        #region Database

        /// <summary>
        /// The database list view.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Database list view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageDatabase, Policy = Constant.Policy.Read)]
        public ActionResult Database(SearchDatabaseViewModel model)
        {
            model = model ?? new SearchDatabaseViewModel();
            model.HasEditPermission = HasRight(SiteFunctionKeys.ManageDatabase, Constant.Policy.Host);
            model.Database = _databaseService.GetAll(model.DbServerId).ToList();
            model.DatabaseServers = _databaseServerService.GetAll(isActive: true).ToList();

            return View(model);
        }

        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <param name="databaseId">The database identifier.</param>
        /// <returns>Database create new view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageDatabase, Policy = Constant.Policy.Host)]
        public ActionResult CreateDatabase(int databaseId)
        {
            var model = new DatabaseViewModel
            {
                DbServerId = databaseId,
                DatabaseServers = _databaseServerService.GetAll(isActive: true).ToList()
            };

            return View(model);
        }

        /// <summary>
        /// Creates the database.
        /// </summary>
        /// <param name="model">The database view model.</param>
        /// <returns>Database list view.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageDatabase, Policy = Constant.Policy.Host)]
        public ActionResult CreateDatabase(DatabaseViewModel model)
        {
            model.DatabaseServers = _databaseServerService.GetAll(isActive: true).ToList();

            if (ModelState.IsValid)
            {
                List<Database> databases = _databaseService.GetAll(model.DbServerId).ToList();
                Database database = databases.Where(x => string.Equals(x.Name, model.Name, StringComparison.OrdinalIgnoreCase)
                                                        && string.Equals(x.ServerAlias, model.ServerAlias, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (!ReferenceEquals(database, null))
                {
                    ModelState.AddModelError("Name", string.Format(ViewResources.DuplicateMsg, database.Name));
                    ModelState.AddModelError("DbServerId", string.Format(ViewResources.DuplicateMsg, database.DatabaseServer.Name));
                    ModelState.AddModelError("ServerAlias", string.Format(ViewResources.DuplicateMsg, database.ServerAlias));

                    return View(model);
                }

                CommandResult command = (CommandResult)_databaseService.Insert(
                    new Database
                    {
                        Name = model.Name,
                        ServerAlias = model.ServerAlias,
                        IsActive = model.IsActive,
                        DatabaseServer = new DatabaseServer { Id = model.DbServerId }
                    });

                ViewBag.Success = command.IsSuccessful;
            }

            return View(model);
        }

        /// <summary>
        /// Edits the database.
        /// </summary>
        /// <param name="id">The database identifier.</param>
        /// <returns>Database edit view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageDatabase, Policy = Constant.Policy.Host)]
        public ActionResult EditDatabase(int id)
        {
            Database database = _databaseService.GetById(id);

            var model = new DatabaseViewModel
            {
                DatabaseServers = _databaseServerService.GetAll(isActive: true).ToList(),
                Id = database.Id,
                Name = database.Name,
                ServerAlias = database.ServerAlias,
                IsActive = database.IsActive,
                DbServerId = database.DatabaseServer.Id
            };

            return View(model);
        }

        /// <summary>
        /// Edits the database.
        /// </summary>
        /// <param name="model">The database view model.</param>
        /// <returns>Database list view.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageDatabase, Policy = Constant.Policy.Host)]
        public ActionResult EditDatabase(DatabaseViewModel model)
        {
            model.DatabaseServers = _databaseServerService.GetAll(isActive: true).ToList();

            if (ModelState.IsValid)
            {
                List<Database> databases = _databaseService.GetAll(model.DbServerId).ToList();
                Database database = databases.Where(x => string.Equals(x.Name, model.Name, StringComparison.OrdinalIgnoreCase)
                                                         && x.Id != model.Id).FirstOrDefault();

                if (!ReferenceEquals(database, null))
                {
                    ModelState.AddModelError("Name", string.Format(ViewResources.DuplicateMsg, database.Name));

                    return View(model);
                }

                CommandResult command = (CommandResult)_databaseService.Update(
                    new Database
                    {
                        Id = model.Id,
                        Name = model.Name,
                        ServerAlias = model.ServerAlias,
                        IsActive = model.IsActive,
                        DatabaseServer = new DatabaseServer { Id = model.DbServerId }
                    });

                ViewBag.Success = command.IsSuccessful;
            }

            return View(model);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ManageDatabase, Policy = Constant.Policy.Host)]
        public ActionResult UpdateStatusDatabase(int databaseId, bool isActive)
        {
            if (Request.IsAjaxRequest())
            {
                CommandResult command = (CommandResult)_databaseService.UpdateStatus(databaseId, isActive);

                return Json(command.IsSuccessful);
            }

            return Json(false);
        }

        #endregion

        #region Log management

        /// <summary>
        /// The view execute stored procedure log.
        /// </summary>
        /// <param name="model">The log view model.</param>
        /// <returns>The log view.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ConnectionStringLog, Policy = Constant.Policy.Read)]
        public ActionResult Log(SearchLogViewModel model)
        {
            int totalLogs = 0;

            IQueryable<LogExecSp> listLog = _logService.SearchLog(
                                                model.PageIndex,
                                                model.PageSize,
                                                ref totalLogs,
                                                model.Name,
                                                model.Username,
                                                model.From,
                                                model.To,
                                                model.FilterType);

            model.LogDatas = new PagerList<LogExecSp>(model.PageIndex, model.PageSize, totalLogs, listLog);

            return View(model);
        }

        #endregion Log management
    }
}