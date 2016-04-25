using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KV_reloaded
{
    public class ErrorParser : Exception
    {
        public int Line;
        public int Symbol;
        public KvError KvError;

        public ErrorParser(KvError error, int line, int symbol)
        {
            KvError = error;
            Line = line;
            Symbol = symbol;
        }

        public string GetMessage()
        {
            string errorText = "";
            errorText += "\n" + "Line - " + Line;
            errorText += "\n" + "Symbol - " + Symbol;
            errorText += "\n" + KvError.ToStringLang();

            return errorText;
        }
    }

    public enum KvError
    {
        NotOveredComment, // Незавершенный комментарий. "/ Коммент"
        EmptyKey,
        NotStartedBlock,
        NotEndedBlock,
        UndefinitedSymbols,
        LonelyKey,
    }

    public static class KvErrorStatic
    {
        public static string ToStringLang(this KvError kvError)
        {
            switch (kvError)
            {
                case KvError.NotOveredComment:
                    return "Comment is not overed!"; //todo пихнуть в ресурсы и руссифицировать

                case KvError.EmptyKey:
                    return "Empty key!";

                case KvError.NotStartedBlock:
                    return "Not started block";

                case KvError.NotEndedBlock:
                    return "Not ended block";

                case KvError.UndefinitedSymbols:
                    return "Undefinited symbols";

                case KvError.LonelyKey:
                    return "Lonely key";

                default:
                    return "Undefinited error!";
            }
        }
    }
}