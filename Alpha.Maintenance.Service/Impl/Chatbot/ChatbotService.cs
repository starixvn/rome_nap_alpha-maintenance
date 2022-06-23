using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Repository.Criteria.Chatbot;
using Alpha.Maintenance.Repository.Impl;
using Alpha.Maintenance.Service.Interface.Chatbot;
using System.Collections.Generic;

namespace Alpha.Maintenance.Service.Impl.Chatbot
{
    public class ChatbotService : IChatbotService
    {
        private readonly ChatbotRepository chatbotRepository;

        public ChatbotService()
            : this(null)
        {
        }

        public ChatbotService(ChatbotRepository chatbotRepository)
        {
            this.chatbotRepository = chatbotRepository ?? new ChatbotRepositoryImpl();
        }

        public IEnumerable<Bot> GetBots(bool isTest, bool enabled)
            => chatbotRepository.GetBots(new GetAllBotCriteria(isTest, enabled));

        public Bot GetBot(int botId)
            => chatbotRepository.GetBot(new GetBotCriteria(botId));
    }
}
