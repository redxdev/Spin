using Spin.Builder;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spin.Parser
{
    public class FunctionElement : IExpressionElement
    {
        public string Name { get; set; }
        public IEnumerable<object> Arguments { get; set; }

        public FunctionElement(string name, IEnumerable<object> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public void Execute(Sequence sequence, LineBuilder builder)
        {
            sequence.ExecuteFunction(Name, builder, Arguments.ToArray());
        }
        
        public override string ToString()
        {
            return $"{{{{{Name} {string.Join(", ", Arguments)}}}}}";
        }
    }
}
