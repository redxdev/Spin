using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Spin.Attributes;
using Spin.Builder;
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
        public static void Hide(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("hide", arguments, 0);

            var subBuilder = new LineBuilder();
            subElement.Execute(sequence, subBuilder);
        }

        [SequenceBlock("if")]
        public static void If(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Min("if", arguments, 1);

            foreach (var value in arguments.Select(o => sequence.Resolve(o)).Select(o => Convert.ToBoolean(o, CultureInfo.InvariantCulture)))
            {
                if (!value)
                {
                    builder.PushEmpty();
                    return;
                }
            }

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifnot")]
        public static void IfNot(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Min("ifnot", arguments, 1);

            foreach (var value in arguments.Select(o => Convert.ToBoolean(sequence.Resolve(o), CultureInfo.InvariantCulture)))
            {
                if (value)
                {
                    builder.PushEmpty();
                    return;
                }
            }

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifeq")]
        public static void IfEq(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifeq", arguments, 2);
            var values = arguments.Select(o => sequence.Resolve(o)).ToArray();

            if (!values[0].Equals(values[1]))
            {
                builder.PushEmpty();
                return;
            }

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifneq")]
        public static void IfNeq(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifneq", arguments, 2);
            var values = arguments.Select(o => sequence.Resolve(o)).ToArray();

            if (values[0].Equals(values[1]))
            {
                builder.PushEmpty();
                return;
            }

            subElement.Execute(sequence, builder);
        }

        [SequenceBlock("ifset")]
        public static void IfSet(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifset", arguments, 1);
            if (arguments[0] is VariableRef vref)
            {
                if (sequence.ContainsVariable(vref))
                {
                    subElement.Execute(sequence, builder);
                }
                else
                {
                    builder.PushEmpty();
                }
            }
            else
            {
                throw new SequenceVariableException($"Expected a variable for {{ifset}}, found {arguments[0]}");
            }
        }

        [SequenceBlock("ifunset")]
        public static void IfUnset(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
        {
            ArgumentUtils.Count("ifunset", arguments, 1);
            if (arguments[0] is VariableRef vref)
            {
                if (!sequence.ContainsVariable(vref))
                {
                    subElement.Execute(sequence, builder);
                }
                else
                {
                    builder.PushEmpty();
                }
            }
            else
            {
                throw new SequenceVariableException($"Expected a variable for {{ifunset}}, found {arguments[0]}");
            }
        }

        [SequenceBlock("ifgt")]
        public static void IfGt(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
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
            else
            {
                builder.PushEmpty();
            }
        }

        [SequenceBlock("ifgte")]
        public static void IfGte(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
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
            else
            {
                builder.PushEmpty();
            }
        }

        [SequenceBlock("iflt")]
        public static void IfLt(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
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
            else
            {
                builder.PushEmpty();
            }
        }

        [SequenceBlock("iflte")]
        public static void IfLte(Sequence sequence, LineBuilder builder, IExpressionElement subElement, object[] arguments)
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
            else
            {
                builder.PushEmpty();
            }
        }
    }
}
