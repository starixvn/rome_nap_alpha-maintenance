namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using System;
    using Fanex.Data.Repository;

    public class SelectAnnouncementsCriteria : CriteriaBase
    {
        public string Environment { get; set; } = string.Empty;

        public string System { get; set; } = string.Empty;

        public DateTime FromDate { get; set; } = DateTime.Today;

        public DateTime ToDate { get; set; } = DateTime.Today;

        public bool? IsActive { get; set; } = null;

        public override string GetSettingKey() => "Age_Announcements_Select";

        public override bool IsValid() => FromDate <= ToDate;
    }
}
