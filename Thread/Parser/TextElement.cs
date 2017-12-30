using System;
using System.Collections.Generic;
using System.Text;

namespace Thread.Parser
{
    public class TextElement : IExpressionElement
    {
        public string Text { get; set; }

        public TextElement(string text)
        {
            Text = text;
        }

        public void Execute(Sequence sequence, StringBuilder builder)
        {
            builder.Append(Text);
        }
    }
}
