namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using System;
    using Fanex.Data.Repository;

    public class GetAnnouncementsCriteria : CriteriaBase
    {
        public string Environment { get; set; } = string.Empty;

        public DateTime FromDate { get; set; } = DateTime.Now;

        public DateTime ToDate { get; set; } = DateTime.Now;

        public override string GetSettingKey()
        {
            return "Annoucements_Select";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
