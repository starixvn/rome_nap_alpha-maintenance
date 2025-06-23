namespace Alpha.Maintenance.Repository.Criteria.ResetPotentialWinLimitJob
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class GetResetPotentialWinLimitJobSettingCriteria : NonQueryCommand
    {
        public GetResetPotentialWinLimitJobSettingCriteria(string vName)
        {
            VName = vName;
        }

        public string VName { get; }

        public string VValue { get; set; }

        public override string GetSettingKey() => "SystemSettings_Select";

        public override bool IsValid() => true;
    }
}
