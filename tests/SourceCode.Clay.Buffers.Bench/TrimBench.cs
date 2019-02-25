#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace SourceCode.Clay.Buffers.Bench
{
    /*
    */

    //[MemoryDiagnoser]
    public class TrimBench
    {
        private const int _iterations = 100000;
        private const int N = 1;

        public Memory<byte> Data = new byte[]
        {
            10, 10, 10, 10, 10, 9, 10, 8,
            123, 231, 124, 241, 125, 251, 126, 162,
            9, 10, 7, 10, 8, 10, 10, 10
        };

        //[Benchmark(Baseline = true, OperationsPerInvoke = _iterations * N)]
        //public long SingleViaNone()
        //{
        //    long sum = 0;

        //    Span<byte> span = Data.Span;
        //    for (uint n = 0; n < N; n++)
        //    {
        //        sum += TrimViaNone(span, (byte)10).Length;
        //    }

        //    return sum;
        //}

        //[Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        //public long SingleViaIn()
        //{
        //    long sum = 0;

        //    Span<byte> span = Data.Span;
        //    for (uint n = 0; n < N; n++)
        //    {
        //        sum += TrimViaIn(span, (byte)10).Length;
        //    }

        //    return sum;
        //}

        //[Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        //public long SingleViaRef()
        //{
        //    long sum = 0;

        //    Span<byte> span = Data.Span;
        //    for (uint n = 0; n < N; n++)
        //    {
        //        sum += TrimViaRef(span, (byte)10).Length;
        //    }

        //    return sum;
        //}

        [Benchmark(Baseline = true, OperationsPerInvoke = _iterations * N)]
        public long SingleNoGuard()
        {
            long sum = 0;

            Span<byte> span = Data.Span;
            for (uint n = 0; n < N; n++)
            {
                sum += TrimNoGuard<byte>(span, Array.Empty<byte>()).Length;
                sum += TrimNoGuard<byte>(span, new byte[] { 10 }).Length;
                sum += TrimNoGuard<byte>(span, new byte[] { 10, 9, 8 }).Length;
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public long SingleViaSwitch()
        {
            long sum = 0;

            Span<byte> span = Data.Span;
            for (uint n = 0; n < N; n++)
            {
                sum += TrimViaSwitch<byte>(span, Array.Empty<byte>()).Length;
                sum += TrimViaSwitch<byte>(span, new byte[] { 10 }).Length;
                sum += TrimViaSwitch<byte>(span, new byte[] { 10, 9, 8 }).Length;
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public long SingleViaIf()
        {
            long sum = 0;

            Span<byte> span = Data.Span;
            for (uint n = 0; n < N; n++)
            {
                sum += TrimViaIf<byte>(span, Array.Empty<byte>()).Length;
                sum += TrimViaIf<byte>(span, new byte[] { 10 }).Length;
                sum += TrimViaIf<byte>(span, new byte[] { 10, 9, 8 }).Length;
            }

            return sum;
        }

        [Benchmark(Baseline = false, OperationsPerInvoke = _iterations * N)]
        public long SingleViaNestedIf()
        {
            long sum = 0;

            Span<byte> span = Data.Span;
            for (uint n = 0; n < N; n++)
            {
                sum += TrimViaNestedIf<byte>(span, Array.Empty<byte>()).Length;
                sum += TrimViaNestedIf<byte>(span, new byte[] { 10 }).Length;
                sum += TrimViaNestedIf<byte>(span, new byte[] { 10, 9, 8 }).Length;
            }

            return sum;
        }

        #region Via None

        /// <summary>
        /// Removes all leading and trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> TrimViaNone<T>(ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampViaNoneStart(span, trimElement);
            int length = ClampViaNoneEnd(span, start, trimElement);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> TrimViaNoneStart<T>(ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampViaNoneStart(span, trimElement);
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> TrimViaNoneEnd<T>(ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int length = ClampViaNoneEnd(span, 0, trimElement);
            return span.Slice(0, length);
        }

        /// <summary>
        /// Delimits all leading occurrences of a specified element.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        private static int ClampViaNoneStart<T>(ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = 0;

            if (trimElement == null)
            {
                for (; start < span.Length; start++)
                {
                    if (span[start] != null)
                        break;
                }
            }
            else
            {
                for (; start < span.Length; start++)
                {
                    if (!trimElement.Equals(span[start]))
                        break;
                }
            }

            return start;
        }

        /// <summary>
        /// Delimits all trailing occurrences of a specified element.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="start">The start index from which to being searching.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        private static int ClampViaNoneEnd<T>(ReadOnlySpan<T> span, int start, T trimElement)
            where T : IEquatable<T>
        {
            // start==len==0, start<len, start==len after ClampStart trims all
            Debug.Assert((uint)start < span.Length);

            int end = span.Length - 1;

            if (trimElement == null)
            {
                for (; end >= start; end--)
                {
                    if (span[end] != null)
                        break;
                }
            }
            else
            {
                for (; end >= start; end--)
                {
                    if (!trimElement.Equals(span[start]))
                        break;
                }
            }

            return end - start + 1;
        }

        #endregion

        #region Via In

        /// <summary>
        /// Removes all leading and trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> TrimViaIn<T>(ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampViaInStart(in span, trimElement);
            int length = ClampViaInEnd(in span, start, trimElement);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> TrimViaInStart<T>(ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampViaInStart(in span, trimElement);
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> TrimViaInEnd<T>(ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int length = ClampViaInEnd(in span, 0, trimElement);
            return span.Slice(0, length);
        }

        /// <summary>
        /// Delimits all leading occurrences of a specified element.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        private static int ClampViaInStart<T>(in ReadOnlySpan<T> span, in T trimElement)
            where T : IEquatable<T>
        {
            int start = 0;

            if (trimElement == null)
            {
                for (; start < span.Length; start++)
                {
                    if (span[start] != null)
                        break;
                }
            }
            else
            {
                for (; start < span.Length; start++)
                {
                    if (!trimElement.Equals(span[start]))
                        break;
                }
            }

            return start;
        }

        /// <summary>
        /// Delimits all trailing occurrences of a specified element.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="start">The start index from which to being searching.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        private static int ClampViaInEnd<T>(in ReadOnlySpan<T> span, int start, in T trimElement)
            where T : IEquatable<T>
        {
            // start==len==0, start<len, start==len after ClampStart trims all
            Debug.Assert((uint)start < span.Length);

            int end = span.Length - 1;

            if (trimElement == null)
            {
                for (; end >= start; end--)
                {
                    if (span[end] != null)
                        break;
                }
            }
            else
            {
                for (; end >= start; end--)
                {
                    if (!trimElement.Equals(span[start]))
                        break;
                }
            }

            return end - start + 1;
        }

        #endregion

        #region Via Ref

        /// <summary>
        /// Removes all leading and trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> TrimViaRef<T>(ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampViaRefStart(ref span, ref trimElement);
            int length = ClampViaRefEnd(ref span, start, ref trimElement);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> TrimViaRefStart<T>(ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int start = ClampViaRefStart(ref span, ref trimElement);
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a specified element.
        /// </summary>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        public static ReadOnlySpan<T> TrimViaRefEnd<T>(ReadOnlySpan<T> span, T trimElement)
            where T : IEquatable<T>
        {
            int length = ClampViaRefEnd(ref span, 0, ref trimElement);
            return span.Slice(0, length);
        }

        /// <summary>
        /// Delimits all leading occurrences of a specified element.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        private static int ClampViaRefStart<T>(ref ReadOnlySpan<T> span, ref T trimElement)
            where T : IEquatable<T>
        {
            int start = 0;

            if (trimElement == null)
            {
                for (; start < span.Length; start++)
                {
                    if (span[start] != null)
                        break;
                }
            }
            else
            {
                for (; start < span.Length; start++)
                {
                    if (!trimElement.Equals(span[start]))
                        break;
                }
            }

            return start;
        }

        /// <summary>
        /// Delimits all trailing occurrences of a specified element.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="start">The start index from which to being searching.</param>
        /// <param name="trimElement">The specified element to look for and remove.</param>
        private static int ClampViaRefEnd<T>(ref ReadOnlySpan<T> span, int start, ref T trimElement)
            where T : IEquatable<T>
        {
            // start==len==0, start<len, start==len after ClampStart trims all
            Debug.Assert((uint)start < span.Length);

            int end = span.Length - 1;

            if (trimElement == null)
            {
                for (; end >= start; end--)
                {
                    if (span[end] != null)
                        break;
                }
            }
            else
            {
                for (; end >= start; end--)
                {
                    if (!trimElement.Equals(span[start]))
                        break;
                }
            }

            return end - start + 1;
        }

        #endregion

        #region Multiple with no guard

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimNoGuard<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            int start = ClampStart(span, trimElements);
            int length = ClampEnd(span, start, trimElements);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimNoGuardStart<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            int start = ClampStart(span, trimElements);
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimNoGuardEnd<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            int length = ClampEnd(span, 0, trimElements);
            return span.Slice(0, length);
        }

        /// <summary>
        /// Delimits all leading occurrences of a specified element.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        private static int ClampStart<T>(in ReadOnlySpan<T> span, in ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            int start = 0;

            for (; start < span.Length; start++)
            {
                if (!SequentialContains(trimElements, span[start]))
                    break;
            }

            return start;
        }

        /// <summary>
        /// Delimits all trailing occurrences of a specified element.
        /// </summary>
        /// <param name="span">The source span from which the element is removed.</param>
        /// <param name="start">The start index from which to being searching.</param>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        private static int ClampEnd<T>(in ReadOnlySpan<T> span, int start, in ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            // start==len==0, start<len, start==len after ClampStart trims all
            Debug.Assert((uint)start < span.Length);

            int end = span.Length - 1;

            for (; end >= start; end--)
            {
                if (!SequentialContains(trimElements, span[start]))
                    break;
            }

            return end - start + 1;
        }

        /// <summary>
        /// Scans for a specified item in the provided collection, returning true if found, else false.
        /// Optimized for a small number of elements.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <param name="item">The item to try find.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool SequentialContains<T>(in ReadOnlySpan<T> trimElements, in T item)
            where T : IEquatable<T>
        {
            // Non-vectorized scan is optimized for a small number of elements
            for (int i = 0; i < trimElements.Length; i++)
            {
                if (trimElements[i] == null)
                {
                    if (item == null)
                        return true;
                }
                else if (trimElements[i].Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region Multiple via switch

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimViaSwitch<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            switch (trimElements.Length)
            {
                case 0:
                    return span;
                case 1:
                    return TrimViaIn(span, trimElements[0]);
            }

            int start = ClampStart(span, trimElements);
            int length = ClampEnd(span, start, trimElements);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimViaSwitchStart<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            switch (trimElements.Length)
            {
                case 0:
                    return span;
                case 1:
                    return TrimViaInStart(span, trimElements[0]);
            }

            int start = ClampStart(span, trimElements);
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimViaSwitchEnd<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            switch (trimElements.Length)
            {
                case 0:
                    return span;
                case 1:
                    return TrimViaInEnd(span, trimElements[0]);
            }

            int length = ClampEnd(span, 0, trimElements);
            return span.Slice(0, length);
        }

        #endregion

        #region Multiple via if

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimViaIf<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length == 0)
                return span;
            else if (trimElements.Length == 1)
                return TrimViaIn(span, trimElements[0]);

            int start = ClampStart(span, trimElements);
            int length = ClampEnd(span, start, trimElements);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimViaIfStart<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length == 0)
                return span;
            else if (trimElements.Length == 1)
                return TrimViaIn(span, trimElements[0]);

            int start = ClampStart(span, trimElements);
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimViaIfEnd<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            if (trimElements.Length == 0)
                return span;
            else if (trimElements.Length == 1)
                return TrimViaIn(span, trimElements[0]);

            int length = ClampEnd(span, 0, trimElements);
            return span.Slice(0, length);
        }

        #endregion

        #region Multiple via nested if

        /// <summary>
        /// Removes all leading and trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimViaNestedIf<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            // Nested if optimizes for N > 1 (use other overloads for N <= 1)
            if (trimElements.Length <= 1)
            {
                return trimElements.Length == 0 ? span : TrimViaIn(span, trimElements[0]);
            }

            int start = ClampStart(span, trimElements);
            int length = ClampEnd(span, start, trimElements);
            return span.Slice(start, length);
        }

        /// <summary>
        /// Removes all leading occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimViaNestedIfStart<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            // Nested if optimizes for N > 1
            if (trimElements.Length <= 1)
            {
                return trimElements.Length == 0 ? span : TrimViaInStart(span, trimElements[0]);
            }

            int start = ClampStart(span, trimElements);
            return span.Slice(start);
        }

        /// <summary>
        /// Removes all trailing occurrences of a set of elements specified
        /// in a readonly span from the span.
        /// </summary>
        /// <param name="trimElements">The span which contains the set of elements to remove.</param>
        /// <remarks>If <paramref name="trimElements"/> is empty, the span is returned unaltered.</remarks>
        public static ReadOnlySpan<T> TrimViaNestedIfEnd<T>(ReadOnlySpan<T> span, ReadOnlySpan<T> trimElements)
            where T : IEquatable<T>
        {
            // Nested if optimizes for N > 1
            if (trimElements.Length <= 1)
            {
                return trimElements.Length == 0 ? span : TrimViaInEnd(span, trimElements[0]);
            }

            int length = ClampEnd(span, 0, trimElements);
            return span.Slice(0, length);
        }

        #endregion
    }
}
