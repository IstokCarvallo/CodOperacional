using System.Text.RegularExpressions;
using System.Globalization;

namespace DesktopCodOperacional.Helpers
{
    public static class ExportHelper
    {
        public static string FormatHeader(string text)
        {
            return Regex.Replace(text, "(\\B[A-Z])", " $1");
        }

        public static string FormatDate(object? value)
        {
            if (value == null)
                return "-";

            if (value is DateTime dt)
                return dt.ToString("dd-MM-yyyy HH:mm", new CultureInfo("es-CL"));

            return value.ToString() ?? "-";
        }
    }
}
