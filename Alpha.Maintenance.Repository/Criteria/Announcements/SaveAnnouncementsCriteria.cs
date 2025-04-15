namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using System;
    using Fanex.Data.Repository;
    using Alpha.Announcements.Models;
    using Fanex.Data;

    public enum ActionType
    {
        Create = 1,
        Update = 2
    }

    public class SaveAnnouncementsCriteria : NonQueryCommand
    {
        [SpParam(SqlParamName = "AnnouncementType")]
        public int Type => IsPopup ? (int)AnnouncementType.EmergencyPopup : (int)AnnouncementType.Normal;

        [SpParam(Ignored = true)]
        public bool IsPopup { get; set; }

        public DateTime ExpiredDate { get; set; }

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

        public override string GetSettingKey()
        {
            throw new NotImplementedException();
        }

        public override bool IsValid() => true;
    }
}
