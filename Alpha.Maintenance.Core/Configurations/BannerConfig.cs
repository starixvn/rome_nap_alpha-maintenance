namespace Alpha.Maintenance.Core.Configurations
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;

    public static class BannerConfig
    {
        public const string MaxFileSize = "100KB";

        public const int DesktopBannerWidth = 1920;
        public const int DesktopBannerHeight = 48;

        public const int MobileBannerWidth = 750;
        public const int MobileBannerHeight = 96;

        public const string AllowedFileExtensions = "jpeg jpg png";

        private static readonly NameValueCollection appSettings = ConfigurationManager.AppSettings;

        public static string[] Environments => Split(appSettings["banner:Environments"]);

        public static string[] Sites => Split(appSettings["banner:Sites"]);

        public static string[] LanguageCodes => Split(appSettings["banner:LanguageCodes"]);

        private static string[] Split(string value)
            => value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    }
}
