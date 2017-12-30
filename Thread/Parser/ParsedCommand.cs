using System.Collections.Generic;
using System.Linq;

namespace Thread.Parser
{
    public class ParsedCommand
    {
        public string Name { get; set; }
        public IEnumerable<object> Arguments { get; set; }

        public ParsedCommand(string name, IEnumerable<object> arguments)
        {
            Name = name;
            Arguments = arguments;
        }

        public void Execute(Sequence sequence)
        {
            sequence.ExecuteCommand(Name, Arguments.ToArray());
        }
    }
}
