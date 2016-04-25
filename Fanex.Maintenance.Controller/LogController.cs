// --------------------------------------------------------------------------
// <copyright file="LogController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.9.20">Created</change>
//   <change who="Ricky.Nguyen" date="2012.10.16">Style cop</change>
//   <change who="Jason.Ly" date="2013.8.7">Modify Get Logs</change>
// </history>

namespace Fanex.Maintenance.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Fanex.Maintenance.Core;
    using Fanex.Maintenance.Core.Model;
    using Fanex.Maintenance.Repository.Criteria.Log;
    using Fanex.Maintenance.Service.Impl.AppLog;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Log controller.
    /// </summary>
    [NapSuperAuth]
    public class LogController : BaseController
    {
        #region Member

        /// <summary>
        /// Log Service.
        /// </summary>
        private readonly LogService _logService;

        /// <summary>
        /// Log Category Service.
        /// </summary>
        private readonly LogCategoryService _categoryService;

        #endregion Member

        #region Ctor

        public LogController()
            : this(null, null)
        {
        }

        public LogController(LogService logService, LogCategoryService categoryService)
        {
            _logService = logService ?? LogService.Create(ShowLogTimeZone);
            _categoryService = categoryService ?? LogCategoryService.Create();
        }

        #endregion Ctor

        #region Action

        /// <summary>
        /// Log list page.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ViewLog, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            InitDataView();
            InitPageSize();

            ViewBag.ListLog = null;

            var criteria = new GetLogCriteria
            {
                CategoryId = 0,
                MachineId = 0,
                IsProduction = null,
                IsMonitored = null,
                From = DateTime.Now.AddHours(11),   // to GMT+7 (default)
                To = DateTime.Now.AddHours(11),     // to GMT+7 (default)
                Page = 0,
                Size = 10,
                Severity = "All",
                ToGMT = ShowLogTimeZone
            };

            criteria.HasEdit = HasRight(SiteFunctionKeys.ViewLog, Constant.Policy.Host);

            return View(criteria);
        }

        /// <summary>
        /// Get log page.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ViewLog, Policy = Constant.Policy.Read)]
        public ActionResult Index(GetLogCriteria criteria)
        {
            InitDataView();
            InitPageSize();

            var totalRows = 0;

            DateTime from = criteria.From.AddHours(-1 * criteria.ToGMT);
            DateTime to = criteria.To.AddDays(1).AddSeconds(-1).AddHours(-1 * criteria.ToGMT);

            ////Get logs by date
            IEnumerable<Log> listLogs = _logService.With(
                criteria.CategoryId,
                criteria.MachineId,
                criteria.IsProduction,
                criteria.IsMonitored,
                criteria.Severity,
                criteria.ToGMT)
                .From(from)
                .To(to)
                .WithPagination(criteria.Page, criteria.Size)
                .GetLogs(ref totalRows);

            if (listLogs.Any())
            {
                listLogs = listLogs.Select<Log, Log>(log =>
                {
                    log.Message = HttpUtility.HtmlEncode(log.Message);
                    log.FormattedMessage = HttpUtility.HtmlEncode(log.FormattedMessage);

                    return log;
                });
            }

            return Json(new
            {
                TotalRows = totalRows,
                ListLog = listLogs
            });
        }

        /// <summary>
        /// Get details message.
        /// </summary>
        /// <param name="id">Log message ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ViewLog, Policy = Constant.Policy.Read)]
        public ActionResult GetLogMessage(int id)
        {
            if (Request.IsAjaxRequest())
            {
                return Json(_logService.GetMessage(id));
            }

            return Json(null);
        }

        #endregion Action

        #region Ajax

        /// <summary>
        /// Remove Log.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ViewLog, Policy = Constant.Policy.Host)]
        public ActionResult RemoveLog()
        {
            if (Request.IsAjaxRequest())
            {
                return Json(_logService.From(DateTime.Now.AddDays(-5)).RemoveLogs());
            }

            return Json(0);
        }

        /// <summary>
        /// Update log's monitored status.
        /// </summary>
        /// <param name="logId">Log object ID.</param>
        /// <param name="isMonitored">Is monitored or not.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ViewLog, Policy = Constant.Policy.Host)]
        public ActionResult MonitorLog(string logId, bool isMonitored)
        {
            string updatedUsername = string.Empty;

            if (Request.IsAjaxRequest())
            {
                int? userId = this.CurrentSession.UserId;
                userId = isMonitored ? userId : null;
                int affectedRows = _logService.MonitorLog(logId, userId, isMonitored);
                updatedUsername = affectedRows != 0 ? this.CurrentSession.Username : string.Empty;
            }

            return Json(updatedUsername);
        }

        #endregion Ajax

        #region Private

        /// <summary>
        /// Initialize Page Size.
        /// </summary>
        private void InitPageSize()
        {
            List<SelectListItem> listPageSize = new List<SelectListItem>();
            listPageSize.Add(new SelectListItem
            {
                Text = "10",
                Value = "10"
            });

            listPageSize.Add(new SelectListItem
            {
                Text = "30",
                Value = "30"
            });

            listPageSize.Add(new SelectListItem
            {
                Text = "50",
                Value = "50"
            });

            listPageSize.Add(new SelectListItem
            {
                Text = "100",
                Value = "100"
            });

            ViewBag.PageSize = listPageSize;
        }

        /// <summary>
        /// Initialize Machine Types data.
        /// </summary>
        private void InitMachineTypes()
        {
            List<SelectListItem> machineTypes = new List<SelectListItem>();
            machineTypes.Add(new SelectListItem
            {
                Text = ViewResource.All,
                Value = "null"
            });
            machineTypes.Add(new SelectListItem
            {
                Text = ViewResource.MachineType_Production,
                Value = "true"
            });
            machineTypes.Add(new SelectListItem
            {
                Text = ViewResource.MachineType_UAT,
                Value = "false"
            });

            ViewBag.MachineTypes = machineTypes;
        }

        /// <summary>
        /// Initialize Log Status data.
        /// </summary>
        private void InitLogStatus()
        {
            List<SelectListItem> logStatus = new List<SelectListItem>();
            logStatus.Add(new SelectListItem
            {
                Text = ViewResource.All,
                Value = "null"
            });
            logStatus.Add(new SelectListItem
            {
                Text = ViewResource.LogStatus_Monitored,
                Value = "true"
            });
            logStatus.Add(new SelectListItem
            {
                Text = ViewResource.LogStatus_Unmonitored,
                Value = "false"
            });

            ViewBag.LogStatus = logStatus;
        }

        /// <summary>
        /// Initialize Severities data.
        /// </summary>
        private void InitSeverities()
        {
            ////Init Serverity
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem
            {
                Text = ViewResource.All,
                Value = "All"
            });
            list.Add(new SelectListItem
            {
                Text = ViewResource.Severity_Critical,
                Value = "Critical"
            });
            list.Add(new SelectListItem
            {
                Text = ViewResource.Severity_Error,
                Value = "Error"
            });
            list.Add(new SelectListItem
            {
                Text = ViewResource.Severity_Information,
                Value = "Information"
            });
            list.Add(new SelectListItem
            {
                Text = ViewResource.Severity_Verbose,
                Value = "Verbose"
            });
            list.Add(new SelectListItem
            {
                Text = ViewResource.Severity_Warning,
                Value = "Warning"
            });
            ViewBag.ListSeverity = list;
        }

        /// <summary>
        /// Initialize Timezone data.
        /// </summary>
        private void InitTimeZone()
        {
            List<SelectListItem> list = new List<SelectListItem>();

            list.Add(new SelectListItem
            {
                Text = "GMT-4",
                Value = "-4"
            });
            list.Add(new SelectListItem
            {
                Text = "GMT+7",
                Value = "7"
            });
            list.Add(new SelectListItem
            {
                Text = "GMT+8",
                Value = "8"
            });

            ViewBag.ListTimeZone = list;
        }

        /// <summary>
        /// Initialize data for View.
        /// </summary>
        private void InitDataView()
        {
            InitSeverities();
            InitLogStatus();
            InitTimeZone();

            // Default selection for drop-down
            SelectListItem defaultSelection = new SelectListItem
            {
                Text = ViewResource.All,
                Value = "0",
                Selected = true
            };

            // Get Category from DB
            List<SelectListItem> listCategory = _categoryService.GetAll().Select(c => new SelectListItem
            {
                Text = c.CategoryName,
                Value = c.CategoryID.ToString()
            }).ToList();
            listCategory.Insert(0, defaultSelection);
            ViewBag.ListCategory = listCategory;
        }

        #endregion Private
    }
}