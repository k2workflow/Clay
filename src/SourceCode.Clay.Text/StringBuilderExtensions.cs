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

        //public static StringBuilder AppendBuilder(this StringBuilder x, StringBuilder y)
        //{
        //    if (x == null || x.Length == 0) return y;
        //    if (y == null || y.Length == 0) return x;

        //    var len = x.Length + y.Length;

        //    StringBuilder concat;
        //    if (len <= StringBuilderCache.Max)
        //    {
        //        // Get a fresh/cached builder
        //        concat = StringBuilderCache.Acquire(len);

        //        // Concat chars from @x
        //        for (var i = 0; i < x.Length; i++)
        //            concat.Append(x[i]);
        //    }
        //    else
        //    {
        //        // Minimize iterative allocations
        //        x.EnsureCapacity(len);
        //        concat = x;
        //    }

        //    // Concat chars from @y
        //    for (var i = 0; i < y.Length; i++)
        //        concat.Append(y[i]);

        //    return concat;
        //}

        //public static StringBuilder AppendBuilderLine(this StringBuilder x, StringBuilder y)
        //{
        //    var concat = x.AppendBuilder(y);

        //    return concat?.AppendLine();
        //}
    }
}
