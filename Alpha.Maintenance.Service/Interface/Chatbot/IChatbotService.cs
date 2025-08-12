using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Repository.Criteria.Chatbot;
using System.Collections.Generic;

namespace Alpha.Maintenance.Service.Interface.Chatbot
{
    public interface IChatbotService
    {
        IEnumerable<Bot> GetBots(bool isTest, bool enabled);

        Bot GetBot(int botId);

        Result CreateBot(CreateBotCriteria criteria);

        Result UpdateBot(UpdateBotCriteria criteria);

        IEnumerable<BotSystemSetting> GetBotSystemSetting(string serviceName = "");

        Result UpdateBotSystemSetting(int id, string value, int creator, string ip);
    }
}
