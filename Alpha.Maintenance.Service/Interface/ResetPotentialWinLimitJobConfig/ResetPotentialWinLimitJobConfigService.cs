using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Core.Model.ResetPotentialWinLimitJobConfig;

namespace Alpha.Maintenance.Service.Interface.ResetPotentialWinLimitJobConfig
{
    public interface ResetPotentialWinLimitJobConfigService
    {
        ResetPotentialWinLimitJobSettingModel GetResetPotentialWinLimitJobConfig();
        Result Update(ResetPotentialWinLimitJobSettingModel setting);
    }
}
