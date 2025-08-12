using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Repository.Criteria.Chatbot;
using Alpha.Maintenance.Service.Interface.Chatbot;
using Fanex.Data.Repository;
using System.Collections.Generic;

namespace Alpha.Maintenance.Service.Impl.Chatbot
{
    public class ChatbotService : IChatbotService
    {
        private readonly IDynamicRepository dynamicRepository;

        public ChatbotService()
            : this(null)
        {
        }

        public ChatbotService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
        }

        public IEnumerable<Bot> GetBots(bool isTest, bool enabled)
            => dynamicRepository.Fetch<Bot>(new GetAllBotCriteria(isTest, enabled));

        public Bot GetBot(int botId)
            => dynamicRepository.Get<Bot>(new GetBotCriteria(botId));

        public Result CreateBot(CreateBotCriteria criteria)
        {
            dynamicRepository.Execute(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };
        }

        public Result UpdateBot(UpdateBotCriteria criteria)
        {
            dynamicRepository.Execute(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };
        }

        public IEnumerable<BotSystemSetting> GetBotSystemSetting(string serviceName = "")
           => dynamicRepository.Fetch<BotSystemSetting>(new GetBotSystemSettingCriteria(serviceName));

        public Result UpdateBotSystemSetting(int id, string value, int creator, string ip)
        {
            var criteria = new UpdateBotSystemSettingCriteria
            {
                Id = id,
                Value = value,
                Creator = creator,
                Ip = ip
            };
            dynamicRepository.Execute(criteria);

            return new Result
            {
                Code = criteria.ErrCode,
                Message = criteria.ErrMsg
            };
        }
    }
}
