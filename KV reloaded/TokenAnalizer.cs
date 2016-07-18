using System;
using System.Collections.Generic;

namespace KV_reloaded
{
    public static class TokenAnalizer
    {
        public static List<KVToken> AnaliseText(string text)
        {
            List<KVToken> tokens = new List<KVToken>();
            int n = 0;
            int line = 1;
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
        
        public static KVToken GetKVToken(string text, KVToken parent, ref int n, ref int line, ref int symbol)
        {
            KVToken kvToken = new KVToken {Parent = parent};
            CommentPlace commentPlace = CommentPlace.BeforeKey;
            ParserToken tok;
            bool key = true;

            tok = GetToken(text, ref n, ref line, ref symbol);
            while (true)
            {
                if (tok.Type == ParserTokenType.NewLine || tok.Type == ParserTokenType.Space || tok.Type == ParserTokenType.Comment)
                {
                    if (kvToken.comments[(int) commentPlace] == null)
                        kvToken.comments[(int) commentPlace] = "";

                    if (tok.Type == ParserTokenType.Comment && tok.Text.IndexOf("//@", StringComparison.Ordinal) == 0)
                    {
                        kvToken.SystemComment = SystemComment.AnalyseSystemComment(tok.Text);
                    }
                    else
                        kvToken.comments[(int) commentPlace] += tok.Text;
                }
                else if(tok.Type == ParserTokenType.Text)
                {
                    if (key)
                    {
                        if(string.IsNullOrEmpty(tok.Text))
                            throw new ErrorParser(KvError.EmptyKey, line, symbol);
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
                else if (tok.Type == ParserTokenType.NewBlock)
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
                else if(tok.Type == ParserTokenType.EndBlock)
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

        public static ParserToken GetToken(string text, ref int n, ref int line, ref int symbol)
        {
            ParserToken tok = new ParserToken();

            if (n >= text.Length)
            {
                tok.Type = ParserTokenType.Eof;
                return tok;
            }

            if (SomeUtils.StringUtils.IsSpaceOrTab(text[n]))
            {
                tok.Type = ParserTokenType.Space;
                tok.Text = ParserUtils.SkipSpace(text, ref n, ref line, ref symbol);

                return tok;
            }

            switch (text[n])
            {
                case '\n':
                    tok.Type = ParserTokenType.NewLine;
                    tok.Text = "\n";
                    n++;
                    line++;
                    symbol = 0;
                    return tok;

                case '/':
                    tok.Type = ParserTokenType.Comment;
                    tok.Text = ParserUtils.SkipComment(text, ref n, ref line, ref symbol);
                    return tok;

                case '\"':
                    tok.Type = ParserTokenType.Text;
                    tok.Text = ParserUtils.SkipText(text, ref n, ref line, ref symbol);
                    return tok;

                case '{':
                    tok.Type = ParserTokenType.NewBlock;
                    n++;
                    symbol++;
                    return tok;

                case '}':
                    tok.Type = ParserTokenType.EndBlock;
                    n++;
                    symbol++;
                    return tok;
            }

            throw new ErrorParser(KvError.UndefinitedSymbols, line, symbol);
        }
    }
}