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
        public ComposeAnnouncementViewModel(Announcement announcement, ActionType actionType, IEnumerable<ISpecification<IUser>> allSpecifications)
        {
            Announcement = announcement;
            ActionType = actionType;

            if (Announcement.Specifications == null)
            {
                NotExistedSpecifications = allSpecifications;
            }
            else
            {
                NotExistedSpecifications = allSpecifications.Where(spec =>
                {
                    return !Announcement.Specifications.Any(existedSpec => existedSpec.Id.Equals(spec.Id));
                }).Select(spec => spec);
            }
        }

        public Announcement Announcement { get; set; }

        public IEnumerable<SelectListItem> Enviroments
        {
            get
            {
                return AppSettings.AnnouncementConfig.Environments.Select(
                      environment => new SelectListItem
                      {
                          Text = environment,
                          Value = environment,
                          Selected = Announcement.Environments.Contains(environment)
                      });
            }
        }

        public IEnumerable<SelectListItem> Systems
        {
            get
            {
                return AppSettings.AnnouncementConfig.Systems.Select(
                        site => new SelectListItem
                        {
                            Text = site,
                            Value = site,
                            Selected = Announcement.Systems.Contains(site)
                        });
            }
        }

        public IEnumerable<ISpecification<IUser>> NotExistedSpecifications { get; set; }

        public ActionType ActionType { get; set; } = ActionType.Create;
    }
}
