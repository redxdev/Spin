using System.Text;

namespace Spin.Parser
{
    public interface IExpressionElement
    {
        void Execute(Sequence sequence, StringBuilder builder);
    }
}
