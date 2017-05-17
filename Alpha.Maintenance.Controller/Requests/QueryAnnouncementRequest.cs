namespace Alpha.Maintenance.Controller.Requests
{
    using System;

    public class QueryAnnouncementRequest
    {
        public DateTime Date { get; set; }

        public string Environment { get; set; }

        public string System { get; set; }
    }
}