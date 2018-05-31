namespace Alpha.Maintenance.Core.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SuperLimitDate
    {
        [Required(ErrorMessage = "User name is required")]
        public string UserName { get; set; }

        public int CustId { get; set; }

        public DateTime? MinDate { get; set; }

        public int? LimitDays { get; set; }

        public string TypeDate { get; set; }
    }
}
