namespace Alpha.Maintenance.Controller.ViewModels
{
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Model;

    public class BalanceRecordViewModel
    {
        public BalanceRecordViewModel(
            IEnumerable<BalanceRecordJob> balanceRecordJobs,
            string environment,
            string dbAlias,
            bool hasHostPermission)
        {
            BalanceRecordJobs = balanceRecordJobs;
            Environment = environment;
            DBAlias = dbAlias;
            HasHostPermission = hasHostPermission;
        }

        public IEnumerable<BalanceRecordJob> BalanceRecordJobs { get; }

        public string Environment { get; }

        public string DBAlias { get; }

        public bool HasHostPermission { get; }
    }
}
