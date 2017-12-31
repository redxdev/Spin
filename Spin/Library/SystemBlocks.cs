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

            foreach (var value in arguments.Select(o => sequence.Resolve(o)).Select(o => Convert.ToBoolean(o, CultureInfo.InvariantCulture)))
            {
                if (!value)
                    return;
            }

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifnot")]
        public static void IfNot(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Min("ifnot", arguments, 1);

            foreach (var value in arguments.Select(o => Convert.ToBoolean(sequence.Resolve(o), CultureInfo.InvariantCulture)))
            {
                if (value)
                    return;
            }

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifeq")]
        public static void IfEq(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifeq", arguments, 2);
            var values = arguments.Select(o => sequence.Resolve(o)).ToArray();

            if (!values[0].Equals(values[1]))
                return;

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifneq")]
        public static void IfNeq(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifneq", arguments, 2);
            var values = arguments.Select(o => sequence.Resolve(o)).ToArray();

            if (values[0].Equals(values[1]))
                return;

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifset")]
        public static void IfSet(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifset", arguments, 1);
            if (arguments[0] is VariableRef vref)
            {
                if (sequence.ContainsVariable(vref))
                {
                    subElement.Execute(sequence, builder);
                }
            }
            else
            {
                throw new SequenceVariableException($"Expected a variable for {{ifset}}, found {arguments[0]}");
            }
        }

        [SequenceBlock("ifunset")]
        public static void IfUnset(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifunset", arguments, 1);
            if (arguments[0] is VariableRef vref)
            {
                if (!sequence.ContainsVariable(vref))
                {
                    subElement.Execute(sequence, builder);
                }
            }
            else
            {
                throw new SequenceVariableException($"Expected a variable for {{ifunset}}, found {arguments[0]}");
            }
        }

        [SequenceBlock("ifgt")]
        public static void IfGt(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifgt", arguments, 2);

            var firstValue = sequence.Resolve(arguments[0]) as IComparable;
            var secondValue = sequence.Resolve(arguments[1]) as IComparable;

            if (firstValue == null)
                throw new SequenceException($"First argument to {{ifgt}} must implement IComparable");

            if (secondValue == null)
                throw new SequenceException($"Second argument to {{ifgt}} must implement IComparable");

            if (firstValue.CompareTo(secondValue) > 0)
            {
                subElement.Execute(sequence, builder);
            }
        }

        [SequenceBlock("ifgte")]
        public static void IfGte(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifgte", arguments, 2);
            
            var firstValue = sequence.Resolve(arguments[0]) as IComparable;
            var secondValue = sequence.Resolve(arguments[1]) as IComparable;

            if (firstValue == null)
                throw new SequenceException($"First argument to {{ifgte}} must implement IComparable");

            if (secondValue == null)
                throw new SequenceException($"Second argument to {{ifgte}} must implement IComparable");

            if (firstValue.CompareTo(secondValue) >= 0)
            {
                subElement.Execute(sequence, builder);
            }
        }

        [SequenceBlock("iflt")]
        public static void IfLt(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("iflt", arguments, 2);

            var firstValue = sequence.Resolve(arguments[0]) as IComparable;
            var secondValue = sequence.Resolve(arguments[1]) as IComparable;

            if (firstValue == null)
                throw new SequenceException($"First argument to {{iflt}} must implement IComparable");

            if (secondValue == null)
                throw new SequenceException($"Second argument to {{iflt}} must implement IComparable");

            if (firstValue.CompareTo(secondValue) < 0)
            {
                subElement.Execute(sequence, builder);
            }
        }

        [SequenceBlock("iflte")]
        public static void IfLte(Sequence sequence, StringBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("iflt", arguments, 2);

            var firstValue = sequence.Resolve(arguments[0]) as IComparable;
            var secondValue = sequence.Resolve(arguments[1]) as IComparable;

            if (firstValue == null)
                throw new SequenceException($"First argument to {{iflte}} must implement IComparable");

            if (secondValue == null)
                throw new SequenceException($"Second argument to {{iflte}} must implement IComparable");

            if (firstValue.CompareTo(secondValue) <= 0)
            {
                subElement.Execute(sequence, builder);
            }
        }
    }
}
