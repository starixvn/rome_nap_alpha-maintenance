namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using System;

    public class UpdateAnnouncementsCriteria : SaveAnnouncementsCriteria
    {
        public int LastEditorId { get; set; }

        public string LastEditorName { get; set; }

        public DateTime LastEditDate { get; set; }

        public override string GetSettingKey() => "Age_Announcements_Update";
    }
}
