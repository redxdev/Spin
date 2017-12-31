using System;

namespace Spin
{
    public class SequenceArgumentException : SequenceException
    {
        public SequenceArgumentException()
        {
        }

        public SequenceArgumentException(string message)
            : base(message)
        {
        }

        public SequenceArgumentException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
