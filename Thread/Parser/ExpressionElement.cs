using System.Text;

namespace Thread.Parser
{
    public interface IExpressionElement
    {
        void Execute(Sequence sequence, StringBuilder builder);
    }
}
