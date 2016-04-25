// -----------------------------------------------------------------------
// <copyright file="ServiceMonitoringCriteria.cs" company="Nexcel Solutions Vietnam">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2014.6.26">Created</change>
// </history>

namespace Fanex.Maintenance.Repository.Criteria.ServiceMonitoring
{
    /// <summary>
    /// Service Monitoring Criteria.
    /// </summary>
    public class ServiceMonitoringCriteria
    {
        public string IP { get; set; }

        public string ServiceName { get; set; }

        public int Status { get; set; }

        public string ConfigFileName { get; set; }

        public string FileText { get; set; }
    }
}
