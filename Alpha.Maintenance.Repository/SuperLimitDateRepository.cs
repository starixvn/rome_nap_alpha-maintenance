namespace Alpha.Maintenance.Repository
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Model;
    using Alpha.Maintenance.Repository.Criteria.SuperLimitDate;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Fanex.NAP.Core.Command;

    public interface ISuperLimitDateRepository
    {
        CommandResult ExecuteNonQuery(CriteriaBase criteria);
        IEnumerable<SuperLimitDate> Get(GetAllSuperLimitDateCriteria criteria);
    }

    public class SuperLimitDateRepository : ISuperLimitDateRepository
    {
        public IEnumerable<SuperLimitDate> Get(GetAllSuperLimitDateCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();

                var list = db.Query<SuperLimitDate>(param);

                criteria.TotalRecords = Convert.ToInt32(param["totalrecords"]);

                return list;
            }
        }

        public CommandResult ExecuteNonQuery(CriteriaBase criteria)
        {
            Hashtable hash = criteria.GetSpParams();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }

        }

        private CommandResult GetCommandResult(Hashtable hash)
        {
            var errCode = hash["errcode"];
            var errMsg = hash["errmsg"];

            return new CommandResult
            {
                Code = (errCode is DBNull) ? 0 : Convert.ToInt32(errCode),
                Message = (errMsg is DBNull) ? string.Empty : Convert.ToString(errMsg)
            };
        }
    }
}
