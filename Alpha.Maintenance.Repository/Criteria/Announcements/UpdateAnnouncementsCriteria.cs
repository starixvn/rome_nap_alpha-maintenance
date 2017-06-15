namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using System;
    using Fanex.Data.Repository;

    public class UpdateAnnouncementsCriteria : CriteriaBase
    {
        public string Code { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }

        public int AnnouncementType { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Environments { get; set; }

        public string Systems { get; set; }

        public int Order { get; set; }

        public string Contents { get; set; }

        public string Specifications { get; set; }

        public override string GetSettingKey()
        {
            return "Announcements_Update";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
