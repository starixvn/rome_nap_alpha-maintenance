namespace Alpha.Maintenance.Repository.Criteria.SuperLimitDate
{
    using Fanex.Data.Repository;

    public class GetAllSuperLimitDateCriteria : CriteriaBase
    {
        public string UserName { get; set; }

        public string Site { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRecords { get; set; }

        public override string GetSettingKey() => "Acc_CustomerDateLimitSel";
        
        public override bool IsValid()
            => true;
    }
}
