using System;

namespace Thread.Attributes
{
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
