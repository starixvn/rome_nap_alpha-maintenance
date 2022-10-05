namespace Alpha.Maintenance.Controller.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UploadBannerViewModel
    {
        public UploadBannerViewModel()
        {
        }

        public UploadBannerViewModel(
            bool hasHostPermission,
            string pageTitle,
            IDictionary<string, string> desktopBanners = null,
            IDictionary<string, string> mobileBanners = null)
        {
            HasHostPermission = hasHostPermission;
            PageTitle = pageTitle;
            DesktopBanners = desktopBanners ?? new Dictionary<string, string>();
            MobileBanners = mobileBanners ?? new Dictionary<string, string>();
        }

        public bool HasHostPermission { get; }

        public string PageTitle { get; }

        public int Id { get; set; }

        public string Title { get; set; }

        public bool IsActive { get; set; }

        public DateTime ReleasedDate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string Environments { get; set; }

        public string Sites { get; set; }

        public string GetDesktopKey(string language)
            => $"desk{language.Replace("-", string.Empty)}Image";

        public string GetMobileKey(string language)
            => $"mobile{language.Replace("-", string.Empty)}Image";

        public IDictionary<string, string> DesktopBanners { get; }

        public IDictionary<string, string> MobileBanners { get; }

        public IDictionary<string, bool> SelectedEnvironments
            => Split(Environments);

        public IDictionary<string, bool> SelectedSites
            => Split(Sites);

        private static IDictionary<string, bool> Split(string text)
            => string.IsNullOrWhiteSpace(text)
            ? new Dictionary<string, bool>()
            : text
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .ToDictionary(kvp => kvp.Trim(), kvp => true);
    }
}
