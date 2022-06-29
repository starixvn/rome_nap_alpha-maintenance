using Alpha.Maintenance.Controller.ViewModels;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Service.Impl.Chatbot;
using Alpha.Maintenance.Service.Interface.Chatbot;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telegram.Bot;

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
        public ActionResult Index() => View(new ChatbotFilterViewModel(HasRight(SiteFunctionKeys.ChatbotManagement, Constant.Policy.Host)));

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetBots(bool enabled, bool isTest) => PartialView("BotList", chatbotService.GetBots(isTest, enabled));

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Host)]
        public ActionResult Create() => View("Bot", new ChatBotDetailViewModel(true, HasRight(SiteFunctionKeys.ChatbotManagement, Constant.Policy.Host)));

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Host)]
        public ActionResult Create(ChatBotDetailViewModel viewModel)
        {
            var result = chatbotService.CreateBot(
                viewModel.Username,
                viewModel.DisplayName,
                viewModel.System,
                viewModel.AccessToken,
                viewModel.Active,
                viewModel.IsTest,
                CurrentSession.UserId,
                CurrentContext.IP);

            if (result.IsOk)
            {
                return RedirectToAction("Index");
            }
            else
            {
                viewModel.ErrorMessage = result.Message;
                return View(viewModel);
            }
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Host)]
        public ActionResult Edit(int botId)
        {
            var bot = chatbotService.GetBot(botId);

            return View("Bot", new ChatBotDetailViewModel(false, HasRight(SiteFunctionKeys.ChatbotManagement, Constant.Policy.Host))
            {
                BotId = bot.BotId,
                DisplayName = bot.DisplayName,
                Username = bot.Username,
                AccessToken = bot.AccessToken,
                Active = bot.Status,
                IsTest = bot.IsTest,
                System = bot.System
            });
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Host)]
        public ActionResult Edit(ChatBotDetailViewModel viewModel)
        {
            var result = chatbotService.UpdateBot(
               viewModel.BotId,
               viewModel.Username,
               viewModel.DisplayName,
               viewModel.System,
               viewModel.AccessToken,
               viewModel.Active,
               viewModel.IsTest,
               CurrentSession.UserId,
               CurrentContext.IP);

            if (result.IsOk)
            {
                return RedirectToAction("Index");
            }
            else
            {
                viewModel.ErrorMessage = result.Message;
                return View(viewModel);
            }
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Host)]
        public ActionResult SetWebhook(string token)
        {
            return View(new SetChatbotWebhookViewModel { Token = token });
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Host)]
        public async Task<ActionResult> SetWebhook(SetChatbotWebhookViewModel viewModel)
        {
            try
            {
                var botClient = CreateBotClient(viewModel.Token);
                await botClient.SetWebhookAsync($"{viewModel.WebhookUri}/bot/{viewModel.Token}");

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                viewModel.Message = ex.Message;
                return View(viewModel);
            }
            
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Host)]
        public async Task<ActionResult> DeleteWebhook(string token)
        {
            try
            {
                var botClient = CreateBotClient(token);
                await botClient.DeleteWebhookAsync();
                return Json(new Result { Code = 0 });
            }
            catch(Exception ex)
            {
                return Json(Result.CreateErrorResult(ex.Message));
            }
        }

        private ITelegramBotClient CreateBotClient(string token)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var proxyUri = ConfigurationManager.AppSettings["Proxy"];
            if (string.IsNullOrEmpty(proxyUri))
            {
                return new TelegramBotClient(token);
            }

            var webProxy = new WebProxy(proxyUri);
            var httpClient = new HttpClient(new HttpClientHandler { Proxy = webProxy, UseProxy = true });
            return new TelegramBotClient(token, httpClient);
        }
    }
}
