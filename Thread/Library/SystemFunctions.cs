using System;
using System.Globalization;
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
        public static void Value(Sequence sequence, StringBuilder builder, object[] arguments)
        {
            ArgumentUtils.Count("value", arguments, 1);
            builder.Append(sequence.GetVariable(Convert.ToString(arguments[0], CultureInfo.InvariantCulture)));
        }

        [SequenceFunction("s")]
        [SequenceFunction("set")]
        public static void Set(Sequence sequence, StringBuilder builder, object[] arguments)
        {
            ArgumentUtils.Count("set", arguments, 2);
            sequence.SetVariable(Convert.ToString(arguments[0], CultureInfo.InvariantCulture), arguments[1]);
        }

        /// <summary>
        /// Runs a command
        /// </summary>
        [SequenceFunction("c")]
        [SequenceFunction("cmd")]
        public static void Command(Sequence sequence, StringBuilder builder, object[] arguments)
        {
            ArgumentUtils.Min("cmd", arguments, 1);
            sequence.ExecuteCommand(Convert.ToString(arguments[0], CultureInfo.InvariantCulture), arguments.ToList().GetRange(1, arguments.Length - 1).ToArray());
        }

        [SequenceFunction("e")]
        [SequenceFunction("echo")]
        public static void Echo(Sequence sequence, StringBuilder builder, object[] arguments)
        {
            ArgumentUtils.Min("echo", arguments, 1);
            foreach (var arg in arguments)
            {
                builder.Append(arg);
            }
        }

        [SequenceFunction("noop")]
        public static void Noop(Sequence sequence, StringBuilder builder, object[] arguments)
        {
            ArgumentUtils.Count("noop", arguments, 0);
        }
    }
}
