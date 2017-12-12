using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml.Serialization;
using Alpha.Maintenance.Controller;
using Alpha.Maintenance.Core.Configurations;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Service.Impl.DbProfiler;
using Alpha.Maintenance.Site.Areas.DatabaseProfiler.Models;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;

namespace Alpha.Maintenance.Site.Areas.DatabaseProfiler.Controllers
{
    [NapSuperAuth]
    public class TracedLogController : BaseController
    {
        private readonly LogService _logService;
        private readonly string _dbServerXmlPath;
        private readonly string _siteXmlPath;
        public TracedLogController()
        {
            _dbServerXmlPath = HostingEnvironment.MapPath(AppSettings.XMLServerListFilePath);
            _siteXmlPath = HostingEnvironment.MapPath(AppSettings.XMLSiteListFilePath);
            _logService = new LogService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.DBProfiler, Policy = Constant.Policy.Read)]
        public ActionResult Index()
        {
            InitialServer();
            InitialSite();

            return View();
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.DBProfiler, Policy = Constant.Policy.Read)]
        public ActionResult Logs(DBProfilerModel model)
        {
            var hasError = false;
            var errorMsg = string.Empty;

            var list = _logService
                 .With(model)
                 .GetLogs(ref hasError, ref errorMsg);

            if (hasError)
                ViewBag.ErrorMsg = errorMsg;

            return View(list);
        }

        private void InitialServer()
        {
            var ServerList = ReadXMLData<Server>(_dbServerXmlPath, "Servers");
            if (ServerList.Any())
            {
                List<SelectListItem> list = new List<SelectListItem>();

                ServerList.ForEach(x =>
                {
                    list.Add(new SelectListItem
                    {
                        Text = x.Name + "(" + x.Ip + ")",
                        Value = x.Ip
                    });
                });

                ViewBag.ListServer = list;
            }
        }

        private void InitialSite()
        {
            var SiteList = ReadXMLData<Models.Site>(_siteXmlPath, "Sites");
            if (SiteList.Any())
            {
                List<SelectListItem> list = new List<SelectListItem>();

                SiteList.ForEach(x =>
                {
                    list.Add(new SelectListItem
                    {
                        Text = x.Name,
                        Value = x.Id
                    });
                });

                ViewBag.ListSite = list;
            }
        }

        private List<T> ReadXMLData<T>(string xmlPath, string rootAttributeName)
        {
            var listData = new List<T>();

            if (System.IO.File.Exists(xmlPath))
            {
                var deserializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(rootAttributeName));

                using (FileStream stream = System.IO.File.OpenRead(xmlPath))
                {
                    listData = deserializer.Deserialize(stream) as List<T>;
                }
            }

            return listData;
        }
    }
}
