namespace Alpha.Maintenance.Core.Model
{
    public class Bot
    {
        public Bot(int botId, string displayName, string username, string system, string accessToken, bool status, bool isTest)
        {
            BotId = botId;
            DisplayName = displayName;
            Username = username;
            System = system;
            AccessToken = accessToken;
            Status = status;
            IsTest = isTest;
        }

        public int BotId { get; }

        public string DisplayName { get; }

        public string Username { get; }

        public string System { get; }

        public string AccessToken { get; }

        public bool Status { get; }

        public bool IsTest { get; }
    }
}
