namespace Alpha.Maintenance.Controller.Request
{
    using System.Web.Mvc;
    using Core.Model;

    public class InsertOrUpdateAnnouncementDemand
    {
        [AllowHtml]
        public string Announcement { get; set; }

       public ActionType Action { get; set; }
    }
}
