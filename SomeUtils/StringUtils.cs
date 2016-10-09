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

        public static bool IsDigit(string str)
        {
            return str.All(ch => char.IsDigit(ch) || ch == '.' || ch == '-');
        }

        public static int FindFirstPrevSymbol(string text, char symbol, int start)
        {
            if (text.Length <= start)
                return -1;
            while (start > 0)
            {
                if (text[start] == symbol)
                {
                    return start;
                }

                start--;
            }

            return -1;
        }
    }
}