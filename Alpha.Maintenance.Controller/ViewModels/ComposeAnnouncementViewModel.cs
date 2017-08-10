namespace Alpha.Maintenance.Controller.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Announcements.Models;
    using Announcements.Specs;
    using Repository.Criteria.Announcements;

    public class ComposeAnnouncementViewModel
    {
        public ComposeAnnouncementViewModel(
            Announcement announcement,
            ActionType actionType,
            IEnumerable<ISpecification<IParam>> allSpecifications)
        {
            Announcement = announcement;
            ActionType = actionType;
            NotExistedSpecifications = FilterNotExistedSpecs(allSpecifications, Announcement.Specifications);
        }

        public Announcement Announcement { get; set; }

        public IEnumerable<ISpecification<IParam>> NotExistedSpecifications { get; set; }

        public ActionType ActionType { get; set; } = ActionType.Create;

        private IEnumerable<ISpecification<IParam>> FilterNotExistedSpecs(
            IEnumerable<ISpecification<IParam>> allSpecifications,
            IEnumerable<ISpecification<IParam>> existedSpecifications)
            => existedSpecifications == null ? allSpecifications :
                allSpecifications.Where(spec => !existedSpecifications.Any(existedSpec => existedSpec.Id.Equals(spec.Id)));
    }
}
