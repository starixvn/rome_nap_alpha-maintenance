namespace Alpha.Maintenance.Controller.Request
{
    using System.Web.Mvc;
    using Repository.Criteria.Announcements;

    public class SaveAnnouncementDemand
    {
        [AllowHtml]
       public string Announcement { get; set; }

       public ActionType Action { get; set; }
    }
}
