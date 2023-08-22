using Alpha.Maintenance.Core.Model;

namespace Alpha.Maintenance.Service.Interface.AutoTransferJobConfig
{
    public interface AutoTransferJobConfigService
    {
        AutoTransferJobConfigModel GetAutoTransferJobConfig(string site);
        Result Update(AutoTransferJobConfigModel config);
    }
}
