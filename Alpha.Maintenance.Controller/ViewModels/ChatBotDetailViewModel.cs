namespace Alpha.Maintenance.Controller.ViewModels
{
    public class ChatBotDetailViewModel
    {
        public ChatBotDetailViewModel()
        {
        }

        public ChatBotDetailViewModel(bool isCreate, bool hasHostPermission)
        {
            IsCreate = isCreate;
            HasHostPermission = hasHostPermission;
        }

        public bool IsCreate { get; }

        public int BotId { get; set; }

        public string DisplayName { get; set; }

        public string Username { get; set; }

        public string System { get; set; }

        public string AccessToken { get; set; }

        public bool Active { get; set; }

        public bool IsTest { get; set; }

        public string ErrorMessage { get; set; }

        public bool HasHostPermission { get; }
    }
}
