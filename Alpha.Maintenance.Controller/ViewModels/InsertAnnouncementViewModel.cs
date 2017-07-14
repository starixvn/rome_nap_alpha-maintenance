namespace Alpha.Maintenance.Controller.ViewModels
{
    using System.Web.Mvc;
    using Core.Model;

    public class InsertAnnouncementViewModel
    {
        [AllowHtml]
        public string Announcement { get; set; }

       public ActionType Action { get; set; }
    }
}
