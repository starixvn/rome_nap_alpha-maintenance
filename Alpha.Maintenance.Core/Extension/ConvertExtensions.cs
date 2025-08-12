
namespace Alpha.Maintenance.Core.Extension
{
    using System;
    using System.Data;

    public static class ConvertExtensions
    {
        public static byte ToByte(object value)
        {
            byte val = 0;

            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return val;
            }

            byte.TryParse(value.ToString(), out val);

            return val;
        }

        public static byte ToByte(this IDataReader obj, string fieldName)
        {
            byte val = 0;

            if (obj[fieldName] == null || obj[fieldName] == DBNull.Value || obj[fieldName].ToString() == string.Empty)
            {
                return val;
            }

            byte.TryParse(obj[fieldName].ToString(), out val);

            return val;
        }

        public static short ToInt16(object value)
        {
            short val = 0;

            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return val;
            }

            short.TryParse(value.ToString(), out val);

            return val;
        }

        public static short ToInt16(this IDataReader obj, string fieldName)
        {
            short val = 0;

            if (obj[fieldName] == null || obj[fieldName] == DBNull.Value || obj[fieldName].ToString() == string.Empty)
            {
                return val;
            }

            short.TryParse(obj[fieldName].ToString(), out val);

            return val;
        }

        public static decimal ToDecimal(object value)
        {
            var val = 0.0m;

            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return val;
            }

            decimal.TryParse(value.ToString(), out val);

            return val;
        }

        public static decimal ToDecimal(this IDataReader obj, string fieldName)
        {
            var val = 0.0m;

            if (obj[fieldName] == null || obj[fieldName] == DBNull.Value || obj[fieldName].ToString() == string.Empty)
            {
                return val;
            }

            decimal.TryParse(obj[fieldName].ToString(), out val);

            return val;
        }

        public static bool ToBoolean(object value)
        {
            var val = false;

            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return val;
            }

            if (value.ToString() == "1")
            {
                return true;
            }

            bool.TryParse(value.ToString(), out val);

            return val;
        }

        public static bool ToBoolean(this IDataReader obj, string fieldName)
        {
            var val = false;

            if (obj[fieldName] == null || obj[fieldName] == DBNull.Value || obj[fieldName].ToString() == string.Empty)
            {
                return val;
            }

            bool.TryParse(obj[fieldName].ToString(), out val);

            return val;
        }

        public static string ToString(object value)
        {
            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return string.Empty;
            }

            return Convert.ToString(value);
        }

        public static string ToString(this IDataReader obj, string fieldName)
        {
            if (obj[fieldName] == null || obj[fieldName] == DBNull.Value || obj[fieldName].ToString() == string.Empty)
            {
                return string.Empty;
            }

            return Convert.ToString(obj[fieldName]);
        }

        public static double ToDouble(object value)
        {
            var val = 0.0d;

            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return val;
            }

            double.TryParse(value.ToString(), out val);

            return val;
        }

        public static double ToDouble(this IDataReader obj, string fieldName)
        {
            var val = 0.0d;

            if (obj[fieldName] == null || obj[fieldName] == DBNull.Value || obj[fieldName].ToString() == string.Empty)
            {
                return val;
            }

            double.TryParse(obj[fieldName].ToString(), out val);

            return val;
        }

        public static DateTime ToDateTime(object value)
        {
            var val = DateTime.MinValue;

            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return val;
            }

            DateTime.TryParse(value.ToString(), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out val);

            return val;
        }

        public static DateTime ToDateTime(this IDataReader obj, string fieldName)
        {
            var val = DateTime.MinValue;

            if (obj[fieldName] == null || obj[fieldName] == DBNull.Value || obj[fieldName].ToString() == string.Empty)
            {
                return val;
            }

            DateTime.TryParse(obj[fieldName].ToString(), System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out val);

            return val;
        }

        public static int ToInt32(object value)
        {
            var val = 0;

            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return val;
            }

            if (value is decimal || value is double)
            {
                val = Convert.ToInt32(value);
            }
            else
            {
                int.TryParse(value.ToString(), out val);
            }

            return val;
        }

        public static int ToInt32(this IDataReader obj, string fieldName)
        {
            var val = 0;

            if (obj[fieldName] == null || obj[fieldName] == DBNull.Value || obj[fieldName].ToString() == string.Empty)
            {
                return val;
            }

            if (obj[fieldName] is decimal || obj[fieldName] is double)
            {
                val = Convert.ToInt32(obj[fieldName]);
            }
            else
            {
                int.TryParse(obj[fieldName].ToString(), out val);
            }

            return val;
        }

        public static long ToInt64(object value)
        {
            var val = 0L;

            if (value == null || value == DBNull.Value || value.ToString() == string.Empty)
            {
                return val;
            }

            if (value is decimal || value is double)
            {
                val = Convert.ToInt64(value);
            }
            else
            {
                long.TryParse(value.ToString(), out val);
            }

            return val;
        }

        public static long ToInt64(this IDataReader obj, string fieldName)
        {
            var val = 0L;

            if (obj[fieldName] == null || obj[fieldName] == DBNull.Value || obj[fieldName].ToString() == string.Empty)
            {
                return val;
            }

            if (obj[fieldName] is decimal || obj[fieldName] is double)
            {
                val = Convert.ToInt64(obj[fieldName]);
            }
            else
            {
                long.TryParse(obj[fieldName].ToString(), out val);
            }

            return val;
        }
    }
}
