namespace Alpha.Maintenance.Service.Impl.WinLossMonitor
{
    using System;
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Repository.Criteria.WinLossMonitor;
    using Alpha.Maintenance.Service.Interface.WinLossMonitor;
    using Fanex.Data.Repository;
    using Fanex.Logging;

    public class WinLossMonitorService : IWinLossMonitorService
    {
        private readonly IDynamicRepository dynamicRepository;

        public WinLossMonitorService(IDynamicRepository dynamicRepository = null)
            => this.dynamicRepository = dynamicRepository ?? new DynamicRepository();

        public GlobalSetting GetGlobalSetting(string dbAlias)
        {
            var settings = dynamicRepository.Fetch<GlobalSettingEntity>(new GetGlobalSettingCriteria(dbAlias));

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

        public IEnumerable<Result> UpdateGlobalSettings(GlobalSettingEntity[] entities, string dbAlias)
        {
            var resultList = new List<Result>();

            foreach (var entity in entities)
            {
                var result = new Result();

                try
                {
                    dynamicRepository.Execute(new UpdateGlobalSettingCriteria(
                        entity.ServiceName,
                        entity.SettingName,
                        entity.SettingValue ?? string.Empty,
                        dbAlias));
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
