using System;

namespace Spin.Attributes
{
    /// <summary>
    /// Marks a method as being usable as a block in a sequence. Method must adhere to <see cref="Sequence.ExpressionBlock"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class SequenceBlockAttribute : Attribute
    {
        public string Name { get; set; }

        public SequenceBlockAttribute(string name)
        {
            Name = name;
        }
    }
}
