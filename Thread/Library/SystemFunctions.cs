using System.Text;
using Thread.Attributes;
using Thread.Utility;

namespace Thread.Library
{
    public static class SystemFunctions
    {
        [SequenceFunction("v")]
        [SequenceFunction("value")]
        public static void Value(Sequence sequence, StringBuilder builder, string[] arguments)
        {
            ArgumentUtils.Count("value", arguments, 1);
            builder.Append(sequence.GetVariable(arguments[0]));
        }
    }
}
