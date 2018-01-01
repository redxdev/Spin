using Spin.Builder;
using System.Collections.Generic;
using System.Linq;

namespace Spin.Parser
{
    public class BlockElement : IExpressionElement
    {
        public string Name { get; set; }
        public IEnumerable<object> Arguments { get; set; }
        public IExpressionElement SubExpression { get; set; }

        public BlockElement(string name, IEnumerable<object> arguments, IExpressionElement subExpression)
        {
            Name = name;
            Arguments = arguments;
            SubExpression = subExpression;
        }

        public void Execute(Sequence sequence, LineBuilder builder)
        {
            sequence.ExecuteBlock(Name, builder, SubExpression, Arguments.ToArray());
        }

        public override string ToString()
        {
            return $"{{{Name} {string.Join(", ", Arguments)}}}{SubExpression}{{/}}";
        }
    }
}
