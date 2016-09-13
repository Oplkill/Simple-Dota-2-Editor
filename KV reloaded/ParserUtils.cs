using System.Linq;

namespace KV_reloaded
{
    public static class ParserUtils
    {
        public static string SkipComment(string text, ref int n, ref int line, ref int symbol)
        {
            int i = n;
            n++;
            symbol++;
            if (text[n] != '/')
                throw new ErrorParser(KvError.NotOveredComment, line, symbol);
            n++;
            symbol++;
            
            while (text[n] != '\n')
            {
                n++;
                symbol++;
            }
            n++;
            line++;
            symbol = 0;

            return text.Substring(i, n - i);
        }

        public static string SkipText(string text, ref int n, ref int line, ref int symbol)
        {
            n++;
            symbol++;
            int i = n;
            while (text[n] != '\"')
            {
                n++;
                symbol++;
            }
            string str = text.Substring(i, n - i);
            n++;
            symbol++;

            return str;
        }

        public static string SkipSpace(string text, ref int n, ref int line, ref int symbol)
        {
            int i = n;
            while (SomeUtils.StringUtils.IsSpaceOrTab(text[n]))
            {
                n++;
                symbol++;
                if (n >= text.Length)
                    return text.Substring(i);
            }
            return text.Substring(i, n - i);
        }

        public static bool AllBlocksHasPare(string str)
        {
            return str.Count(s => s == '{') == str.Count(s => s == '}');
        }
    }
}