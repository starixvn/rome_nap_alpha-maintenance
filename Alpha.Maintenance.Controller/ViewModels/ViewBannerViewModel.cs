namespace Alpha.Maintenance.Controller.ViewModels
{
    using System;
    using System.Collections.Generic;
    using Alpha.Maintenance.Core.Enums;
    using Alpha.Maintenance.Core.Model;

    public class ViewBannerViewModel
    {
        private const int NumberDaysOfWeek = 7;
        public ViewBannerViewModel()
        {
            var today = DateTime.Today;
            FromDate = today;
            ToDate = today.AddDays(NumberDaysOfWeek);
        }

        public BannerFilterBy FilterBy { get; set; } = BannerFilterBy.ReleasedDate;

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string Environment { get; set; }

        public string Site { get; set; }

        public IEnumerable<Banner> Banners { get; set; }

        public bool HasHostPermission { get; set; }
    }
}
