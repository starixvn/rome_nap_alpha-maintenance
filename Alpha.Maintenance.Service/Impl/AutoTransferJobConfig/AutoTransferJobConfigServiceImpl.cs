using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Repository.Criteria.AutoTransferJobConfig;
using Alpha.Maintenance.Repository.Impl;
using Alpha.Maintenance.Service.Interface.AutoTransferJobConfig;
using Fanex.Data.Repository;

namespace Alpha.Maintenance.Service.Impl.AutoTransferJobConfig
{
    public class AutoTransferJobConfigServiceImpl : AutoTransferJobConfigService
    {
        private readonly IDynamicRepository repository;
        private readonly AutoTransferJobConfigRepository autoTransferJobConfigRepository;

        public AutoTransferJobConfigServiceImpl(DynamicRepository repository = null, AutoTransferJobConfigRepository autoTransferJobConfigRepository = null)
        {
            this.repository = repository ?? new DynamicRepository();
            this.autoTransferJobConfigRepository = autoTransferJobConfigRepository ?? new AutoTransferJobConfigRepositoryImpl();
        }

        public AutoTransferJobConfigModel GetAutoTransferJobConfig(string site)
        {
            var criteria = new GetAutoTransferJobConfigCriteria(site);
            var data = repository.Get<AutoTransferJobConfigModel>(criteria);
            data.Site = site;

            return data;
        }

        public Result Update(AutoTransferJobConfigModel config)
        {
            var defaultTime = GetDefaultTime(config.DefaultTime);

            if (string.IsNullOrWhiteSpace(defaultTime))
            {
                return new Result { Code = 1, Message = "Invalid input values" };
            }

            var criteria = new UpdateAutoTransferJobConfigCriteria(
                config.Site, defaultTime, config.ManualTime, config.RunningMode, config.IsActive);

            return autoTransferJobConfigRepository.ExecuteNonQuery(criteria);
        }

        private string GetDefaultTime(string defaultTime)
        {
            var parts = defaultTime.Split(':');
            if (parts.Length < 2 || !int.TryParse(parts[0], out int h) || !int.TryParse(parts[1], out int m))
            {
                return string.Empty;
            }

            return h.ToString("00") + ":" + m.ToString("00");
        }
    }
}
