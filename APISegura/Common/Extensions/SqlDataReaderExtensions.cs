using Microsoft.Data.SqlClient;

namespace APISegura.Common.Extensions
{
    public static class SqlDataReaderExtensions
    {
        public static string GetSafeString(
            this SqlDataReader reader,
            string columnName)
        {
            return reader[columnName] != DBNull.Value
                ? reader[columnName]?.ToString() ?? string.Empty
                : string.Empty;
        }

        public static int GetSafeInt32(
            this SqlDataReader reader,
            string columnName)
        {
            return reader[columnName] != DBNull.Value
                ? Convert.ToInt32(reader[columnName])
                : 0;
        }

        public static long GetSafeInt64(
            this SqlDataReader reader,
            string columnName)
        {
            return reader[columnName] != DBNull.Value
                ? Convert.ToInt64(reader[columnName])
                : 0;
        }

        public static decimal GetSafeDecimal(
            this SqlDataReader reader,
            string columnName)
        {
            return reader[columnName] != DBNull.Value
                ? Convert.ToDecimal(reader[columnName])
                : 0;
        }

        public static double GetSafeDouble(
            this SqlDataReader reader,
            string columnName)
        {
            return reader[columnName] != DBNull.Value
                ? Convert.ToDouble(reader[columnName])
                : 0;
        }

        public static bool GetSafeBoolean(
            this SqlDataReader reader,
            string columnName)
        {
            return reader[columnName] != DBNull.Value
                && Convert.ToBoolean(reader[columnName]);
        }

        public static DateTime GetSafeDateTime(
            this SqlDataReader reader,
            string columnName)
        {
            return reader[columnName] != DBNull.Value
                ? Convert.ToDateTime(reader[columnName])
                : DateTime.MinValue;
        }

        public static Guid GetSafeGuid(
            this SqlDataReader reader,
            string columnName)
        {
            return reader[columnName] != DBNull.Value
                ? Guid.Parse(reader[columnName].ToString()!)
                : Guid.Empty;
        }

        public static T? GetSafeNullable<T>(
            this SqlDataReader reader,
            string columnName)
            where T : struct
        {
            return reader[columnName] != DBNull.Value
                ? (T?)Convert.ChangeType(reader[columnName], typeof(T))
                : null;
        }
    }
}
