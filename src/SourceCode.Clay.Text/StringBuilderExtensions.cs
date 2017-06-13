using System;
using System.Text;

namespace SourceCode.Clay.Text
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendFormatLine(this StringBuilder sb, string format, object arg0)
            => sb?.AppendFormat(format, arg0).AppendLine();

        public static StringBuilder AppendFormatLine(this StringBuilder sb, string format, object arg0, object arg1)
            => sb?.AppendFormat(format, arg0, arg1).AppendLine();

        public static StringBuilder AppendFormatLine(this StringBuilder sb, string format, object arg0, object arg1, object arg3)
            => sb?.AppendFormat(format, arg0, arg1, arg3).AppendLine();

        public static StringBuilder AppendFormatLine(this StringBuilder sb, string format, params object[] args)
            => sb?.AppendFormat(format, args).AppendLine();

        public static StringBuilder AppendFormatLine(this StringBuilder sb, IFormatProvider provider, string format, object arg0)
            => sb?.AppendFormat(provider, format, arg0).AppendLine();

        public static StringBuilder AppendFormatLine(this StringBuilder sb, IFormatProvider provider, string format, object arg0, object arg1)
            => sb?.AppendFormat(provider, format, arg0, arg1).AppendLine();

        public static StringBuilder AppendFormatLine(this StringBuilder sb, IFormatProvider provider, string format, object arg0, object arg1, object arg3)
            => sb?.AppendFormat(provider, format, arg0, arg1, arg3).AppendLine();

        public static StringBuilder AppendFormatLine(this StringBuilder sb, IFormatProvider provider, string format, params object[] args)
            => sb?.AppendFormat(provider, format, args).AppendLine();

        public static StringBuilder AppendBuilder(this StringBuilder sb, StringBuilder arg)
        {
            if (sb == null || sb.Length == 0) return arg;
            if (arg == null || arg.Length == 0) return sb;

            // Minimize iterative reallocations
            var len = sb.Length + arg.Length;
            sb.EnsureCapacity(len);

            // Concat chars from @y
            for (var i = 0; i < arg.Length; i++)
                sb.Append(arg[i]);

            return sb;
        }

        public static StringBuilder AppendBuilderLine(this StringBuilder sb, StringBuilder arg)
        {
            var concat = sb.AppendBuilder(arg);

            return concat?.AppendLine();
        }
    }
}
