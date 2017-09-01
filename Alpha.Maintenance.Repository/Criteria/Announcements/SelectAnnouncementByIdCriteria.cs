namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using Fanex.Data.Repository;

    public class SelectAnnouncementByIdCriteria : CriteriaBase
    {
        public int Id { get; set; } = 0;

        public override string GetSettingKey() => "Age_Announcements_SelectById";

        public override bool IsValid() => Id > 0;
    }
}
