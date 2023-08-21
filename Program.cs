using System.Reflection;
using KaroCompiler;
using KaroCompiler.LexerElements;

string fileContent = File.ReadAllText(Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) + "/exampleFiles/helloWorld.karo");

Lexer l = new Lexer(fileContent);

ILexerElement[] elements = l.Parse();

Console.WriteLine();