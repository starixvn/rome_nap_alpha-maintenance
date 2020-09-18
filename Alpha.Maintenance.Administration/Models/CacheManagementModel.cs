namespace Alpha.Maintenance.Administration.Models
{
    using Alpha.Maintenance.Controller.Models.AdminTools;
    using System.Collections.Generic;

    public class CacheManagementModel
    {
        public List<string> Keys { get; set; }

        public CacheKeyPattern Pattern { get; set; }
    }
}
