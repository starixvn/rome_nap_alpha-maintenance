namespace Alpha.Maintenance.Service.Interface.WinLossMonitor
{
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Model;

    public interface IWinLossMonitorService
    {
        GlobalSetting GetGlobalSetting(string dbAlias);

        IEnumerable<Result> UpdateGlobalSettings(GlobalSettingEntity[] entities, string dbAlias);
    }
}
