namespace Alpha.Maintenance.Repository.Criteria.WinLossMonitor
{
    using Fanex.Data.Repository;

    public class GetGlobalSettingCriteria : CriteriaBase
    {
        public override string GetSettingKey()
            => "Acc_WLMonitored_Service_GlobalSettingSel";
    }
}
