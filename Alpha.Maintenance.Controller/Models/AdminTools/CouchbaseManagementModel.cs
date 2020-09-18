namespace Alpha.Maintenance.Controller.Models.AdminTools
{
    using System.Collections.Generic;

    public class CouchbaseManagementModel
    {
        public CacheKeyPattern Pattern { get; set; }

        public List<string> Keys { get; set; }
    }
}
