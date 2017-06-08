namespace Alpha.Maintenance.Repository.Criteria.Announcements
{
    using Fanex.Data.Repository;

    public class GetAllAnnouncementsCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Annoucements_SelectAll";
        }
    }
}
