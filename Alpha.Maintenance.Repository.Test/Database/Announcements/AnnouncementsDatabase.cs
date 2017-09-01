namespace Alpha.Maintenance.Repository.Test.Database.Announcements
{
    using System.Data;
    using Helpers;

    public static class AnnouncementsDatabase
    {
        private static DataSet announcements = FileHelper.LoadDataSet("\\Database\\Announcements\\AnnouncementsHasData.xml");

        public static DataSet Announcements => announcements;
    }
}