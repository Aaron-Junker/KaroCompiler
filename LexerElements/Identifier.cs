namespace KaroCompiler.LexerElements
{
    internal class Identifier : ILexerElement
    {
        public string Value;

        public Identifier(string value, int line, int column) : base(line, column)
        {
            Value = value;
        }
    }
}