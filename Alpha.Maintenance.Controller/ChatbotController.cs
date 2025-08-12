using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Alpha.Maintenance.Controller.ViewModels;
using Alpha.Maintenance.Core.Model;
using Alpha.Maintenance.Core.Web;
using Alpha.Maintenance.Repository.Criteria.Chatbot;
using Alpha.Maintenance.Service.Impl.Chatbot;
using Alpha.Maintenance.Service.Interface.Chatbot;
using Fanex.NAP.Core.Business;
using Fanex.NAP.Core.Web.Attribute;
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
            var request = new CreateBotCriteria
            {
                Username = viewModel.Username,
                DisplayName = viewModel.DisplayName,
                System = viewModel.System,
                AccessToken = viewModel.AccessToken,
                Status = viewModel.Active,
                IsTest = viewModel.IsTest,
                Creator = CurrentSession.UserId,
                IP = CurrentContext.IP
            };
            var result = chatbotService.CreateBot(request);

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
            var request = new UpdateBotCriteria
            {
                BotId = viewModel.BotId,
                Username = viewModel.Username,
                DisplayName = viewModel.DisplayName,
                System = viewModel.System,
                AccessToken = viewModel.AccessToken,
                Status = viewModel.Active,
                IsTest = viewModel.IsTest,
                Creator = CurrentSession.UserId,
                IP = CurrentContext.IP
            };
            var result = chatbotService.UpdateBot(request);

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

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Host)]
        public ActionResult Settings(string serviceName)
        {
            var serviceNames = chatbotService.GetBotSystemSetting();
            var selectedServiceName = serviceName ?? serviceNames.First().ServiceName;
            var viewModel = new BotSystemSettingViewModel
            {
                ServiceNames = serviceNames.DistinctBy(bot => new { bot.ServiceName }),
                BotSystemSettings = serviceNames.Where(bot => bot.ServiceName == serviceName || bot.ServiceName == selectedServiceName),
                SelectedService = selectedServiceName
            };

            return View("BotSystem", viewModel);
        }

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Read)]
        public ActionResult GetBotSystemSettings(string serviceName) => PartialView("BotSystemSettingList", chatbotService.GetBotSystemSetting(serviceName));

        [HttpGet]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Host)]
        public ActionResult UpdateBotSystemSetting(int id)
        {
            var result = chatbotService.GetBotSystemSetting().FirstOrDefault(botSystemSetting => botSystemSetting.Id == id);
            var viewModel = new BotSystemSettingDetailViewModel
            {
                Id = result.Id,
                ServiceName = result.ServiceName,
                SettingName = result.SettingName,
                Value = result.Value
            };
            return View("BotSystemSetting", viewModel);
        }

        [HttpPost]
        [NapAuth(FunctionKey = SiteFunctionKeys.ChatbotManagement, Policy = Constant.Policy.Host)]
        public ActionResult UpdateBotSystemSetting(BotSystemSettingDetailViewModel viewModel)
        {
            var result = chatbotService.UpdateBotSystemSetting(viewModel.Id, viewModel.Value, CurrentSession.UserId, CurrentContext.IP);
            if (result.IsOk)
            {
                return RedirectToAction("Settings", new { serviceName = viewModel.ServiceName });
            }
            else
            {
                viewModel.ErrorMessage = result.Message;
                return View("BotSystemSetting", viewModel);
            }
        }
    }
}
