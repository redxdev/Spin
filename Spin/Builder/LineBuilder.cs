using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spin.Builder
{
    public class LineBuilder
    {
        private List<ILineElement> _elementStack = new List<ILineElement>();

        public int Count
        {
            get
            {
                return _elementStack.Count;
            }
        }

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

        public void PushEmpty()
        {
            Push(new EmptyElement());
        }

        public void PushString(string value)
        {
            Push(new StringElement(value));
        }

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

        public ILineElement[] GetElements()
        {
            return _elementStack.ToArray();
        }

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
