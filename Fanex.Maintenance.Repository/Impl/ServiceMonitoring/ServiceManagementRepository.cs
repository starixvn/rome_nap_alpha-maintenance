// -----------------------------------------------------------------------
// <copyright file="ServiceManagementRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2014.6.26">Created</change>
// </history>

namespace Fanex.Maintenance.Repository.Impl.ServiceMonitoring
{
    using System.Collections.Generic;
    using System.ServiceProcess;
    using Fanex.Maintenance.Core.Extension;
    using Fanex.Maintenance.Repository.Interface.ServiceMonitoring;
    using Fanex.ServiceManagement;
    using Fanex.ServiceManagement.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ServiceManagementRepository : IServiceManagementRepository
    {
        private ServiceUtility _serviceUtility;

        public ServiceManagementRepository(string ip, string username, string password)
        {
            this._serviceUtility = new ServiceUtility(ip, username, password);
        }

        public bool TestConnection()
        {
            return this._serviceUtility.TestConnection();
        }

        public List<ServiceViewModel> GetServices()
        {
            return this._serviceUtility.GetServices();
        }

        public ServiceController GetServiceByName(string serviceName)
        {
            return this._serviceUtility.GetServiceByName(serviceName);
        }

        public void StartService(ServiceController service)
        {
            this._serviceUtility.StartService(service);
        }

        public void StopService(ServiceController service)
        {
            this._serviceUtility.StopService(service);
        }

        public void RestartService(ServiceController service)
        {
            this._serviceUtility.RestartService(service);
        }

        public List<string> GetServiceConfigs(string serviceName)
        {
            return this._serviceUtility.GetServiceConfigs(serviceName);
        }

        public string GetServiceConfigFile(string serviceName, string fileName)
        {
            return this._serviceUtility.GetServiceConfigFile(serviceName, fileName);
        }

        public bool SaveServiceConfigFile(string serviceName, string fileName, string fileText)
        {
            return this._serviceUtility.SaveServiceConfigFile(serviceName, fileName, fileText);
        }

        /// <summary>
        /// Gets the ETAG.
        /// </summary>
        /// <param name="servicesList">The tasks list.</param>
        /// <returns>ETAG string.</returns>
        public string GetEtag(List<ServiceViewModel> servicesList)
        {
            string result = string.Empty;

            for (int i = 0; i < servicesList.Count; i++)
            {
                result += servicesList[i].Name.ToString();
                result += servicesList[i].Status.ToString();
            }

            return Extension.GetMd5Hash(result);
        }
    }
}
