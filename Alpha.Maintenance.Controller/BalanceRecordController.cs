namespace Alpha.Maintenance.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Alpha.Maintenance.Controller.ViewModels;
    using Alpha.Maintenance.Core.Configurations;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Service.Impl.BalanceRecord;
    using Alpha.Maintenance.Service.Interface.BalanceRecord;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapSuperAuth]
    public class BalanceRecordController : BaseController
    {
        private const string DB05 = "DB05";
        private const string DB05BK1 = "DB05BK1";
        private const string UAT = "UAT";

        private readonly IBalanceRecordService balanceRecordService;
        private readonly IEnumerable<string> jobNames;

        public BalanceRecordController() : this(null)
        {
        }

        private BalanceRecordController(IBalanceRecordService balanceRecordService)
        {
            this.balanceRecordService = balanceRecordService ?? new BalanceRecordService();
            jobNames = AppSettings.BalanceRecordJobNames.ToList();
        }

        [NapAuth(FunctionKey = SiteFunctionKeys.DataSyncServices, Policy = Constant.Policy.Read)]
        public ActionResult Index()
            => GetJobStatus(DB05);

        [NapAuth(FunctionKey = SiteFunctionKeys.DataSyncServices, Policy = Constant.Policy.Read)]
        public ActionResult Backup()
            => GetJobStatus(DB05BK1);

        private ActionResult GetJobStatus(string dbAlias)
        {
            var hasHostPermission = HasRight(SiteFunctionKeys.DataSyncServices, Constant.Policy.Host);

            var proJobs = new BalanceRecordViewModel(
                balanceRecordService.GetJobStatus(string.Empty, dbAlias),
                string.Empty,
                dbAlias,
                hasHostPermission);
            var uatJobs = new BalanceRecordViewModel(
                balanceRecordService.GetJobStatus(UAT, dbAlias),
                UAT,
                dbAlias,
                hasHostPermission);

            return View("~/Views/BalanceRecord/Index.cshtml", Tuple.Create(proJobs, uatJobs));
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.DataSyncServices, Policy = Constant.Policy.Host)]
        public ActionResult Update(BalanceRecordJob[] jobs)
            => Json(balanceRecordService.UpdateJobStatus(jobs), JsonRequestBehavior.DenyGet);
    }
}
