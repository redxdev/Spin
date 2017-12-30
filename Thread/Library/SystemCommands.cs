using System;
using System.Globalization;
using Thread.Attributes;
using Thread.Utility;

namespace Thread.Library
{
    public static class SystemCommands
    {
        [SequenceCommand("s")]
        [SequenceCommand("set")]
        public static void Set(Sequence sequence, object[] arguments)
        {
            ArgumentUtils.Count("set", arguments, 2);
            sequence.SetVariable(Convert.ToString(arguments[0], CultureInfo.InvariantCulture), arguments[1]);
        }
    }
}
