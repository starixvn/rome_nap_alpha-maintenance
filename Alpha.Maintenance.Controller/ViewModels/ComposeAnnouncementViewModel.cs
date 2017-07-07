using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Alpha.Announcements.Models;
using Alpha.Announcements.Specs;
using Alpha.Maintenance.Core.Model;

namespace Alpha.Maintenance.Controller.ViewModels
{
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

            NotExistedSpecifications = Announcement.Specifications == null ? 
                allSpecifications :
                allSpecifications.Where(spec =>
                {
                    return !Announcement.Specifications.Any(existedSpec => existedSpec.Id.Equals(spec.Id));
                });

        }

        public Announcement Announcement { get; set; }

        public IEnumerable<SelectListItem> Enviroments{ get; set; }

        public IEnumerable<SelectListItem> Systems { get; set; }

        public IEnumerable<ISpecification<IParam>> NotExistedSpecifications { get; set; }

        public ActionType ActionType { get; set; } = ActionType.Create;
    }
}
