namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using System;
    using Fanex.Data.Repository;

    public enum ActionType
    {
        Create = 1,
        Update = 2
    }

    public class InsertOrUpdateAnnouncementsCriteria : CriteriaBase
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }

        public int AnnouncementType { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Environments { get; set; }

        public string Systems { get; set; }

        public int Ordinal { get; set; }

        public string Contents { get; set; }

        public string Specifications { get; set; }

        [SpParam(Ignored = true)]
        public ActionType Action { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => Action.Equals(ActionType.Create) ? "Age_Announcements_Insert" : "Age_Announcements_Update";

        public override bool IsValid() => Id > 0 && FromDate <= ToDate;
    }
}
