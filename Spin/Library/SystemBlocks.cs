using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Spin.Attributes;
using Spin.Parser;
using Spin.Utility;

namespace Spin.Library
{
    public static class SystemBlocks
    {
        /// <summary>
        /// Hides the output of any sub-expressions. Sub-expressions will still be run.
        /// </summary>
        [SequenceBlock("h")]
        [SequenceBlock("hide")]
        public static void Hide(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("hide", arguments, 0);

            var subBuilder = new StringBuilder();
            subElement.Execute(sequence, subBuilder);
        }

        [SequenceBlock("if")]
        public static void If(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Min("if", arguments, 1);

            foreach (var name in arguments.Select(o => Convert.ToString(o, CultureInfo.InvariantCulture)))
            {
                if (!sequence.TryGetVariable(name, out object value))
                    return;

                if (!Convert.ToBoolean(value, CultureInfo.InvariantCulture))
                    return;
            }

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifnot")]
        public static void IfNot(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Min("ifnot", arguments, 1);

            foreach (var name in arguments.Select(o => Convert.ToString(o, CultureInfo.InvariantCulture)))
            {
                if (!sequence.TryGetVariable(name, out object value))
                    return;

                if (Convert.ToBoolean(value, CultureInfo.InvariantCulture))
                    return;
            }

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifeq")]
        public static void IfEq(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifeq", arguments, 2);
            var values = arguments.Select(name => sequence.GetVariable(Convert.ToString(name, CultureInfo.InvariantCulture))).ToArray();

            if (!values[0].Equals(values[1]))
                return;

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifneq")]
        public static void IfNeq(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifneq", arguments, 2);
            var values = arguments.Select(name => sequence.GetVariable(Convert.ToString(name, CultureInfo.InvariantCulture))).ToArray();

            if (values[0].Equals(values[1]))
                return;

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifset")]
        public static void IfSet(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifset", arguments, 1);
            if (!sequence.ContainsVariable(Convert.ToString(arguments[0], CultureInfo.InvariantCulture)))
                return;

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifunset")]
        public static void IfUnset(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifunset", arguments, 1);
            if (sequence.ContainsVariable(Convert.ToString(arguments[0], CultureInfo.InvariantCulture)))
                return;

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifgt")]
        public static void IfGt(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifgt", arguments, 2);

            object firstValue = arguments[0];
            object secondValue = arguments[1];

            if (firstValue is string)
            {
                firstValue = sequence.GetVariable((string)firstValue);
            }

            if (secondValue is string)
            {
                secondValue = sequence.GetVariable((string)secondValue);
            }

            double firstNum = Convert.ToDouble(firstValue, CultureInfo.InvariantCulture);
            double secondNum = Convert.ToDouble(secondValue, CultureInfo.InvariantCulture);

            if (firstNum > secondNum)
            {
                subElement.Execute(sequence, builder);
            }
        }

        [SequenceBlock("ifgte")]
        public static void IfGte(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifgte", arguments, 2);

            object firstValue = arguments[0];
            object secondValue = arguments[1];

            if (firstValue is string)
            {
                firstValue = sequence.GetVariable((string)firstValue);
            }

            if (secondValue is string)
            {
                secondValue = sequence.GetVariable((string)secondValue);
            }

            double firstNum = Convert.ToDouble(firstValue, CultureInfo.InvariantCulture);
            double secondNum = Convert.ToDouble(secondValue, CultureInfo.InvariantCulture);

            if (firstNum >= secondNum)
            {
                subElement.Execute(sequence, builder);
            }
        }

        [SequenceBlock("iflt")]
        public static void IfLt(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("iflt", arguments, 2);

            object firstValue = arguments[0];
            object secondValue = arguments[1];

            if (firstValue is string)
            {
                firstValue = sequence.GetVariable((string)firstValue);
            }

            if (secondValue is string)
            {
                secondValue = sequence.GetVariable((string)secondValue);
            }

            double firstNum = Convert.ToDouble(firstValue, CultureInfo.InvariantCulture);
            double secondNum = Convert.ToDouble(secondValue, CultureInfo.InvariantCulture);

            if (firstNum < secondNum)
            {
                subElement.Execute(sequence, builder);
            }
        }

        [SequenceBlock("iflte")]
        public static void IfLte(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("iflte", arguments, 2);

            object firstValue = arguments[0];
            object secondValue = arguments[1];

            if (firstValue is string)
            {
                firstValue = sequence.GetVariable((string)firstValue);
            }

            if (secondValue is string)
            {
                secondValue = sequence.GetVariable((string)secondValue);
            }

            double firstNum = Convert.ToDouble(firstValue, CultureInfo.InvariantCulture);
            double secondNum = Convert.ToDouble(secondValue, CultureInfo.InvariantCulture);

            if (firstNum <= secondNum)
            {
                subElement.Execute(sequence, builder);
            }
        }
    }
}
