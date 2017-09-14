using System;
using System.Reflection;
using Xunit;

namespace SourceCode.Clay.Tests
{
    // Parameters are used to disambiguate theories
#   pragma warning disable xUnit1026 // Theory methods should use all of their parameters
    public class NumberTests
    {
        [
            InlineData("null", null),
            InlineData(nameof(SByte), (sbyte)1),
            InlineData(nameof(Byte), (byte)2),
            InlineData(nameof(Int16), (short)3),
            InlineData(nameof(UInt16), (ushort)4),
            InlineData(nameof(Int32), 5),
            InlineData(nameof(UInt32), 6U),
            InlineData(nameof(Int64), 7L),
            InlineData(nameof(UInt64), 8UL),
            InlineData(nameof(Single), 9.0F),
            InlineData(nameof(Double), 10.0)
        ]
        [Theory(DisplayName = nameof(Number_ContructGet))]
        public void Number_ContructGet(string description, object value)
        {
            var sut = Number.CreateFromObject(value);
            Assert.Equal(value, sut.Value);

            if (value == null) return;

            var t = value.GetType();
            var nullable = typeof(Nullable<>).MakeGenericType(t);
            var nullableCtor = nullable.GetConstructor(new[] { t });

            sut = Number.CreateFromObject(nullableCtor.Invoke(new[] { value }));
            Assert.Equal(value, sut.Value);

            sut = Number.CreateFromObject(Activator.CreateInstance(nullable));
            Assert.False(sut.HasValue);
        }

        [
            InlineData(nameof(SByte), (sbyte)1),
            InlineData(nameof(Byte), (byte)2),
            InlineData(nameof(Int16), (short)3),
            InlineData(nameof(UInt16), (ushort)4),
            InlineData(nameof(Int32), 5),
            InlineData(nameof(UInt32), 6U),
            InlineData(nameof(Int64), 7L),
            InlineData(nameof(UInt64), 8UL),
            InlineData(nameof(Single), 9.0F),
            InlineData(nameof(Double), 10.0)
        ]
        [Theory(DisplayName = nameof(Number_ToString))]
        public void Number_ToString(string description, object value)
        {
            var sut = Number.CreateFromObject(value);
            Assert.Equal(sut.ToString(), value.ToString());
        }

        [
            InlineData(nameof(SByte), nameof(SByte), (sbyte)-100, (sbyte)100, (sbyte)-100, (sbyte)100),
            InlineData(nameof(SByte), nameof(Byte), (sbyte)10, (sbyte)100, (byte)10, (byte)100),
            InlineData(nameof(SByte), nameof(Int16), (sbyte)-100, (sbyte)100, (short)-100, (short)100),
            InlineData(nameof(SByte), nameof(UInt16), (sbyte)10, (sbyte)100, (ushort)10, (ushort)100),
            InlineData(nameof(SByte), nameof(Int32), (sbyte)-100, (sbyte)100, -100, 100),
            InlineData(nameof(SByte), nameof(UInt32), (sbyte)10, (sbyte)100, 10U, 100U),
            InlineData(nameof(SByte), nameof(Int64), (sbyte)-100, (sbyte)100, -100L, 100L),
            InlineData(nameof(SByte), nameof(UInt64), (sbyte)10, (sbyte)100, 10UL, 100UL),
            InlineData(nameof(SByte), nameof(Single), (sbyte)-100, (sbyte)100, -100.0F, 100.0F),
            InlineData(nameof(SByte), nameof(Double), (sbyte)-100, (sbyte)100, -100.0, 100.0),

            InlineData(nameof(Byte), nameof(SByte), (byte)10, (byte)100, (sbyte)10, (sbyte)100),
            InlineData(nameof(Byte), nameof(Byte), (byte)10, (byte)100, (byte)10, (byte)100),
            InlineData(nameof(Byte), nameof(Int16), (byte)10, (byte)100, (short)10, (short)100),
            InlineData(nameof(Byte), nameof(UInt16), (byte)10, (byte)100, (ushort)10, (ushort)100),
            InlineData(nameof(Byte), nameof(Int32), (byte)10, (byte)100, 10, 100),
            InlineData(nameof(Byte), nameof(UInt32), (byte)10, (byte)100, 10U, 100U),
            InlineData(nameof(Byte), nameof(Int64), (byte)10, (byte)100, 10L, 100L),
            InlineData(nameof(Byte), nameof(UInt64), (byte)10, (byte)100, 10UL, 100UL),
            InlineData(nameof(Byte), nameof(Single), (byte)10, (byte)100, 10.0F, 100.0F),
            InlineData(nameof(Byte), nameof(Double), (byte)10, (byte)100, 10.0, 100.0),

            InlineData(nameof(Int16), nameof(SByte), (short)-100, (short)100, (sbyte)-100, (sbyte)100),
            InlineData(nameof(Int16), nameof(Byte), (short)10, (short)100, (byte)10, (byte)100),
            InlineData(nameof(Int16), nameof(Int16), (short)-100, (short)100, (short)-100, (short)100),
            InlineData(nameof(Int16), nameof(UInt16), (short)10, (short)100, (ushort)10, (ushort)100),
            InlineData(nameof(Int16), nameof(Int32), (short) -100, (short)100, -100, 100),
            InlineData(nameof(Int16), nameof(UInt32), (short)10, (short)100, 10U, 100U),
            InlineData(nameof(Int16), nameof(Int64), (short)-100, (short)100, -100L, 100L),
            InlineData(nameof(Int16), nameof(UInt64), (short)10, (short)100, 10UL, 100UL),
            InlineData(nameof(Int16), nameof(Single), (short)-100, (short)100, -100.0F, 100.0F),
            InlineData(nameof(Int16), nameof(Double), (short)-100, (short)100, -100.0, 100.0),

            InlineData(nameof(UInt16), nameof(SByte), (ushort)10, (ushort)100, (sbyte)10, (sbyte)100),
            InlineData(nameof(UInt16), nameof(Byte), (ushort)10, (ushort)100, (byte)10, (byte)100),
            InlineData(nameof(UInt16), nameof(Int16), (ushort)10, (ushort)100, (short)10, (short)100),
            InlineData(nameof(UInt16), nameof(UInt16), (ushort)10, (ushort)100, (ushort)10, (ushort)100),
            InlineData(nameof(UInt16), nameof(Int32), (ushort)10, (ushort)100, 10, 100),
            InlineData(nameof(UInt16), nameof(UInt32), (ushort)10, (ushort)100, 10U, 100U),
            InlineData(nameof(UInt16), nameof(Int64), (ushort)10, (ushort)100, 10L, 100L),
            InlineData(nameof(UInt16), nameof(UInt64), (ushort)10, (ushort)100, 10UL, 100UL),
            InlineData(nameof(UInt16), nameof(Single), (ushort)10, (ushort)100, 10.0F, 100.0F),
            InlineData(nameof(UInt16), nameof(Double), (ushort)10, (ushort)100, 10.0, 100.0),

            InlineData(nameof(Int32), nameof(SByte), -100, 100, (sbyte)-100, (sbyte)100),
            InlineData(nameof(Int32), nameof(Byte), 10, 100, (byte)10, (byte)100),
            InlineData(nameof(Int32), nameof(Int16), -100, 100, (short)-100, (short)100),
            InlineData(nameof(Int32), nameof(UInt16), 10, 100, (ushort)10, (ushort)100),
            InlineData(nameof(Int32), nameof(Int32), -100, 100, -100, 100),
            InlineData(nameof(Int32), nameof(UInt32), 10, 100, 10U, 100U),
            InlineData(nameof(Int32), nameof(Int64), -100, 100, -100L, 100L),
            InlineData(nameof(Int32), nameof(UInt64), 10, 100, 10UL, 100UL),
            InlineData(nameof(Int32), nameof(Single), -100, 100, -100.0F, 100.0F),
            InlineData(nameof(Int32), nameof(Double), -100, 100, -100.0, 100.0),

            InlineData(nameof(UInt32), nameof(SByte), 10U, 100U, (sbyte)10, (sbyte)100),
            InlineData(nameof(UInt32), nameof(Byte), 10U, 100U, (byte)10, (byte)100),
            InlineData(nameof(UInt32), nameof(Int16), 10U, 100U, (short)10, (short)100),
            InlineData(nameof(UInt32), nameof(UInt16), 10U, 100U, (ushort)10, (ushort)100),
            InlineData(nameof(UInt32), nameof(Int32), 10U, 100U, 10, 100),
            InlineData(nameof(UInt32), nameof(UInt32), 10U, 100U, 10U, 100U),
            InlineData(nameof(UInt32), nameof(Int64), 10U, 100U, 10L, 100L),
            InlineData(nameof(UInt32), nameof(UInt64), 10U, 100U, 10UL, 100UL),
            InlineData(nameof(UInt32), nameof(Single), 10U, 100U, 10.0F, 100.0F),
            InlineData(nameof(UInt32), nameof(Double), 10U, 100U, 10.0, 100.0),

            InlineData(nameof(Int64), nameof(SByte), -100L, 100L, (sbyte)-100, (sbyte)100),
            InlineData(nameof(Int64), nameof(Byte), 10L, 100L, (byte)10, (byte)100),
            InlineData(nameof(Int64), nameof(Int16), -100L, 100L, (short)-100, (short)100),
            InlineData(nameof(Int64), nameof(UInt16), 10L, 100L, (ushort)10, (ushort)100),
            InlineData(nameof(Int64), nameof(Int32), -100L, 100L, -100, 100),
            InlineData(nameof(Int64), nameof(UInt32), 10L, 100L, 10U, 100U),
            InlineData(nameof(Int64), nameof(Int64), -100L, 100L, -100L, 100L),
            InlineData(nameof(Int64), nameof(UInt64), 10L, 100L, 10UL, 100UL),
            InlineData(nameof(Int64), nameof(Single), -100L, 100L, -100.0F, 100.0F),
            InlineData(nameof(Int64), nameof(Double), -100L, 100L, -100.0, 100.0),

            InlineData(nameof(UInt64), nameof(SByte), 10UL, 100UL, (sbyte)10, (sbyte)100),
            InlineData(nameof(UInt64), nameof(Byte), 10UL, 100UL, (byte)10, (byte)100),
            InlineData(nameof(UInt64), nameof(Int16), 10UL, 100UL, (short)10, (short)100),
            InlineData(nameof(UInt64), nameof(UInt16), 10UL, 100UL, (ushort)10, (ushort)100),
            InlineData(nameof(UInt64), nameof(Int32), 10UL, 100UL, 10, 100),
            InlineData(nameof(UInt64), nameof(UInt32), 10UL, 100UL, 10U, 100U),
            InlineData(nameof(UInt64), nameof(Int64), 10UL, 100UL, 10L, 100L),
            InlineData(nameof(UInt64), nameof(UInt64), 10UL, 100UL, 10UL, 100UL),
            InlineData(nameof(UInt64), nameof(Single), 10UL, 100UL, 10.0F, 100.0F),
            InlineData(nameof(UInt64), nameof(Double), 10UL, 100UL, 10.0, 100.0),

            InlineData(nameof(Single), nameof(SByte), -100.0F, 100.0F, (sbyte)-100, (sbyte)100),
            InlineData(nameof(Single), nameof(Byte), 10.0F, 100.0F, (byte)10, (byte)100),
            InlineData(nameof(Single), nameof(Int16), -100.0F, 100.0F, (short)-100, (short)100),
            InlineData(nameof(Single), nameof(UInt16), 10.0F, 100.0F, (ushort)10, (ushort)100),
            InlineData(nameof(Single), nameof(Int32), -100.0F, 100.0F, -100, 100),
            InlineData(nameof(Single), nameof(UInt32), 10.0F, 100.0F, 10U, 100U),
            InlineData(nameof(Single), nameof(Int64), -100.0F, 100.0F, -100L, 100L),
            InlineData(nameof(Single), nameof(UInt64), 10.0F, 100.0F, 10UL, 100UL),
            InlineData(nameof(Single), nameof(Single), -100.0F, 100.0F, -100.0F, 100.0F),
            InlineData(nameof(Single), nameof(Double), -100.0F, 100.0F, -100.0, 100.0),

            InlineData(nameof(Double), nameof(SByte), 10.0F, 100.0F, (sbyte)10, (sbyte)100),
            InlineData(nameof(Double), nameof(Byte), 10.0F, 100.0F, (byte)10, (byte)100),
            InlineData(nameof(Double), nameof(Int16), 10.0F, 100.0F, (short)10, (short)100),
            InlineData(nameof(Double), nameof(UInt16), 10.0F, 100.0F, (ushort)10, (ushort)100),
            InlineData(nameof(Double), nameof(Int32), 10.0F, 100.0F, 10, 100),
            InlineData(nameof(Double), nameof(UInt32), 10.0F, 100.0F, 10U, 100U),
            InlineData(nameof(Double), nameof(Int64), 10.0F, 100.0F, 10L, 100L),
            InlineData(nameof(Double), nameof(UInt64), 10.0F, 100.0F, 10UL, 100UL),
            InlineData(nameof(Double), nameof(Single), 10.0F, 100.0F, 10.0F, 100.0F),
            InlineData(nameof(Double), nameof(Double), 10.0F, 100.0F, 10.0, 100.0),
        ]
        [Theory(DisplayName = nameof(Number_Compare))]
        public void Number_Compare(string a, string b, object aLow, object aHigh, object bLow, object bHigh)
        {
            var nullNumber = new Number();
            var aLowNumber = Number.CreateFromObject(aLow);
            var bLowNumber = Number.CreateFromObject(bLow);
            var aHighNumber = Number.CreateFromObject(aHigh);
            var bHighNumber = Number.CreateFromObject(bHigh);

            // Check against own type.
            Assert.True(aLowNumber.CompareTo(aLowNumber) == 0);
            Assert.True(aHighNumber.CompareTo(aHighNumber) == 0);
            Assert.True(aLowNumber.CompareTo(nullNumber) > 0);
            Assert.True(aHighNumber.CompareTo(nullNumber) > 0);
            Assert.True(aLowNumber.CompareTo(aHighNumber) < 0);
            Assert.True(aHighNumber.CompareTo(aLowNumber) > 0);

            // Check against other type.
            Assert.True(aLowNumber.CompareTo(bLowNumber) == 0);
            Assert.True(aHighNumber.CompareTo(bHighNumber) == 0);
            Assert.True(aLowNumber.CompareTo(bHighNumber) < 0);
            Assert.True(aHighNumber.CompareTo(bLowNumber) > 0);
        }

        [
            InlineData(nameof(SByte), nameof(SByte)),
            InlineData(nameof(SByte), nameof(Byte)),
            InlineData(nameof(SByte), nameof(Int16)),
            InlineData(nameof(SByte), nameof(UInt16)),
            InlineData(nameof(SByte), nameof(Int32)),
            InlineData(nameof(SByte), nameof(UInt32)),
            InlineData(nameof(SByte), nameof(Int64)),
            InlineData(nameof(SByte), nameof(UInt64)),
            InlineData(nameof(SByte), nameof(Single)),
            InlineData(nameof(SByte), nameof(Double)),

            InlineData(nameof(Byte), nameof(SByte)),
            InlineData(nameof(Byte), nameof(Byte)),
            InlineData(nameof(Byte), nameof(Int16)),
            InlineData(nameof(Byte), nameof(UInt16)),
            InlineData(nameof(Byte), nameof(Int32)),
            InlineData(nameof(Byte), nameof(UInt32)),
            InlineData(nameof(Byte), nameof(Int64)),
            InlineData(nameof(Byte), nameof(UInt64)),
            InlineData(nameof(Byte), nameof(Single)),
            InlineData(nameof(Byte), nameof(Double)),

            InlineData(nameof(Int16), nameof(SByte)),
            InlineData(nameof(Int16), nameof(Byte)),
            InlineData(nameof(Int16), nameof(Int16)),
            InlineData(nameof(Int16), nameof(UInt16)),
            InlineData(nameof(Int16), nameof(Int32)),
            InlineData(nameof(Int16), nameof(UInt32)),
            InlineData(nameof(Int16), nameof(Int64)),
            InlineData(nameof(Int16), nameof(UInt64)),
            InlineData(nameof(Int16), nameof(Single)),
            InlineData(nameof(Int16), nameof(Double)),

            InlineData(nameof(UInt16), nameof(SByte)),
            InlineData(nameof(UInt16), nameof(Byte)),
            InlineData(nameof(UInt16), nameof(Int16)),
            InlineData(nameof(UInt16), nameof(UInt16)),
            InlineData(nameof(UInt16), nameof(Int32)),
            InlineData(nameof(UInt16), nameof(UInt32)),
            InlineData(nameof(UInt16), nameof(Int64)),
            InlineData(nameof(UInt16), nameof(UInt64)),
            InlineData(nameof(UInt16), nameof(Single)),
            InlineData(nameof(UInt16), nameof(Double)),

            InlineData(nameof(Int32), nameof(SByte)),
            InlineData(nameof(Int32), nameof(Byte)),
            InlineData(nameof(Int32), nameof(Int16)),
            InlineData(nameof(Int32), nameof(UInt16)),
            InlineData(nameof(Int32), nameof(Int32)),
            InlineData(nameof(Int32), nameof(UInt32)),
            InlineData(nameof(Int32), nameof(Int64)),
            InlineData(nameof(Int32), nameof(UInt64)),
            InlineData(nameof(Int32), nameof(Single)),
            InlineData(nameof(Int32), nameof(Double)),

            InlineData(nameof(UInt32), nameof(SByte)),
            InlineData(nameof(UInt32), nameof(Byte)),
            InlineData(nameof(UInt32), nameof(Int16)),
            InlineData(nameof(UInt32), nameof(UInt16)),
            InlineData(nameof(UInt32), nameof(Int32)),
            InlineData(nameof(UInt32), nameof(UInt32)),
            InlineData(nameof(UInt32), nameof(Int64)),
            InlineData(nameof(UInt32), nameof(UInt64)),
            InlineData(nameof(UInt32), nameof(Single)),
            InlineData(nameof(UInt32), nameof(Double)),

            InlineData(nameof(Int64), nameof(SByte)),
            InlineData(nameof(Int64), nameof(Byte)),
            InlineData(nameof(Int64), nameof(Int16)),
            InlineData(nameof(Int64), nameof(UInt16)),
            InlineData(nameof(Int64), nameof(Int32)),
            InlineData(nameof(Int64), nameof(UInt32)),
            InlineData(nameof(Int64), nameof(Int64)),
            InlineData(nameof(Int64), nameof(UInt64)),
            InlineData(nameof(Int64), nameof(Single)),
            InlineData(nameof(Int64), nameof(Double)),

            InlineData(nameof(UInt64), nameof(SByte)),
            InlineData(nameof(UInt64), nameof(Byte)),
            InlineData(nameof(UInt64), nameof(Int16)),
            InlineData(nameof(UInt64), nameof(UInt16)),
            InlineData(nameof(UInt64), nameof(Int32)),
            InlineData(nameof(UInt64), nameof(UInt32)),
            InlineData(nameof(UInt64), nameof(Int64)),
            InlineData(nameof(UInt64), nameof(UInt64)),
            InlineData(nameof(UInt64), nameof(Single)),
            InlineData(nameof(UInt64), nameof(Double)),

            InlineData(nameof(Single), nameof(SByte)),
            InlineData(nameof(Single), nameof(Byte)),
            InlineData(nameof(Single), nameof(Int16)),
            InlineData(nameof(Single), nameof(UInt16)),
            InlineData(nameof(Single), nameof(Int32)),
            InlineData(nameof(Single), nameof(UInt32)),
            InlineData(nameof(Single), nameof(Int64)),
            InlineData(nameof(Single), nameof(UInt64)),
            InlineData(nameof(Single), nameof(Single)),
            InlineData(nameof(Single), nameof(Double)),

            InlineData(nameof(Double), nameof(SByte)),
            InlineData(nameof(Double), nameof(Byte)),
            InlineData(nameof(Double), nameof(Int16)),
            InlineData(nameof(Double), nameof(UInt16)),
            InlineData(nameof(Double), nameof(Int32)),
            InlineData(nameof(Double), nameof(UInt32)),
            InlineData(nameof(Double), nameof(Int64)),
            InlineData(nameof(Double), nameof(UInt64)),
            InlineData(nameof(Double), nameof(Single)),
            InlineData(nameof(Double), nameof(Double)),
        ]
        [Theory(DisplayName = nameof(Number_Compare_Overflow))]
        public void Number_Compare_Overflow(string a, string b)
        {
            var aT = Type.GetType($"System.{a}");
            var bT = Type.GetType($"System.{b}");

            var aMin = Number.CreateFromObject(aT.GetField("MinValue").GetValue(null));
            var bMin = Number.CreateFromObject(bT.GetField("MinValue").GetValue(null));
            var aMax = Number.CreateFromObject(aT.GetField("MaxValue").GetValue(null));
            var bMax = Number.CreateFromObject(bT.GetField("MaxValue").GetValue(null));

            // Simply make sure that these don't throw,
            // there's no simple way to do a compare without Number.
            aMin.CompareTo(bMin);
            aMin.CompareTo(bMax);
            aMax.CompareTo(bMin);
            aMax.CompareTo(bMax);
        }
    }
#   pragma warning restore xUnit1026 // Theory methods should use all of their parameters
}
