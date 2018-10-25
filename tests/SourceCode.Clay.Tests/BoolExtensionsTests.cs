#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Runtime.CompilerServices;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class BoolExtensionsTests
    {
        // Prevent folding by using volatile non-readonly non-constant
        private static volatile bool s_true = true;
        private static volatile bool s_false = false;

        [Fact(DisplayName = nameof(AsByte))]
        public static void AsByte()
        {
            // CLR permits other values for True
            // https://github.com/dotnet/roslyn/issues/24652

            for (int i = 0; i <= byte.MaxValue; i++)
            {
                int expected = i == 0 ? 0 : 1;

                byte n = (byte)i;
                bool tf = Unsafe.As<byte, bool>(ref n);
                int actual = tf.AsByte();

                Assert.Equal(expected, actual);
            }
        }

        [Fact(DisplayName = nameof(If_Byte))]
        public static void If_Byte()
        {
            const byte t0 = 0;
            const byte t1 = 1;
            const byte t2 = 2;
            const byte t3 = 3;
            const byte min = byte.MinValue;
            const byte max = byte.MaxValue;

            Assert.Equal(1, s_true.AsByte());
            Assert.Equal(0, s_false.AsByte());

            Assert.Equal(t1, s_true.If(t1));
            Assert.Equal(t0, s_false.If(t1));

            Assert.Equal(t2, s_true.If(t2));
            Assert.Equal(t3, s_false.If(t1, t3));

            Assert.Equal(t2, s_true.If(t2, t3));
            Assert.Equal(t3, s_false.If(t2, t3));

            Assert.Equal(max, s_true.If(max, min));
            Assert.Equal(max, s_false.If(min, max));
        }

        [Fact(DisplayName = nameof(If_SByte))]
        public static void If_SByte()
        {
            const sbyte t0 = 0;
            const sbyte t1 = 1;
            const sbyte t2 = 2;
            const sbyte t3 = 3;
            const sbyte min = sbyte.MinValue;
            const sbyte max = sbyte.MaxValue;

            Assert.Equal(1, s_true.AsByte());
            Assert.Equal(0, s_false.AsByte());

            Assert.Equal(t1, s_true.If(t1));
            Assert.Equal(t0, s_false.If(t1));

            Assert.Equal(t2, s_true.If(t2));
            Assert.Equal(t3, s_false.If(t1, t3));

            Assert.Equal(t2, s_true.If(t2, t3));
            Assert.Equal(t3, s_false.If(t2, t3));

            Assert.Equal(max, s_true.If(max, min));
            Assert.Equal(max, s_false.If(min, max));
        }

        [Fact(DisplayName = nameof(If_UInt16))]
        public static void If_UInt16()
        {
            const ushort t0 = 0;
            const ushort t1 = 1;
            const ushort t2 = 2;
            const ushort t3 = 3;
            const ushort min = ushort.MinValue;
            const ushort max = ushort.MaxValue;

            Assert.Equal(t1, s_true.If(t1));
            Assert.Equal(t0, s_false.If(t1));

            Assert.Equal(t2, s_true.If(t2));
            Assert.Equal(t3, s_false.If(t1, t3));

            Assert.Equal(t2, s_true.If(t2, t3));
            Assert.Equal(t3, s_false.If(t2, t3));

            Assert.Equal(max, s_true.If(max, min));
            Assert.Equal(max, s_false.If(min, max));
        }

        [Fact(DisplayName = nameof(If_Int16))]
        public static void If_Int16()
        {
            const short t0 = 0;
            const short t1 = 1;
            const short t2 = 2;
            const short t3 = 3;
            const short min = short.MinValue;
            const short max = short.MaxValue;

            Assert.Equal(t1, s_true.If(t1));
            Assert.Equal(t0, s_false.If(t1));

            Assert.Equal(t2, s_true.If(t2));
            Assert.Equal(t3, s_false.If(t1, t3));

            Assert.Equal(t2, s_true.If(t2, t3));
            Assert.Equal(t3, s_false.If(t2, t3));

            Assert.Equal(max, s_true.If(max, min));
            Assert.Equal(max, s_false.If(min, max));
        }

        [Fact(DisplayName = nameof(If_UInt32))]
        public static void If_UInt32()
        {
            const uint t0 = 0;
            const uint t1 = 1;
            const uint t2 = 2;
            const uint t3 = 3;
            const uint min = uint.MinValue;
            const uint max = uint.MaxValue;

            Assert.Equal(t1, s_true.If(t1));
            Assert.Equal(t0, s_false.If(t1));

            Assert.Equal(t2, s_true.If(t2));
            Assert.Equal(t3, s_false.If(t1, t3));

            Assert.Equal(t2, s_true.If(t2, t3));
            Assert.Equal(t3, s_false.If(t2, t3));

            Assert.Equal(max, s_true.If(max, min));
            Assert.Equal(max, s_false.If(min, max));
        }

        [Fact(DisplayName = nameof(If_Int32))]
        public static void If_Int32()
        {
            const int t0 = 0;
            const int t1 = 1;
            const int t2 = 2;
            const int t3 = 3;
            const int min = int.MinValue;
            const int max = int.MaxValue;

            Assert.Equal(t1, s_true.If(t1));
            Assert.Equal(t0, s_false.If(t1));

            Assert.Equal(t2, s_true.If(t2));
            Assert.Equal(t3, s_false.If(t1, t3));

            Assert.Equal(t2, s_true.If(t2, t3));
            Assert.Equal(t3, s_false.If(t2, t3));

            Assert.Equal(max, s_true.If(max, min));
            Assert.Equal(max, s_false.If(min, max));
        }

        [Fact(DisplayName = nameof(If_UInt64))]
        public static void If_UInt64()
        {
            const ulong t0 = 0;
            const ulong t1 = 1;
            const ulong t2 = 2;
            const ulong t3 = 3;
            const ulong min = ulong.MinValue;
            const ulong max = ulong.MaxValue;

            Assert.Equal(t1, s_true.If(t1));
            Assert.Equal(t0, s_false.If(t1));

            Assert.Equal(t2, s_true.If(t2));
            Assert.Equal(t3, s_false.If(t1, t3));

            Assert.Equal(t2, s_true.If(t2, t3));
            Assert.Equal(t3, s_false.If(t2, t3));

            Assert.Equal(max, s_true.If(max, min));
            Assert.Equal(max, s_false.If(min, max));
        }

        [Fact(DisplayName = nameof(If_Int64))]
        public static void If_Int64()
        {
            const long t0 = 0;
            const long t1 = 1;
            const long t2 = 2;
            const long t3 = 3;
            const long min = long.MinValue;
            const long max = long.MaxValue;

            Assert.Equal(t1, s_true.If(t1));
            Assert.Equal(t0, s_false.If(t1));

            Assert.Equal(t2, s_true.If(t2));
            Assert.Equal(t3, s_false.If(t1, t3));

            Assert.Equal(t2, s_true.If(t2, t3));
            Assert.Equal(t3, s_false.If(t2, t3));

            Assert.Equal(max, s_true.If(max, min));
            Assert.Equal(max, s_false.If(min, max));
        }
    }
}
