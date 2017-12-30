using System;

namespace Thread.Attributes
{
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
