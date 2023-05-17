namespace Alpha.Maintenance.Service.Interface.BalanceRecord
{
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Model;

    public interface IBalanceRecordService
    {
        IEnumerable<BalanceRecordJob> GetJobStatus(string enviroment, string dbAlias);

        IEnumerable<Result> UpdateJobStatus(IEnumerable<BalanceRecordJob> jobs);
    }
}
