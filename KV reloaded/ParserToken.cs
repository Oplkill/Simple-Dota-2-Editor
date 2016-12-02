namespace KV_reloaded
{
    public class ParserToken
    {
        public string Text = "";
        public ParserTokenType Type = ParserTokenType.Nil;
    }

    public enum ParserTokenType
    {
        Nil,
        NewLine,
        Space,
        Comment,
        Text,
        NewBlock,
        EndBlock,
        Eof,
    }
}