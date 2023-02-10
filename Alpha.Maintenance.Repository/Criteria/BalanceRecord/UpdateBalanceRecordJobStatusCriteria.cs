namespace Alpha.Maintenance.Repository.Criteria.BalanceRecord
{
    public class UpdateBalanceRecordJobStatusCriteria : GetBalanceRecordJobStatusCriteria
    {
        public UpdateBalanceRecordJobStatusCriteria(string jobName, string enviroment, string vvalue, string dbAlias)
            : base(jobName, enviroment, vvalue, dbAlias)
        {
        }

        public override string GetSettingKey()
            => $"{DBAlias}_SystemSettings_Update";
    }
}
