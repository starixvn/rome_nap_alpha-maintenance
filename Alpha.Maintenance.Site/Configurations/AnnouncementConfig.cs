namespace Alpha.Maintenance.Site.Configurations
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class AnnouncementConfig
    {
        [DataMember(Name = "environments")]
        public List<string> Environments { get; set; }

        [DataMember(Name = "sites")]
        public IEnumerable<string> Sites { get; set; }
    }
}