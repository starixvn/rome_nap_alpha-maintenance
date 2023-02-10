namespace Alpha.Maintenance.Repository.Criteria.BalanceRecord
{
    using Fanex.Data.Repository;

    public class GetBalanceRecordJobStatusCriteria : CriteriaBase
    {
        public GetBalanceRecordJobStatusCriteria(
            string jobName,
            string enviroment,
            string vvalue = "",
            string dbAlias = "DB05")
        {
            VName = $"BT_{jobName}_ServiceStatus{(string.IsNullOrWhiteSpace(enviroment) ? string.Empty : "_UAT")}";
            VValue = vvalue;
            DBAlias = dbAlias;
        }

        public string VName { get; }

        public string VValue { get; }

        [SpParam(Ignored = true)]
        public string DBAlias { get; }

        public override string GetSettingKey()
            => $"{DBAlias}_SystemSettings_Select";
    }
}
