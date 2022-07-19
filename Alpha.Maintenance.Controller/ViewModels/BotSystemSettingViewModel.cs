using Alpha.Maintenance.Core.Model;
using System.Collections.Generic;
namespace Alpha.Maintenance.Controller.ViewModels
{
    public class BotSystemSettingViewModel
    {
        public IEnumerable<BotSystemSetting> ServiceNames { get; set; }

        public string SelectedService { get; set; }
    }
}
