using System;
using System.Collections.Generic;
using System.Text;

namespace Spin.Builder
{
    /// <summary>
    /// Represents a set of text or data in a line.
    /// </summary>
    public interface ILineElement
    {
        /// <summary>
        /// Gets the string value of this element, if there is one.
        /// </summary>
        /// <returns>The string value of this element.</returns>
        string GetStringValue();
    }

    /// <summary>
    /// A string element, which represents plain text.
    /// </summary>
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

    /// <summary>
    /// An empty element, which represents nothing. This may be used as an indicator or separator for other elements.
    /// </summary>
    public class EmptyElement : ILineElement
    {
        public static readonly EmptyElement Instance = new EmptyElement();

        public string GetStringValue()
        {
            return string.Empty;
        }
    }
}
