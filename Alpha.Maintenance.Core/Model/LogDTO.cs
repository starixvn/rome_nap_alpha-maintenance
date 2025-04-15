using System.ComponentModel.DataAnnotations;

namespace Alpha.Maintenance.Core.Model
{
    public class LogDTO : Log
    {
        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public int MachineId { get; set; }

        [Required]
        public string MachineName { get; set; }

        public bool IsProduction { get; set; }

        [Range(-12, 12)]
        public short TimeZone { get; set; }

        public string MachineIP { get; set; }
    }
}
