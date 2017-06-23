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
            ActionType = actionType;
            Specifications = specifications;
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
                return AppSettings.AnnouncementConfig.Sites.Select(
                        site => new SelectListItem
                        {
                            Text = site,
                            Value = site,
                            Selected = Announcement.Systems.Contains(site)
                        });
            }
        }

        public IEnumerable<SelectListItem> ChoosenSpecifications
        {
            get
            {
                if (Announcement.Specifications != null)
                {
                    return Specifications.Where(spec =>
                   {
                       return !Announcement.Specifications.Any(existedSpec => existedSpec.Id.Equals(spec.Id));
                   }).Select(spec => new SelectListItem
                   {
                       Text = spec.Name,
                       Value = spec.Id
                   });
                }
                else
                {
                    return Specifications.Select(spec => new SelectListItem
                    {
                        Text = spec.Name,
                        Value = spec.Id
                    });
                }
            }
        }

        public ActionType ActionType { get; set; } = ActionType.Create;

        public IEnumerable<ISpecification<IUser>> Specifications { get; set; }

    }
}
