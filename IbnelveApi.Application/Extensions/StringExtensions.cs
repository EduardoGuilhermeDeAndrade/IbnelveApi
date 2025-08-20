using System.Text.RegularExpressions;

namespace IbnelveApi.Application.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Remove todos os caracteres que não sejam dígitos.
        /// </summary>
        public static string OnlyNumbers(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return Regex.Replace(value, @"[^\d]", "");
        }

        /// <summary>
        /// Remove caracteres especiais (mantém apenas letras, números e espaço).
        /// </summary>
        public static string RemoveSpecialCharacters(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return Regex.Replace(value, @"[^a-zA-Z0-9\s]", "");
        }
    }
}
