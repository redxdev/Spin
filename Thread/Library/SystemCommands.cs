using Thread.Attributes;
using Thread.Utility;

namespace Thread.Library
{
    public static class SystemCommands
    {
        [SequenceCommand("s")]
        [SequenceCommand("set")]
        public static void Set(Sequence sequence, string[] arguments)
        {
            ArgumentUtils.Count("set", arguments, 2);
            sequence.SetVariable(arguments[0], arguments[1]);
        }
    }
}
