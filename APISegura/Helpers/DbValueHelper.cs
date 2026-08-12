namespace APISegura.Helpers
{
    public static class DbValueHelper
    {
        public static object DbValue<T>(T? value)
            where T : struct
        {
            return value.HasValue
                ? value.Value
                : DBNull.Value;
        }

        public static object DbValue(string? value)
        {
            return value ?? (object)DBNull.Value;
        }
    }
}
