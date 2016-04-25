// -----------------------------------------------------------------------
// <copyright file="ServiceManagement.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2014.6.26">Created</change>
// </history>

namespace Fanex.Maintenance.Service.Impl.ServiceMonitoring
{
    using System.Collections.Generic;
    using System.ServiceProcess;
    using Fanex.Maintenance.Repository.Impl.ServiceMonitoring;
    using Fanex.Maintenance.Repository.Interface.ServiceMonitoring;
    using Fanex.Maintenance.Service.Interface.ServiceMonitoring;
    using Fanex.ServiceManagement.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ServiceManagement
    {
        private IServiceManagementRepository _serviceManagementRepository;

        private IUserManagement _userManagement;

        public ServiceManagement(int userID, string ip)
        {
            this._userManagement = new UserManagement();
            var networkLogin = this._userManagement.GetNetworkLoginByIP(userID, ip);
            this._serviceManagementRepository = new ServiceManagementRepository(
                ip,
                networkLogin == null ? string.Empty : networkLogin.UserName,
                networkLogin == null ? string.Empty : networkLogin.Password);
        }

        public ServiceManagement(NetworkLogin networkLogin)
        {
            this._serviceManagementRepository = new ServiceManagementRepository(networkLogin.IP, networkLogin.UserName, networkLogin.Password);
        }

        public bool TestConnection()
        {
            return this._serviceManagementRepository.TestConnection();
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="status">The status.</param>
        /// <returns>List service view model.</returns>
        public List<ServiceViewModel> GetServices(string serviceName, int status)
        {
            if (serviceName == null)
            {
                serviceName = string.Empty;
            }

            var result = new List<ServiceViewModel>();
            var listServices = this._serviceManagementRepository.GetServices();
            if (listServices != null)
            {
                foreach (var service in listServices)
                {
                    if (service.Name.ToLower().Contains(serviceName.ToLower()))
                    {
                        if (status == 0
                            || (status == (int)ServiceControllerStatus.Running
                                && (service.Status == ServiceControllerStatus.Running.ToString() || service.Status == ServiceControllerStatus.StartPending.ToString()))
                            || (status == (int)ServiceControllerStatus.Stopped
                                && (service.Status == ServiceControllerStatus.Stopped.ToString() || service.Status == ServiceControllerStatus.StopPending.ToString())))
                        {
                            result.Add(service);
                        }
                    }
                }
            }

            return result;
        }

        public ServiceController GetServiceByName(string serviceName)
        {
            return this._serviceManagementRepository.GetServiceByName(serviceName);
        }

        public void StartService(string serviceName)
        {
            this._serviceManagementRepository.StartService(this.GetServiceByName(serviceName));
        }

        public void StopService(string serviceName)
        {
            this._serviceManagementRepository.StopService(this.GetServiceByName(serviceName));
        }

        public void RestartService(string serviceName)
        {
            this._serviceManagementRepository.RestartService(this.GetServiceByName(serviceName));
        }

        public List<string> GetServiceConfigs(string serviceName)
        {
            return this._serviceManagementRepository.GetServiceConfigs(serviceName);
        }

        public string GetServiceConfigFile(string serviceName, string fileName)
        {
            return this._serviceManagementRepository.GetServiceConfigFile(serviceName, fileName);
        }

        public bool SaveServiceConfigFile(string serviceName, string fileName, string fileText)
        {
            return this._serviceManagementRepository.SaveServiceConfigFile(serviceName, fileName, fileText);
        }

        public string GetEtag(List<ServiceViewModel> servicesList)
        {
            return this._serviceManagementRepository.GetEtag(servicesList);
        }
    }
}
