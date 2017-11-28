using System;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Repository.Criteria.IPInformation;

namespace Alpha.Maintenance.Repository.Impl
{
    public interface IPInformationRepository
    {
        IPInfo GetIPInfo(GetIPInformationCriteria criteria);
    }

    public class IPInformationRepositoryImpl : IPInformationRepository
    {
        public IPInfo GetIPInfo(GetIPInformationCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    while (reader != null)
                    {
                        if (reader.Read())
                        {
                            return new IPInfo(
                                dataSource: "IPDB",
                                countryCode: !Convert.IsDBNull(reader["countrySHORT"]) ? Convert.ToString(reader["countrySHORT"]) : string.Empty,
                                countryName: !Convert.IsDBNull(reader["countryLONG"]) ? Convert.ToString(reader["countryLONG"]) : string.Empty,
                                region: !Convert.IsDBNull(reader["ipRegion"]) ? Convert.ToString(reader["ipRegion"]) : string.Empty,
                                city: !Convert.IsDBNull(reader["ipCity"]) ? Convert.ToString(reader["ipCity"]) : string.Empty);
                        }
                        reader.NextResult();
                    }
                }
            }

            return null;
        }
    }
}
