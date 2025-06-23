using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Core.Model.ResetPotentialWinLimitJobConfig;
using Alpha.Maintenance.Repository.Criteria.ResetPotentialWinLimitJob;
using Alpha.Maintenance.Service.Interface.ResetPotentialWinLimitJobConfig;
using Fanex.Data.Repository;

namespace Alpha.Maintenance.Service.Impl.ResetPotentialWinLimitJobConfig
{
    public class ResetPotentialWinLimitJobConfigServiceImpl : ResetPotentialWinLimitJobConfigService
    {
        private readonly IDynamicRepository repository;

        public ResetPotentialWinLimitJobConfigServiceImpl(IDynamicRepository repository = null)
        {
            this.repository = repository ?? new DynamicRepository();
        }

        public ResetPotentialWinLimitJobSettingModel GetResetPotentialWinLimitJobConfig()
        {
            var criteriaConfig = new GetResetPotentialWinLimitJobSettingCriteria("ResetPotentialWinLimitJob_Config");
            repository.Execute(criteriaConfig);

            var criteriaStatus = new GetResetPotentialWinLimitJobSettingCriteria("ResetPotentialWinLimitJob_Status");
            repository.Execute(criteriaStatus);

            var data = new ResetPotentialWinLimitJobSettingModel
            {
                Config = Newtonsoft.Json.JsonConvert.DeserializeObject<ResetPotentialWinLimitJobConfigModel>(criteriaConfig.VValue),
                Status = Newtonsoft.Json.JsonConvert.DeserializeObject<ResetPotentialWinLimitJobStatusModel>(criteriaStatus.VValue)
            };

            return data;
        }

        public Result Update(ResetPotentialWinLimitJobSettingModel setting)
        {
            var criteriaConfig = new UpdateResetPotentialWinLimitJobConfigCriteria("ResetPotentialWinLimitJob_Config", setting.Config.ToString());
            repository.Execute(criteriaConfig);

            var criteriaStatus = new UpdateResetPotentialWinLimitJobConfigCriteria("ResetPotentialWinLimitJob_Status", setting.Status.ToString());
            repository.Execute(criteriaStatus);

            return new Result { Code = 0, Message = "Success" };
        }
    }
}
