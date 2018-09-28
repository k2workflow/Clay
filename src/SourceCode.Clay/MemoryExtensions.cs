#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay
{
    /// <summary>
    /// Utility functions for <see cref="Memory{T}"/> and <see cref="Span{T}"/>.
    /// </summary>
    public static class MemoryExtensions
    {
        /// <summary>
        /// Returns the specified number of items from the left of a span.
        /// Tolerates <paramref name="length"/> values that are too large or too small (or negative).
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static ReadOnlySpan<T> Left<T>(this ReadOnlySpan<T> span, int length)
        {
            if (span.Length == 0 || length <= 0) return default;
            if (length >= span.Length) return span;

            ReadOnlySpan<T> slice = span.Slice(0, length);
            return slice;
        }

        /// <summary>
        /// Returns the specified number of items from the left of a memory.
        /// Tolerates <paramref name="length"/> values that are too large or too small (or negative).
        /// </summary>
        /// <param name="memory">The memory.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static ReadOnlyMemory<T> Left<T>(this ReadOnlyMemory<T> memory, int length)
        {
            if (memory.Length == 0 || length <= 0) return default;
            if (length >= memory.Length) return memory;

            ReadOnlyMemory<T> slice = memory.Slice(0, length);
            return slice;
        }

        /// <summary>
        /// Returns the specified number of items from the right of a span.
        /// Tolerates <paramref name="length"/> values that are too large or too small (or negative).
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static ReadOnlySpan<T> Right<T>(this ReadOnlySpan<T> span, int length)
        {
            if (span.Length == 0 || length <= 0) return default;
            if (length >= span.Length) return span;

            ReadOnlySpan<T> slice = span.Slice(span.Length - length);
            return slice;
        }

        /// <summary>
        /// Returns the specified number of items from the right of a memory.
        /// Tolerates <paramref name="length"/> values that are too large or too small (or negative).
        /// </summary>
        /// <param name="memory">The memory.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public static ReadOnlyMemory<T> Right<T>(this ReadOnlyMemory<T> memory, int length)
        {
            if (memory.Length == 0 || length <= 0) return default;
            if (length >= memory.Length) return memory;

            ReadOnlyMemory<T> slice = memory.Slice(memory.Length - length);
            return slice;
        }

        /// <summary>
        /// Returns the index of the first location of the specified <paramref name="char"/>, starting from the given <paramref name="startIndex"/>.
        /// </summary>
        /// <param name="span">The span.</param>
        /// <param name="char">The character to find.</param>
        /// <param name="startIndex">The index to start searching from.</param>
        /// <returns></returns>
        public static int IndexOf(this ReadOnlySpan<char> span, char @char, int startIndex)
        {
            if (span.Length == 0) return -1;

            ReadOnlySpan<char> slice = span.Slice(startIndex);

            var index = slice.IndexOf(@char);
            if (index == -1)
                return -1;

            index = startIndex + index;
            return index;
        }
    }
}
