using System.IO;
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

            n = SkipComment(text, n);
            if (n == text.Length)
                return text.Substring(i);
            n++;
            line++;
            symbol = 0;

            return text.Substring(i, n - i);
        }

        /// <summary>
        /// returning end of comment (position '\n' symbol or text.Length)
        /// </summary>
        public static int SkipComment(string text, int offset)
        {
            int end = FindSymbol(text, '\n', offset);
            return (end != -1) ? end : text.Length;
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
            symbol = n = SkipSpace(text, n);
            return text.Substring(i, n - i);
        }

        public static int SkipSpace(string text, int offset)
        {
            while (offset < text.Length && SomeUtils.StringUtils.IsSpaceOrTab(text[offset]))
            {
                offset++;
            }

            return offset;
        }

        public static bool AllBlocksHasPare(string str)
        {
            return str.Count(s => s == '{') == str.Count(s => s == '}');
        }

        public static int FindSymbol(string text, char symbol, int start)
        {
            int len = text.Length;
            while (start < len)
            {
                if (text[start] == symbol)
                {
                    int symb = thisSymbolInCommentZone(text, start);
                    if (symb == -1)
                        return start;
                    else
                        start = symb;
                }

                start++;
            }

            return -1;
        }

        /// <summary>
        /// Getting first prev symbol thruegh comments
        /// </summary>
        public static int GetPositionFirstPrevSymbol(string text, char symbol, int start)
        {
            if (text.Length <= start)
                return -1;
            while (start > 0)
            {
                if (text[start] == symbol)
                {
                    int symb = thisSymbolInCommentZone(text, start);
                    if (symb == -1)
                        return start;
                    else
                        start = symb;
                }

                start--;
            }

            return -1;
        }

        /// <summary>
        /// Getting first next symbol thruegh comments
        /// </summary>
        public static int GetPositionFirstNextSymbol(string text, char symbol, int start)
        {
            if (text.Length <= start)
                return -1;
            int len = text.Length;
            while (start < len)
            {
                if (text[start] == symbol)
                {
                    int symb = thisSymbolInCommentZone(text, start);
                    if (symb == -1)
                        break;
                    else if ((symb = SkipComment(text, symb)) != len)
                        start = symb;
                    else
                        return -1;
                }

                start++;
            }

            return start;
        }

        /// <summary>
        /// If it in comment zone - returns start of comment, else - "-1"
        /// </summary>
        public static int thisSymbolInCommentZone(string text, int symbPos)
        {
            while (symbPos > 0 && symbPos < text.Length)
            {
                if (text[symbPos] == '/' && text[symbPos-1] == '/')
                    return symbPos - 1;
                else if (text[symbPos] == '\n')
                    return -1;

                symbPos--;
            }

            return -1;
        }

        public static string GetKeyText(string text, int offset)
        {
            int pos = GetPositionFirstPrevSymbol(text, '\n', offset);
            string line = text.Substring(pos + 1, offset - pos);

            int posStart = GetPositionFirstNextSymbol(line, '\"', 0);
            pos = GetPositionFirstNextSymbol(line, '\"', posStart + 1);

            return line.Substring(posStart + 1, pos - posStart - 1);
        }

        /// <summary>
        /// true - значит это ключ
        /// false - это значение
        /// null - это ни ключ, ни значение. Неопределенно
        /// </summary>
        public static bool? ItsKey(string text, int offset)
        {
            int pos = GetPositionFirstPrevSymbol(text, '\n', offset);
            int localOfsset = offset - pos;
            string line = text.Substring(pos + 1, localOfsset);

            pos = GetPositionFirstNextSymbol(line, '\"', 0);
            if (pos == -1)
                return null;
            if (pos + 1 >= line.Length)
                return true;
            pos = GetPositionFirstNextSymbol(line, '\"', pos + 1);
            if (pos == -1)
                return true;

            pos = GetPositionFirstNextSymbol(line, '\"', pos + 1);
            if (pos == -1 || pos == localOfsset)
                return null;
            pos = GetPositionFirstNextSymbol(line, '\"', pos + 1);
            if (pos == -1)
                return false;
            if (pos < localOfsset)
                return null;
            else
                return false;

            return null;
        }

        /// <summary>
        /// Возвращает текст ключа блока, который содержит этот offset
        /// Если это корень, то возвращается пустая строка
        /// Null если есть ошибка
        /// </summary>
        public static string GetOwnerKeyBlockText(string text, int offset)
        {
            int pos = GetPositionFirstPrevSymbol(text, '{', offset);
            if (pos == -1) return "";
            pos = GetPositionFirstPrevSymbol(text, '\"', pos);
            if (pos == -1) return null;
            int posStart = GetPositionFirstPrevSymbol(text, '\"', pos - 1);
            if (posStart == -1) return null;
            if (posStart + 1 >= pos) return null;

            return text.Substring(posStart + 1, pos - posStart - 1);
        }

        public static bool ThisPositionStartOfComment(string text, int offset)
        {
            return offset + 1 <= text.Length
                   && text[offset] == '/'
                   && text[offset + 1] == '/';
        }
    }
}