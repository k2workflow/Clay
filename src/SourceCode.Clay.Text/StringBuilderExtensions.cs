using System;
using System.Text;

namespace SourceCode.Clay.Text
{
    /// <summary>
    /// Represents <see cref="System.Text.StringBuilder"/> extensions.
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// Appends a formatted string (using zero or more format items) to this instance,
        /// followed by the default line terminator.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> instance to append to.</param>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first format argument.</param>
        /// <returns>A reference to this instance with format appended.</returns>
        public static StringBuilder AppendFormatLine(this StringBuilder sb, string format, object arg0)
            => sb?
            .AppendFormat(format, arg0)
            .AppendLine();

        /// <summary>
        /// Appends a formatted string (using zero or more format items) to this instance,
        /// followed by the default line terminator.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> instance to append to.</param>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first format argument.</param>
        /// <param name="arg1">The second format argument.</param>
        /// <returns>A reference to this instance with format appended.</returns>
        public static StringBuilder AppendFormatLine(this StringBuilder sb, string format, object arg0, object arg1)
            => sb?
            .AppendFormat(format, arg0, arg1)
            .AppendLine();

        /// <summary>
        /// Appends a formatted string (using zero or more format items) to this instance,
        /// followed by the default line terminator.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> instance to append to.</param>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first format argument.</param>
        /// <param name="arg1">The second format argument.</param>
        /// <param name="arg2">The second format argument.</param>
        /// <returns>A reference to this instance with format appended.</returns>
        public static StringBuilder AppendFormatLine(this StringBuilder sb, string format, object arg0, object arg1, object arg2)
            => sb?
            .AppendFormat(format, arg0, arg1, arg2)
            .AppendLine();

        /// <summary>
        /// Appends a formatted string (using zero or more format items) to this instance,
        /// followed by the default line terminator.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> instance to append to.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The format arguments.</param>
        /// <returns>A reference to this instance with format appended.</returns>
        public static StringBuilder AppendFormatLine(this StringBuilder sb, string format, params object[] args)
            => sb?
            .AppendFormat(format, args)
            .AppendLine();

        /// <summary>
        /// Appends a formatted string (using zero or more format items) to this instance,
        /// followed by the default line terminator.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> instance to append to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first format argument.</param>
        /// <returns>A reference to this instance with format appended.</returns>
        public static StringBuilder AppendFormatLine(this StringBuilder sb, IFormatProvider provider, string format, object arg0)
            => sb?
            .AppendFormat(provider, format, arg0)
            .AppendLine();

        /// <summary>
        /// Appends a formatted string (using zero or more format items) to this instance,
        /// followed by the default line terminator.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> instance to append to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first format argument.</param>
        /// <param name="arg1">The second format argument.</param>
        /// <returns>A reference to this instance with format appended.</returns>
        public static StringBuilder AppendFormatLine(this StringBuilder sb, IFormatProvider provider, string format, object arg0, object arg1)
            => sb?
            .AppendFormat(provider, format, arg0, arg1)
            .AppendLine();

        /// <summary>
        /// Appends a formatted string (using zero or more format items) to this instance,
        /// followed by the default line terminator.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> instance to append to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">The format string.</param>
        /// <param name="arg0">The first format argument.</param>
        /// <param name="arg1">The second format argument.</param>
        /// <param name="arg2">The second format argument.</param>
        /// <returns>A reference to this instance with format appended.</returns>
        public static StringBuilder AppendFormatLine(this StringBuilder sb, IFormatProvider provider, string format, object arg0, object arg1, object arg2)
            => sb?
            .AppendFormat(provider, format, arg0, arg1, arg2)
            .AppendLine();

        /// <summary>
        /// Appends a formatted string (using zero or more format items) to this instance,
        /// followed by the default line terminator.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> instance to append to.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <param name="format">The format string.</param>
        /// <param name="args">The format arguments.</param>
        /// <returns>A reference to this instance with format appended.</returns>
        public static StringBuilder AppendFormatLine(this StringBuilder sb, IFormatProvider provider, string format, params object[] args)
            => sb?
            .AppendFormat(provider, format, args)
            .AppendLine();

        /// <summary>
        /// Appends two <see cref="StringBuilder"/> instances.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> instance to append to.</param>
        /// <param name="arg">The <see cref="StringBuilder"/> instance that should be appended to the first.</param>
        /// <returns>The initial instance, with the second instance appended to it.</returns>
        /// <exception cref="System.ArgumentNullException">sb</exception>
        public static StringBuilder AppendBuilder(this StringBuilder sb, StringBuilder arg)
        {
            if (sb == null) throw new ArgumentNullException(nameof(sb));
            if (arg == null || arg.Length == 0) return sb;

            // Minimize iterative reallocations
            var len = sb.Length + arg.Length;
            sb.EnsureCapacity(len);

            // Concat chars from second builder (loop is cheaper than bouncing via ToString)
            for (var i = 0; i < arg.Length; i++)
                sb.Append(arg[i]);

            return sb;
        }

        /// <summary>
        /// Appends two <see cref="StringBuilder"/> instances.
        /// </summary>
        /// <param name="sb">The <see cref="StringBuilder"/> instance to append to.</param>
        /// <param name="arg">The <see cref="StringBuilder"/> instance that should be appended to the first.</param>
        /// <returns>The initial instance, with the second instance appended to it.</returns>
        /// <exception cref="System.ArgumentNullException">sb</exception>
        public static StringBuilder AppendBuilderLine(this StringBuilder sb, StringBuilder arg)
        {
            if (sb == null) throw new ArgumentNullException(nameof(sb));

            var concat = sb.AppendBuilder(arg);
            concat.AppendLine();

            return concat;
        }
    }
}
