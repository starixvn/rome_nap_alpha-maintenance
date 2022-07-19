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
                db.DbSetting.ConnectionString = BuildConnectionString(model.Server, criteria.Site, db.DbSetting.ConnectionString);                
                var result = db.Query<DbProfilerViewModel>(param).ToList();
                model.HasError = Convert.ToBoolean(param["haserror"]);
                model.ErrorMsg = param["errormsg"].ToString();
                return result;
               
            }
        }

        private static string BuildConnectionString(string serverIP, string siteName, string connectionString)
        {
            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
            sqlConnectionStringBuilder.DataSource = serverIP;
            sqlConnectionStringBuilder.ConnectTimeout = 125;

            if (siteName.Equals("FPS", StringComparison.CurrentCultureIgnoreCase))
            {
                sqlConnectionStringBuilder.InitialCatalog = "perfmonDB";
            }

            return  sqlConnectionStringBuilder.ToString();           
        }
    }
}
