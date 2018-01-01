using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Spin.Attributes;
using Spin.Builder;
using Spin.Utility;

namespace Spin.Library
{
    public static class SystemFunctions
    {
        [SequenceFunction("v")]
        [SequenceFunction("value")]
        [SequenceFunction("e")]
        [SequenceFunction("echo")]
        public static void Value(Sequence sequence, LineBuilder builder, object[] arguments)
        {
            ArgumentUtils.Min("value", arguments, 1);
            foreach (var arg in arguments.Select(o => sequence.Resolve(o)))
            {
                builder.PushString(Convert.ToString(arg, CultureInfo.InvariantCulture));
            }
        }

        [SequenceFunction("s")]
        [SequenceFunction("set")]
        public static void Set(Sequence sequence, LineBuilder builder, object[] arguments)
        {
            ArgumentUtils.Count("set", arguments, 2);
            if (arguments[0] is VariableRef vref)
            {
                sequence.SetVariable(vref, sequence.Resolve(arguments[1]));
            }
            else
            {
                throw new SequenceVariableException($"Expected a variable for {{{{set}}}}, found {arguments[0]}");
            }

            builder.PushEmpty();
        }

        /// <summary>
        /// Runs a command
        /// </summary>
        [SequenceFunction("c")]
        [SequenceFunction("cmd")]
        public static void Command(Sequence sequence, LineBuilder builder, object[] arguments)
        {
            ArgumentUtils.Min("cmd", arguments, 1);
            sequence.ExecuteCommand(Convert.ToString(sequence.Resolve(arguments[0]), CultureInfo.InvariantCulture), arguments.ToList().GetRange(1, arguments.Length - 1).ToArray());
            builder.PushEmpty();
        }

        [SequenceFunction("noop")]
        public static void Noop(Sequence sequence, LineBuilder builder, object[] arguments)
        {
            ArgumentUtils.Count("noop", arguments, 0);
            builder.PushEmpty();
        }
    }
}
