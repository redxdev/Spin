using Spin.Builder;

namespace Spin.Parser
{
    public class TextElement : IExpressionElement
    {
        public string Text { get; set; }

        public TextElement(string text)
        {
            Text = text;
        }

        public void Execute(Sequence sequence, LineBuilder builder)
        {
            builder.PushString(Text);
        }
        
        public override string ToString()
        {
            return Text;
        }
    }
}
