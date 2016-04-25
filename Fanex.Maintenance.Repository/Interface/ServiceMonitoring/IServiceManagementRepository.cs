// -----------------------------------------------------------------------
// <copyright file="IServiceManagementRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2014.6.26">Created</change>
// </history>

namespace Fanex.Maintenance.Repository.Interface.ServiceMonitoring
{
    using System.Collections.Generic;
    using System.ServiceProcess;
    using Fanex.ServiceManagement.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IServiceManagementRepository
    {
        bool TestConnection();

        List<ServiceViewModel> GetServices();

        ServiceController GetServiceByName(string serviceName);

        void StartService(ServiceController service);

        void StopService(ServiceController service);

        void RestartService(ServiceController service);

        List<string> GetServiceConfigs(string serviceName);

        string GetServiceConfigFile(string serviceName, string fileName);

        bool SaveServiceConfigFile(string serviceName, string fileName, string fileText);

        string GetEtag(List<ServiceViewModel> servicesList);
    }
}
