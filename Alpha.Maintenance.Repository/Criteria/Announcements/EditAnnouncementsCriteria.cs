namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using System;
    using Fanex.Data.Repository;

    public class EditAnnouncementsCriteria : CriteriaBase
    {
        public string Id { get; set; }

        public override string GetSettingKey()
        {
            return "Announcements_SelectByCode";
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
