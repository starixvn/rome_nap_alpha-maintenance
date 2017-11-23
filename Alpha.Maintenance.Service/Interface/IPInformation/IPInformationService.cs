namespace Alpha.Maintenance.Service.Interface.IPInformation
{
    using System.Collections.Generic;
    using Core.Model;

    public interface IPInformationService
    {
        IPInfo GetIPInfoFromDB(string ip);

        IList<IPInfo> GetIPInfoFromAPIs(string ip);
    }
}
