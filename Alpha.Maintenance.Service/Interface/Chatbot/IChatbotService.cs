using Alpha.Maintenance.Core.Model;
using System.Collections.Generic;

namespace Alpha.Maintenance.Service.Interface.Chatbot
{
    public interface IChatbotService
    {
        IEnumerable<Bot> GetBots(bool isTest, bool enabled);

        Bot GetBot(int botId);
    }
}
