using System;
using System.Collections.Generic;
using System.Text;

namespace Thread.Parser
{
    public class BlockElement : IExpressionElement
    {
        public string BlockName { get; set; }
        public IExpressionElement SubExpression { get; set; }

        public BlockElement(string name, IExpressionElement subExpression)
        {
            BlockName = name;
            SubExpression = subExpression;
        }

        public void Execute(Sequence sequence, StringBuilder builder)
        {

        }
    }
}
