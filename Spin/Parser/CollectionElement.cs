using Spin.Utility;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Spin.Parser
{
    public class CollectionElement : IExpressionElement
    {
        public IEnumerable<IExpressionElement> SubElements { get; set; }

        public CollectionElement(IEnumerable<IExpressionElement> subElements)
        {
            SubElements = subElements;
        }

        public void Execute(Sequence sequence, StringBuilder builder)
        {
            if (SubElements != null)
            {
                foreach (var element in SubElements)
                {
                    int initialLength = builder.Length;
                    element.Execute(sequence, builder);

                    if (initialLength != builder.Length)
                        continue;

                    var matcher = new Regex("\\s");
                    while (builder.EndsWith(matcher))
                    {
                        --builder.Length;
                    }
                }
            }
        }
        
        public override string ToString()
        {
            if (SubElements == null)
                return string.Empty;

            var builder = new StringBuilder();
            foreach (var element in SubElements)
            {
                builder.Append(element.ToString());
            }

            return builder.ToString();
        }
    }
}
