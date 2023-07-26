namespace Alpha.Maintenance.Controller.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Announcements.Models;
    using Announcements.Specs;
    using Core.Model;
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

        public IEnumerable<UserLevel> UserLevels => new List<UserLevel>
        {
            new UserLevel(2, "Agent"),
            new UserLevel(3, "Master"),
            new UserLevel(4, "Super"),
            new UserLevel(0, "Headline")
        };

        public bool IsPopup => Announcement != null && Announcement.IsPopup;

        public ActionType ActionType { get; set; } = ActionType.Create;

        private IEnumerable<ISpecification<IParam>> FilterNotExistedSpecs(
            IEnumerable<ISpecification<IParam>> allSpecifications,
            IEnumerable<ISpecification<IParam>> existedSpecifications)
            => existedSpecifications == null ? allSpecifications :
                allSpecifications.Where(spec => !existedSpecifications.Any(existedSpec => existedSpec.Id.Equals(spec.Id)));

        public string GetDemoImage()
        {
            return "Hello";
        }
    }
}
