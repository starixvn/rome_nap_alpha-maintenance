namespace Alpha.Maintenance.Repository.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using Alpha.Maintenance.Core.Model;
    using Fanex.Data.Repository;

    public interface BannerRepository
    {
        IEnumerable<Banner> GetBanners(ICriteria criteria);

        Tuple<Banner, IEnumerable<BannerDetail>> GetBanner(ICriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);

        void ExecuteNonQuery(string settingKey, Hashtable spParams);
    }

    public class BannerRepositoryImpl : BannerRepository
    {
        public IEnumerable<Banner> GetBanners(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    while (reader.Read())
                    {
                        yield return GetBanner(reader);
                    }
                }
            }
        }

        private static Banner GetBanner(IDataReader reader)
            => new Banner(
                Convert.ToInt32(reader["bannerid"]),
                reader["bannername"].ToString(),
                Convert.ToBoolean(reader["isactive"]),
                Convert.ToDateTime(reader["releaseddate"]),
                Convert.ToDateTime(reader["expirationdate"]),
                reader["environments"].ToString(),
                reader["systems"].ToString(),
                reader["desktop"].ToString(),
                reader["mobile"].ToString(),
                Convert.ToDateTime(reader["createddate"]));

        public Tuple<Banner, IEnumerable<BannerDetail>> GetBanner(ICriteria criteria)
        {
            Banner banner = null;
            IList<BannerDetail> bannerDetails = null;

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                {
                    if (reader.Read())
                    {
                        banner = GetBannerForEdit(reader);

                        if (reader.NextResult())
                        {
                            bannerDetails = new List<BannerDetail>();

                            while (reader.Read())
                            {
                                bannerDetails.Add(GetBannerDetail(reader));
                            }
                        }
                    }
                }
            }

            return new Tuple<Banner, IEnumerable<BannerDetail>>(banner, bannerDetails);
        }

        private static Banner GetBannerForEdit(IDataReader reader)
            => new Banner(
                Convert.ToInt32(reader["bannerid"]),
                reader["bannername"].ToString(),
                Convert.ToBoolean(reader["isactive"]),
                Convert.ToDateTime(reader["releaseddate"]),
                Convert.ToDateTime(reader["expirationdate"]),
                reader["environments"].ToString(),
                reader["systems"].ToString(),
                reader["mobilebackgroundcolor"].ToString());

        private static BannerDetail GetBannerDetail(IDataReader reader)
        {
            var imageContent = reader["imagecontent"];

            return new BannerDetail(
                Convert.ToByte(reader["type"]),
                reader["languagecode"].ToString(),
                imageContent == null || imageContent == DBNull.Value ? null : (byte[])imageContent);
        } 

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

        public void ExecuteNonQuery(string settingKey, Hashtable spParams)
        {
            using (var db = ObjectDbFactory.CreateInstance(settingKey))
            {
                db.ExecuteNonQuery(spParams);
            }
        }
    }
}
