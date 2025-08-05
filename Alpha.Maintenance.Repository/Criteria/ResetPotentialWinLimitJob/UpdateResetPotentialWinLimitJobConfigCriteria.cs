namespace Alpha.Maintenance.Repository.Criteria.ResetPotentialWinLimitJob
{
    using Fanex.Data;
    using Fanex.Data.Repository;

    public class UpdateResetPotentialWinLimitJobConfigCriteria : NonQueryCommand
    {
        public UpdateResetPotentialWinLimitJobConfigCriteria(string vName, string vValue)
        {
            VName = vName;
            VValue = vValue;
        }

        public string VName { get; }

        public string VValue { get; }

        public override string GetSettingKey() => "SystemSettings_Update";

        public override bool IsValid() => !string.IsNullOrEmpty(VName);
    }
}
