using System.Text;
using System.Text.RegularExpressions;

namespace Spin.Utility
{
    public static class StringBuilderUtils
    {
        public static bool EndsWith(this StringBuilder builder, char c)
        {
            if (builder.Length == 0)
                return false;

            char[] chars = new char[1];
            builder.CopyTo(builder.Length - 1, chars, 0, 1);
            return chars[0] == c;
        }

        public static bool EndsWith(this StringBuilder builder, Regex match)
        {
            if (builder.Length == 0)
                return false;

            char[] chars = new char[1];
            builder.CopyTo(builder.Length - 1, chars, 0, 1);
            return match.IsMatch(chars[0].ToString());
        }
    }
}
