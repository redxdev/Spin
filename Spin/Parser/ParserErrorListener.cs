using Antlr4.Runtime;

namespace Spin.Parser
{
    internal class ParserErrorListener : BaseErrorListener
    {
        public static readonly ParserErrorListener Instance = new ParserErrorListener();

        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new SequenceParseException($"syntax error {line}:{charPositionInLine} - {msg}", e);
        }
    }
}
