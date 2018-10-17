#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Xunit;

namespace SourceCode.Clay.Buffers.Tests
{
    public static class ByteHashCodeTests
    {
        [Fact(DisplayName = nameof(ByteHashCode_ToHashCode))]
        public static void ByteHashCode_ToHashCode()
        {
            var arr = new byte[128];
            for (var i = 1; i < arr.Length; i++)
            {
                var span = new Span<byte>(arr, 0, i);

                arr[i - 1] = (byte)(i * 2);
                var combine2 = ByteHashCode.Combine(span);
                var add2 = AddMethod(span);

                arr[i - 1] = (byte)i;
                var combine1 = ByteHashCode.Combine(span);
                var add1 = AddMethod(span);

                Assert.Equal(combine1, add1);
                Assert.Equal(combine2, add2);
                Assert.NotEqual(combine1, combine2);

                var combinePrevious = ByteHashCode.Combine(span.Slice(0, span.Length - 1));
                Assert.NotEqual(combinePrevious, combine1);
            }
        }

        [Fact(DisplayName = nameof(ByteHashCode_HashCode_Identity))]
        public static void ByteHashCode_HashCode_Identity()
        {
            var arr = new byte[128];
            for (var i = 0; i < arr.Length; i++)
                arr[i] = (byte)i;

            var bytes = new Span<byte>(arr, 0, arr.Length);
            Span<int> ints = MemoryMarshal.Cast<byte, int>(bytes);

            var expected = AddMethod(ints);
            var actual = AddMethod(bytes);

            Assert.Equal(expected, actual);
        }

        private static int AddMethod(Span<int> data)
        {
            var hc = new HashCode();
            for (var i = 0; i < data.Length; i++) hc.Add(data[i]);
            return hc.ToHashCode();
        }

        private static int AddMethod(Span<byte> data)
        {
            var hc = new ByteHashCode();
            for (var i = 0; i < data.Length; i++) hc.Add(data[i]);
            return hc.ToHashCode();
        }
    }
}
