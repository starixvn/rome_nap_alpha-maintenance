namespace Alpha.Maintenance.Repository.Impl
{
    using System.Collections.Generic;
    using System.Data;
    using Alpha.Maintenance.Core.Model;
    using Fanex.Data.Repository;

    public interface IWinLossMonitorRepository
    {
        IEnumerable<GlobalSettingEntity> GetGlobalSettings(ICriteria criteria);

        void UpdateGlobalSetting(ICriteria criteria);
    }

    public class WinLossMonitorRepository : IWinLossMonitorRepository
    {
        public IEnumerable<GlobalSettingEntity> GetGlobalSettings(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return GetGlobalSettingEntity(reader);
                    }
                }
            }
        }

        private static GlobalSettingEntity GetGlobalSettingEntity(IDataReader reader)
            => new GlobalSettingEntity(
                reader["servicename"].ToString(),
                reader["settingname"].ToString(),
                reader["settingvalue"].ToString());

        public void UpdateGlobalSetting(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }
    }
}
