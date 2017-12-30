using Antlr4.Runtime;

namespace Thread.Parser
{
    public static class ParserUtilities
    {
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
