namespace Alpha.Maintenance.Repository.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Announcements.Models;
    using Announcements.Specs;
    using Core;
    using Core.Helpers;
    using Core.Model;
    using Criteria.Announcements;
    using Fanex.Common.Extensions;
    using Fanex.Data.Repository;
    using Newtonsoft.Json;

    public interface AnnouncementRepository
    {
        IEnumerable<Announcement> GetAnnouncements(ICriteria criteria);

        Result SaveAnnouncement(ICriteria criteria);

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

        public Result SaveAnnouncement(ICriteria criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria), AnnouncementResources.CriteriaCannotBeNull);
            }

            try
            {
                using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
                {
                    db.ExecuteNonQuery(criteria.GetSpParams());
                    var outputParams = db.GetOuputParams();

                    var code = (int)outputParams["errcode"];

                    return new Result
                    {
                        Code = code,
                        Message = code == 0
                            ? AnnouncementResources.SaveSuccessfully
                            : outputParams["errmsg"].ToString()
                    };
                }
            }
            catch (Exception exception)
            {
                return new Result
                {
                    Code = 101,
                    Message = exception.Message
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
                Code = reader.ToString("Code"),
                Contents = JsonConvert.DeserializeObject<IEnumerable<MessageContent>>(plainText),
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
                ReleasedDate = reader.ToDateTime("FromDate"),
                ExpiredDate = reader.ToDateTime("ToDate"),
                CreatedDate = reader.ToDateTime("CreatedDate"),
                CreatorId = reader.ToInt("CreatorId"),
                CreatorName = reader.ToString("CreatorName"),
                LastEditDate = reader.IsDBNull("LastEditDate") ? (DateTime?)null : reader.ToDateTime("LastEditDate"),
                LastEditorId = reader.IsDBNull("LastEditorId") ? (int?)null : reader.ToInt("LastEditorId"),
                LastEditorName = reader.ToString("LastEditorName"),
                IsPopup = reader["announcementType"].ToString() == "3",
                // Images = reader.ToString("Images"),
            };
        }
    }
}