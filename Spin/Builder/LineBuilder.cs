using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spin.Builder
{
    /// <summary>
    /// Represents a single "line" in a sequence.
    /// </summary>
    /// <remarks>
    /// A <see cref="LineBuilder"/> is passed around by a sequence in order to construct a line. The basic unit it works with is a
    /// <see cref="ILineElement"/>, which can be used to insert complex data inside a line.
    /// </remarks>
    public class LineBuilder
    {
        private List<ILineElement> _elementStack = new List<ILineElement>();

        /// <summary>
        /// The number of elements in this builder.
        /// </summary>
        /// <remarks>
        /// This is not the number of characters contained by this builder, but the number of <see cref="ILineElement"/> objects,
        /// each of which may (and likely will) have more than a single character.
        /// </remarks>
        public int Count
        {
            get
            {
                return _elementStack.Count;
            }
        }

        /// <summary>
        /// Append a <see cref="ILineElement"/> to the end of the builder.
        /// </summary>
        /// <param name="element">The element to append.</param>
        public void Push(ILineElement element)
        {
            // If this is an empty element, we want to consume all earlier whitespace until
            // we hit something that's not whitespace.
            if (element is EmptyElement)
            {
                while (_elementStack.Count > 0)
                {
                    var el = _elementStack[_elementStack.Count - 1];
                    if (el is StringElement str)
                    {
                        if (string.IsNullOrWhiteSpace(str.GetStringValue()))
                        {
                            _elementStack.RemoveAt(_elementStack.Count - 1);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            _elementStack.Add(element);
        }

        /// <summary>
        /// Append an empty element.
        /// </summary>
        public void PushEmpty()
        {
            Push(EmptyElement.Instance);
        }

        /// <summary>
        /// Append a string.
        /// </summary>
        /// <param name="value">The string to append.</param>
        public void PushString(string value)
        {
            Push(new StringElement(value));
        }

        /// <summary>
        /// Append all elements from another builder into this one.
        /// </summary>
        /// <param name="other">The other builder.</param>
        /// <param name="pushEmpty">When true and the other builder is empty, this will push an empty element instead of nothing.</param>
        public void PushBuilder(LineBuilder other, bool pushEmpty = true)
        {
            if (pushEmpty && other._elementStack.Count == 0)
            {
                PushEmpty();
                return;
            }

            foreach (var el in other._elementStack)
            {
                Push(el);
            }
        }

        /// <summary>
        /// Get the list of elements this builder contains.
        /// </summary>
        /// <returns>The list of elements.</returns>
        public ILineElement[] GetElements()
        {
            return _elementStack.ToArray();
        }

        /// <summary>
        /// Convert the contained elements into a string.
        /// </summary>
        /// <param name="trimResult">When true, whitespace at the beginning and end of the string will be removed.</param>
        /// <param name="trimLineBreaks">When true, single line breaks will be ignored.</param>
        /// <returns>The string representation of the contained elements.</returns>
        public string BuildString(bool trimResult = true, bool trimLineBreaks = true)
        {
            var builder = new StringBuilder();
            foreach (var el in _elementStack)
            {
                builder.Append(el.GetStringValue());
            }

            var result = builder.ToString();
            if (trimResult)
            {
                result = result.Trim();
            }

            if (trimLineBreaks)
            {
                result = string.Join("", result.Split('\n')
                    .Select(line =>
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            return line.Trim() + " ";
                        }

                        return "\n";
                    }));
            }

            return result;
        }
    }
}
