namespace Alpha.Maintenance.Core.Extension
{
    using System;

    public static class DateTimeExtensions
    {
        private const string ShortDate = "MMM dd, yyyy";

        private const string LongDate = "MMM dd,yyyy HH:mm";

        public static string ToShortDate(this DateTime dateTime)
            => dateTime.ToString(ShortDate);

        public static string ToLongDate(this DateTime dateTime)
           => dateTime.ToString(LongDate);
    }
}
