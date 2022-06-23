using System;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Repository.Criteria.Chatbot;
using Fanex.Common;
using System.Collections.Generic;
using System.Data;
using Fanex.Data.Repository;

namespace Alpha.Maintenance.Repository.Impl
{
    public interface ChatbotRepository
    {
        IEnumerable<Bot> GetBots(GetAllBotCriteria criteria);

        Bot GetBot(GetBotCriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);
    }

    public class ChatbotRepositoryImpl : ChatbotRepository
    {
        public IEnumerable<Bot> GetBots(GetAllBotCriteria criteria)
        {
            var data = new List<Bot>();

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            data.Add(BuildBot(reader));
                        }
                    }
                }
            }

            return data;
        }

        public Bot GetBot(GetBotCriteria criteria)
        {
            Bot data = null;

            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader(criteria))
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            data = BuildBot(reader);
                        }
                    }
                }
            }

            return data;
        }

        public Result ExecuteNonQuery(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
                var outputParams = db.GetOuputParams();

                return new Result
                {
                    Code = Convert.ToInt32(outputParams["ErrCode"]),
                    Message = outputParams["ErrMsg"].ToString()
                };
            }
        }

        private Bot BuildBot(IDataReader reader)
            => new Bot(
                Converter.ToInt(reader["BotId"]),
                Converter.ToString2(reader["DisplayName"]),
                Converter.ToString2(reader["username"]),
                Converter.ToString2(reader["System"]),
                Converter.ToString2(reader["Accesstoken"]),
                Converter.ToBool(reader["Status"]),
                Converter.ToBool(reader["IsTest"]));
    }
}
