// -----------------------------------------------------------------------
// <copyright file="NetworkLogin.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.truong" date="2014.6.25">Created</change>
// </history>

namespace Fanex.ServiceManagement.Model
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class NetworkLogin
    {
        public NetworkLogin()
        {
            this.IP = string.Empty;
            this.UserName = string.Empty;
            this.Password = string.Empty;
        }

        public NetworkLogin(string ip, string username, string password)
        {
            this.IP = ip;
            this.UserName = username;
            this.Password = password;
        }

        public string IP { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
