namespace Alpha.Maintenance.Repository.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Announcements.Models;
    using Core.Model;
    using Criteria.Announcements;
    using Fanex.Common.Extensions;
    using Fanex.Data;

    public interface AnnouncementRepository
    {
        IEnumerable<Site> GetSites();

        IEnumerable<Announcement> GetAnnouncements(GetAnnouncementsCriteria criteria);
    }

    public class AnnouncementRepositoryImpl : AnnouncementRepository
    {
        public IEnumerable<Site> GetSites()
        {
            return new List<Site>
            {
                new Site { Id = 1, Name = "Site 1" }
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

        private Announcement BuildAnnouncement(IDataReader reader)
        {
            var announcement = new Announcement
            {
                AnnouncementType = (AnnouncementType)reader.ToInt("AnnouncementType"),
                Code = reader.ToString("Code"),
                //Contents = reader.ToString("AnnouncementType"),
                Environments = reader.ToString("Environments").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries),
                Id = reader.ToInt("Id"),
                IsActive = reader.ToBool("IsActive"),
                //Specifications = reader.ToString("AnnouncementType"),
                Systems = reader.ToString("Systems").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries),
                Title = reader.ToString("Title"),
                FromDate = reader.ToDateTime("FromDate"),
                ToDate = reader.ToDateTime("ToDate")
            };

            return announcement;
        }
    }
}