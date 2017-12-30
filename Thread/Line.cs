using Thread.Parser;

namespace Thread
{
    public struct Line
    {
        public string Name { get; private set; }
        public IExpressionElement PrimaryElement { get; set; }
        public IExpressionElement[] CommandElements { get; set; }

        public Line(string name, IExpressionElement primaryElement, IExpressionElement[] commandElements)
        {
            Name = name;
            PrimaryElement = primaryElement;
            CommandElements = commandElements;
        }
    }
}
