using Alpha.Maintenance.Controller.ViewModels;
using Alpha.Maintenance.Service.Impl.Chatbot;
using Alpha.Maintenance.Service.Interface.Chatbot;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;
using System.Web.Mvc;

namespace Alpha.Maintenance.Controller
{
    public class ChatbotController : BaseController
    {
        private readonly IChatbotService chatbotService;

        public ChatbotController()
            : this(null)
        {
        }

        public ChatbotController(IChatbotService chatbotService)
        {
            this.chatbotService = chatbotService ?? new ChatbotService();
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Read)]
        public ActionResult Index() => View(new ChatbotFilterViewModel());

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetBots(bool enabled, bool isTest) => PartialView("BotList", chatbotService.GetBots(isTest, enabled));

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Host)]
        public ActionResult Create() => View("Bot", new ChatBotDetailViewModel { IsCreate = true });

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Host)]
        public ActionResult Create(ChatBotDetailViewModel viewModel)
        {

            return RedirectToAction("Index");
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int botId)
        {
            var bot = chatbotService.GetBot(botId);

            return View("Bot", new ChatBotDetailViewModel
            {
                IsCreate = false,
                BotId = bot.BotId,
                DisplayName = bot.DisplayName,
                Username = bot.Username,
                AccessToken = bot.AccessToken,
                Active = bot.Status,
                IsTest = bot.IsTest,
                System = bot.System
            });
        }
    }
}
