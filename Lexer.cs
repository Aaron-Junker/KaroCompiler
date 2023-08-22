using System.Text;
using KaroCompiler.LexerElements;
using String = KaroCompiler.LexerElements.String;

namespace KaroCompiler
{
    internal class Lexer
    {
        private readonly string _fileContent;

        private int _pos;

        private bool _endOfDocument;

        private bool _currentlyParsingString;

        private char[] _ignoreChars = { '\t', '\n', '\r' };

        private int _currentLine = 1;

        private int _currentColumn = 1;

        private char C
        {
            get
            {
                _pos++;

                try
                {
                    _ignoreChars = !_currentlyParsingString
                        ? new[] { '\t', '\n', '\r', ' ' }
                        : new[] { '\t', '\n', '\r' };
                    while (_ignoreChars.Contains(_fileContent[_pos - 1]))
                    {
                        if (_fileContent[_pos - 1] == '\n')
                        {
                            _currentLine++;
                            _currentColumn = 1;
                        }

                        _pos++;
                        _currentColumn++;
                    }

                    return _fileContent[_pos - 1];
                }
                catch (IndexOutOfRangeException)
                {
                    _pos--;
                    _endOfDocument = true;
                    return '\0';
                }
            }
        }

        public Lexer(string fileContent)
        {
            _fileContent = fileContent;
        }

        public ILexerElement[] Parse()
        {
            List<ILexerElement> elements = new();
            char currentChar = C;
            while (!_endOfDocument)
            {
                switch (currentChar)
                {
                    case ';':
                    {
                        elements.Add(new Semicolon(_currentLine, _currentColumn));
                        break;
                    }
                    case '#':
                    {
                        _currentlyParsingString = true;
                        elements.Add(new HashSymbol(_currentLine, _currentColumn));
                        elements.Add(new String(ParseStringOrIdentifier(new[] { ';' }), _currentLine, _currentColumn + 1));
                        elements.Add(new Semicolon(_currentLine, _currentColumn + 1 + ((String)elements.Last()).Value.Length));
                        _currentlyParsingString = false;
                        break;
                    }
                    case '"':
                    {
                        _currentlyParsingString = true;
                        elements.Add(new String(ParseStringOrIdentifier(new[] { '"' }), _currentLine, _currentColumn));
                        _currentlyParsingString = false;
                        break;
                    }
                    case '{':
                    {
                        elements.Add(new OpenCurlyBracket(_currentLine, _currentColumn));
                        break;
                    }
                    case '}':
                    {
                        elements.Add(new CloseCurlyBracket(_currentLine, _currentColumn));
                        break;
                    }
                    case '[':
                    {
                        elements.Add(new OpenSquareBracket(_currentLine, _currentColumn));
                        break;
                    }
                    case ']':
                    {
                        elements.Add(new CloseSquareBracket(_currentLine, _currentColumn));
                        break;
                    }
                    case '(':
                    {
                        elements.Add(new OpenParenthesis(_currentLine, _currentColumn));
                        break;
                    }
                    case ')':
                    {
                        elements.Add(new CloseParenthesis(_currentLine, _currentColumn));
                        break;
                    }
                    case '.':
                    {
                        elements.Add(new Dot(_currentLine, _currentColumn));
                        break;
                    }
                    default:
                        _pos--;
                        elements.Add(new Identifier(ParseStringOrIdentifier(new[] { '.', ';', '=', '!', '?', '[', ']', '(', ')', '<', '>', '{', '}' }), _currentLine, _currentColumn));
                        _pos--;
                        break;
                }

                currentChar = C;
            }

            return elements.ToArray();
        }

        private string ParseStringOrIdentifier(char[] untilCharList)
        {
            StringBuilder sb = new();
            char currentChar = C;
            while (!untilCharList.Contains(currentChar))
            {
                sb.Append(currentChar);
                currentChar = C;
                if (_endOfDocument)
                    throw new Exception("Unexpected end of document");
            }

            return sb.ToString();
        }
    }
}