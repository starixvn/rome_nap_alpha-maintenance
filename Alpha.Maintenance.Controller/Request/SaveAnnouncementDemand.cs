namespace Alpha.Maintenance.Controller.Request
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Announcements.Models;
    using Core.Helpers;
    using Newtonsoft.Json;

    public class SaveAnnouncementDemand
    {
        public SaveAnnouncementDemand()
            : this(null)
        {
        }

        public SaveAnnouncementDemand(string announcement)
        {
            Announcement = announcement;
        }

        [AllowHtml]
        public string Announcement { get; set; }

        public bool IsCopy { get; set; }

        public Announcement DeserializeAnnouncement
            => JsonConvert.DeserializeObject<Announcement>(Announcement, new SpecificationConverter());

        public string Systems => String.Join(", ", DeserializeAnnouncement.Systems.ToArray());

        public string Environments => String.Join(", ", DeserializeAnnouncement.Environments.ToArray());

        public string EncodedContents
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(DeserializeAnnouncement.Contents)));

        public bool IsPopup => DeserializeAnnouncement.AnnouncementType == AnnouncementType.EmergencyPopup;
    }
}
