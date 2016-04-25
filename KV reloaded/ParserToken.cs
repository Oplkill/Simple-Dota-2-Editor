namespace KV_reloaded
{
    public class ParserToken
    {
        public string Text = "";
        public ParserTokenType Type = ParserTokenType.nil;
    }

    public enum ParserTokenType
    {
        nil,
        NewLine,
        Space,
        Comment,
        Text,
        NewBlock,
        EndBlock,
        Eof,
    }
}