﻿using System;

namespace Spin
{
    public class SequenceVariableException : SequenceException
    {
        public SequenceVariableException()
        {
        }

        public SequenceVariableException(string message)
            : base(message)
        {
        }

        public SequenceVariableException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
