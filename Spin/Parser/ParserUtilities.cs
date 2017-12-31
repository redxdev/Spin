using Antlr4.Runtime;

namespace Spin.Parser
{
    public static class ParserUtilities
    {
        public static SpinDocument ParseDocumentString(string input)
        {
            return ParseDocument(new AntlrInputStream(input));
        }

        public static SpinDocument ParseDocument(ICharStream input)
        {
            var lexer = new SpinLexer(input);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(LexerErrorListener.Instance);

            var tokenStream = new CommonTokenStream(lexer);

            var parser = new SpinParser(tokenStream);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(ParserErrorListener.Instance);

            return parser.document().doc;
        }

        public static ParsedCommand[] ParseCommandString(string input)
        {
            return ParseCommand(new AntlrInputStream(input));
        }

        public static ParsedCommand[] ParseCommand(ICharStream input)
        {
            var lexer = new SpinLexer(input);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(LexerErrorListener.Instance);

            var tokenStream = new CommonTokenStream(lexer);

            var parser = new SpinParser(tokenStream);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(ParserErrorListener.Instance);

            return parser.command().commands.ToArray();
        }

        public static IExpressionElement ParseExpressionString(string input)
        {
            return ParseExpression(new AntlrInputStream(input));
        }

        public static IExpressionElement ParseExpression(ICharStream input)
        {
            var lexer = new SpinLexer(input);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(LexerErrorListener.Instance);

            var tokenStream = new CommonTokenStream(lexer);

            var parser = new SpinParser(tokenStream);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(ParserErrorListener.Instance);

            return parser.expression().element;
        }
    }
}
