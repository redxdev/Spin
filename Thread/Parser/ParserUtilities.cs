using Antlr4.Runtime;

namespace Thread.Parser
{
    public static class ParserUtilities
    {
        public static ThreadDocument ParseDocumentString(string input)
        {
            return ParseDocument(new AntlrInputStream(input));
        }

        public static ThreadDocument ParseDocument(ICharStream input)
        {
            var lexer = new ThreadLexer(input);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(LexerErrorListener.Instance);

            var tokenStream = new CommonTokenStream(lexer);

            var parser = new ThreadParser(tokenStream);
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
            var lexer = new ThreadLexer(input);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(LexerErrorListener.Instance);

            var tokenStream = new CommonTokenStream(lexer);

            var parser = new ThreadParser(tokenStream);
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
            var lexer = new ThreadLexer(input);
            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(LexerErrorListener.Instance);

            var tokenStream = new CommonTokenStream(lexer);

            var parser = new ThreadParser(tokenStream);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(ParserErrorListener.Instance);

            return parser.expression().element;
        }
    }
}
