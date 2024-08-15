namespace Alpha.Maintenance.Core.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Json;
    using System.Web;

    public static class AppSettings
    {
        static AppSettings()
        {
            LoadAnnouncementConfig();
        }

        public static string ONEBookUMUri
        {
            get
            {
                return ConfigurationManager.AppSettings["ONEBookUMUri"];
            }
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

        public static string GetConfigValue(string configName) => ConfigurationManager.AppSettings[configName];

        public static IEnumerable<string> APINames
            => ConfigurationManager.AppSettings.AllKeys
            .Where(key => key.EndsWith(":ApiUrl"))
            .Select(key => key.Substring(0, key.IndexOf(':')));

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

        public static IEnumerable<string> BalanceRecordJobNames
            => GetConfigValue("BalanceRecord:JobNames")
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(j => j.Trim());

        public static IEnumerable<string> SiteDBs
            => GetConfigValue("SiteDBs")
            .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(j => j.Trim());
    }
}