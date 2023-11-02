namespace Alpha.Maintenance.Repository.Criteria.AutoTransferJobConfig
{
    using System;
    using Alpha.Maintenance.Core.Constants;
    using Fanex.Data.Repository;

    public class UpdateAutoTransferJobConfigCriteria : CriteriaBase
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
        public string Site { get; }

        public string DefaultTime { get; }

        public DateTime? ManualTime { get; }

        public string RunningMode { get; }

        public bool IsActive { get; }

        public string Environment { get; }

        public string TestSuperIds { get; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => $"{Site}_Age_ClearBalanceCust_Job_ConfigUpd{(Environment == AutoTransferEnvironmentConstants.UAT ? "_xtest" : string.Empty)}";

        public override bool IsValid() => true;
    }
}
