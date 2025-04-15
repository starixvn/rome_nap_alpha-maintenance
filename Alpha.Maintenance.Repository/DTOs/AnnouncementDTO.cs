using System;

namespace Alpha.Maintenance.Repository.DTOs
{
    public class AnnouncementDTO
    {
        public string AnnouncementType { get; set; }

        public string Code { get; set; }

        public string Contents { get; set; }

        public int Id { get; set; }

        public bool IsActive { get; set; }

        public string Specifications { get; set; }

        public string Title { get; set; }

        public string Environments { get; set; }

        public string Systems { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatorId { get; set; }

        public string CreatorName { get; set; }

        public DateTime? LastEditDate { get; set; }

        public int? LastEditorId { get; set; }

        public string LastEditorName { get; set; }
    }
}
