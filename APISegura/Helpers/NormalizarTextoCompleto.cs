using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace APISegura.Helpers
{
    public class TextComparisonResult
    {
        public bool SonIguales { get; set; }
        public string TextoANormalizado { get; set; } = string.Empty;
        public string TextoBNormalizado { get; set; } = string.Empty;
    }

    public static class TextNormalizer
    {
        private static readonly Regex MultipleSpacesRegex = new Regex(@"\s+", RegexOptions.Compiled);

        public static string Normalizar(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
                       
            string textoFormD = input.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(textoFormD.Length);

            foreach (char c in textoFormD)
            {
                UnicodeCategory category = CharUnicodeInfo.GetUnicodeCategory(c);

                if (category == UnicodeCategory.NonSpacingMark)
                    continue;

                if (char.IsPunctuation(c) || char.IsSymbol(c))
                    continue;

                sb.Append(c);
            }

            string textoLimpio = sb.ToString()
                .Normalize(NormalizationForm.FormC)
                .ToUpperInvariant();

            return MultipleSpacesRegex.Replace(textoLimpio, " ").Trim();
        }

        /// Compara dos cadenas y determina si son equivalentes tras normalizarlas.
        public static bool SonIguales(string? textoA, string? textoB)
        {
            return string.Equals(Normalizar(textoA), Normalizar(textoB), StringComparison.Ordinal);
        }

        /// Compara dos cadenas y devuelve el detalle con los textos limpios y el resultado.
        public static TextComparisonResult Comparar(string? textoA, string? textoB)
        {
            string normalizadoA = Normalizar(textoA);
            string normalizadoB = Normalizar(textoB);

            return new TextComparisonResult
            {
                TextoANormalizado = normalizadoA,
                TextoBNormalizado = normalizadoB,
                SonIguales = string.Equals(normalizadoA, normalizadoB, StringComparison.Ordinal)
            };
        }
    }


}
