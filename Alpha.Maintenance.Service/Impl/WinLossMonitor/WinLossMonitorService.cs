namespace Alpha.Maintenance.Service.Impl.WinLossMonitor
{
    using System;
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Repository.Criteria.WinLossMonitor;
    using Alpha.Maintenance.Repository.Impl;
    using Alpha.Maintenance.Service.Interface.WinLossMonitor;
    using Fanex.Logging;

    public class WinLossMonitorService : IWinLossMonitorService
    {
        private readonly IWinLossMonitorRepository repository;

        public WinLossMonitorService(IWinLossMonitorRepository repository = null)
            => this.repository = repository ?? new WinLossMonitorRepository();

        public GlobalSetting GetGlobalSetting()
        {
            var settings = repository.GetGlobalSettings(new GetGlobalSettingCriteria());

            return new GlobalSetting(ToDictionary(settings));
        }

        private static IDictionary<string, string> ToDictionary(IEnumerable<GlobalSettingEntity> settings)
        {
            var settingsDict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in settings)
            {
                settingsDict[$"{item.ServiceName}_{item.SettingName}"] = item.SettingValue;
            }

            return settingsDict;
        }

        public IEnumerable<Result> UpdateGlobalSettings(GlobalSettingEntity[] entities)
        {
            var resultList = new List<Result>();

            foreach (var entity in entities)
            {
                var result = new Result();

                try
                {
                    repository.UpdateGlobalSetting(
                        new UpdateGlobalSettingCriteria(entity.ServiceName, entity.SettingName, entity.SettingValue ?? string.Empty));
                }
                catch (Exception ex)
                {
                    result = new Result
                    {
                        Code = 500,
                        Message = $"{entity.ServiceName} - {entity.SettingName} - {entity.SettingValue}: Updated fail."
                    };

                    LogService.Error(ex.Message, ex);
                }

                resultList.Add(result);
            }

            return resultList;
        }
    }
}
