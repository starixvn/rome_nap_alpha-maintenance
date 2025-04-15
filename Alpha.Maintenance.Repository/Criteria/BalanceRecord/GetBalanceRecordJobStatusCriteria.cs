namespace Alpha.Maintenance.Repository.Criteria.BalanceRecord
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetBalanceRecordJobStatusCriteria : NonQueryCommand
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

        public string VName { get; set; }

        public string VValue { get; set; }

        [SpParam(Ignored = true)]
        public string DBAlias { get; set; }

        public override string GetSettingKey()
            => $"{DBAlias}_SystemSettings_Select";

        public override bool IsValid()
            => true;
    }
}
