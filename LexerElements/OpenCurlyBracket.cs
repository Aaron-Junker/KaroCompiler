using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaroCompiler.LexerElements
{
    internal class OpenCurlyBracket : ILexerElement
    {
        public OpenCurlyBracket(int line, int column) : base(line, column)
        {
        }
    }
}
