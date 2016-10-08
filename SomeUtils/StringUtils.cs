using System.Linq;

namespace SomeUtils
{
    public static class StringUtils
    {
        /// <summary>
        /// Умножает строку str на number раз
        /// </summary>
        public static string GetStringInNumber(string str, int number)
        {
            string text = "";

            for (int i = 0; i < number; i++)
            {
                text += str;
            }

            return text;
        }

        /// <summary>
        /// Char c is " ", "\t", "\r"
        /// </summary>
        public static bool IsSpaceOrTab(char c)
        {
            return c == ' ' || c == '\t' || c == '\r';
        }

        public static bool isDigit(string str)
        {
            return str.All(ch => char.IsDigit(ch) || ch == '.' || ch == '-');
        }
    }
}