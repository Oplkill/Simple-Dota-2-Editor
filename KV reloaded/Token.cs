using System.Collections.Generic;

namespace KV_reloaded
{
    public class Token
    {
        public string Key = "";
        public string Value = "";
        public string Comment = "";

        public TokenType Type = TokenType.nil;

        public List<Token> Children = null;
        public Token Parent = null;

        //--------------------------------

        /// <summary>
        /// Строка от начала строки, до начала Ключа или комментария. Тут содержится оформление: Пробелы и табы
        /// </summary>
        public string BeforeKey = "";
        /// <summary>
        /// Строка от конца Ключа, до конца строки или до начала Значения
        /// </summary>
        public string AfterKey = "";
        /// <summary>
        /// Строка от конца Значения до конца строки
        /// </summary>
        public string AfterValue = "";
    }

    public enum TokenType
    {
        nil,
        KVsimple,
        KVblock,
        KVcomment,
        KVline,
    }
}