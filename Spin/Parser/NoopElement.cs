using Spin.Builder;

namespace Spin.Parser
{
    public class NoopElement : IExpressionElement
    {
        public void Execute(Sequence sequence, LineBuilder builder)
        {
        }
    }
}
