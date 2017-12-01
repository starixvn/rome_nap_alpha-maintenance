namespace Alpha.Maintenance.Repository.Impl
{
    using Core.Model;
    using Criteria.IPInformation;
    using Fanex.Common;

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
                                countryCode: Converter.ToString2(reader["countrySHORT"]),
                                countryName: Converter.ToString2(reader["countryLONG"]),
                                region: Converter.ToString2(reader["ipRegion"]),
                                city: Converter.ToString2(reader["ipCity"]));
                        }

                        reader.NextResult();
                    }
                }
            }

            return null;
        }
    }
}
