namespace Alpha.Maintenance.Repository.Criteria.SuperLimitDate
{
    using Fanex.Data.Repository;

    public class DeleteSuperLimitDateCriteria : NonQueryCommand
    {
        public int UserId { get; set; }

        public int CustId { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CustomerDateLimitDel";

        public override bool IsValid() => UserId > 0 && CustId > 0;
    }
}
