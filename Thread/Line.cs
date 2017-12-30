using Thread.Parser;

namespace Thread
{
    public struct Line
    {
        public IExpressionElement PrimaryElement { get; set; }
        public IExpressionElement[] CommandElements { get; set; }
    }
}
