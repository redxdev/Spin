using System.Text;

namespace Spin.Parser
{
    public class VariableElement : IExpressionElement
    {
        public VariableRef Variable { get; set; }

        public VariableElement(VariableRef vref)
        {
            Variable = vref;
        }

        public void Execute(Sequence sequence, StringBuilder builder)
        {
            builder.Append(sequence.GetVariable(Variable));
        }
    }
}
