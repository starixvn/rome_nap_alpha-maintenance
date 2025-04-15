namespace Alpha.Maintenance.Core.Model
{
    public class Bot
    {
        public int BotId { get; set; }

        public string DisplayName { get; set; }

        public string Username { get; set; }

        public string System { get; set; }

        public string AccessToken { get; set; }

        public bool Status { get; set; }

        public bool IsTest { get; set; }
    }
}
