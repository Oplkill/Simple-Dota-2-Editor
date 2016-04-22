using System.Collections.Generic;
using System.Linq;

namespace KV_reloaded
{
    public class KVToken
    {
        public string Key = "";
        public string Value = "";

        public KVTokenType Type;

        public List<KVToken> Children = null;
        public KVToken Parent = null;
        public SystemComment SystemComment = null;

        //--------------------------------

        public string[] comments = new string[2]; // 2 - это количество элементов в enum CommentPlace


        public void RemoveChild(string childKey)
        {
            KVToken child = null;
            foreach (var ch in Children.Where(ch => ch.Key == childKey && ch.Type == KVTokenType.KVblock))
            {
                child = ch;
            }
            if(child != null)
                Children.Remove(child);
        }

        public KVToken GetChild(string childKey)
        {
            return Children.FirstOrDefault(ch => ch.Key == childKey);
        }

        public string ChilderToString()
        {
            return Children.Aggregate("", (current, ch) => current + ch.ToString());
        }

        public override string ToString()
        {
            string str = "";

            if (Type == KVTokenType.KVsimple)
            {
                str += comments[(int)CommentPlace.BeforeKey] ?? "";
                str += "\"" + Key + "\"";
                str += comments[(int) CommentPlace.AfterKey] ?? "";
                str += "\"" + Value + "\"";
            }
            else if(Type == KVTokenType.Comment)
            {
                str += Value;
            }
            else
            {
                str += SystemComment?.ToString();
                str += comments[(int)CommentPlace.BeforeKey] ?? "";
                str += "\"" + Key + "\"";
                str += comments[(int) CommentPlace.AfterKey] ?? "";
                str += "{";
                str = Children.Aggregate(str, (current, child) => current + child.ToString());
                str += "}";
            }

            return str;
        }
    }

    /// <summary>
    /// Комментарии и/или пробелы/табы в определенных местах
    /// </summary>
    public enum CommentPlace
    {
        BeforeKey,
        AfterKey,
    }

    public enum KVTokenType
    {
        KVsimple,
        KVblock,
        Comment,
    }
}