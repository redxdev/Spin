using System;
using System.Globalization;
using Spin.Builder;

namespace Spin.Parser
{
    public class VariableElement : IExpressionElement
    {
        public VariableRef Variable { get; set; }

        public VariableElement(VariableRef vref)
        {
            Variable = vref;
        }

        public void Execute(Sequence sequence, LineBuilder builder)
        {
            builder.PushString(Convert.ToString(sequence.GetVariable(Variable), CultureInfo.InvariantCulture));
        }
    }
}
