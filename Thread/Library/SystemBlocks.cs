using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Thread.Attributes;
using Thread.Parser;
using Thread.Utility;

namespace Thread.Library
{
    public static class SystemBlocks
    {
        /// <summary>
        /// Hides the output of any sub-expressions. Sub-expressions will still be run.
        /// </summary>
        [SequenceBlock("h")]
        [SequenceBlock("hide")]
        public static void Hide(Sequence sequence, StringBuilder builder, IExpressionElement subElement, string[] arguments)
        {
            ArgumentUtils.Count("hide", arguments, 0);

            var subBuilder = new StringBuilder();
            subElement.Execute(sequence, subBuilder);
        }

        [SequenceBlock("if")]
        public static void If(Sequence sequence, StringBuilder builder, IExpressionElement subElement, string[] arguments)
        {
            ArgumentUtils.Min("if", arguments, 1);

            foreach (var name in arguments)
            {
                var value = Convert.ToBoolean(sequence.GetVariable(name), CultureInfo.InvariantCulture);
                if (!value)
                    return;
            }

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifnot")]
        public static void IfNot(Sequence sequence, StringBuilder builder, IExpressionElement subElement, string[] arguments)
        {
            ArgumentUtils.Min("ifnot", arguments, 1);

            foreach (var name in arguments)
            {
                var value = Convert.ToBoolean(sequence.GetVariable(name), CultureInfo.InvariantCulture);
                if (value)
                    return;
            }

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifeq")]
        public static void IfEq(Sequence sequence, StringBuilder builder, IExpressionElement subElement, string[] arguments)
        {
            ArgumentUtils.Count("ifeq", arguments, 2);
            var values = arguments.Select(name => sequence.GetVariable(name)).ToArray();

            if (values[0] != values[1])
                return;

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifneq")]
        public static void IfNeq(Sequence sequence, StringBuilder builder, IExpressionElement subElement, string[] arguments)
        {
            ArgumentUtils.Count("ifneq", arguments, 2);
            var values = arguments.Select(name => sequence.GetVariable(name)).ToArray();

            if (values[0] == values[1])
                return;

            subElement.Execute(sequence, builder);
        }
    }
}
