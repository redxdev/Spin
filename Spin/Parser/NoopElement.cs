using System;
using System.Collections.Generic;
using System.Text;

namespace Spin.Parser
{
    public class NoopElement : IExpressionElement
    {
        public void Execute(Sequence sequence, StringBuilder builder)
        {
        }
    }
}
