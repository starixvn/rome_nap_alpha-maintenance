namespace Alpha.Maintenance.Service.Impl.BalanceRecord
{
    using System;
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Configurations;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Repository.Criteria.BalanceRecord;
    using Alpha.Maintenance.Service.Interface.BalanceRecord;
    using Fanex.Data.Repository;
    using Fanex.Logging;

    public class BalanceRecordService : IBalanceRecordService
    {
        private readonly IDynamicRepository dynamicRepository;

        private const int UpdateFailStatus = 500;

        public BalanceRecordService(IDynamicRepository dynamicRepository = null)
            => this.dynamicRepository = dynamicRepository ?? new DynamicRepository();

        public IEnumerable<BalanceRecordJob> GetJobStatus(string enviroment, string dbAlias)
        {
            var jobNames = AppSettings.BalanceRecordJobNames;

            foreach (var item in jobNames)
            {
                yield return new BalanceRecordJob(
                    item,
                    "1" == GetJobStatus(new GetBalanceRecordJobStatusCriteria(item, enviroment, dbAlias: dbAlias)),
                    enviroment,
                    dbAlias);
            }
        }

        private string GetJobStatus(GetBalanceRecordJobStatusCriteria criteria)
        {
            dynamicRepository.Execute(criteria);

            return criteria.VValue;
        }

        public IEnumerable<Result> UpdateJobStatus(IEnumerable<BalanceRecordJob> jobs)
        {
            var resultList = new List<Result>();

            foreach (var item in jobs)
            {
                var result = new Result();

                try
                {
                    var criteria = new UpdateBalanceRecordJobStatusCriteria(
                        item.Name,
                        item.Environment,
                        item.Status ? "1" : "0",
                        item.DBAlias);

                    dynamicRepository.Execute(criteria);
                }
                catch (Exception ex)
                {
                    result = new Result
                    {
                        Code = UpdateFailStatus,
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
