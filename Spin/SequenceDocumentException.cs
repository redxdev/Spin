using System;
using System.Collections.Generic;
using System.Text;

namespace Spin
{
    public class SequenceDocumentException : SequenceException
    {
        public SequenceDocumentException()
        {
        }

        public SequenceDocumentException(string message)
            : base(message)
        {
        }

        public SequenceDocumentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
