using Spin.Parser;

namespace Spin
{
    /// <summary>
    /// A single "unit" of dialogue in a sequence.
    /// </summary>
    public struct Line
    {
        /// <summary>
        /// The name of the line.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The primary element of the line - this is generally the text to display.
        /// </summary>
        public IExpressionElement PrimaryElement { get; set; }

        /// <summary>
        /// The list of commands to run for this line. These are represented as unexecuted expressions,
        /// as commands can contain expressions.
        /// </summary>
        public IExpressionElement[] CommandElements { get; set; }

        public Line(string name, IExpressionElement primaryElement, IExpressionElement[] commandElements)
        {
            Name = name;
            PrimaryElement = primaryElement;
            CommandElements = commandElements;
        }
    }
}
