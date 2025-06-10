using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Repository.Criteria.AutoTransferJobConfig;
using Alpha.Maintenance.Service.Interface.AutoTransferJobConfig;
using Fanex.Data.Repository;

namespace Alpha.Maintenance.Service.Impl.AutoTransferJobConfig
{
    public class AutoTransferJobConfigServiceImpl : AutoTransferJobConfigService
    {
        private readonly IDynamicRepository repository;

        public AutoTransferJobConfigServiceImpl(DynamicRepository repository = null)
        {
            this.repository = repository ?? new DynamicRepository();
        }

        public AutoTransferJobConfigModel GetAutoTransferJobConfig(string site, string env)
        {
            var criteria = new GetAutoTransferJobConfigCriteria(site, env);
            var data = repository.Get<AutoTransferJobConfigModel>(criteria);

            data.Site = site;
            data.Environment = env;

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
                config.Site, defaultTime, config.ManualTime, config.RunningMode, config.IsActive, config.Environment, config.TestSuperIds);
            repository.Execute(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };
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
