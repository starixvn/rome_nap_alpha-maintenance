namespace Alpha.Maintenance.Repository.Criteria.AutoTransferJobConfig
{
    using System;
    using Fanex.Data.Repository;

    public class UpdateAutoTransferJobConfigCriteria : CriteriaBase
    {
        public UpdateAutoTransferJobConfigCriteria(
            string site,
            string defaultTime,
            DateTime? manualTime,
            string runningMode,
            bool isActive)
        {
            Site = site;
            DefaultTime = defaultTime;
            ManualTime = manualTime;
            RunningMode = runningMode;
            IsActive = isActive;
        }

        [SpParam(Ignored = true)]
        public string Site { get; }

        public string DefaultTime { get; }

        public DateTime? ManualTime { get; }

        public string RunningMode { get; }

        public bool IsActive { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => $"{Site}_Age_ClearBalanceCust_Job_ConfigUpd";

        public override bool IsValid() => true;
    }
}
