namespace Alpha.Maintenance.Core
{
    using System;

    public static class SystemTime
    {
        [ThreadStatic]
        private static DateTime? today;

        [ThreadStatic]
        private static DateTime? now;

        public static DateTime Today
        {
            get { return today ?? DateTime.Today; }
            internal set { today = value; }
        }

        public static DateTime Now
        {
            get { return now ?? DateTime.Now; }
            internal set { now = value; }
        }
    }
}