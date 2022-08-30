using Alpha.Maintenance.Core.Model;
using System.Collections.Generic;

namespace Alpha.Maintenance.Service.Interface.Chatbot
{
    public interface IChatbotService
    {
        IEnumerable<Bot> GetBots(bool isTest, bool enabled);

        Bot GetBot(int botId);

        Result CreateBot(string username, string displayName, string system, string accessToken, bool status, bool isTest, int creator, string ip);

        Result UpdateBot(int botId, string username, string displayName, string system, string accessToken, bool status, bool isTest, int creator, string ip);

        IEnumerable<BotSystemSetting> GetBotSystemSetting(string serviceName = "");

        Result UpdateBotSystemSetting(int id, string value, int creator, string ip);
    }
}
