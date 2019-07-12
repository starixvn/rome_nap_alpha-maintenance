namespace Alpha.Maintenance.Repository.Impl
{
    using Core.Model;
    using Criteria.IPInformation;
    using Fanex.Common;

    public interface IPInformationRepository
    {
        IPInfo GetIPInfo(GetIPInformationCriteria criteria);

        IPInfo GetIPInfoFromTableMini(GetIPInfoFromTableMiniCriteria criteria);
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

        public IPInfo GetIPInfoFromTableMini(GetIPInfoFromTableMiniCriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader.Read())
                    {
                        return new IPInfo(
                            dataSource: "IPDB (Table Mini)",
                            countryCode: Converter.ToString2(reader["countrySHORT"]),
                            countryName: Converter.ToString2(reader["countryLONG"]),
                            region: string.Empty,
                            city: string.Empty);
                    }
                }
            }

            return new IPInfo("IPDB (Table Mini)", "-", "-", "-", "-");
        }
    }
}