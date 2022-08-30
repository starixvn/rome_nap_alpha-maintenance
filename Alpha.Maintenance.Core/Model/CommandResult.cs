using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alpha.Maintenance.Core.Model
{
    public class CommandResult
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public bool IsSuccessful => Code == 0;
    }
}
