namespace Alpha.Maintenance.Repository.Criteria.AutoTransferJobConfig
{
    using System;
    using Alpha.Maintenance.Core.Constants;
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UpdateAutoTransferJobConfigCriteria : NonQueryCommand
    {
        public UpdateAutoTransferJobConfigCriteria(
            string site,
            string defaultTime,
            DateTime? manualTime,
            string runningMode,
            bool isActive,
            string environment,
            string testSuperIds)
        {
            Site = site;
            DefaultTime = defaultTime;
            ManualTime = manualTime;
            RunningMode = runningMode;
            IsActive = isActive;

            Environment = environment;
            TestSuperIds = testSuperIds;
        }

        [SpParam(Ignored = true)]
        public string Site { get; set; }

        public string DefaultTime { get; set; }

        public DateTime? ManualTime { get; set; }

        public string RunningMode { get; set; }

        public bool IsActive { get; set; }

        public string Environment { get; set; }

        public string TestSuperIds { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => $"{Site}_Age_ClearBalanceCust_Job_ConfigUpd{(Environment == AutoTransferEnvironmentConstants.UAT ? "_xtest" : string.Empty)}";

        public override bool IsValid() => true;
    }
}
