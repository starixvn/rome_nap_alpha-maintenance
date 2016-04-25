// -----------------------------------------------------------------------
// <copyright file="IUserManagementRepository.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Maintenance.Repository.Interface.ServiceMonitoring
{
    using System.Collections.Generic;
    using Fanex.ServiceManagement.Model;

    public interface IUserManagementRepository
    {
        List<NetworkLogin> GetListServerIP(int userID);

        List<NetworkLogin> GetListServer(int userID);

        void UpdateListServer(int userID, List<NetworkLogin> serverList);

        NetworkLogin GetNetworkLoginByIP(int userID, string ip);

        void UpdateNetworkLoginByIP(int userID, string ip, string userName, string password);

        List<string> GetListFavouriteServices(int userID);

        void UpdateListFavouriteServices(int userID, List<string> servicesList);
    }
}
