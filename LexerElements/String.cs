namespace KaroCompiler.LexerElements
{
    internal class String : ILexerElement
    {
        public string Value;

        public String(string value, int line, int column) : base(line, column)
        {
            Value = value;
        }
    }
}