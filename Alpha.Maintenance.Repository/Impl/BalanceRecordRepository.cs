namespace Alpha.Maintenance.Repository.Impl
{
    using Fanex.Data.Repository;

    public interface IBalanceRecordRepository
    {
        string GetJobStatus(ICriteria criteria);

        void UpdateJobStatus(ICriteria criteria);
    }

    public class BalanceRecordRepository : IBalanceRecordRepository
    {
        public string GetJobStatus(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());

                return db.GetOuputParams()["vvalue"].ToString();
            }
        }

        public void UpdateJobStatus(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }
    }
}
