namespace Alpha.Maintenance.Repository.Tests.Database.IPInformation
{
    using System.Data;
    using Helpers;

    public static class IPInformationDatabase
    {
        private static DataSet ipInformation = FileHelper.LoadDataSet("\\Database\\IPInformation\\IPInformationHasData.xml");

        public static DataSet IPInformation => ipInformation;
    }
}
