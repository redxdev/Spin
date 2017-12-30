using System;

namespace Thread
{
    public class SequenceParseException : SequenceException
    {
        public SequenceParseException()
        {
        }

        public SequenceParseException(string message)
            : base(message)
        {
        }

        public SequenceParseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
