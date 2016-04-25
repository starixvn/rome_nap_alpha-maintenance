// --------------------------------------------------------------------------
// <copyright file="MailController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Edward.Nguyen" date="2012.9.20">Created</change>
//   <change who="Ricky.Nguyen" date="2012.10.16">StyleCop</change>
// </history>

namespace Fanex.Maintenance.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.MailDeliver.Model;
    using Fanex.Maintenance.Repository.Criteria.MailService;
    using Fanex.Maintenance.Service.Impl.Mail;
    using Fanex.Maintenance.Service.Interface.Mail;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;
    using Fanex.NAP.Core.Web.Attribute;

    /// <summary>
    /// Mail Controller.
    /// </summary>
    [NapSuperAuth]
    public class MailController : BaseController
    {
        #region Member

        /// <summary>
        /// Mail Service.
        /// </summary>
        private readonly IMailService _mailServer;

        private readonly IUserService _userService;

        #endregion Member

        #region Ctor

        public MailController()
            : this(null, null)
        {
        }

        public MailController(IMailService mailService, IUserService userService)
        {
            this._mailServer = mailService ?? MailService.Create();
            this._userService = userService ?? new UserService();
        }

        #endregion Ctor

        #region Action

        #region MailList

        /// <summary>
        /// Get mail list.
        /// </summary>
        /// <param name="appId">Application ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ViewMailReport, Policy = Constant.Policy.Read)]
        public ActionResult MailList(int appId = 0)
        {
            InitPageSize();
            InitDataMailConfig(appId);
            InitMailStatus();

            var totalRows = 0;

            ViewBag.DateFrom = DateTime.Now.ToString("dd/MM/yyyy");
            ViewBag.DateTo = DateTime.Now.ToString("dd/MM/yyyy");

            var listMail = new List<MailRecord>();
            listMail = this._mailServer.WithApp(appId)
                .FromDate(DateTime.Now.ToShortDateString())
                .ToDate(DateTime.Now.ToShortDateString())
                .WithStatus(0)
                .WithPagination(1, 10)
                .GetListMailByDate(ref totalRows);

            return View(listMail);
        }

        /// <summary>
        /// Get mail list.
        /// </summary>
        /// <param name="from">From DateTime string.</param>
        /// <param name="to">To DateTime string.</param>
        /// <param name="applicationID">If application id = 0 get all.</param>
        /// <param name="mailStatus">Mail status.</param>
        /// <param name="size">Number of items.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ViewMailReport, Policy = Constant.Policy.Read)]
        public ActionResult MailList(string from, string to, int applicationID = 0, int mailStatus = 0, int size = 10)
        {
            InitPageSize();

            InitDataMailConfig();

            InitMailStatus();

            var totalRows = 0;

            DateTime dateFrom = DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dateTo = DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var listMail = new List<MailRecord>();
            listMail = this._mailServer.WithApp(applicationID)
                .FromDate(dateFrom.ToString())
                .ToDate(dateTo.ToString())
                .WithStatus(mailStatus)
                .WithPagination(1, size)
                .GetListMailByDate(ref totalRows);

            ViewBag.DateFrom = from;
            ViewBag.DateTo = to;

            return View(listMail);
        }

        #endregion MailList

        #region MailSetting

        /// <summary>
        /// Get all mail setting.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Read)]
        public ActionResult MailSetting()
        {
            List<MailSetting> listMailSetting = this._mailServer.GetListSetting();
            ViewBag.HasEdit = HasRight(SiteFunctionKeys.SmtpServers, Constant.Policy.Host);

            return View(listMailSetting);
        }

        /// <summary>
        /// Add mail setting.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Host)]
        public ActionResult AddMailSetting()
        {
            return View(new MailSettingViewModel());
        }

        /// <summary>
        /// Add mail setting.
        /// </summary>
        /// <param name="model">Mail setting view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Host)]
        public ActionResult AddMailSetting(MailSettingViewModel model)
        {
            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                model.Password = model.Password == null ? string.Empty : model.Password;
                model.SettingName = model.Server + ":" + model.Port + " (" + model.Username + ")";
                MailSetting mailSetting = model.GetMailSetting();

                ViewBag.Success = this._mailServer.StoreSetting(mailSetting);
            }

            return View(model);
        }

        /// <summary>
        /// Add mail setting popup.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult AddMailSettingPopUp()
        {
            return View(new MailSetting());
        }

        /// <summary>
        /// Edit mail setting.
        /// </summary>
        /// <param name="id">Setting ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Host)]
        public ActionResult EditMailSetting(int id)
        {
            MailSetting mailSetting = _mailServer.GetSetting(id);
            var model = new MailSettingViewModel(mailSetting);

            return View(model);
        }

        /// <summary>
        /// Edit mail setting.
        /// </summary>
        /// <param name="model">Mail setting view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.SmtpServers, Policy = Constant.Policy.Host)]
        public ActionResult EditMailSetting(MailSettingViewModel model)
        {
            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                model.Password = model.Password == null ? string.Empty : model.Password;
                model.SettingName = model.Server + ":" + model.Port + " (" + model.Username + ")";
                MailSetting mailSetting = model.GetMailSetting();
                ViewBag.Success = _mailServer.StoreSetting(mailSetting);
            }

            return View(model);
        }

        #endregion MailSetting

        #region MailConfig

        /// <summary>
        /// Get all mail config.
        /// </summary>
        /// <param name="id">Setting ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Read)]
        public ActionResult MailConfig(int id = 0)
        {
            var mailSetting = _mailServer.GetSetting(id);
            string serverIp = mailSetting != null ? mailSetting.Server : string.Empty;
            InitDataIpServer(serverIp);

            List<MailConfig> listMailConfig = _mailServer.GetListConfig(serverIp);

            ViewBag.HasEdit = HasRight(SiteFunctionKeys.ApplicationManagement, Constant.Policy.Host);

            return View(listMailConfig);
        }

        /// <summary>
        /// Get mail config filter by server IP.
        /// </summary>
        /// <param name="serverIp">Server IP address.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Read)]
        public ActionResult MailConfig(string serverIp = "")
        {
            InitDataIpServer(serverIp);

            List<MailConfig> listMailConfig = this._mailServer.GetListConfig(serverIp);

            ViewBag.HasEdit = HasRight(SiteFunctionKeys.ApplicationManagement, Constant.Policy.Host);

            return View(listMailConfig);
        }

        /// <summary>
        /// Add new mail config.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Host)]
        public ActionResult AddMailConfig()
        {
            InitDataMailSetting();

            return View(new MailConfigViewModel());
        }

        /// <summary>
        /// Add new mail config.
        /// </summary>
        /// <param name="model">Mail config view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Host)]
        public ActionResult AddMailConfig(MailConfigViewModel model)
        {
            InitDataMailSetting();

            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                MailConfig mailConfig = model.GetMailConfig();

                ViewBag.Success = this._mailServer.StoreConfig(mailConfig);
            }

            return View(model);
        }

        /// <summary>
        /// Add new mail config popup.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult AddMailConfigPopUp()
        {
            InitDataMailSetting();

            return View(new MailConfig());
        }

        /// <summary>
        /// Edit mail config by config id.
        /// </summary>
        /// <param name="id">Mail config ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Host)]
        public ActionResult EditMailConfig(int id)
        {
            InitDataMailSetting();

            ViewBag.Success = false;

            MailConfig mailConfig = _mailServer.GetConfig(id);
            var model = new MailConfigViewModel(mailConfig);

            return View(model);
        }

        /// <summary>
        /// Edit mail configuration.
        /// </summary>
        /// <param name="model">Mail configuration view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ApplicationManagement, Policy = Constant.Policy.Host)]
        public ActionResult EditMailConfig(MailConfigViewModel model)
        {
            InitDataMailSetting();

            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                MailConfig mailConfig = model.GetMailConfig();

                ViewBag.Success = _mailServer.StoreConfig(mailConfig);
            }

            return View(model);
        }

        #endregion MailConfig

        #region MailFunction

        /// <summary>
        /// Get all mail function.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult MailFunction()
        {
            List<MailFunction> listMailFunction = _mailServer.GetListFunction();

            return View(listMailFunction);
        }

        /// <summary>
        /// Add new mail function.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult AddMailFunction()
        {
            InitDataMailSetting();
            InitDataMailConfig();

            return View(new MailFunctionViewModel());
        }

        /// <summary>
        /// Add new mail function.
        /// </summary>
        /// <param name="model">Mail function view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult AddMailFunction(MailFunctionViewModel model)
        {
            InitDataMailSetting();
            InitDataMailConfig();

            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                MailFunction mailFunction = model.GetMailFunction();

                ViewBag.Success = _mailServer.StoreFunction(mailFunction);
            }

            return View(model);
        }

        /// <summary>
        /// Edit mail function.
        /// </summary>
        /// <param name="id">Mail function ID.</param>
        /// <returns>Action result.</returns>
        [HttpGet]
        public ActionResult EditMailFunction(int id)
        {
            InitDataMailSetting();
            InitDataMailConfig();

            MailFunction mailFunction = _mailServer.GetFunction(id);

            var model = new MailFunctionViewModel(mailFunction);

            return View(model);
        }

        /// <summary>
        /// Edit mail function.
        /// </summary>
        /// <param name="model">Mail function view model.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult EditMailFunction(MailFunctionViewModel model)
        {
            InitDataMailSetting();
            InitDataMailConfig();

            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                MailFunction mailFunction = model.GetMailFunction();

                ViewBag.Success = _mailServer.StoreFunction(mailFunction);
            }

            return View(model);
        }

        #endregion MailFunction

        #region Compose Email

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ComposeEmail, Policy = Constant.Policy.Read)]
        public ActionResult ComposeEmail()
        {
            InitDataMailConfig();
            return View(new MailRecord());
        }

        /// <summary>
        /// Composes the email.
        /// </summary>
        /// <param name="mailRecord">The mail record.</param>
        /// <returns>Return view.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ComposeEmail, Policy = Constant.Policy.Read)]
        public ActionResult ComposeEmail(MailRecord mailRecord)
        {
            InitDataMailConfig();

            ConfigurationManager.AppSettings["app_mail_id"] = mailRecord.AppID.ToString();

            if (!string.IsNullOrWhiteSpace(mailRecord.FileAttachments))
            {
                string extension = Path.GetExtension(mailRecord.FileAttachments);
                var fileExtension = new FileExtentions();

                switch (extension.ToLower())
                {
                    case ".xls":
                        fileExtension = FileExtentions.MS_EXCEL;
                        break;
                    case ".doc":
                        fileExtension = FileExtentions.MS_DOC;
                        break;
                    case ".jpg":
                        fileExtension = FileExtentions.IMG_JPG;
                        break;
                    case ".gif":
                        fileExtension = FileExtentions.IMG_GIF;
                        break;
                    case ".png":
                        fileExtension = FileExtentions.IMG_PNG;
                        break;
                    case ".pdf":
                        fileExtension = FileExtentions.PDF;
                        break;
                    default:
                        fileExtension = FileExtentions.TEXT;
                        break;
                }

                mailRecord.Attachments = new List<MailAttachment>();
                var attachment = new Fanex.MailDeliver.Model.MailAttachment(mailRecord.FileAttachments, fileExtension);
                mailRecord.Attachments.Add(attachment);
            }

            using (var client = new Fanex.MailDeliver.MailClient.API.MailDeliver())
            {
                client.SendMail(mailRecord);
            }

            ViewBag.Success = true;

            return View(mailRecord);
        }
        #endregion

        #endregion Action

        #region Ajax

        /// <summary>
        /// Add mail setting for popup view.
        /// </summary>
        /// <param name="server">Server IP address.</param>
        /// <param name="port">Pot number.</param>
        /// <param name="username">Current username.</param>
        /// <param name="password">Current password.</param>
        /// <param name="isAutho">Is authorized or not.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult AddMailSettingPopUp(
            string server,
            int port,
            string username,
            string password,
            bool isAutho)
        {
            if (Request.IsAjaxRequest())
            {
                var mail = new MailSetting
                {
                    SettingName = server + ":" + port.ToString() + " (" + username + ") ",
                    Server = server,
                    Port = port,
                    Username = username,
                    Password = password,
                    IsAuth = isAutho
                };

                _mailServer.StoreSetting(mail);
                return Json(mail);
            }

            return Json(false);
        }

        /// <summary>
        /// Add mail config for popup view.
        /// </summary>
        /// <param name="appName">Application name.</param>
        /// <param name="description">Mail config description.</param>
        /// <param name="settingId">Setting ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult AddMailConfigPopUp(string appName, string description, int settingId)
        {
            var mailConfig = new MailConfig
            {
                AppName = appName,
                Description = description,
                Setting = new MailSetting
                {
                    SettingID = settingId
                }
            };

            InitDataMailSetting();

            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                ViewBag.Success = _mailServer.StoreConfig(mailConfig);
            }

            return Json(mailConfig);
        }

        /// <summary>
        /// Resend mail.
        /// </summary>
        /// <param name="mailId">Mail object ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ResendMail, Policy = Constant.Policy.Host)]
        public ActionResult ResendMail(int mailId)
        {
            if (Request.IsAjaxRequest())
            {
                bool check = _mailServer.SendMail(mailId);
                return Json(check);
            }

            return Json(false);
        }

        /// <summary>
        /// Get message detail of mail.
        /// </summary>
        /// <param name="mailId">Mail object ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ViewMailDetails, Policy = Constant.Policy.Read)]
        public ActionResult GetMessage(int mailId)
        {
            if (Request.IsAjaxRequest())
            {
                MailRecord mail = _mailServer.GetMail(mailId);
                return View(mail);
            }

            return View(new MailRecord());
        }

        /// <summary>
        /// Get mail error.
        /// </summary>
        /// <param name="mailId">Mail object ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult GetError(int mailId)
        {
            if (Request.IsAjaxRequest())
            {
                List<MailError> mail = _mailServer.GetError(mailId);
                return View(mail);
            }

            return View(new List<MailError>());
        }

        /// <summary>
        /// Get list mail pagination.
        /// </summary>
        /// <param name="status">Mail status.</param>
        /// <param name="from">From DateTime string.</param>
        /// <param name="to">To DateTime string.</param>
        /// <param name="page">Page index.</param>
        /// <param name="size">Page size.</param>
        /// <param name="applicationID">Application ID.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult MailListPagi(
            int status,
            string from,
            string to,
            int page = 0,
            int size = 50,
            int applicationID = 0)
        {
            InitDataMailConfig();

            var totalRows = 0;

            DateTime dateFrom = !string.IsNullOrWhiteSpace(from) ? DateTime.ParseExact(from, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now;
            DateTime dateTo = !string.IsNullOrWhiteSpace(from) ? DateTime.ParseExact(to, "dd/MM/yyyy", CultureInfo.InvariantCulture) : DateTime.Now;

            var listMail = new List<MailRecord>();
            listMail = _mailServer.WithApp(applicationID)
                .FromDate(dateFrom.ToString())
                .ToDate(dateTo.ToString())
                .WithPagination(page, size)
                .WithStatus(status)
                .GetListMailByDate(ref totalRows);

            var policyViewDetail = _userService.GetPermission(CurrentSession.UserId, this.SiteId, SiteFunctionKeys.ViewMailDetails);
            var policyResend = _userService.GetPermission(CurrentSession.UserId, this.SiteId, SiteFunctionKeys.ResendMail);

            return Json(new
            {
                TotalRows = totalRows,
                HasViewDetail = policyViewDetail,
                HasResend = policyResend,
                ListMail = listMail
            });
        }

        /// <summary>
        /// Get list mail setting.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult GetMailSetting()
        {
            if (Request.IsAjaxRequest())
            {
                List<MailSetting> listMailSetting = _mailServer.GetListSetting();

                return Json(listMailSetting);
            }

            return Json(null);
        }

        /// <summary>
        /// Get list mail config.
        /// </summary>
        /// <returns>Action result.</returns>
        [HttpPost]
        public ActionResult GetMailConfig()
        {
            if (Request.IsAjaxRequest())
            {
                List<MailConfig> listMailConfig = _mailServer.GetListConfig();

                return Json(listMailConfig);
            }

            return Json(null);
        }

        #endregion Ajax

        #region InitData

        /// <summary>
        /// Initialize Page Size.
        /// </summary>
        private void InitPageSize()
        {
            var listPageSize = new List<SelectListItem>();
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
        /// Initialize list mail status for view.
        /// </summary>
        private void InitMailStatus()
        {
            var listMailStatus = new List<SelectListItem>();
            listMailStatus.Add(new SelectListItem
            {
                Text = MailStatus.All.ToString(),
                Value = ((int)MailStatus.All).ToString()
            });

            listMailStatus.Add(new SelectListItem
            {
                Text = MailStatus.Fail.ToString(),
                Value = ((int)MailStatus.Fail).ToString()
            });

            listMailStatus.Add(new SelectListItem
            {
                Text = MailStatus.Success.ToString(),
                Value = ((int)MailStatus.Success).ToString()
            });

            ViewBag.MailStatus = listMailStatus;
        }

        /// <summary>
        /// Initialize list mail config for view.
        /// </summary>
        /// <param name="appId">Application ID.</param>
        private void InitDataMailConfig(int appId = 0)
        {
            List<MailConfig> listConfig = _mailServer.GetListConfig(string.Empty);
            var listConfigPermission = new List<MailConfig>();

            var configPermission = ConfigurationManager.AppSettings["MailPermission:Application"];
            var applicationPermissions = configPermission.Split(';');

            foreach (var itemApplicationPermission in applicationPermissions)
            {
                var permission = itemApplicationPermission.Split(':');
                var functionKey = permission[0];
                var appliationId = int.Parse(permission[1]);

                var hasPermission = HasRight(functionKey, Constant.Policy.Read)
                    && CurrentSession.Level == (int)Constant.Level.User;

                if (hasPermission)
                {
                    listConfigPermission.AddRange(listConfig.Where(x => x.AppID == appliationId));
                }
            }

            if (listConfigPermission.Any())
            {
                listConfig = listConfigPermission;
            }

            ViewBag.HasLimitedConfigPermission = listConfigPermission.Any();

            ViewBag.ListMailConfig = listConfig.Select(c => new SelectListItem
            {
                Text = c.AppName,
                Value = c.AppID.ToString(),
                Selected = c.AppID == appId
            }).ToList();
        }

        /// <summary>
        /// Initialize list mail setting for view.
        /// </summary>
        private void InitDataMailSetting()
        {
            List<MailSetting> listMailSetting = _mailServer.GetListSetting();

            ViewBag.ListMailSetting = listMailSetting.Select(s => new SelectListItem
            {
                Text = s.SettingName,
                Value = s.SettingID.ToString(),
            }).ToList();
        }

        /// <summary>
        /// Initialize list server IP for view.
        /// </summary>
        /// <param name="serverIp">Server IP address.</param>
        private void InitDataIpServer(string serverIp = "")
        {
            List<MailSetting> listMailSetting = _mailServer.GetListSetting();
            listMailSetting = listMailSetting.GroupBy(s => s.Server).Select(s => s.First()).ToList();

            ViewBag.ListServerIp = listMailSetting.Select(s => new SelectListItem
            {
                Text = s.Server,
                Value = s.Server,
                Selected = s.Server == serverIp
            }).ToList();
        }

        #endregion InitData
    }
}