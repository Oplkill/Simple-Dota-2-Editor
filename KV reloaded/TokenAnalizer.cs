using System.Collections.Generic;

namespace KV_reloaded
{
    public static class TokenAnalizer
    {
        public static List<KVToken> AnaliseText(string text)
        {
            List<KVToken> tokens = new List<KVToken>();
            int n = 0;
            int line = 0;
            int symbol = 0;

            while (text.Length > n)
            {
                var tok = GetKVToken(text, null, ref n, ref line, ref symbol);
                if(tok == null)
                    break;
                tokens.Add(tok);
            }

            return tokens;
        }

        private static bool isSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\r';
        }
        
        public static KVToken GetKVToken(string text, KVToken parent, ref int n, ref int line, ref int symbol)
        {
            KVToken kvToken = new KVToken {Parent = parent};
            CommentPlace commentPlace = CommentPlace.BeforeKey;
            Token tok;
            bool key = true;

            tok = GetToken(text, ref n, ref line, ref symbol);
            while (true)
            {
                if (tok.Type == TokenType.NewLine || tok.Type == TokenType.Space || tok.Type == TokenType.Comment)
                {
                    if (kvToken.comments[(int) commentPlace] == null)
                        kvToken.comments[(int) commentPlace] = "";
                    kvToken.comments[(int) commentPlace] += tok.Text;
                }
                else if(tok.Type == TokenType.Text)
                {
                    if (key)
                    {
                        kvToken.Key = tok.Text;
                        commentPlace = CommentPlace.AfterKey;
                        key = false;
                    }
                    else
                    {
                        kvToken.Value = tok.Text;
                        kvToken.Type = KVTokenType.KVsimple;
                        break;
                    }
                }
                else if (tok.Type == TokenType.NewBlock)
                {
                    kvToken.Type = KVTokenType.KVblock;
                    kvToken.Children = new List<KVToken>();
                    KVToken childKvToken = GetKVToken(text, kvToken, ref n, ref line, ref symbol);
                    while (childKvToken != null)
                    {
                        kvToken.Children.Add(childKvToken);
                        if(childKvToken.Type == KVTokenType.Comment)
                            break;
                        childKvToken = GetKVToken(text, kvToken, ref n, ref line, ref symbol);
                    }
                    return kvToken;
                }
                else if(tok.Type == TokenType.EndBlock)
                {
                    if (kvToken.comments[(int) commentPlace] == null)
                        return null;

                    kvToken.Type = KVTokenType.Comment;
                    kvToken.Value = kvToken.comments[(int) commentPlace];
                    kvToken.comments[(int) commentPlace] = null;
                    return kvToken;
                }
                else
                {
                    return null;
                }
                tok = GetToken(text, ref n, ref line, ref symbol);
            }


            return kvToken;
        }

        public static Token GetToken(string text, ref int n, ref int line, ref int symbol)
        {
            Token str = new Token();

            if (n >= text.Length)
                return str;

            if (text[n] == '\n')
            {
                str.Type = TokenType.NewLine;
                str.Text = "\n";
                n++;
                line++;
                symbol = 0;

                return str;
            }

            if (isSpace(text[n]))
            {
                str.Type = TokenType.Space;
                int i = n;
                while (isSpace(text[n]))
                {
                    n++;
                    symbol++;
                }
                str.Text = text.Substring(i, n - i);

                return str;
            }

            if (text[n] == '/')
            {
                int i = n;
                n++;
                symbol++;
                n++;
                symbol++;
                str.Type = TokenType.Comment;
                while (text[n] != '\n')
                {
                    n++;
                    symbol++;
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
                symbol++;
                str.Type = TokenType.Text;
                int i = n;
                while (text[n] != '\"')
                {
                    n++;
                    symbol++;
                }
                str.Text = text.Substring(i, n - i);
                n++;
                symbol++;

                return str;
            }

            if (text[n] == '{')
            {
                n++;
                symbol++;
                str.Type = TokenType.NewBlock;

                return str;
            }

            if (text[n] == '}')
            {
                n++;
                symbol++;
                str.Type = TokenType.EndBlock;

                return str;
            }

            return str;
        }

        public class Token
        {
            public string Text = "";
            public TokenType Type = TokenType.nil;
            
        }

        public enum TokenType
        {
            nil,
            NewLine,
            Space,
            Comment,
            Text,
            NewBlock,
            EndBlock,
        }

    }
}