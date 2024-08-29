namespace Alpha.Maintenance.Controller.ViewModels
{
    public class LicenseeAdministrationViewModel
    {
        public string Url { get; set; }
        public string Payload { get; set; }
    }

    public class CreateCurrencyViewModel : LicenseeAdministrationViewModel
    {
        public string SiteName { get; set; }
    }

    public class CreateDummyPlayerViewModel : LicenseeAdministrationViewModel
    {
        public int AgentId { get; set; }
    }
}