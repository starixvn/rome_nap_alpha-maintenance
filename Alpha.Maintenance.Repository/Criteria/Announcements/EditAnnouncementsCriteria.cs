namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using System;
    using Fanex.Data.Repository;

    public class EditAnnouncementsCriteria : CriteriaBase
    {
        public string Announcements_SelectByCode { get; set; }

        public override string GetSettingKey()
        {
            return "Announcements_Select";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
