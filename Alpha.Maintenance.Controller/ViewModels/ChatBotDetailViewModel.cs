namespace Alpha.Maintenance.Controller.ViewModels
{
    public class ChatBotDetailViewModel
    {
        public bool IsCreate { get; set; }

        public int BotId { get; set; }

        public string DisplayName { get; set; }

        public string Username { get; set; }

        public string System { get; set; }

        public string AccessToken { get; set; }

        public bool Active { get; set; }

        public bool IsTest { get; set; }
    }
}
