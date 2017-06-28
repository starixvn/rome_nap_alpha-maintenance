namespace Alpha.Maintenance.Core.Configurations
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Model;

    [DataContract]
    public class AnnouncementConfig
    {
        [DataMember(Name = "environments")]
        public IEnumerable<string> Environments { get; set; }

        [DataMember(Name = "systems")]
        public IEnumerable<string> Systems { get; set; }

        [DataMember(Name = "languages")]
        public IEnumerable<string> Languages { get; set; }

        [DataMember(Name = "levels")]
        public IEnumerable<Specfication> Levels { get; set; }
    }
}