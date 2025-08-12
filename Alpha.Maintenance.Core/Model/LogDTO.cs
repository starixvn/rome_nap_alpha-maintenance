using System.ComponentModel.DataAnnotations;

namespace Alpha.Maintenance.Core.Model
{
    public class LogDTO : Log
    {
        private const int MaxTimeZone = 12;
        private const int MinTimeZone = -12;

        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public int MachineId { get; set; }

        [Required]
        public string MachineName { get; set; }

        public bool IsProduction { get; set; }

        [Range(MinTimeZone, MaxTimeZone)]
        public short TimeZone { get; set; }

        public string MachineIP { get; set; }
    }
}
