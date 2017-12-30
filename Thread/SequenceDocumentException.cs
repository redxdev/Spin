using System;
using System.Collections.Generic;
using System.Text;

namespace Thread
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
