namespace Alpha.Maintenance.Controller.ViewModels
{
    public class ChatbotFilterViewModel
    {
        public ChatbotFilterViewModel()
        {
        }

        public ChatbotFilterViewModel(bool hasHostPermission)
        {
            HasHostPermission = hasHostPermission;
        }

        public bool Enabled { get; set; } = true;

        public bool IsTest { get; set; } = false;

        public bool HasHostPermission { get; }
    }
}
