namespace Alpha.Maintenance.Core.Configurations
{
    using System.Configuration;
    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Web;

    public static class AppSettings
    {
        static AppSettings()
        {
            LoadAnnouncementConfig();
        }

        public static AnnouncementConfig AnnouncementConfig { get; internal set; }

        public static string AnnouncementConfigFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["Announcement:ConfigFilePath"] ?? "~/App_Data/announcementConfig.json";
            }
        }

        public static string XMLServerListFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["DBProfiler:ServerConfigFilePath"] ?? "~/App_Data/LOCAL/DBServerList.xml";
            }
        }

        public static string XMLSiteListFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["DBProfiler:SiteConfigFilePath"] ?? "~/App_Data/Sites.xml";
            }
        }

        private static void LoadAnnouncementConfig()
        {
            if (HttpContext.Current != null
                && HttpContext.Current.Server != null)
            {
                var configFilePath = HttpContext.Current.Server.MapPath(AnnouncementConfigFilePath);
                if (File.Exists(configFilePath))
                {
                    using (Stream stream = File.OpenRead(configFilePath))
                    {
                        var serializer = new DataContractJsonSerializer(typeof(AnnouncementConfig));
                        AnnouncementConfig = (AnnouncementConfig)serializer.ReadObject(stream);
                    }
                }
                else
                {
                    throw new FileNotFoundException("Cannot find announcement json file");
                }
            }
        }
    }
}