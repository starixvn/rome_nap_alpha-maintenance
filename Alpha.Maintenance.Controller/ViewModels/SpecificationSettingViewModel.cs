using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Alpha.Announcements.Models;
using Alpha.Announcements.Specs;
using Alpha.Maintenance.Core.Configurations;
using Alpha.Maintenance.Core.Model;

namespace Alpha.Maintenance.Controller.ViewModels
{
    public class SpecificationSettingViewModel
    {
        public SpecificationSettingViewModel(Announcement announcement, IEnumerable<ISpecification<IUser>> specifications)
        {
            Announcement = announcement;
            Specifications = specifications;
        }

        public Announcement Announcement { get; set; }

        public ActionType ActionType { get; set; } = ActionType.Create;

        public IEnumerable<ISpecification<IUser>> Specifications { get; set; }
    }
}
