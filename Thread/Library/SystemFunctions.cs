using System.Linq;
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

        [SequenceFunction("s")]
        [SequenceFunction("set")]
        public static void Set(Sequence sequence, StringBuilder builder, string[] arguments)
        {
            ArgumentUtils.Count("set", arguments, 2);
            sequence.SetVariable(arguments[0], arguments[1]);
        }

        /// <summary>
        /// Runs a command
        /// </summary>
        [SequenceFunction("c")]
        [SequenceFunction("cmd")]
        public static void Command(Sequence sequence, StringBuilder builder, string[] arguments)
        {
            ArgumentUtils.Min("cmd", arguments, 1);
            sequence.ExecuteCommand(arguments[0], arguments.ToList().GetRange(1, arguments.Length - 1).ToArray());
        }
    }
}
