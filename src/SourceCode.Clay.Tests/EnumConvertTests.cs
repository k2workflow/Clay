#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class EnumConvertTests
    {
        private enum EmptyEnum : byte
        {
        }

        private enum ByteEnum : byte
        {
            MinValue = byte.MinValue,
            MaxValue = byte.MaxValue,

            SByteMinValue = unchecked((byte)sbyte.MinValue),
            UInt16MaxValue = unchecked((byte)ushort.MaxValue)
        }

        private enum SByteEnum : sbyte
        {
            MinValue = sbyte.MinValue,
            MaxValue = sbyte.MaxValue,

            Int16MinValue = unchecked((sbyte)short.MinValue),
            ByteMaxValue = unchecked((sbyte)byte.MaxValue)
        }

        private enum Int16Enum : short
        {
            MinValue = short.MinValue,
            MaxValue = short.MaxValue,

            Int32MinValue = unchecked((short)int.MinValue),
            UInt16MaxValue = unchecked((short)ushort.MaxValue)
        }

        private enum UInt16Enum : ushort
        {
            MinValue = ushort.MinValue,
            MaxValue = ushort.MaxValue,

            Int16MinValue = unchecked((ushort)short.MinValue),
            UInt32MaxValue = unchecked((ushort)uint.MaxValue)
        }

        private enum Int32Enum : int
        {
            MinValue = int.MinValue,
            MaxValue = int.MaxValue,

            Int64MinValue = unchecked((int)long.MinValue),
            UInt32MaxValue = unchecked((int)uint.MaxValue)
        }

        private enum UInt32Enum : uint
        {
            MinValue = uint.MinValue,
            MaxValue = uint.MaxValue,

            Int32MinValue = unchecked((uint)int.MinValue),
            UInt64MaxValue = unchecked((uint)ulong.MaxValue)
        }

        private enum Int64Enum : long
        {
            MinValue = long.MinValue,
            MaxValue = long.MaxValue,

            UInt64MaxValue = unchecked((long)ulong.MaxValue)
        }

        private enum UInt64Enum : ulong
        {
            MinValue = ulong.MinValue,
            MaxValue = ulong.MaxValue,

            Int64MinValue = unchecked((ulong)long.MinValue)
        }

        // TODO: Negative tests should pass
        //[Fact(DisplayName = nameof(EnumConvert_EmptyEnum))]
        //public static void EnumConvert_EmptyEnum()
        //{
        //    var x = EnumConvert.ToEnum<EmptyEnum>(0);
        //    var y = EnumConvert.ToEnum<EmptyEnum>(0);

        //    Assert.Throws<OverflowException>(() => EnumConvert.ToEnum<EmptyEnum>(0));
        //    Assert.Throws<OverflowException>(() => EnumConvert.ToEnum<EmptyEnum>(1));

        //    Assert.Throws<OverflowException>(() => EnumConvert.ToEnumChecked<EmptyEnum>(0));
        //    Assert.Throws<OverflowException>(() => EnumConvert.ToEnumChecked<EmptyEnum>(1));

        //    Assert.Throws<OverflowException>(() => EnumConvert.ConvertToEnumChecked<EmptyEnum>(0));
        //    Assert.Throws<OverflowException>(() => EnumConvert.ConvertToEnumChecked<EmptyEnum>(1));
        //}

        [Fact(DisplayName = nameof(EnumConvert_ToEnumCheckedOfSByte))]
        public static void EnumConvert_ToEnumCheckedOfSByte()
        {
            Assert.Equal(SByteEnum.MinValue, EnumConvert.ToEnumChecked<SByteEnum>(sbyte.MinValue));
            Assert.Equal(SByteEnum.MaxValue, EnumConvert.ToEnumChecked<SByteEnum>(sbyte.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToEnumChecked<SByteEnum>(short.MinValue));
            Assert.Throws<OverflowException>(() => EnumConvert.ToEnumChecked<SByteEnum>(byte.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumCheckedOfUInt16))]
        public static void EnumConvert_ToEnumCheckedOfUInt16()
        {
            Assert.Equal(UInt16Enum.MinValue, EnumConvert.ConvertToEnumChecked<UInt16Enum>(ushort.MinValue));
            Assert.Equal(UInt16Enum.MaxValue, EnumConvert.ConvertToEnumChecked<UInt16Enum>(ushort.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToEnumChecked<UInt16Enum>(short.MinValue));
            Assert.Throws<OverflowException>(() => EnumConvert.ToEnumChecked<UInt16Enum>(uint.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumCheckedOfInt16))]
        public static void EnumConvert_ToEnumCheckedOfInt16()
        {
            Assert.Equal(Int16Enum.MinValue, EnumConvert.ToEnumChecked<Int16Enum>(short.MinValue));
            Assert.Equal(Int16Enum.MaxValue, EnumConvert.ToEnumChecked<Int16Enum>(short.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToEnumChecked<Int16Enum>(int.MinValue));
            Assert.Throws<OverflowException>(() => EnumConvert.ConvertToEnumChecked<Int16Enum>(ushort.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumCheckedOfUInt32))]
        public static void EnumConvert_ToEnumCheckedOfUInt32()
        {
            Assert.Equal(UInt32Enum.MinValue, EnumConvert.ToEnumChecked<UInt32Enum>(uint.MinValue));
            Assert.Equal(UInt32Enum.MaxValue, EnumConvert.ToEnumChecked<UInt32Enum>(uint.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToEnumChecked<UInt32Enum>(int.MinValue));
            Assert.Throws<OverflowException>(() => EnumConvert.ToEnumChecked<UInt32Enum>(ulong.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumCheckedOfInt32))]
        public static void EnumConvert_ToEnumCheckedOfInt32()
        {
            Assert.Equal(Int32Enum.MinValue, EnumConvert.ToEnumChecked<Int32Enum>(int.MinValue));
            Assert.Equal(Int32Enum.MaxValue, EnumConvert.ToEnumChecked<Int32Enum>(int.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToEnumChecked<Int32Enum>(long.MinValue));
            Assert.Throws<OverflowException>(() => EnumConvert.ToEnumChecked<Int32Enum>(uint.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumCheckedOfUInt64))]
        public static void EnumConvert_ToEnumCheckedOfUInt64()
        {
            Assert.Equal(UInt64Enum.MinValue, EnumConvert.ToEnumChecked<UInt64Enum>(ulong.MinValue));
            Assert.Equal(UInt64Enum.MaxValue, EnumConvert.ToEnumChecked<UInt64Enum>(ulong.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToEnumChecked<UInt64Enum>(long.MinValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumCheckedOfInt64))]
        public static void EnumConvert_ToEnumCheckedOfInt64()
        {
            Assert.Equal(Int64Enum.MinValue, EnumConvert.ToEnumChecked<Int64Enum>(long.MinValue));
            Assert.Equal(Int64Enum.MaxValue, EnumConvert.ToEnumChecked<Int64Enum>(long.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToEnumChecked<Int64Enum>(ulong.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToByteChecked))]
        public static void EnumConvert_ToByteChecked()
        {
            Assert.Equal(byte.MinValue, EnumConvert.ToByteChecked(ByteEnum.MinValue));
            Assert.Equal(byte.MaxValue, EnumConvert.ToByteChecked(ByteEnum.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToByteChecked(SByteEnum.MinValue));
            Assert.Throws<OverflowException>(() => EnumConvert.ToByteChecked(UInt16Enum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToSByteChecked))]
        public static void EnumConvert_ToSByteChecked()
        {
            Assert.Equal(sbyte.MinValue, EnumConvert.ToSByteChecked(SByteEnum.MinValue));
            Assert.Equal(sbyte.MaxValue, EnumConvert.ToSByteChecked(SByteEnum.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToSByteChecked(Int16Enum.MinValue));
            Assert.Throws<OverflowException>(() => EnumConvert.ToSByteChecked(ByteEnum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToUInt16Checked))]
        public static void EnumConvert_ToUInt16Checked()
        {
            Assert.Equal(ushort.MinValue, EnumConvert.ToUInt16Checked(UInt16Enum.MinValue));
            Assert.Equal(ushort.MaxValue, EnumConvert.ToUInt16Checked(UInt16Enum.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToUInt16Checked(Int16Enum.MinValue));
            Assert.Throws<OverflowException>(() => EnumConvert.ToUInt16Checked(UInt32Enum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToInt16Checked))]
        public static void EnumConvert_ToInt16Checked()
        {
            Assert.Equal(short.MinValue, EnumConvert.ToInt16Checked(Int16Enum.MinValue));
            Assert.Equal(short.MaxValue, EnumConvert.ToInt16Checked(Int16Enum.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToInt16Checked(Int32Enum.MinValue));
            Assert.Throws<OverflowException>(() => EnumConvert.ToInt16Checked(UInt16Enum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToUInt32Checked))]
        public static void EnumConvert_ToUInt32Checked()
        {
            Assert.Equal(uint.MinValue, EnumConvert.ToUInt32Checked(UInt32Enum.MinValue));
            Assert.Equal(uint.MaxValue, EnumConvert.ToUInt32Checked(UInt32Enum.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToUInt32Checked(Int32Enum.MinValue));
            Assert.Throws<OverflowException>(() => EnumConvert.ToUInt32Checked(UInt64Enum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToInt32Checked))]
        public static void EnumConvert_ToInt32Checked()
        {
            Assert.Equal(int.MinValue, EnumConvert.ToInt32Checked(Int32Enum.MinValue));
            Assert.Equal(int.MaxValue, EnumConvert.ToInt32Checked(Int32Enum.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToInt32Checked(Int64Enum.MinValue));
            Assert.Throws<OverflowException>(() => EnumConvert.ToInt32Checked(UInt32Enum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToUInt64Checked))]
        public static void EnumConvert_ToUInt64Checked()
        {
            Assert.Equal(ulong.MinValue, EnumConvert.ToUInt64Checked(UInt64Enum.MinValue));
            Assert.Equal(ulong.MaxValue, EnumConvert.ToUInt64Checked(UInt64Enum.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToUInt64Checked(Int64Enum.MinValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToInt64Checked))]
        public static void EnumConvert_ToInt64Checked()
        {
            Assert.Equal(long.MinValue, EnumConvert.ToInt64Checked(Int64Enum.MinValue));
            Assert.Equal(long.MaxValue, EnumConvert.ToInt64Checked(Int64Enum.MaxValue));

            Assert.Throws<OverflowException>(() => EnumConvert.ToInt64Checked(UInt64Enum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumOfByte))]
        public static void EnumConvert_ToEnumOfByte()
        {
            Assert.Equal(ByteEnum.MinValue, EnumConvert.ToEnum<ByteEnum>(byte.MinValue));
            Assert.Equal(ByteEnum.MaxValue, EnumConvert.ToEnum<ByteEnum>(byte.MaxValue));

            Assert.Equal(ByteEnum.SByteMinValue, EnumConvert.ToEnum<ByteEnum>(sbyte.MinValue));
            Assert.Equal(ByteEnum.UInt16MaxValue, EnumConvert.ToEnum<ByteEnum>(ushort.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumOfSByte))]
        public static void EnumConvert_ToEnumOfSByte()
        {
            Assert.Equal(SByteEnum.MinValue, EnumConvert.ToEnum<SByteEnum>(sbyte.MinValue));
            Assert.Equal(SByteEnum.MaxValue, EnumConvert.ToEnum<SByteEnum>(sbyte.MaxValue));

            Assert.Equal(SByteEnum.Int16MinValue, EnumConvert.ToEnum<SByteEnum>(short.MinValue));
            Assert.Equal(SByteEnum.ByteMaxValue, EnumConvert.ToEnum<SByteEnum>(byte.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumOfUInt16))]
        public static void EnumConvert_ToEnumOfUInt16()
        {
            Assert.Equal(UInt16Enum.MinValue, EnumConvert.ToEnum<UInt16Enum>(ushort.MinValue));
            Assert.Equal(UInt16Enum.MaxValue, EnumConvert.ToEnum<UInt16Enum>(ushort.MaxValue));

            Assert.Equal(UInt16Enum.Int16MinValue, EnumConvert.ToEnum<UInt16Enum>(short.MinValue));
            Assert.Equal(UInt16Enum.UInt32MaxValue, EnumConvert.ToEnum<UInt16Enum>(uint.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumOfInt16))]
        public static void EnumConvert_ToEnumOfInt16()
        {
            Assert.Equal(Int16Enum.MinValue, EnumConvert.ToEnum<Int16Enum>(short.MinValue));
            Assert.Equal(Int16Enum.MaxValue, EnumConvert.ToEnum<Int16Enum>(short.MaxValue));

            Assert.Equal(Int16Enum.Int32MinValue, EnumConvert.ToEnum<Int16Enum>(int.MinValue));
            Assert.Equal(Int16Enum.UInt16MaxValue, EnumConvert.ToEnum<Int16Enum>(ushort.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumOfUInt32))]
        public static void EnumConvert_ToEnumOfUInt32()
        {
            Assert.Equal(UInt32Enum.MinValue, EnumConvert.ToEnum<UInt32Enum>(uint.MinValue));
            Assert.Equal(UInt32Enum.MaxValue, EnumConvert.ToEnum<UInt32Enum>(uint.MaxValue));

            Assert.Equal(UInt32Enum.Int32MinValue, EnumConvert.ToEnum<UInt32Enum>(int.MinValue));
            Assert.Equal(UInt32Enum.UInt64MaxValue, EnumConvert.ToEnum<UInt32Enum>(ulong.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumOfInt32))]
        public static void EnumConvert_ToEnumOfInt32()
        {
            Assert.Equal(Int32Enum.MinValue, EnumConvert.ToEnum<Int32Enum>(int.MinValue));
            Assert.Equal(Int32Enum.MaxValue, EnumConvert.ToEnum<Int32Enum>(int.MaxValue));

            Assert.Equal(Int32Enum.Int64MinValue, EnumConvert.ToEnum<Int32Enum>(long.MinValue));
            Assert.Equal(Int32Enum.UInt32MaxValue, EnumConvert.ToEnum<Int32Enum>(uint.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumOfUInt64))]
        public static void EnumConvert_ToEnumOfUInt64()
        {
            Assert.Equal(UInt64Enum.MinValue, EnumConvert.ToEnum<UInt64Enum>(ulong.MinValue));
            Assert.Equal(UInt64Enum.MaxValue, EnumConvert.ToEnum<UInt64Enum>(ulong.MaxValue));

            Assert.Equal(UInt64Enum.Int64MinValue, EnumConvert.ToEnum<UInt64Enum>(long.MinValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToEnumOfInt64))]
        public static void EnumConvert_ToEnumOfInt64()
        {
            Assert.Equal(Int64Enum.MinValue, EnumConvert.ToEnum<Int64Enum>(long.MinValue));
            Assert.Equal(Int64Enum.MaxValue, EnumConvert.ToEnum<Int64Enum>(long.MaxValue));

            Assert.Equal(Int64Enum.UInt64MaxValue, EnumConvert.ToEnum<Int64Enum>(ulong.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToByte))]
        public static void EnumConvert_ToByte()
        {
            Assert.Equal(byte.MinValue, EnumConvert.ToByte(ByteEnum.MinValue));
            Assert.Equal(byte.MaxValue, EnumConvert.ToByte(ByteEnum.MaxValue));

            Assert.Equal(unchecked((byte)sbyte.MinValue), EnumConvert.ToByte(SByteEnum.MinValue));
            Assert.Equal(unchecked((byte)ushort.MaxValue), EnumConvert.ToByte(UInt16Enum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToSByte))]
        public static void EnumConvert_ToSByte()
        {
            Assert.Equal(sbyte.MinValue, EnumConvert.ToSByte(SByteEnum.MinValue));
            Assert.Equal(sbyte.MaxValue, EnumConvert.ToSByte(SByteEnum.MaxValue));

            Assert.Equal(unchecked((sbyte)short.MinValue), EnumConvert.ToSByte(Int16Enum.MinValue));
            Assert.Equal(unchecked((sbyte)byte.MaxValue), EnumConvert.ToSByte(ByteEnum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToUInt16))]
        public static void EnumConvert_ToUInt16()
        {
            Assert.Equal(ushort.MinValue, EnumConvert.ToUInt16(UInt16Enum.MinValue));
            Assert.Equal(ushort.MaxValue, EnumConvert.ToUInt16(UInt16Enum.MaxValue));

            Assert.Equal(unchecked((ushort)short.MinValue), EnumConvert.ToUInt16(Int16Enum.MinValue));
            Assert.Equal(unchecked((ushort)uint.MaxValue), EnumConvert.ToUInt16(UInt32Enum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToInt16))]
        public static void EnumConvert_ToInt16()
        {
            Assert.Equal(short.MinValue, EnumConvert.ToInt16(Int16Enum.MinValue));
            Assert.Equal(short.MaxValue, EnumConvert.ToInt16(Int16Enum.MaxValue));

            Assert.Equal(unchecked((short)int.MinValue), EnumConvert.ToInt16(Int32Enum.MinValue));
            Assert.Equal(unchecked((short)ushort.MaxValue), EnumConvert.ToInt16(UInt16Enum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToUInt32))]
        public static void EnumConvert_ToUInt32()
        {
            Assert.Equal(uint.MinValue, EnumConvert.ToUInt32(UInt32Enum.MinValue));
            Assert.Equal(uint.MaxValue, EnumConvert.ToUInt32(UInt32Enum.MaxValue));

            Assert.Equal(unchecked((uint)int.MinValue), EnumConvert.ToUInt32(Int32Enum.MinValue));
            Assert.Equal(unchecked((uint)ulong.MaxValue), EnumConvert.ToUInt32(UInt64Enum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToInt32))]
        public static void EnumConvert_ToInt32()
        {
            Assert.Equal(int.MinValue, EnumConvert.ToInt32(Int32Enum.MinValue));
            Assert.Equal(int.MaxValue, EnumConvert.ToInt32(Int32Enum.MaxValue));

            Assert.Equal(unchecked((int)long.MinValue), EnumConvert.ToInt32(Int64Enum.MinValue));
            Assert.Equal(unchecked((int)uint.MaxValue), EnumConvert.ToInt32(UInt32Enum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToUInt64))]
        public static void EnumConvert_ToUInt64()
        {
            Assert.Equal(ulong.MinValue, EnumConvert.ToUInt64(UInt64Enum.MinValue));
            Assert.Equal(ulong.MaxValue, EnumConvert.ToUInt64(UInt64Enum.MaxValue));

            Assert.Equal(unchecked((ulong)long.MinValue), EnumConvert.ToUInt64(Int64Enum.MinValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_ToInt64))]
        public static void EnumConvert_ToInt64()
        {
            Assert.Equal(long.MinValue, EnumConvert.ToInt64(Int64Enum.MinValue));
            Assert.Equal(long.MaxValue, EnumConvert.ToInt64(Int64Enum.MaxValue));

            Assert.Equal(unchecked((long)ulong.MaxValue), EnumConvert.ToInt64(UInt64Enum.MaxValue));
        }

        [Fact(DisplayName = nameof(EnumConvert_Cached_EmptyEnum))]
        public static void EnumConvert_Cached_EmptyEnum()
        {
            Assert.Equal(Enum.GetValues(typeof(EmptyEnum)).Length, EnumConvert.Length<EmptyEnum>());
        }

        [Fact(DisplayName = nameof(EnumConvert_Cached_ValidEnum))]
        public static void EnumConvert_Cached_ValidEnum()
        {
            Assert.Equal(Enum.GetValues(typeof(ByteEnum)).Length, EnumConvert.Length<ByteEnum>());
        }
    }
}
