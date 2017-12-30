using System;

namespace Thread.Utility
{
    public static class ArgumentUtils
    {
        public static void Min(string name, object[] args, int min)
        {
            if (min == 0)
                return;

            if (args == null || args.Length < min)
                throw new SequenceArgumentException($"Expected {min} or more arguments for {name}, got {args?.Length ?? 0}");
        }

        public static void Max(string name, object[] args, int max)
        {
            if (args != null && args.Length > max)
                throw new SequenceArgumentException($"Expected at most {max} arguments for {name}, got {args.Length}");
        }

        public static void Between(string name, object[] args, int min, int max)
        {
            if (max <= min)
                throw new ArgumentException($"Expected min < max, got {min} and {max}");

            if (min == 0 && args == null)
                return;

            if (args == null || args.Length < min || args.Length > max)
                throw new SequenceArgumentException($"Expected between {min} and {max} arguments for {name}, got {args?.Length ?? 0}");
        }

        public static void Count(string name, object[] args, int count)
        {
            if (count == 0 && args == null)
                return;

            if (args.Length != count)
                throw new SequenceArgumentException($"Expected exactly {count} arguments for {name}, got {args?.Length ?? 0}");
        }
    }
}
