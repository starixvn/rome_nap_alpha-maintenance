namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using System;
    using Fanex.Data.Repository;
    using Alpha.Announcements.Models;
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Model;

    public enum ActionType
    {
        Create = 1,
        Update = 2
    }

    public class SaveAnnouncementsCriteria : CriteriaBase
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

        [SpParam(Ignored = true)]
        public List<UploadFileModel> Images { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override bool IsValid() => true;
    }
}
