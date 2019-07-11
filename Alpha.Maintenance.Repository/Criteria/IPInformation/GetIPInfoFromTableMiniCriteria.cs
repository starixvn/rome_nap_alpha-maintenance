namespace Alpha.Maintenance.Repository.Criteria.IPInformation
{
    using Fanex.Data.Repository;

    public class GetIPInfoFromTableMiniCriteria : CriteriaBase
    {
        public GetIPInfoFromTableMiniCriteria(
            bool isIPV6,
            ulong ipNumber)
        {
            IsIPV6 = isIPV6;
            IPNumber = ipNumber;
        }

        public bool IsIPV6 { get; }

        public ulong IPNumber { get; }

        public override string GetSettingKey() => "Acc_GetIPInfo_TableMini";

        public override bool IsValid() => IPNumber > 0;
    }
}