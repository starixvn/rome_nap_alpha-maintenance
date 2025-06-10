namespace Alpha.Maintenance.Repository.Criteria.AutoTransferJobConfig
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetAutoTransferJobConfigCriteria : CriteriaBase
    {
        public GetAutoTransferJobConfigCriteria(string site, string environment)
        {
            Site = site;
            Environment = environment;
        }

        [SpParam(Ignored = true)]
        public string Site { get; }

        public string Environment { get; }

        public override string GetSettingKey() => $"{Site}_Age_ClearBalanceCust_Job_ConfigSel";

        public override bool IsValid() => true;
    }
}
