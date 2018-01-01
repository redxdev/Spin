using System;
using System.Collections.Generic;
using System.Text;

namespace Spin.Builder
{
    public interface ILineElement
    {
        string GetStringValue();
    }

    public class StringElement : ILineElement
    {
        public string Value { get; set; }

        public StringElement(string value)
        {
            Value = value;
        }

        public string GetStringValue()
        {
            return Value;
        }
    }

    public class EmptyElement : ILineElement
    {
        public string GetStringValue()
        {
            return string.Empty;
        }
    }
}
