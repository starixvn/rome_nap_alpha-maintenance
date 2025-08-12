using System;
using System.Collections.Generic;
using Alpha.Maintenance.Core.Configurations;
using Alpha.Maintenance.Core.Extension;

namespace Alpha.Maintenance.Core.Model
{
    public class AutoTransferJobConfigModel
    {
        public string Site { get; set; }

        public string DefaultTime { get; set; }

        public DateTime? ManualTime { get; set; }

        public string RunningMode { get; set; }

        public bool IsActive { get; set; }

        public string Environment { get; set; }

        public string TestSuperIds { get; set; }

        public string DisplayedManualTime
        {
            get
            {
                if (HasManualTime)
                {
                    var mt = ManualTime.Value;

                    return new DateTime(mt.Year, mt.Month, mt.Day, mt.Hour, mt.Minute, 0).ToLongDate();
                }

                return string.Empty;
            }
        }

        public static readonly IEnumerable<string> RunningModes = new List<string> { "Normal", "Resume" };

        public static readonly IEnumerable<string> SiteDBs = AppSettings.SiteDBs;

        public string DefaultManualTime => SystemTime.Today.ToLongDate();

        public bool HasManualTime => ManualTime.HasValue && ManualTime.Value >= SystemTime.Now;
    }
}
