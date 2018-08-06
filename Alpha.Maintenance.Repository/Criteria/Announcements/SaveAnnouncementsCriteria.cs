namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using System;
    using Fanex.Data.Repository;

    public enum ActionType
    {
        Create = 1,
        Update = 2
    }

    public class SaveAnnouncementsCriteria : CriteriaBase
    {
        public string Code { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }        

        public DateTime ReleasedDate { get; set; }        

        public string Environments { get; set; }

        public string Systems { get; set; }
        
        public string Contents { get; set; }

        public string Specifications { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid() => true;
    }
}
