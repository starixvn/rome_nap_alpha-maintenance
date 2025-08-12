namespace Alpha.Maintenance.Repository.Criteria.DBProfiler
{
    using Fanex.Data.Repository;

    public class GetLogCriteria: CriteriaBase
    {
        private const int MaxTraceTime = 120;
        public string Site { get; set; }

        public int TraceTimer { get; set; }

        public string TextDataLike { get; set; }

        public string TextDataLike2 { get; set; }

        public string TextDataLike3 { get; set; }

        public bool TraceStartEvents { get; set; }

        public bool Mode { get; set; }                

        public string ErrorMsg { get; set; }

        public bool HasError { get; set; }

        public override bool IsValid()
        {
            if (TraceTimer > MaxTraceTime || (string.IsNullOrWhiteSpace(TextDataLike) && string.IsNullOrWhiteSpace(TextDataLike2) && string.IsNullOrWhiteSpace(TextDataLike3)))
            {
                return false;
            }

            return true;
        }

        public override string GetSettingKey()
        {
            return "vndba_sp_trace_WebApp";
        }
    }
}
