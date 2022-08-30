using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Repository.Criteria.Chatbot;
using Alpha.Maintenance.Repository.Impl;
using Alpha.Maintenance.Service.Interface.Chatbot;
using System.Collections.Generic;

namespace Alpha.Maintenance.Service.Impl.Chatbot
{
    public class ChatbotService : IChatbotService
    {
        private readonly IChatbotRepository chatbotRepository;

        public ChatbotService()
            : this(null)
        {
        }

        public ChatbotService(IChatbotRepository chatbotRepository)
        {
            this.chatbotRepository = chatbotRepository ?? new ChatbotRepositoryImpl();
        }

        public IEnumerable<Bot> GetBots(bool isTest, bool enabled)
            => chatbotRepository.GetBots(new GetAllBotCriteria(isTest, enabled));

        public Bot GetBot(int botId)
            => chatbotRepository.GetBot(new GetBotCriteria(botId));

        public Result CreateBot(string username, string displayName,  string system, string accessToken, bool status, bool isTest, int creator, string ip)
            => chatbotRepository.ExecuteNonQuery(new CreateBotCriteria(username, displayName, system, accessToken, status, isTest, creator, ip));

        public Result UpdateBot(int botId, string username, string displayName, string system, string accessToken, bool status, bool isTest, int creator, string ip)
            => chatbotRepository.ExecuteNonQuery(new UpdateBotCriteria(botId, username, displayName, system, accessToken, status, isTest, creator, ip));

        public IEnumerable<BotSystemSetting> GetBotSystemSetting(string serviceName = "")
           => chatbotRepository.GetBotSystemSetting(new GetBotSystemSettingCriteria(serviceName));

        public Result UpdateBotSystemSetting(int id, string value, int creator, string ip)
            => chatbotRepository.ExecuteNonQuery(new UpdateBotSystemSettingCriteria(id, value, creator, ip));
    }
}
