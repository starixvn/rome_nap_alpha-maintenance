namespace Alpha.Maintenance.Core.Configurations
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    public static class BannerConfig
    {
        public const string MaxFileSize = "100KB";

        public const int DesktopBannerWidth = 280;
        public const int DesktopBannerHeight = 70;

        public const int MobileBannerWidth = 600;
        public const int MobileBannerHeight = 140;

        public const string AllowedFileExtensions = "jpeg jpg png";

        private static readonly NameValueCollection appSettings = ConfigurationManager.AppSettings;

        public static string[] Environments => Split(appSettings["banner:Environments"]);

        public static string[] Sites => Split(appSettings["banner:Sites"]);

        public static string[] LanguageCodes => Split(appSettings["banner:LanguageCodes"]);

        private static string[] Split(string value)
            => value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    }
}
