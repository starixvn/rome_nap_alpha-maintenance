namespace Alpha.Maintenance.Controller.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Announcements.Models;
    using Announcements.Specs;
    using Core.Configurations;
    using Core.Model;

    public class ComposeAnnouncementViewModel
    {
        public ComposeAnnouncementViewModel(
            Announcement announcement,
            ActionType actionType,
            IEnumerable<ISpecification<IParam>> allSpecifications,
            IEnumerable<SelectListItem> enviroments = null,
            IEnumerable<SelectListItem> systems = null)
        {
            Announcement = announcement;
            ActionType = actionType;
            Enviroments = enviroments ?? BuildSelectedList(announcement.Environments, AppSettings.AnnouncementConfig.Environments);
            Systems = systems ?? BuildSelectedList(announcement.Systems, AppSettings.AnnouncementConfig.Systems);
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

        private IEnumerable<SelectListItem> BuildSelectedList(
          IEnumerable<string> valueRange,
          IEnumerable<string> values)
              => values.Select(
                      value => new SelectListItem
                      {
                          Text = value,
                          Value = value,
                          Selected = valueRange.Contains(value)
                      });
    }
}
