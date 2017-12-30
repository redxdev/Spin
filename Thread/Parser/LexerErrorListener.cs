using Antlr4.Runtime;

namespace Thread.Parser
{
    internal class LexerErrorListener : IAntlrErrorListener<int>
    {
        public static readonly LexerErrorListener Instance = new LexerErrorListener();

        public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new SequenceParseException($"syntax error {line}:{charPositionInLine} - {msg}", e);
        }
    }
}
