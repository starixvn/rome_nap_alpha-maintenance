namespace Alpha.Maintenance.Repository.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Announcements.Client.Helpers;
    using Announcements.Models;
    using Announcements.Specs;
    using Core;
    using Core.Model;
    using Criteria.Announcements;
    using Fanex.Common.Extensions;
    using Fanex.Data.Repository;
    using Newtonsoft.Json;

    public interface AnnouncementRepository
    {
        IEnumerable<Announcement> GetAnnouncements(ICriteria criteria);

        Result InsertOrUpdateAnnouncement(ICriteria criteria);

        Announcement GetAnnouncementById(SelectAnnouncementByIdCriteria criteria);
    }

    public class AnnouncementRepositoryImpl : AnnouncementRepository
    {
        public AnnouncementRepositoryImpl()
        {
        }

        public IEnumerable<Announcement> GetAnnouncements(ICriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), AnnouncementResources.CriteriaCannotBeNull);
            }

            var data = new List<Announcement>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
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

        public Result InsertOrUpdateAnnouncement(ICriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), AnnouncementResources.CriteriaCannotBeNull);
            }

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                var code = (int)outputParams["errcode"];

                return new Result
                {
                    Code = code,
                    Message = code == 0
                        ? AnnouncementResources.UpdateSuccessfully
                        : outputParams["errmsg"].ToString()
                };
            }
        }

        public Announcement GetAnnouncementById(SelectAnnouncementByIdCriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), AnnouncementResources.CriteriaCannotBeNull);
            }

            var data = new Announcement();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
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
            var encodedContents = Convert.FromBase64String(reader.ToString("Contents"));
            var plainText = Encoding.UTF8.GetString(encodedContents);

            return new Announcement
            {
                AnnouncementType = (AnnouncementType)reader.ToInt("AnnouncementType"),
                Code = reader.ToString("Code"),
                CreatedDate = reader.ToDateTime("CreatedDate"),
                Contents = JsonConvert.DeserializeObject<IEnumerable<MessageContent>>(plainText),
                Ordinal = reader.ToInt("Ordinal"),
                Environments = reader.ToString("Environments").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                        .Select(environment => environment.Trim()),
                Id = reader.ToInt("Id"),
                IsActive = reader.ToBool("IsActive"),
                Specifications = JsonConvert.DeserializeObject<IEnumerable<ISpecification<IParam>>>(
                            reader.ToString("Specifications"),
                            new SpecificationConverter()),
                Systems = reader.ToString("Systems").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                                    .Select(system => system.Trim()),
                Title = reader.ToString("Title"),
                FromDate = reader.ToDateTime("FromDate"),
                ToDate = reader.ToDateTime("ToDate")
            };
        }
    }
}