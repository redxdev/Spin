using System;

namespace Spin
{
    public class SequenceException : Exception
    {
        public SequenceException()
        {
        }

        public SequenceException(string message)
            : base(message)
        {
        }

        public SequenceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
