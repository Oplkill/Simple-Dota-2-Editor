using System.Collections.Generic;

namespace KV_reloaded
{
    public class TokenAnalizer
    {//todo всяк сюда зашедший, остерегайся тут гавнокода и раздутых конструкций
        private List<Token> tokens;
        private int line = 0;
        private int symbol = 0; // Номер символа в стоке
        private int n = 0; // Номер символа во всем тексте
        private string text;

        public List<Token> AnaliseText(string _text)
        {
            tokens = new List<Token>();
            text = _text;

            while (text.Length > n)
            {
                var tok = GetToken(null);
                tokens.Add(tok);
            }

            return tokens;
        }

        private bool isSpace(char c)
        {
            if (c == ' ' || c == '\t' || c == '\r')
                return true;

            return false;
        }

        private Token GetToken(Token parent)
        {
            Token tok = new Token {Parent = parent};

            StringGetted syGetted = getNextString();
            if (syGetted.Type == StringGetted.StrType.Comment)
            {
                tok.Type = TokenType.KVcomment;
                tok.Comment = syGetted.Text;
                return tok;
            }
            else if (syGetted.Type == StringGetted.StrType.NewLine)
            {
                tok.Type = TokenType.KVline;
                return tok;
            }
            else if (syGetted.Type == StringGetted.StrType.Space)
            {
                tok.BeforeKey = syGetted.Text;
                syGetted = getNextString();
                if (syGetted.Type == StringGetted.StrType.Comment)
                {
                    tok.Type = TokenType.KVcomment;
                    tok.Comment = syGetted.Text;
                    return tok;
                }
                else if(syGetted.Type == StringGetted.StrType.NewLine)
                {
                    tok.Type = TokenType.KVline;
                    return tok;
                }
                else if(syGetted.Type == StringGetted.StrType.Text)
                {
                    tok.Key = syGetted.Text;
                }
            }
            else if (syGetted.Type == StringGetted.StrType.Text)
            {
                tok.Key = syGetted.Text;
            }

            //

            syGetted = getNextString();
            tok.AfterKey = syGetted.Text;
            
            //

            syGetted = getNextString();
            if (syGetted.Type == StringGetted.StrType.Space || syGetted.Type == StringGetted.StrType.Comment || syGetted.Type == StringGetted.StrType.NewLine)
            {
                tok.AfterKey += syGetted.Text;
                syGetted = getNextString();
            }
            if (syGetted.Type == StringGetted.StrType.Text)
            {
                tok.Type = TokenType.KVsimple;
                tok.Value = syGetted.Text;
                syGetted = getNextString();
                tok.AfterValue = syGetted.Text;
                return tok;
            }

            //-------------

            
            



            return tok;
        }

        private StringGetted getNextString()
        {
            StringGetted str = new StringGetted {Type = StringGetted.StrType.nil, Text = ""};

            if (text[n] == '\n')
            {
                str.Type = StringGetted.StrType.NewLine;
                str.Text = "\n";
                n++;
                line++;
                symbol = 0;

                return str;
            }

            if (isSpace(text[n]))
            {
                str.Type = StringGetted.StrType.Space;
                int i = n;
                while (isSpace(text[n]))
                {
                    n++;
                }
                str.Text = text.Substring(i, n - i);

                return str;
            }

            if (text[n] == '/')
            {
                int i = n;
                n++;
                n++;
                str.Type = StringGetted.StrType.Comment;
                while (text[n] != '\n')
                {
                    n++;
                }
                str.Text = text.Substring(i, n - i);
                n++;
                line++;
                symbol = 0;

                return str;
            }

            if (text[n] == '\"')
            {
                n++;
                str.Type = StringGetted.StrType.Text;
                int i = n;
                while (text[n] != '\"')
                {
                    n++;
                }
                str.Text = text.Substring(i, n - i);
                n++;

                return str;
            }

            if (text[n] == '{')
            {
                n++;
                str.Type = StringGetted.StrType.NewBlock;

                return str;
            }

            return str;
        }

        private struct StringGetted
        {
            public string Text;
            public StrType Type;
            public enum StrType
            {
                nil,
                NewLine,
                Space,
                Comment,
                Text,
                NewBlock,
            }
        }

        
    }
}