using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alpha.Maintenance.Core.Model
{
    public class DBProfilerModel
    {
        public string Site { get; set; }

        public int TraceTimer { get; set; }

        public string TextDataLike { get; set; }

        public string TextDataLike2 { get; set; }

        public string TextDataLike3 { get; set; }

        public bool TraceStartEvents { get; set; }

        public bool Mode { get; set; }

        public string Server { get; set; }

        public string ErrorMsg { get; set; }

        public bool HasError { get; set; }

        public int TotalPage { get; set; }

        public int Page { get; set; }
    }
}
