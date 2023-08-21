using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    while (_ignoreChars.Contains(_fileContent[_pos - 1])) _pos++;
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
                        elements.Add(new Semicolon());
                        break;
                    }
                    case '#':
                    {
                        _currentlyParsingString = true;
                        elements.Add(new HashSymbol());
                        elements.Add(new String(ParseStringOrIdentifier(new char[] { ';' })));
                        elements.Add(new Semicolon());
                        _currentlyParsingString = false;
                        break;
                    }
                    case '"':
                    {
                        _currentlyParsingString = true;
                        elements.Add(new String(ParseStringOrIdentifier(new char[] { '"' })));
                        _currentlyParsingString = false;
                        break;
                    }
                    case '{':
                    {
                        elements.Add(new OpenCurlyBracket());
                        break;
                    }
                    case '}':
                    {
                        elements.Add(new CloseCurlyBracket());
                        break;
                    }
                    case '[':
                    {
                        elements.Add(new OpenSquareBracket());
                        break;
                    }
                    case ']':
                    {
                        elements.Add(new CloseSquareBracket());
                        break;
                    }
                    case '(':
                    {
                        elements.Add(new OpenParenthesis());
                        break;
                    }
                    case ')':
                    {
                        elements.Add(new CloseParenthesis());
                        break;
                    }
                    case '.':
                    {
                        elements.Add(new Dot());
                        break;
                    }
                    default:
                        _pos--;
                        elements.Add(new Identifier(ParseStringOrIdentifier(new[] { '.', ';', '=', '!', '?', '[', ']', '(', ')', '<', '>', '{', '}' })));
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