using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thread.Parser
{
    public class FunctionElement : IExpressionElement
    {
        public string Name { get; set; }
        public IEnumerable<string> Arguments { get; set; }

        public FunctionElement(string name, IEnumerable<string> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public void Execute(Sequence sequence, StringBuilder builder)
        {
            sequence.ExecuteFunction(Name, builder, Arguments.ToArray());
        }
        
        public override string ToString()
        {
            return $"{{{{{Name} {string.Join(", ", Arguments)}}}}}";
        }
    }
}
