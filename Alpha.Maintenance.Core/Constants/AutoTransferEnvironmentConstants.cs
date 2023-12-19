using System.Collections.Generic;

namespace Alpha.Maintenance.Core.Constants
{
    public static class AutoTransferEnvironmentConstants
    {
        public const string PRO = "PRO";
        public const string UAT = "UAT";

        public static List<string> AllEnvironments => new List<string> { UAT, PRO };
    }
}
