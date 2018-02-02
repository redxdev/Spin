using System;

namespace Spin.Attributes
{
    /// <summary>
    /// Marks a method as being usable as a command in a sequence. Must adhere to <see cref="Sequence.CommandFunction"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class SequenceCommandAttribute : Attribute
    {
        public string Name { get; set; }

        public SequenceCommandAttribute(string name)
        {
            Name = name;
        }
    }
}
