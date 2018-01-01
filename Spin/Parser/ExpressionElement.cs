using Spin.Builder;

namespace Spin.Parser
{
    public interface IExpressionElement
    {
        void Execute(Sequence sequence, LineBuilder builder);
    }
}
