using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Repository.Criteria.DBProfiler;
using Fanex.Data;

namespace Alpha.Maintenance.Repository.Impl.DBProfiler
{
    public class LogRespository : BaseRepository<DbProfilerViewModel>
    {
        public List<DbProfilerViewModel> GetLog(ref DBProfilerModel model)
        {
            var criteria = new GetLogCriteria
            {
                Site = model.Site,
                Mode = model.Mode,
                TextDataLike = model.TextDataLike,
                TextDataLike2 = model.TextDataLike2,
                TextDataLike3 = model.TextDataLike3,
                TraceStartEvents = model.TraceStartEvents,
                TraceTimer = model.TraceTimer,
                ErrorMsg = string.Empty,
                HasError = false
            };

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                var param = criteria.GetSpParams();

                var dbSetting = db.DbSetting;
                var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(dbSetting.ConnectionString);

                sqlConnectionStringBuilder.DataSource = model.Server;
                sqlConnectionStringBuilder.ConnectTimeout = 125;

                if (criteria.Site.Equals("FPS", StringComparison.CurrentCultureIgnoreCase))
                {
                    sqlConnectionStringBuilder.InitialCatalog = "perfmonDB";
                }

                dbSetting.ConnectionString = sqlConnectionStringBuilder.ToString();

                using (var objectDb = new ObjectDb(dbSetting))
                {
                    var result = objectDb.Query<DbProfilerViewModel>(param).ToList();

                    model.HasError = Convert.ToBoolean(param["haserror"]);
                    model.ErrorMsg = param["errormsg"].ToString();

                    return result;
                }
            }
        }
    }
}
