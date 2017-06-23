namespace Alpha.Maintenance.Repository.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Announcements.Client.Helpers;
    using Announcements.Models;
    using Announcements.Specs;
    using Core.Model;
    using Criteria.Announcements;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Newtonsoft.Json;

    public interface AnnouncementRepository
    {
        IEnumerable<Site> GetSites();

        IEnumerable<Announcement> GetAnnouncements(GetAnnouncementsCriteria criteria);

        Result InsertAnnouncement(ICriteria criteria);

        Announcement EditAnnouncement(EditAnnouncementsCriteria criteria);

    }

    public class AnnouncementRepositoryImpl : AnnouncementRepository
    {
        public IEnumerable<Site> GetSites()
        {
            return new List<Site>
            {
                new Site { Id = 1, Name = "Site 1" },
                new Site { Id = 2, Name = "Site 2" },
                new Site { Id = 3, Name = "Site 3" },
                new Site { Id = 4, Name = "Site 4" }
            };
        }

        public IEnumerable<Announcement> GetAnnouncements(GetAnnouncementsCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var data = new List<Announcement>();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            data.Add(BuildAnnouncement(reader));
                        }
                    }
                }
            }

            return data;
        }

        public Result InsertAnnouncement(ICriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();
                var code = 0; //(int)outputParams["errcode"];

                return new Result
                {
                    Code = code,
                    Message = code == 0
                        ? "Update successfully!"
                        : outputParams["errmsg"].ToString()
                };
            }
        }

        public Announcement EditAnnouncement(EditAnnouncementsCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), "Criteria cannot be null");
            }

            var data = new Announcement();

            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            data = BuildAnnouncement(reader);
                        }
                    }
                }
            }

            return data;
        }

        private Announcement BuildAnnouncement(IDataReader reader)
        {
            var announcement = new Announcement
            {
                AnnouncementType = (AnnouncementType)reader.ToInt("AnnouncementType"),
                Code = reader.ToString("Code"),
                Contents = JsonConvert.DeserializeObject<IEnumerable<MessageContent>>(reader.ToString("Contents")),
                Order = reader.ToInt("Order"),
                Environments = reader.ToString("Environments").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                .Select(environment => environment.Trim()),
                Id = reader.ToInt("Id"),
                IsActive = reader.ToBool("IsActive"),
                Specifications = JsonConvert.DeserializeObject<IEnumerable<ISpecification<IUser>>>(
                    reader.ToString("Specifications"),
                    new SpecificationConverter()),
                Systems = reader.ToString("Systems").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                            .Select(system => system.Trim()),
                Title = reader.ToString("Title"),
                FromDate = reader.ToDateTime("FromDate"),
                ToDate = reader.ToDateTime("ToDate")
            };

            return announcement;
        }
    }
}