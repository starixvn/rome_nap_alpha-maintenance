using System;
using Alpha.Maintenance.Core.Extension;

namespace Alpha.Maintenance.Core.Model.ResetPotentialWinLimitJobConfig
{
    public class ResetPotentialWinLimitJobStatusModel
    {
        public DateTime? RunningDate { get; set; }
        public int LastMaxCustID { get; set; }
        public DateTime? CompletedTime { get; set; }

        public string DisplayedRunningDate
        {
            get
            {
                if (RunningDate.HasValue)
                {
                    var mt = RunningDate.Value;

                    return new DateTime(mt.Year, mt.Month, mt.Day, mt.Hour, mt.Minute, 0).ToLongDate();
                }

                return string.Empty;
            }
        }

        public string DisplayedCompletedTime
        {
            get
            {
                if (CompletedTime.HasValue)
                {
                    var mt = CompletedTime.Value;

                    return new DateTime(mt.Year, mt.Month, mt.Day, mt.Hour, mt.Minute, 0).ToLongDate();
                }

                return string.Empty;
            }
        }

        public override string ToString() {
            var rd = RunningDate.HasValue ? $"\"{RunningDate.Value.ToString("yyyy-MM-dd HH:mm:ss")}\"" : "null";   
            var ct = CompletedTime.HasValue ? $"\"{CompletedTime.Value.ToString("yyyy-MM-dd HH:mm:ss")}\"" : "null";
            return $"{{ \"RunningDate\": {rd}, \"LastMaxCustID\": {LastMaxCustID}, \"CompletedTime\": {ct} }}";
        }

        public string DefaultSelectedDate => DateTime.Now.Date.ToLongDate();
    }
}
