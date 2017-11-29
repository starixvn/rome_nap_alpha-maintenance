namespace Alpha.Maintenance.Repository.Criteria.IPInformation
{
    using Fanex.Data.Repository;

    public class GetIPInformationCriteria : CriteriaBase
    {
        public GetIPInformationCriteria(string ip)
        {
            IP = ip;
        }

        public string IP { get; }

        public override string GetSettingKey() => "Acc_GetIPInfo_Full";

        public override bool IsValid() => !string.IsNullOrWhiteSpace(IP);
    }
}
