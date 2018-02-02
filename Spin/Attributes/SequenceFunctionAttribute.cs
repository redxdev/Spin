using System;

namespace Spin.Attributes
{
    /// <summary>
    /// Marks a method as being usable as a function in a sequence. Must adhere to <see cref="Sequence.ExpressionFunction"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class SequenceFunctionAttribute : Attribute
    {
        public string Name { get; set; }

        public SequenceFunctionAttribute(string name)
        {
            Name = name;
        }
    }
}
