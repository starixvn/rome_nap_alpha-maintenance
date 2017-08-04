namespace Alpha.Maintenance.Controller.Request
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Web.Mvc;
    using Announcements.Client.Helpers;
    using Announcements.Models;
    using Newtonsoft.Json;

    public class SaveAnnouncementDemand
    {
        [AllowHtml]
        public string Announcement { get; set; }

        public bool IsCopy { get; set; }

        public Announcement DeserializeAnnouncement
            => JsonConvert.DeserializeObject<Announcement>(Announcement, new SpecificationConverter());

        public string Systems => String.Join(", ", DeserializeAnnouncement.Systems.ToArray());

        public string Environments => String.Join(", ", DeserializeAnnouncement.Environments.ToArray());

        public string EncodedContents
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(DeserializeAnnouncement.Contents)));
    }
}
