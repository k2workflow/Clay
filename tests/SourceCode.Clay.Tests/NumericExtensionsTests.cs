#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class NumericExtensionsTests
    {
        [Fact(DisplayName = nameof(Clamp_Byte))]
        public static void Clamp_Byte()
        {
            Assert.Equal((byte)0, ((byte)0).Clamp(0, 0));
            Assert.Equal((byte)1, ((byte)1).Clamp(0, 2));

            Assert.Equal((byte)1, ((byte)0).Clamp(1, 2));
            Assert.Equal((byte)2, ((byte)3).Clamp(1, 2));
        }

        [Fact(DisplayName = nameof(Clamp_Int16))]
        public static void Clamp_Int16()
        {
            Assert.Equal((short)0, ((short)0).Clamp(0, 0));
            Assert.Equal((short)0, ((short)0).Clamp(-1, 1));

            Assert.Equal((short)1, ((short)0).Clamp(1, 2));
            Assert.Equal((short)2, ((short)3).Clamp(1, 2));
        }

        [Fact(DisplayName = nameof(Clamp_Int32))]
        public static void Clamp_Int32()
        {
            Assert.Equal(0, 0.Clamp(0, 0));
            Assert.Equal(0, 0.Clamp(-1, 1));

            Assert.Equal(1, 0.Clamp(1, 2));
            Assert.Equal(2, 3.Clamp(1, 2));
        }

        [Fact(DisplayName = nameof(Clamp_Int64))]
        public static void Clamp_Int64()
        {
            Assert.Equal(0L, 0L.Clamp(0, 0));
            Assert.Equal(0L, 0L.Clamp(-1, 1));

            Assert.Equal(1L, 0L.Clamp(1, 2));
            Assert.Equal(2L, 3L.Clamp(1, 2));
        }

        [Fact(DisplayName = nameof(Clamp_Double))]
        public static void Clamp_Double()
        {
            Assert.Equal(0d, 0d.Clamp(0, 0));
            Assert.Equal(0d, 0d.Clamp(-1, 1));

            Assert.Equal(1d, 0d.Clamp(1, 2));
            Assert.Equal(2d, 3d.Clamp(1, 2));
        }

        [Fact(DisplayName = nameof(Clamp_Single))]
        public static void Clamp_Single()
        {
            Assert.Equal(0f, 0f.Clamp(0, 0));
            Assert.Equal(0f, 0f.Clamp(-1, 1));

            Assert.Equal(1f, 0f.Clamp(1, 2));
            Assert.Equal(2f, 3f.Clamp(1, 2));
        }

        [Fact(DisplayName = nameof(Clamp_Decimal))]
        public static void Clamp_Decimal()
        {
            Assert.Equal(0m, 0m.Clamp(0, 0));
            Assert.Equal(0m, 0m.Clamp(-1, 1));

            Assert.Equal(1m, 0m.Clamp(1, 2));
            Assert.Equal(2m, 3m.Clamp(1, 2));
        }
    }
}
