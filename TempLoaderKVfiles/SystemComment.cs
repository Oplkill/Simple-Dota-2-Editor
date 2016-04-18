using System.Collections.Generic;
using System.Linq;

namespace TempLoaderKVfiles
{
    public class SystemComment
    {
        public List<KV> KVList = new List<KV>();

        public override string ToString()
        {
            if (KVList.Count == 0)
                return "";

            string text = KVList.Aggregate("//@", (current, kv) => current + (" #" + kv.Key + "=\"" + kv.Value + "\""));
            return text + "\n";
        }

        public KV FindKV(string key)
        {
            return KVList.FirstOrDefault(kv => kv.Key == key);
        }

        public static SystemComment AnalyseSystemComment(string comment)
        {
            SystemComment sysComm = new SystemComment();

            int findedKey = comment.IndexOf('#');
            while (findedKey != -1)
            {
                comment = comment.Substring(findedKey);
                KV kv = new KV();
                kv.Key = comment.Substring(1, findEnd(comment, false) - 1);
                int findedValue = comment.IndexOf('\"') + 1;
                kv.Value = comment.Substring(findedValue, findEnd(comment.Substring(findedValue), true));
                sysComm.KVList.Add(kv);
                comment = comment.Substring(findedValue);

                findedKey = comment.IndexOf('#');
            }

            return sysComm;
        }

        private static int findEnd(string text, bool inValue)
        {
            int n = 0;
            while (n < text.Length)
            {
                if ((text[n] == ' ' || text[n] == '=') && !inValue)
                {
                    return n;
                }
                if (text[n] == '\"')
                    return n;
                n++;
            }

            return -1;
        }
    }

    public class KV
    {
        public string Key;
        public string Value;
    }
}