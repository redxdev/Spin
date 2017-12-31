using System;
using System.Globalization;
using Spin.Attributes;
using Spin.Utility;

namespace Spin.Library
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

        [SequenceCommand("next")]
        [SequenceCommand("begin")]
        public static void Next(Sequence sequence, object[] arguments)
        {
            ArgumentUtils.Count("next", arguments, 1);
            sequence.SetNextLine(Convert.ToString(arguments[0], CultureInfo.InvariantCulture));
        }

        [SequenceCommand("skip")]
        public static void Skip(Sequence sequence, object[] arguments)
        {
            ArgumentUtils.Count("skip", arguments, 0);
            sequence.StartNextLine();
            sequence.ExecuteCurrentLine();
        }

        [SequenceCommand("seq")]
        [SequenceCommand("sequence")]
        public static void Sequence(Sequence sequence, object[] arguments)
        {
            ArgumentUtils.Count("sequence", arguments, 1);
            sequence.LoadAndStartDocument(Convert.ToString(arguments[0], CultureInfo.InvariantCulture));
        }

        [SequenceCommand("reset")]
        public static void Reset(Sequence sequence, object[] arguments)
        {
            ArgumentUtils.Count("reset", arguments, 0);
            sequence.ResetVariables();
        }
    }
}
