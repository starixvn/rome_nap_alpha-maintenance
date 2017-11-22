
namespace Alpha.Maintenance.Service.Interface.IPInformation
{
    using Core.Model;

    public interface IPInformationService
    {
        IPInfo GetIPInformation(string ip);
    }
}
