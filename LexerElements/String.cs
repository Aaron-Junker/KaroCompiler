namespace KaroCompiler.LexerElements
{
    internal class String : ILexerElement
    {
        public string Value;

        public String(string value)
        {
            this.Value = value;
        }
    }
}