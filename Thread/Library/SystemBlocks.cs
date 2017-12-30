using System;
using System.Collections.Generic;
using System.Text;
using Thread.Attributes;
using Thread.Parser;
using Thread.Utility;

namespace Thread.Library
{
    public static class SystemBlocks
    {
        /// <summary>
        /// Hides the output of any sub-expressions. Sub-expressions will still be run.
        /// </summary>
        [SequenceBlock("h")]
        [SequenceBlock("hide")]
        public static void Hide(Sequence sequence, StringBuilder builder, IExpressionElement subElement, string[] arguments)
        {
            ArgumentUtils.Count("hide", arguments, 0);

            var subBuilder = new StringBuilder();
            subElement.Execute(sequence, subBuilder);
        }
    }
}
