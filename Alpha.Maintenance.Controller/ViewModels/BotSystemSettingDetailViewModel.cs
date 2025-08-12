namespace Alpha.Maintenance.Controller.ViewModels
{
    public class BotSystemSettingDetailViewModel
    {
        public int Id { get; set; }

        public string ServiceName { get; set; }

        public string SettingName { get; set; }

        public string Value { get; set; }

        public string ErrorMessage { get; set; } = "";
    }
}
