namespace Fanex.MailDelivery.MailStation.Criteria
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Data.Repository;

    /// <summary>
    /// Result criteria.
    /// </summary>
    public class ResultCriteria : CriteriaBase
    {
        public ResultCriteria()
        {
            this.ErrCode = 0;
            this.ErrMsg = string.Empty;
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
