namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using System;
    using System.ComponentModel;
    using Core;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public enum FilterBy
    {
        [Description("Created Date")]
        CreatedDate = 1,

        [Description("Release Date")]
        ReleasedDate = 2,

        [Description("Expiration Date")]
        ExpiredDate = 3
    }

    public class SelectAnnouncementsCriteria : CriteriaBase
    {
        public string Environment { get; set; } = string.Empty;

        public string System { get; set; } = string.Empty;

        public DateTime FromDate { get; set; } = SystemTime.Today.AddMonths(-1);

        public DateTime ToDate { get; set; } = SystemTime.Today;

        public bool? IsActive { get; set; } = null;

        public FilterBy FilterBy { get; set; } = FilterBy.CreatedDate;

        public override string GetSettingKey() => "Age_Announcements_Select";

        public override bool IsValid() => FromDate <= ToDate;
    }
}
