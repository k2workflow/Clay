#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Buffers
{
    public static class SpanExtensions
    {
        private const int IntrosortSizeThreshold = 16;

        public static void Sort<T>(this Span<T> span, Comparison<T> comparison)
        {
            if (comparison is null) throw new ArgumentNullException(nameof(comparison));

            // Short-circuit for small N
            var len = (uint)span.Length;
            switch (len)
            {
                // 0 or 1 members
                case 0:
                case 1:
                    return;

                // 2 members
                case 2:
                    SwapIfGreater(span, comparison, 0, 1); // 4,3 => 3,4
                    return;

                // 3 members
                case 3:
                    SwapIfGreater(span, comparison, 0, 1); // 4,3,2 => 3,4,2
                    SwapIfGreater(span, comparison, 1, 2); // 3,4,2 => 3,2,4
                    SwapIfGreater(span, comparison, 0, 1); // 3,2,4 => 2,3,4
                    return;
            }

            // N > 3 due to previous checks
            var limit = BitOps.Log2Floor(len);

            limit <<= 1; // mul 2

            IntrospectiveSort(span, comparison, 0, span.Length - 1, limit);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Swap<T>(Span<T> span, int a, int b)
        {
            var tmp = span[a];
            span[a] = span[b];
            span[b] = tmp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SwapIfGreater<T>(Span<T> span, Comparison<T> comparison, int a, int b)
        {
            if (comparison(span[a], span[b]) > 0)
                Swap(span, a, b);
        }

        private static void IntrospectiveSort<T>(Span<T> span, Comparison<T> comparison, int lo, int hi, int depthLimit)
        {
            while (hi > lo)
            {
                var partitionSize = hi - lo + 1;
                if (partitionSize <= IntrosortSizeThreshold)
                {
                    if (partitionSize == 1) return;

                    if (partitionSize == 2)
                    {
                        SwapIfGreater(span, comparison, lo, hi);
                        return;
                    }

                    if (partitionSize == 3)
                    {
                        SwapIfGreater(span, comparison, lo, hi - 1);
                        SwapIfGreater(span, comparison, lo, hi);
                        SwapIfGreater(span, comparison, hi - 1, hi);
                        return;
                    }

                    InsertionSort(span, comparison, lo, hi);
                    return;
                }

                if (depthLimit == 0)
                {
                    HeapSort(span, comparison, lo, hi);
                    return;
                }
                depthLimit--;

                var p = PickPivot(span, comparison, lo, hi);
                IntrospectiveSort(span, comparison, p + 1, hi, depthLimit);
                hi = p - 1;
            }
        }

        private static void InsertionSort<T>(Span<T> span, Comparison<T> comparison, int lo, int hi)
        {
            for (var i = lo; i < hi; i++)
            {
                var j = i;
                var t = span[i + 1];

                while (j >= lo && comparison(t, span[j]) < 0)
                {
                    span[j + 1] = span[j];
                    j--;
                }

                span[j + 1] = t;
            }
        }

        private static void DownHeap<T>(Span<T> span, Comparison<T> comparison, int i, int n, int lo)
        {
            var d = span[lo + i - 1];

            while (i <= n >> 1) // div 2
            {
                var child = i << 1; // mul 2

                if (child < n && comparison(span[lo + child - 1], span[lo + child]) < 0)
                    child++;

                if (!(comparison(d, span[lo + child - 1]) < 0))
                    break;

                span[lo + i - 1] = span[lo + child - 1];
                i = child;
            }

            span[lo + i - 1] = d;
        }

        private static int PickPivot<T>(Span<T> span, Comparison<T> comparison, int lo, int hi)
        {
            // Compute median-of-three.  But also partition them, since we've done the comparison.
            var mid = lo + ((hi - lo) >> 1); // div 2

            // Sort lo, mid and hi appropriately, then pick mid as the pivot.
            SwapIfGreater(span, comparison, lo, mid);
            SwapIfGreater(span, comparison, lo, hi);
            SwapIfGreater(span, comparison, mid, hi);

            var pivot = span[mid];
            Swap(span, mid, hi - 1);

            // We already partitioned lo and hi and put the pivot in hi - 1.  And we pre-increment & decrement below.
            var left = lo;
            var right = hi - 1;

            while (left < right)
            {
                while (left < (hi - 1) && comparison(span[++left], pivot) < 0) { }
                while (right > lo && comparison(pivot, span[--right]) < 0) { }

                if (left >= right) break;

                Swap(span, left, right);
            }

            // Put pivot in the right location.
            Swap(span, left, hi - 1);
            return left;
        }

        internal static void HeapSort<T>(Span<T> span, Comparison<T> comparison, int lo, int hi)
        {
            var n = hi - lo + 1;
            for (var i = n >> 1; i >= 1; i = i - 1) // div 2
            {
                DownHeap(span, comparison, i, n, lo);
            }
            for (var i = n; i > 1; i = i - 1)
            {
                Swap(span, lo, lo + i - 1);
                DownHeap(span, comparison, 1, i - 1, lo);
            }
        }
    }
}
