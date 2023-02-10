namespace Alpha.Maintenance.Core.Model
{
    public class BalanceRecordJob
    {
        public BalanceRecordJob()
        {
        }

        public BalanceRecordJob(
            string name,
            bool status,
            string environment,
            string dbAlias)
        {
            Name = name;
            Status = status;
            Environment = environment;
            DBAlias = dbAlias;
        }

        public string Name { get; set; }

        public bool Status { get; set; }

        public string Environment { get; set; }

        public string DBAlias { get; set; }
    }
}
