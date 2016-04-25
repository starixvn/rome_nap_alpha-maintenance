// -----------------------------------------------------------------------
// <copyright file="IUserManagement.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Maintenance.Service.Interface.ServiceMonitoring
{
    using System.Collections.Generic;
    using Fanex.ServiceManagement.Model;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IUserManagement
    {
        List<NetworkLogin> GetListServerIP(int userID);

        List<NetworkLogin> GetListServer(int userID);

        void UpdateListServer(int userID, List<NetworkLogin> serverList);

        void UpdateNetworkLoginByIP(int userID, NetworkLogin networkLogin);

        NetworkLogin GetNetworkLoginByIP(int userID, string ip);

        void UpdateNetworkLoginByIP(int userID, string ip, string userName, string password);
    }
}
