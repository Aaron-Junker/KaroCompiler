namespace KaroCompiler.LexerElements
{
    internal class Identifier : ILexerElement
    {
        public string Value;

        public Identifier(string value)
        {
            this.Value = value;
        }
    }
}