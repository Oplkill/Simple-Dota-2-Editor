using System.Collections.Generic;

namespace KV_reloaded
{
    public class KVToken
    {
        public string Key = "";
        public string Value = "";

        public KVTokenType Type;

        public List<KVToken> Children = null;
        public KVToken Parent = null;

        //--------------------------------

        public string[] comments = new string[2]; // 2 - это количество элементов в enum CommentPlace
    }

    /// <summary>
    /// Комментарии и/или пробелы/табы в определенных местах
    /// </summary>
    public enum CommentPlace
    {
        BeforeKey,
        AfterKey,
        //AfterValue,
    }

    public enum KVTokenType
    {
        KVsimple,
        KVblock,
        Comment,
    }
}