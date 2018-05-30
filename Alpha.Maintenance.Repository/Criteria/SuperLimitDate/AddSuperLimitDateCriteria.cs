namespace Alpha.Maintenance.Repository.Criteria.SuperLimitDate
{
    using System;
    using Fanex.Data.Repository;

    public class AddSuperLimitDateCriteria : CriteriaBase
    {
        public int UserId { get; set; }

        public string CustName { get; set; }

        public DateTime? MinDate { get; set; }

        public int? LimitDays { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey() => "Acc_CustomerDateLimitIns";

        public override bool IsValid()
            => UserId > 0 && !string.IsNullOrEmpty(CustName) && !MinDate.HasValue && !LimitDays.HasValue;
    }
}
