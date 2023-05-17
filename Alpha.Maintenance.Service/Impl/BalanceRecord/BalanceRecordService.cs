namespace Alpha.Maintenance.Service.Impl.BalanceRecord
{
    using System;
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Configurations;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Repository.Criteria.BalanceRecord;
    using Alpha.Maintenance.Repository.Impl;
    using Alpha.Maintenance.Service.Interface.BalanceRecord;
    using Fanex.Logging;

    public class BalanceRecordService : IBalanceRecordService
    {
        private readonly IBalanceRecordRepository repository;

        public BalanceRecordService(IBalanceRecordRepository repository = null)
            => this.repository = repository ?? new BalanceRecordRepository();

        public IEnumerable<BalanceRecordJob> GetJobStatus(string enviroment, string dbAlias)
        {
            var jobNames = AppSettings.BalanceRecordJobNames;

            foreach (var item in jobNames)
            {
                yield return new BalanceRecordJob(
                    item,
                    "1" == repository.GetJobStatus(new GetBalanceRecordJobStatusCriteria(item, enviroment, dbAlias: dbAlias)),
                    enviroment,
                    dbAlias);
            }
        }

        public IEnumerable<Result> UpdateJobStatus(IEnumerable<BalanceRecordJob> jobs)
        {
            var resultList = new List<Result>();

            foreach (var item in jobs)
            {
                var result = new Result();

                try
                {
                    repository.UpdateJobStatus(new UpdateBalanceRecordJobStatusCriteria(
                        item.Name,
                        item.Environment,
                        item.Status ? "1" : "0",
                        item.DBAlias));
                }
                catch (Exception ex)
                {
                    result = new Result
                    {
                        Code = 500,
                        Message = $"{item.Name}_{item.Environment}: Updated status to {item.Status} fail."
                    };

                    LogService.Error(ex.Message, ex);
                }

                resultList.Add(result);
            }

            return resultList;
        }
    }
}
