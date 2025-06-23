using System;
using Alpha.Maintenance.Core.Extension;

namespace Alpha.Maintenance.Core.Model.ResetPotentialWinLimitJobConfig
{
    public class ResetPotentialWinLimitJobConfigModel
    {
        public string DefaultTime { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ManualTime { get; set; }

        public string DefaultManualTime => DateTime.Now.Date.ToLongDate();
        public bool HasManualTime => ManualTime.HasValue && ManualTime.Value >= DateTime.Now;

        public string DisplayedManualTime
        {
            get
            {
                var d = DateTime.Now;
                if (HasManualTime)
                {
                    var mt = ManualTime.Value;

                    return new DateTime(mt.Year, mt.Month, mt.Day, mt.Hour, mt.Minute, 0).ToLongDate();
                }

                return string.Empty;
            }
        }

        public override string ToString() {
            var mt = ManualTime.HasValue ? $"\"{ManualTime.Value.ToString("yyyy-MM-dd HH:mm:ss")}\"" : "null"; 
            var ia = IsActive ? "true" : "false";
            return $"{{ \"DefaultTime\": \"{DefaultTime}\", \"IsActive\": {ia}, \"ManualTime\": {mt} }}";
        }
    }
}
