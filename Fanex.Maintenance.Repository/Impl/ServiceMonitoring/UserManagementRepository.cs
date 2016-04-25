// -----------------------------------------------------------------------
// <copyright file="UserManagementRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Maintenance.Repository.Impl.ServiceMonitoring
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Maintenance.Repository.Interface.ServiceMonitoring;
    using Fanex.ServiceManagement.Model;

    /// <summary>
    /// User Management Repository.
    /// </summary>
    public class UserManagementRepository : IUserManagementRepository
    {
        /// <summary>
        /// Gets the list server IP.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>Network login collection.</returns>
        public List<NetworkLogin> GetListServerIP(int userID)
        {
            var result = new List<NetworkLogin>();
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + "//ServiceMonitoringCache//" + userID.ToString();
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (StreamReader file = new StreamReader(File.Open(
                folderPath + "//ServerList.txt",
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite)))
            {
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var elements = line.Split(',');
                        result.Add(new NetworkLogin()
                        {
                            IP = elements[0],
                            UserName = string.Empty,
                            Password = string.Empty
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the list server.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>List Network login.</returns>
        public List<NetworkLogin> GetListServer(int userID)
        {
            var result = new List<NetworkLogin>();
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + "//ServiceMonitoringCache//" + userID.ToString();
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (StreamReader file = new StreamReader(File.Open(
                folderPath + "//ServerList.txt",
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite)))
            {
                while (!file.EndOfStream)
                {
                    var line = file.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        var elements = line.Split(',');
                        result.Add(new NetworkLogin()
                        {
                            IP = elements[0],
                            UserName = elements[1],
                            Password = Cryptography.CfsDeCrypt(elements[2])
                        });
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the list server.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="serverList">The server list.</param>
        public void UpdateListServer(int userID, List<NetworkLogin> serverList)
        {
            if (serverList != null && serverList.Count != 0)
            {
                string folderPath = AppDomain.CurrentDomain.BaseDirectory + "//ServiceMonitoringCache//" + userID.ToString();
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                using (StreamWriter file = new StreamWriter(File.Open(
                    folderPath + "//ServerList.txt",
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite)))
                {
                    foreach (var serverItem in serverList)
                    {
                        file.WriteLine(NetworkLoginToCSVString(serverItem));
                    }
                }
            }
        }

        public NetworkLogin GetNetworkLoginByIP(int userID, string ip)
        {
            var listServers = this.GetListServer(userID);
            NetworkLogin networkLogin = listServers.Where(m => (m.IP == ip)).FirstOrDefault();
            return networkLogin;
        }

        /// <summary>
        /// Updates the network login by IP.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="ip">The server IP.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        public void UpdateNetworkLoginByIP(int userID, string ip, string userName, string password)
        {
            var listServers = this.GetListServer(userID);
            if (listServers == null || listServers.Count == 0)
            {
                listServers = new List<NetworkLogin>();
                listServers.Add(new NetworkLogin() { IP = ip, UserName = userName, Password = password });
                this.UpdateListServer(userID, listServers);
                return;
            }

            bool isExists = false;
            foreach (var server in listServers)
            {
                if (server.IP == ip)
                {
                    isExists = true;
                    server.UserName = userName;
                    server.Password = password;
                    break;
                }
            }

            if (!isExists)
            {
                listServers.Add(new NetworkLogin() { IP = ip, UserName = userName, Password = password });
            }

            this.UpdateListServer(userID, listServers);
        }

        /// <summary>
        /// Gets the list favorite services.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns>List favorite services.</returns>
        public List<string> GetListFavouriteServices(int userID)
        {
            var result = new List<string>();
            string folderPath = AppDomain.CurrentDomain.BaseDirectory + "//ServiceMonitoringCache//" + userID.ToString();
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using (StreamReader file = new StreamReader(File.Open(
                folderPath + "//FavouriteServices.txt",
                FileMode.OpenOrCreate,
                FileAccess.ReadWrite,
                FileShare.ReadWrite)))
            {
                var line = file.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    var elements = line.Split(',');
                    result = new List<string>(line.Split(','));
                }
            }

            return result;
        }

        /// <summary>
        /// Updates the list favorite services.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="servicesList">The services list.</param>
        public void UpdateListFavouriteServices(int userID, List<string> servicesList)
        {
            if (servicesList != null && servicesList.Count != 0)
            {
                string folderPath = AppDomain.CurrentDomain.BaseDirectory + "//ServiceMonitoringCache//" + userID.ToString();
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                using (StreamWriter file = new StreamWriter(File.Open(
                    folderPath + "//FavouriteServices.txt",
                    FileMode.OpenOrCreate,
                    FileAccess.ReadWrite,
                    FileShare.ReadWrite)))
                {
                    file.WriteLine(string.Join(",", servicesList.ToArray()));
                }
            }
        }

        /// <summary>
        /// Networks the login to CSV string.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns>CSV string.</returns>
        private string NetworkLoginToCSVString(NetworkLogin server)
        {
            return server.IP + ',' + server.UserName + ',' + Cryptography.CfsEnCrypt(server.Password);
        }
    }
}
