namespace KaroCompiler.LexerElements
{
    internal abstract class ILexerElement
    {
        public int Line;
        public int Column;

        protected ILexerElement(int line = 0, int column = 0)
        {
            this.Line = line;
            this.Column = column;
        }
    }
}