using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Alpha.Announcements.Models;
using Alpha.Announcements.Specs;
using Alpha.Maintenance.Core.Configurations;
using Alpha.Maintenance.Core.Model;

namespace Alpha.Maintenance.Controller.ViewModels
{
    public class ComposeAnnouncementViewModel
    {
        public ComposeAnnouncementViewModel(Announcement announcement, ActionType actionType, IEnumerable<ISpecification<IUser>> specifications)
        {
            Announcement = announcement;

            Enviroments = AppSettings.AnnouncementConfig.Environments.Select(
                environment => new SelectListItem
                {
                    Text = environment,
                    Value = environment,
                    Selected = announcement.Environments.Contains(environment)
                });

            Systems = AppSettings.AnnouncementConfig.Sites.Select(
                site => new SelectListItem
                {
                    Text = site,
                    Value = site,
                    Selected = announcement.Systems.Contains(site)
                });

            ActionType = actionType;

            Specifications = specifications;

            ChoosenSpecifications = Specifications.Where(spec =>
            {
                return !announcement.Specifications.Any(existedSpec => existedSpec.Id.Equals(spec.Id));
            }).Select(spec => new SelectListItem
            {
                Text = spec.Name,
                Value = spec.Id
            });

        }

        public Announcement Announcement { get; set; }

        public IEnumerable<SelectListItem> Enviroments { get; set; }

        public IEnumerable<SelectListItem> Systems { get; set; }

        public IEnumerable<SelectListItem> ChoosenSpecifications { get; set; }

        public ActionType ActionType { get; set; } = ActionType.Create;

        public IEnumerable<ISpecification<IUser>> Specifications { get; set; }

    }
}
