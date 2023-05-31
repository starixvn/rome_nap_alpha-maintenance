namespace Alpha.Maintenance.Repository.Impl
{
    using System;
    using Alpha.Maintenance.Core.Model;
    using Fanex.Data.Repository;

    public interface AutoTransferJobConfigRepository
    {
        Result ExecuteNonQuery(ICriteria criteria);
    }

    public class AutoTransferJobConfigRepositoryImpl : AutoTransferJobConfigRepository
    {
        public Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                return new Result
                {
                    Code = Convert.ToInt32(outputParams["errcode"]),
                    Message = outputParams["errmsg"].ToString()
                };
            }
        }
    }
}
