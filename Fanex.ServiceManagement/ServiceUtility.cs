// -----------------------------------------------------------------------
// <copyright file="ServiceUtility.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2014.6.26">Created</change>
// </history>

namespace Fanex.ServiceManagement
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Management;
    using System.Net;
    using System.ServiceProcess;
    using System.Text.RegularExpressions;
    using Fanex.Logging;
    using Fanex.ServiceManagement.Model;
    using Fanex.ServiceManagement.Utility;

    /// <summary>
    /// Service Utility.
    /// </summary>
    public class ServiceUtility
    {
        private Impersonate _impersonate;
        private NetworkCredential _networkCredential;
        private string _ip;
        private string _userName;
        private string _password;
        private TimeSpan _pendingTime;
        private bool _isSelfConnect;

        public ServiceUtility(string ip, string username, string password)
        {
            if (ConfigurationManager.AppSettings["WaitForServiceChanging"] != null)
            {
                this._pendingTime = new TimeSpan(0, 0, Convert.ToInt32(ConfigurationManager.AppSettings["WaitForServiceChanging"]));
            }
            else
            {
                this._pendingTime = new TimeSpan(0, 0, 10);
            }

            this._ip = ip;
            this._userName = username;
            this._password = password;
            this._networkCredential = new NetworkCredential(username, password);
            this._impersonate = new Impersonate(string.Empty, this._userName, this._password);
            this._isSelfConnect = this.IsLocalIpAddress(this._ip);
        }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <returns>True: connect successful; False: Connect unsuccessful.</returns>
        public bool TestConnection()
        {
            if (this._isSelfConnect)
            {
                return true;
            }

            try
            {
                if (this._impersonate.impersonateValidUser())
                {
                    ServiceController.GetServices(this._ip);
                    this._impersonate.undoImpersonation();
                    return true;
                }
            }
            catch
            {
                this._impersonate.undoImpersonation();
                return false;
            }

            return false;
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <returns>List service view model.</returns>
        public List<ServiceViewModel> GetServices()
        {
            List<ServiceViewModel> result = new List<ServiceViewModel>();
            ConnectionOptions connection = new ConnectionOptions();
            connection.Username = this._userName;
            connection.Password = this._password;
            ManagementScope scope = new ManagementScope(
                    "\\\\" + (this._isSelfConnect ? "." : this._ip) + "\\root\\CIMV2", this._isSelfConnect ? null : connection);
            scope.Connect();

            ObjectQuery query = new ObjectQuery(
                "SELECT * FROM Win32_Service");

            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher(scope, query);
            var servicesCollection = searcher.Get();
            if (servicesCollection != null && servicesCollection.Count != 0)
            {
                foreach (var service in searcher.Get())
                {
                    result.Add(new ServiceViewModel()
                    {
                        Name = Convert.ToString(service["Name"]),
                        Status = Convert.ToString(service["State"]),
                        Description = Convert.ToString(service["Description"]),
                        StartupType = Convert.ToString(service["StartMode"])
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the name of the service by.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>Service controller.</returns>
        public ServiceController GetServiceByName(string serviceName)
        {
            ServiceController result = null;
            if (this._isSelfConnect)
            {
                result = new ServiceController(serviceName);
            }
            else
            {
                if (this._impersonate.impersonateValidUser())
                {
                    result = new ServiceController(serviceName, this._ip);
                    this._impersonate.undoImpersonation();
                }
            }

            return result;
        }

        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="service">The service.</param>
        public void StartService(ServiceController service)
        {
            if (service == null || service.Status == ServiceControllerStatus.Running || service.Status == ServiceControllerStatus.StartPending)
            {
                LogService.Debug(service.ServiceName + " is null.");
                return;
            }

            if (this._isSelfConnect || this._impersonate.impersonateValidUser())
            {
                service.Start();
                try
                {
                    service.WaitForStatus(ServiceControllerStatus.Running, this._pendingTime);
                }
                catch
                {
                    // Cannot complete action (there is some error in service's code)
                    // Write log or do S.T
                    // Now Do nothing
                    LogService.Debug(service.ServiceName + " cannot be started.");
                }

                if (!this._isSelfConnect)
                {
                    this._impersonate.undoImpersonation();
                }
            }
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        /// <param name="service">The service.</param>
        public void StopService(ServiceController service)
        {
            if (service == null || service.Status == ServiceControllerStatus.Stopped || service.Status == ServiceControllerStatus.StopPending)
            {
                LogService.Error(service.ServiceName + " is null.");
                return;
            }

            if (this._isSelfConnect || this._impersonate.impersonateValidUser())
            {
                service.Stop();
                LogService.Debug(service.ServiceName + " is stoping.");
                try
                {
                    service.WaitForStatus(ServiceControllerStatus.Stopped, this._pendingTime);
                }
                catch
                {
                    // Cannot complete action
                    // Write log or do S.T
                    // Now Do nothing
                    LogService.Error(service.ServiceName + " cannot be stopped.");
                }

                if (!this._isSelfConnect)
                {
                    this._impersonate.undoImpersonation();
                }
            }
        }

        public void RestartService(ServiceController service)
        {
            this.StopService(service);
            service.Refresh();
            this.StartService(service);
        }

        /// <summary>
        /// Gets the service config.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>List service config file name.</returns>
        public List<string> GetServiceConfigs(string serviceName)
        {
            var result = new List<string>();
            string servicePath = this.GetServicePhysicalPath(serviceName) + ".config";
            string fileName = Path.GetFileName(servicePath);
            if (this._isSelfConnect)
            {
                try
                {
                    using (var fs = new FileStream(servicePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        // Detect file exists.
                        result.Add(fileName);
                    }
                }
                catch (FileNotFoundException)
                {
                    // File not exists
                    LogService.Error("File " + servicePath + " is not existed");
                }
            }
            else
            {
                using (new NetworkConnection(@"\\" + this._ip, this._networkCredential))
                {
                    try
                    {
                        servicePath = this.ResolveFilePath(servicePath, this._ip);
                        using (var fs = new FileStream(servicePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            // Detect file exists.
                            result.Add(fileName);
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        // File not exists
                        LogService.Error("File " + servicePath + " is not existed");
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the service configuration file.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>File name.</returns>
        public string GetServiceConfigFile(string serviceName, string fileName)
        {
            string servicePath = this.GetServicePhysicalPath(serviceName);
            string fullPath = Path.GetDirectoryName(servicePath) + "\\" + fileName;
            string result = string.Empty;
            if (this._isSelfConnect)
            {
                try
                {
                    using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        // Detect file exists.
                        result = new StreamReader(fs).ReadToEnd();
                    }
                }
                catch (FileNotFoundException)
                {
                    // File not exists
                    LogService.Error("File " + servicePath + " is not existed");
                }
            }
            else
            {
                using (new NetworkConnection(@"\\" + this._ip, this._networkCredential))
                {
                    try
                    {
                        fullPath = this.ResolveFilePath(fullPath, this._ip);
                        using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            // Detect file exists.
                            result = new StreamReader(fs).ReadToEnd();
                        }
                    }
                    catch (FileNotFoundException)
                    {
                        // File not exists
                        LogService.Error("File " + servicePath + " is not existed");
                    }
                }
            }

            return Regex.Replace(result, "<!--.*?-->", string.Empty);
        }

        /// <summary>
        /// Saves the service configuration file.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileText">The file text.</param>
        /// <returns>True: File save successful; False: File save unsuccessful.</returns>
        public bool SaveServiceConfigFile(string serviceName, string fileName, string fileText)
        {
            string servicePath = this.GetServicePhysicalPath(serviceName);
            string fullPath = Path.GetDirectoryName(servicePath) + "\\" + fileName;
            string result = string.Empty;
            if (this._isSelfConnect)
            {
                try
                {
                    System.IO.File.WriteAllText(fullPath, fileText);
                    return true;
                }
                catch (FileNotFoundException)
                {
                    // File not exists
                    LogService.Error("File " + fullPath + " is not existed");
                    return false;
                }
            }
            else
            {
                using (new NetworkConnection(@"\\" + this._ip, this._networkCredential))
                {
                    try
                    {
                        fullPath = this.ResolveFilePath(fullPath, this._ip);
                        System.IO.File.WriteAllText(fullPath, fileText);
                        return true;
                    }
                    catch (FileNotFoundException)
                    {
                        // File not exists
                        LogService.Error("File " + fullPath + " is not existed");
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the remote service path.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>Service's directory path.</returns>
        private string GetServicePhysicalPath(string serviceName)
        {
            ConnectionOptions conn = new ConnectionOptions();
            conn.Username = this._userName;
            conn.Password = this._password;
            ManagementScope scope = new ManagementScope("\\\\" + (this._isSelfConnect ? "." : this._ip) + "\\root\\default", this._isSelfConnect ? null : conn);
            var registry = new ManagementClass(scope, new ManagementPath("StdRegProv"), null);

            // Returns a specific value for a specified key
            ManagementBaseObject inputParams = registry.GetMethodParameters("GetStringValue");
            inputParams["hDefKey"] = 0x80000002; // LOCAL_MACHINE
            inputParams["sSubKeyName"] = @"SYSTEM\CurrentControlSet\Services\" + serviceName;
            inputParams["sValueName"] = "ImagePath";
            ManagementBaseObject outputParams = registry.InvokeMethod("GetStringValue", inputParams, null);
            return outputParams["sValue"].ToString().Trim('"');
        }

        /// <summary>
        /// Determines whether [is local IP address] [the specified host].
        /// </summary>
        /// <param name="host">The host name or IP.</param>
        /// <returns>True: Is local IP; False: Is remote IP.</returns>
        private bool IsLocalIpAddress(string host)
        {
            try
            {
                // get host IP addresses
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);

                // get local IP addresses
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                // test if any host IP equals to any local IP or to localhost
                foreach (IPAddress hostIP in hostIPs)
                {
                    // is localhost
                    if (IPAddress.IsLoopback(hostIP))
                    {
                        return true;
                    }

                    // is local address
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP))
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
            }

            return false;
        }

        private string ResolveFilePath(string filePath, string ip)
        {
            if (this._isSelfConnect)
            {
                return filePath;
            }

            return @"\\" + ip + "\\" + filePath.Replace(':', '$');
        }
    }
}
