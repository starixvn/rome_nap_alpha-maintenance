namespace Alpha.Maintenance.Repository.Criteria.WinLossMonitor
{
    using Fanex.Data.Repository;

    public class GetGlobalSettingCriteria : CriteriaBase
    {
        public GetGlobalSettingCriteria(string dbAlias)
            => DBAlias = dbAlias;

        [SpParam(Ignored = true)]
        public string DBAlias { get; }

        public override string GetSettingKey()
            => string.IsNullOrWhiteSpace(DBAlias)
            ? "Acc_WLMonitored_Service_GlobalSettingSel"
            : $"{DBAlias}_Acc_WLMonitored_Service_GlobalSettingSel";
    }
}
