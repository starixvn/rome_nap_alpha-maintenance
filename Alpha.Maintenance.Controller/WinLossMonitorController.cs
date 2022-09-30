namespace Alpha.Maintenance.Controller
{
    using System;
    using System.Web.Mvc;
    using Alpha.Maintenance.Controller.ViewModels;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Service.Impl.WinLossMonitor;
    using Alpha.Maintenance.Service.Interface.WinLossMonitor;
    using Fanex.NAP.Core.Business;
    using Fanex.NAP.Core.Web.Attribute;

    [NapSuperAuth]
    public class WinLossMonitorController : BaseController
    {
        private const string UAT = "UAT";
        private const string WLMonitoredAccount = "WLMonitoredAccount";
        private const string Dep_WLMonitoredAccount = "Dep_WLMonitoredAccount";

        private readonly IWinLossMonitorService winLossMonitorService;

        public WinLossMonitorController() : this(null)
        {
        }

        private WinLossMonitorController(IWinLossMonitorService winLossMonitorService)
            => this.winLossMonitorService = winLossMonitorService ?? new WinLossMonitorService();

        [NapAuth(FunctionKey = SiteFunctionKeys.MyWatchlistMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Credit(string dbAlias) => GetSettings(WLMonitoredAccount, dbAlias);

        [NapAuth(FunctionKey = SiteFunctionKeys.MyWatchlistMonitor, Policy = Constant.Policy.Read)]
        public ActionResult Deposit(string dbAlias) => GetSettings(Dep_WLMonitoredAccount, dbAlias);

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.MyWatchlistMonitor, Policy = Constant.Policy.Host)]
        public ActionResult Update(GlobalSettingEntity[] globalSettings, string dbAlias)
            => Json(winLossMonitorService.UpdateGlobalSettings(globalSettings, dbAlias), JsonRequestBehavior.DenyGet);

        private ActionResult GetSettings(string serviceName, string dbAlias)
        {
            var hasHostPermission = HasRight(SiteFunctionKeys.MyWatchlistMonitor, Constant.Policy.Host);
            var globalSetting = winLossMonitorService.GetGlobalSetting(dbAlias);

            return View("~/Views/WinLossMonitor/Index.cshtml", new Tuple<WinLossMonitorViewModel, WinLossMonitorViewModel>(
                GetWinLossMonitorViewModel(globalSetting, serviceName, string.Empty, hasHostPermission, dbAlias),
                GetWinLossMonitorViewModel(globalSetting, serviceName, UAT, hasHostPermission, dbAlias)
            ));
        }

        private static WinLossMonitorViewModel GetWinLossMonitorViewModel(
            GlobalSetting globalSetting,
            string serviceName,
            string env,
            bool hasHostPermission,
            string dbAlias)
        {
            var isCreditJob = WLMonitoredAccount == serviceName;
            var viewModel = new WinLossMonitorViewModel
            {
                ChangeAccumulatedWinLossJob = globalSetting.IsRunningJob(env, serviceName, nameof(WinLossMonitorViewModel.ChangeAccumulatedWinLossJob)),
                LastCheckTime = globalSetting.GetLastCheckTime(env, serviceName),
                ChangeThresholdWinLossJob = globalSetting.IsRunningJob(env, serviceName, nameof(WinLossMonitorViewModel.ChangeThresholdWinLossJob)),
                LastScanSettingDate = globalSetting.GetLastScanSettingDate(env, serviceName),
                SuperUUS = globalSetting.GetSuperUUS(serviceName),
                Environment = env,
                IsCreditJob = isCreditJob,
                ServiceName = string.IsNullOrWhiteSpace(env) ? serviceName : $"{env}-{serviceName}",
                HasHostPermission = hasHostPermission,
                DBAlias = dbAlias
            };

            if (isCreditJob)
            {
                viewModel.AccumulateWinLossJob = globalSetting.IsRunningJob(env, serviceName, nameof(WinLossMonitorViewModel.AccumulateWinLossJob));
                viewModel.LastId = globalSetting.GetLastId(env, serviceName);
            }

            return viewModel;
        }
    }
}
