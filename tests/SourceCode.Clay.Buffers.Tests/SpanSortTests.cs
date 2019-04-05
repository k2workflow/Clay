#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class SpanSortTests
    {
        private static int IntComparison(byte a, byte b) => a.CompareTo(b);

        private static void HeapSortAndAssert(Span<byte> array)
        {
            MemoryExtensions.HeapSort(array, IntComparison, 0, array.Length - 1);
            for (var i = 1; i < array.Length; i++)
            {
                Assert.True(array[i - 1] <= array[i]);
            }
        }

        private static void SortAndAssert(Span<byte> array)
        {
            array.Sort(IntComparison);
            for (var i = 1; i < array.Length; i++)
            {
                Assert.True(array[i - 1] <= array[i]);
            }
        }

        [Fact]
        public static void SpanExtensions_Sort_Empty()
        {
            SortAndAssert(default);
        }

        [Fact]
        public static void SpanExtensions_Sort_1()
        {
            SortAndAssert(new byte[1] { 1 });
        }

        [Fact]
        public static void SpanExtensions_Sort_2()
        {
            SortAndAssert(new byte[2] { 2, 1 });
        }

        [Fact]
        public static void SpanExtensions_Sort_3()
        {
            SortAndAssert(new byte[3] { 3, 1, 2 });
        }

        [Fact]
        public static void SpanExtensions_Sort_Sorted()
        {
            var arr = new byte[]
            {
                1, 2, 3, 4, 5
            };
            SortAndAssert(arr);
        }

        [Fact]
        public static void SpanExtensions_Sort_Reversed()
        {
            var arr = new byte[]
            {
                5, 4, 3, 2, 1
            };
            SortAndAssert(arr);
        }

        [Fact]
        public static void SpanExtensions_Sort_Big()
        {
            var arr = new byte[10000];
            for (var i = 0; i < arr.Length; i++)
                arr[i] = (byte)(i * i);
            SortAndAssert(arr);
        }

        [Fact]
        public static void SpanExtensions_HeapSort_Big()
        {
            var arr = new byte[10000];
            for (var i = 0; i < arr.Length; i++)
                arr[i] = (byte)(i * i);
            HeapSortAndAssert(arr);
        }
    }
}
