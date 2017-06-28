namespace Alpha.Maintenance.Core.Configurations
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class AnnouncementConfig
    {
        [DataMember(Name = "environments")]
        public IEnumerable<string> Environments { get; set; }

        [DataMember(Name = "systems")]
        public IEnumerable<string> Systems { get; set; }

        [DataMember(Name = "languages")]
        public IEnumerable<string> Languages { get; set; }
    }
}