using System;
using System.Collections.Generic;
using System.Text;

namespace Thread.Parser
{
    public class NoopElement : IExpressionElement
    {
        public void Execute(Sequence sequence, StringBuilder builder)
        {
        }
    }
}
