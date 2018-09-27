#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents extensions for <see cref="ReadOnlyMemory{T}"/>.
    /// </summary>
    public static class MemoryExtensions
    {
        /// <summary>
        /// Performs an optimized item-by-item comparison, using a custom <see cref="IEqualityComparer{T}"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="x">Memory 1</param>
        /// <param name="y">Memory 2</param>
        /// <param name="comparer">The comparer to use to test for equality.</param>
        /// <returns></returns>
        public static bool MemoryEquals<TSource>(this ReadOnlyMemory<TSource> x, in ReadOnlyMemory<TSource> y, in IEqualityComparer<TSource> comparer)
        {
            if (x.Length != y.Length) return false; // (n, m)
            if (x.Length == 0) return true; // (0, 0)

            IEqualityComparer<TSource> cmpr = comparer ?? EqualityComparer<TSource>.Default;

            // Memory.Span throws if IsEmpty == true
            ReadOnlySpan<TSource> xs = x.Span;
            ReadOnlySpan<TSource> ys = y.Span;

            // Check items in sequential order
            for (var i = 0; i < xs.Length; i++)
                if (!cmpr.Equals(xs[i], ys[i]))
                    return false;

            return true;
        }

        /// <summary>
        /// Performs an optimized item-by-item comparison, using the default comparer for the type.
        /// </summary>
        /// <typeparam name="TSource">The type of items.</typeparam>
        /// <param name="x">Memory 1</param>
        /// <param name="y">Memory 2</param>
        /// <returns></returns>
        public static bool MemoryEquals<TSource>(this ReadOnlyMemory<TSource> x, in ReadOnlyMemory<TSource> y)
            => MemoryEquals(x, y, null);
    }
}
