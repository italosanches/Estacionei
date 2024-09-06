using System.Text.RegularExpressions;

namespace Estacionei.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveSpecialCharacters(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            Regex regex = new Regex("[^a-zA-Z0-9]");

            return regex.Replace(str, string.Empty);
        }

    }
}
