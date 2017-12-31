using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public void Execute(Sequence sequence, StringBuilder builder)
        {
            sequence.ExecuteBlock(Name, builder, SubExpression, Arguments.ToArray());
        }

        public override string ToString()
        {
            return $"{{{Name} {string.Join(", ", Arguments)}}}{SubExpression}{{/}}";
        }
    }
}
