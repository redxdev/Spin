using System;

namespace Thread.Attributes
{
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
