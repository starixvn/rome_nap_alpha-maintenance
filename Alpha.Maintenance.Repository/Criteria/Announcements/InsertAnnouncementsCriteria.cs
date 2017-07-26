namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using System;

    public class InsertAnnouncementsCriteria : SaveAnnouncementsCriteria
    {
        public int CreatorId { get; set; }

        public string CreatorName { get; set; }

        public DateTime CreatedDate { get; set; }

        public override string GetSettingKey() => "Age_Announcements_Insert";
    }
}
