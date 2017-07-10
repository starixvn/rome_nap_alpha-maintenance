namespace Alpha.Maintenance.Controller.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Announcements.Models;
    using Announcements.Specs;
    using Core.Model;

    public class ComposeAnnouncementViewModel
    {
        public ComposeAnnouncementViewModel(
            Announcement announcement,
            ActionType actionType,
            IEnumerable<ISpecification<IParam>> allSpecifications,
            IEnumerable<SelectListItem> enviroments,
            IEnumerable<SelectListItem> systems)
        {
            Announcement = announcement;
            ActionType = actionType;
            Enviroments = enviroments;
            Systems = systems;
            NotExistedSpecifications = FilterNotExistedSpecs(allSpecifications, Announcement.Specifications);
        }

        public Announcement Announcement { get; set; }

        public IEnumerable<SelectListItem> Enviroments { get; set; }

        public IEnumerable<SelectListItem> Systems { get; set; }

        public IEnumerable<ISpecification<IParam>> NotExistedSpecifications { get; set; }

        public ActionType ActionType { get; set; } = ActionType.Create;

        private IEnumerable<ISpecification<IParam>> FilterNotExistedSpecs(
            IEnumerable<ISpecification<IParam>> allSpecifications,
            IEnumerable<ISpecification<IParam>> existedSpecifications)
            => existedSpecifications == null ? allSpecifications :
                 allSpecifications.Where(spec =>
                    {
                        return !existedSpecifications.Any(existedSpec => existedSpec.Id.Equals(spec.Id));
                    });
    }
}
