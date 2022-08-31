namespace Alpha.Maintenance.Service.Interface.IPInformation
{
    using Core.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPInformationService
    {
        Task<IEnumerable<IPInfo>> GetIPInfos(string ip);
    }
}