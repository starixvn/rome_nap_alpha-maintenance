namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using System;
    using Fanex.Data.Repository;

    public class SelectAnnouncementByIdCriteria : CriteriaBase
    {
        public string Id { get; set; }

        public override string GetSettingKey() => "Age_Announcements_SelectById";

        public override bool IsValid() => true;
    }
}
