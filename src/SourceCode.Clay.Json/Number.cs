#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SourceCode.Clay.Json
{
#pragma warning disable CA2225 // Operator overloads have named alternates

    /// <summary>
    /// Represents an efficient discriminated union across all the primitive number types.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)] // 17 bytes, aligned up to 20
    public readonly struct Number : IEquatable<Number>, IComparable<Number>, IFormattable, IConvertible
    {
        // Signed

        [FieldOffset(0)] // [0..0]
        internal readonly sbyte _sbyte;

        [FieldOffset(0)] // [0..1]
        internal readonly short _int16;

        [FieldOffset(0)] // [0..3]
        internal readonly int _int32;

        [FieldOffset(0)] // [0..7]
        internal readonly long _int64;

        // Unsigned

        [FieldOffset(0)] // [0..0]
        internal readonly byte _byte;

        [FieldOffset(0)] // [0..1]
        internal readonly ushort _uint16;

        [FieldOffset(0)] // [0..3]
        internal readonly uint _uint32;

        [FieldOffset(0)] // [0..7]
        internal readonly ulong _uint64;

        // Float

        [FieldOffset(0)] // [0..3]
        internal readonly float _single;

        [FieldOffset(0)] // [0..7]
        internal readonly double _double;

        [FieldOffset(0)] // [0..15]
        internal readonly decimal _decimal;

        // Discriminator

        [FieldOffset(16)] // [16..16]
        private readonly byte _typeCode;

        public NumberKinds Kind
        {
            get
            {
                switch (ValueTypeCode)
                {
                    // Signed
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    default:
                    case TypeCode.Int64:
                        return NumberKinds.Integer | NumberKinds.Signed;

                    // Unsigned
                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return NumberKinds.Integer;

                    // Real
                    case TypeCode.Single:
                    case TypeCode.Double:
                        return NumberKinds.Real | NumberKinds.Signed;

                    // Decimal
                    case TypeCode.Decimal:
                        return NumberKinds.Decimal | NumberKinds.Signed;
                }
            }
        }

        public bool IsZero
        {
            get
            {
                switch (ValueTypeCode)
                {
                    // Signed
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    default:
                    case TypeCode.Int64:

                    // Unsigned
                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:

                    // Float
                    case TypeCode.Single:
                    case TypeCode.Double:
                        return _uint64 == 0;

                    // Decimal
                    case TypeCode.Decimal:
                        return _decimal == 0;
                }
            }
        }

#pragma warning disable CA1720 // Identifier contains type name

        /// <summary>
        /// Gets the value as a <see cref="sbyte"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.SByte"/>.</exception>
        public sbyte SByte => GetValue(_sbyte, TypeCode.SByte);

        /// <summary>
        /// Gets the value as a <see cref="byte"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.Byte"/>.</exception>
        public byte Byte => GetValue(_byte, TypeCode.Byte);

        /// <summary>
        /// Gets the value as a <see cref="ushort"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.UInt16"/>.</exception>
        public ushort UInt16 => GetValue(_uint16, TypeCode.UInt16);

        /// <summary>
        /// Gets the value as a <see cref="short"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.Int16"/>.</exception>
        public short Int16 => GetValue(_int16, TypeCode.Int16);

        /// <summary>
        /// Gets the value as a <see cref="uint"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.UInt32"/>.</exception>
        public uint UInt32 => GetValue(_uint32, TypeCode.UInt32);

        /// <summary>
        /// Gets the value as a <see cref="int"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.Int32"/>.</exception>
        public int Int32 => GetValue(_int32, TypeCode.Int32);

        /// <summary>
        /// Gets the value as a <see cref="ulong"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.UInt64"/>.</exception>
        public ulong UInt64 => GetValue(_uint64, TypeCode.UInt64);

        /// <summary>
        /// Gets the value as a <see cref="long"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.Int64"/>.</exception>
        public long Int64 => GetValue(_int64, TypeCode.Int64);

        /// <summary>
        /// Gets the value as a <see cref="float"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.Single"/>.</exception>
        public float Single => GetValue(_single, TypeCode.Single);

        /// <summary>
        /// Gets the value as a <see cref="double"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.Double"/>.</exception>
        public double Double => GetValue(_double, TypeCode.Double);

        /// <summary>
        /// Gets the value as a <see cref="decimal"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.Decimal"/>.</exception>
        public decimal Decimal => GetValue(_decimal, TypeCode.Decimal);

#pragma warning restore CA1720 // Identifier contains type name

        /// <summary>
        /// Gets the value boxed in a <see cref="object"/>.
        /// </summary>
        public object Value
        {
            get
            {
                switch (ValueTypeCode)
                {
                    // Signed
                    case TypeCode.SByte: return _sbyte;
                    case TypeCode.Int16: return _int16;
                    case TypeCode.Int32: return _int32;
                    default:
                    case TypeCode.Int64: return _int64;

                    // Unsigned
                    case TypeCode.Byte: return _byte;
                    case TypeCode.UInt16: return _uint16;
                    case TypeCode.UInt32: return _uint32;
                    case TypeCode.UInt64: return _uint64;

                    // Float
                    case TypeCode.Single: return _single;
                    case TypeCode.Double: return _double;
                    case TypeCode.Decimal: return _decimal;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="TypeCode"/> that indicates what the number type is.
        /// </summary>
        public TypeCode ValueTypeCode => _typeCode == 0 ? TypeCode.Int64 : (TypeCode)_typeCode;

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        public Number(sbyte value) : this()
        {
            _typeCode = (byte)TypeCode.SByte;
            _sbyte = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        public Number(byte value) : this()
        {
            _typeCode = (byte)TypeCode.Byte;
            _byte = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        public Number(short value) : this()
        {
            _typeCode = (byte)TypeCode.Int16;
            _int16 = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        public Number(ushort value) : this()
        {
            _typeCode = (byte)TypeCode.UInt16;
            _uint16 = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        public Number(int value) : this()
        {
            _typeCode = (byte)TypeCode.Int32;
            _int32 = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        public Number(uint value) : this()
        {
            _typeCode = (byte)TypeCode.UInt32;
            _uint32 = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        public Number(long value) : this()
        {
            _typeCode = (byte)TypeCode.Int64;
            _int64 = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        public Number(ulong value) : this()
        {
            _typeCode = (byte)TypeCode.UInt64;
            _uint64 = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        public Number(float value) : this()
        {
            _typeCode = (byte)TypeCode.Single;
            _single = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        public Number(double value) : this()
        {
            _typeCode = (byte)TypeCode.Double;
            _double = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        public Number(decimal value) : this()
        {
            _typeCode = (byte)TypeCode.Decimal;
            _decimal = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct, given an unknown
        /// value type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="Number"/> instance.</returns>
        public static Number CreateFromObject(object value)
            => value is null
            ? throw new ArgumentNullException(nameof(value))
            : s_objectFactory(value.GetType().TypeHandle, value);

        private static readonly Func<RuntimeTypeHandle, object, Number> s_objectFactory = CreateObjectFactory();

        private static Func<RuntimeTypeHandle, object, Number> CreateObjectFactory()
        {
            ParameterExpression rthParam = Expression.Parameter(typeof(RuntimeTypeHandle), "rth");
            ParameterExpression objParam = Expression.Parameter(typeof(object), "value");

            Type[] types = new[]
            {
                // Signed
                typeof(sbyte),
                typeof(short),
                typeof(int),
                typeof(long),

                // Unsigned
                typeof(byte),
                typeof(ushort),
                typeof(uint),
                typeof(ulong),

                // Float
                typeof(float),
                typeof(double),
                typeof(decimal)
            };

            var cases = new SwitchCase[types.Length];
            for (var i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                ConstructorInfo ctor = typeof(Number).GetConstructor(new[] { type });
                UnaryExpression cast = Expression.Convert(objParam, type);
                NewExpression create = Expression.New(ctor, cast);
                cases[i] = Expression.SwitchCase(create, Expression.Constant(type.TypeHandle));
            }

            ConstructorInfo thrwCtor = typeof(ArgumentOutOfRangeException).GetConstructor(new[] { typeof(string) });
            UnaryExpression thrw = Expression.Throw(Expression.New(thrwCtor, Expression.Constant("value")), typeof(Number));

            MethodInfo comp = typeof(Number).GetMethod(nameof(TypeHandleEquals), BindingFlags.NonPublic | BindingFlags.Static);

            SwitchExpression swtch = Expression.Switch(
                rthParam,
                thrw,
                comp,
                cases
            );

            Func<RuntimeTypeHandle, object, Number> lambda = Expression.Lambda<Func<RuntimeTypeHandle, object, Number>>(swtch, rthParam, objParam).Compile();
            return lambda;
        }

        private static bool TypeHandleEquals(RuntimeTypeHandle a, RuntimeTypeHandle b) => a.Equals(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T GetValue<T>(T field, TypeCode typeCode)
            where T : struct
        {
            if (_typeCode != (byte)typeCode) throw new InvalidOperationException();
            return field;
        }

        /// <summary>Returns a string representation of the <see cref="Number"/> value.</summary>
        /// <param name="format">The format of the number to use.</param>
        /// <returns>A string representation of the number.</returns>
        public string ToString(string format) => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a string representation of the <see cref="Number"/> value.</summary>
        /// <param name="provider">The format provider to use.</param>
        /// <returns>A string representation of the number.</returns>
        public string ToString(IFormatProvider provider) => ToString(null, provider);

        /// <summary>Returns a string representation of the <see cref="Number"/> value.</summary>
        /// <returns>A <see cref="System.String" /> containing the number.</returns>
        public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

        /// <summary>Returns a string representation of the <see cref="Number"/> value.</summary>
        /// <param name="format">The format of the number to use.</param>
        /// <param name="provider">The format provider to use.</param>
        /// <returns>A string representation of the number.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (ValueTypeCode)
            {
                // Signed
                case TypeCode.SByte: return _sbyte.ToString(format, formatProvider);
                case TypeCode.Int16: return _int16.ToString(format, formatProvider);
                case TypeCode.Int32: return _int32.ToString(format, formatProvider);
                default:
                case TypeCode.Int64: return _int64.ToString(format, formatProvider);

                // Unsigned
                case TypeCode.Byte: return _byte.ToString(format, formatProvider);
                case TypeCode.UInt16: return _uint16.ToString(format, formatProvider);
                case TypeCode.UInt32: return _uint32.ToString(format, formatProvider);
                case TypeCode.UInt64: return _uint64.ToString(format, formatProvider);

                // Float
                case TypeCode.Single: return _single.ToString(format, formatProvider);
                case TypeCode.Double: return _double.ToString(format, formatProvider);
                case TypeCode.Decimal: return _decimal.ToString(format, formatProvider);
            }
        }

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current instance.</param>
        public override bool Equals(object obj)
            => obj is Number num
            && Equals(num);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(Number other) => NumberComparer.Default.Equals(this, other);

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode() => NumberComparer.Default.GetHashCode(this);

        /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order.</returns>
        /// <param name="other">An object to compare with this instance.</param>
        public int CompareTo(Number other) => NumberComparer.Default.Compare(this, other);

        public static bool operator ==(Number x, Number y) => x.Equals(y);

        public static bool operator !=(Number x, Number y) => !(x == y);

        public static bool operator <(Number x, Number y) => x.CompareTo(y) < 0;

        public static bool operator >(Number x, Number y) => x.CompareTo(y) > 0;

        public static bool operator <=(Number x, Number y) => x.CompareTo(y) <= 0;

        public static bool operator >=(Number x, Number y) => x.CompareTo(y) >= 0;

        public static implicit operator Number(sbyte value) => new Number(value);

        public static implicit operator Number(byte value) => new Number(value);

        public static implicit operator Number(short value) => new Number(value);

        public static implicit operator Number(ushort value) => new Number(value);

        public static implicit operator Number(int value) => new Number(value);

        public static implicit operator Number(uint value) => new Number(value);

        public static implicit operator Number(long value) => new Number(value);

        public static implicit operator Number(ulong value) => new Number(value);

        public static implicit operator Number(float value) => new Number(value);

        public static implicit operator Number(double value) => new Number(value);

        public static implicit operator Number(decimal value) => new Number(value);

        TypeCode IConvertible.GetTypeCode() => TypeCode.Object;

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return Convert.ToBoolean(_byte);
                case TypeCode.Double: return Convert.ToBoolean(_double);
                case TypeCode.Int16: return Convert.ToBoolean(_int16);
                case TypeCode.Int32: return Convert.ToBoolean(_int32);
                case TypeCode.Int64: return Convert.ToBoolean(_int64);
                case TypeCode.SByte: return Convert.ToBoolean(_sbyte);
                case TypeCode.Single: return Convert.ToBoolean(_single);
                case TypeCode.UInt16: return Convert.ToBoolean(_uint16);
                case TypeCode.UInt32: return Convert.ToBoolean(_uint32);
                case TypeCode.UInt64: return Convert.ToBoolean(_uint64);
                case TypeCode.Decimal: return Convert.ToBoolean(_decimal);
                default: return default;
            }
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return _byte;
                case TypeCode.Double: return Convert.ToByte(_double);
                case TypeCode.Int16: return Convert.ToByte(_int16);
                case TypeCode.Int32: return Convert.ToByte(_int32);
                case TypeCode.Int64: return Convert.ToByte(_int64);
                case TypeCode.SByte: return Convert.ToByte(_sbyte);
                case TypeCode.Single: return Convert.ToByte(_single);
                case TypeCode.UInt16: return Convert.ToByte(_uint16);
                case TypeCode.UInt32: return Convert.ToByte(_uint32);
                case TypeCode.UInt64: return Convert.ToByte(_uint64);
                case TypeCode.Decimal: return Convert.ToByte(_decimal);
                default: return default;
            }
        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToChar(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToChar(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToChar(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToChar(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToChar(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToChar(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToChar(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToChar(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToChar(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToChar(provider);
                case TypeCode.Decimal: return ((IConvertible)_decimal).ToChar(provider);
                default: return default;
            }
        }

        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToDateTime(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToDateTime(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToDateTime(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToDateTime(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToDateTime(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToDateTime(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToDateTime(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToDateTime(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToDateTime(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToDateTime(provider);
                case TypeCode.Decimal: return ((IConvertible)_decimal).ToDateTime(provider);
                default: return default;
            }
        }

        public decimal ToDecimal()
        {
            switch (ValueTypeCode)
            {
                // Signed
                case TypeCode.SByte: return _sbyte;
                case TypeCode.Int16: return _int16;
                case TypeCode.Int32: return _int32;
                case TypeCode.Int64: return _int64;

                // Unsigned
                case TypeCode.Byte: return _byte;
                case TypeCode.UInt16: return _uint16;
                case TypeCode.UInt32: return _uint32;
                case TypeCode.UInt64: return _uint64;

                // Float
                case TypeCode.Single: return (decimal)_single;
                case TypeCode.Double: return (decimal)_double;
                case TypeCode.Decimal: return _decimal;

                default: return default;
            }
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return Convert.ToDecimal(_byte);
                case TypeCode.Double: return Convert.ToDecimal(_double);
                case TypeCode.Int16: return Convert.ToDecimal(_int16);
                case TypeCode.Int32: return Convert.ToDecimal(_int32);
                case TypeCode.Int64: return Convert.ToDecimal(_int64);
                case TypeCode.SByte: return Convert.ToDecimal(_sbyte);
                case TypeCode.Single: return Convert.ToDecimal(_single);
                case TypeCode.UInt16: return Convert.ToDecimal(_uint16);
                case TypeCode.UInt32: return Convert.ToDecimal(_uint32);
                case TypeCode.UInt64: return Convert.ToDecimal(_uint64);
                case TypeCode.Decimal: return _decimal;
                default: return default;
            }
        }

        public double ToDouble()
        {
            switch (ValueTypeCode)
            {
                // Signed
                case TypeCode.SByte: return _sbyte;
                case TypeCode.Int16: return _int16;
                case TypeCode.Int32: return _int32;
                case TypeCode.Int64: return _int64;

                // Unsigned
                case TypeCode.Byte: return _byte;
                case TypeCode.UInt16: return _uint16;
                case TypeCode.UInt32: return _uint32;
                case TypeCode.UInt64: return _uint64;

                // Float
                case TypeCode.Single: return _single;
                case TypeCode.Double: return _double;
                case TypeCode.Decimal: return (double)_decimal;

                default: return default;
            }
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                // Signed
                case TypeCode.SByte: return Convert.ToDouble(_sbyte);
                case TypeCode.Int16: return Convert.ToDouble(_int16);
                case TypeCode.Int32: return Convert.ToDouble(_int32);
                case TypeCode.Int64: return Convert.ToDouble(_int64);

                // Unsigned
                case TypeCode.Byte: return Convert.ToDouble(_byte);
                case TypeCode.UInt16: return Convert.ToDouble(_uint16);
                case TypeCode.UInt32: return Convert.ToDouble(_uint32);
                case TypeCode.UInt64: return Convert.ToDouble(_uint64);

                // Float
                case TypeCode.Single: return Convert.ToDouble(_single);
                case TypeCode.Double: return _double;
                case TypeCode.Decimal: return Convert.ToDouble(_decimal);

                default: return default;
            }
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return Convert.ToInt16(_byte);
                case TypeCode.Double: return Convert.ToInt16(_double);
                case TypeCode.Int16: return _int16;
                case TypeCode.Int32: return Convert.ToInt16(_int32);
                case TypeCode.Int64: return Convert.ToInt16(_int64);
                case TypeCode.SByte: return Convert.ToInt16(_sbyte);
                case TypeCode.Single: return Convert.ToInt16(_single);
                case TypeCode.UInt16: return Convert.ToInt16(_uint16);
                case TypeCode.UInt32: return Convert.ToInt16(_uint32);
                case TypeCode.UInt64: return Convert.ToInt16(_uint64);
                case TypeCode.Decimal: return Convert.ToInt16(_decimal);
                default: return default;
            }
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return Convert.ToInt32(_byte);
                case TypeCode.Double: return Convert.ToInt32(_double);
                case TypeCode.Int16: return Convert.ToInt32(_int16);
                case TypeCode.Int32: return _int32;
                case TypeCode.Int64: return Convert.ToInt32(_int64);
                case TypeCode.SByte: return Convert.ToInt32(_sbyte);
                case TypeCode.Single: return Convert.ToInt32(_single);
                case TypeCode.UInt16: return Convert.ToInt32(_uint16);
                case TypeCode.UInt32: return Convert.ToInt32(_uint32);
                case TypeCode.UInt64: return Convert.ToInt32(_uint64);
                case TypeCode.Decimal: return Convert.ToInt32(_decimal);
                default: return default;
            }
        }

        public long ToInt64()
        {
            switch (ValueTypeCode)
            {
                // Signed
                case TypeCode.SByte: return _sbyte;
                case TypeCode.Int16: return _int16;
                case TypeCode.Int32: return _int32;
                case TypeCode.Int64: return _int64;

                // Unsigned
                case TypeCode.Byte: return _byte;
                case TypeCode.UInt16: return _uint16;
                case TypeCode.UInt32: return _uint32;
                case TypeCode.UInt64: return (long)_uint64;

                // Float
                case TypeCode.Single: return (long)_single;
                case TypeCode.Double: return (long)_double;
                case TypeCode.Decimal: return (long)_decimal;

                default: return default;
            }
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                // Signed
                case TypeCode.SByte: return Convert.ToInt64(_sbyte);
                case TypeCode.Int16: return Convert.ToInt64(_int16);
                case TypeCode.Int32: return Convert.ToInt64(_int32);
                case TypeCode.Int64: return _int64;

                // Unsigned
                case TypeCode.Byte: return Convert.ToInt64(_byte);
                case TypeCode.UInt16: return Convert.ToInt64(_uint16);
                case TypeCode.UInt32: return Convert.ToInt64(_uint32);
                case TypeCode.UInt64: return Convert.ToInt64(_uint64);

                // Float
                case TypeCode.Single: return Convert.ToInt64(_single);
                case TypeCode.Double: return Convert.ToInt64(_double);
                case TypeCode.Decimal: return Convert.ToInt64(_decimal);

                default: return default;
            }
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return Convert.ToSByte(_byte);
                case TypeCode.Double: return Convert.ToSByte(_double);
                case TypeCode.Int16: return Convert.ToSByte(_int16);
                case TypeCode.Int32: return Convert.ToSByte(_int32);
                case TypeCode.Int64: return Convert.ToSByte(_int64);
                case TypeCode.SByte: return _sbyte;
                case TypeCode.Single: return Convert.ToSByte(_single);
                case TypeCode.UInt16: return Convert.ToSByte(_uint16);
                case TypeCode.UInt32: return Convert.ToSByte(_uint32);
                case TypeCode.UInt64: return Convert.ToSByte(_uint64);
                case TypeCode.Decimal: return Convert.ToSByte(_decimal);
                default: return default;
            }
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return Convert.ToSingle(_byte);
                case TypeCode.Double: return Convert.ToSingle(_double);
                case TypeCode.Int16: return Convert.ToSingle(_int16);
                case TypeCode.Int32: return Convert.ToSingle(_int32);
                case TypeCode.Int64: return Convert.ToSingle(_int64);
                case TypeCode.SByte: return Convert.ToSingle(_sbyte);
                case TypeCode.Single: return _single;
                case TypeCode.UInt16: return Convert.ToSingle(_uint16);
                case TypeCode.UInt32: return Convert.ToSingle(_uint32);
                case TypeCode.UInt64: return Convert.ToSingle(_uint64);
                case TypeCode.Decimal: return Convert.ToSingle(_decimal);
                default: return default;
            }
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToType(conversionType, provider);
                case TypeCode.Double: return ((IConvertible)_double).ToType(conversionType, provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToType(conversionType, provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToType(conversionType, provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToType(conversionType, provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToType(conversionType, provider);
                case TypeCode.Single: return ((IConvertible)_single).ToType(conversionType, provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToType(conversionType, provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToType(conversionType, provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToType(conversionType, provider);
                case TypeCode.Decimal: return ((IConvertible)_decimal).ToType(conversionType, provider);
                default: return default;
            }
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return Convert.ToUInt16(_byte);
                case TypeCode.Double: return Convert.ToUInt16(_double);
                case TypeCode.Int16: return Convert.ToUInt16(_int16);
                case TypeCode.Int32: return Convert.ToUInt16(_int32);
                case TypeCode.Int64: return Convert.ToUInt16(_int64);
                case TypeCode.SByte: return Convert.ToUInt16(_sbyte);
                case TypeCode.Single: return Convert.ToUInt16(_single);
                case TypeCode.UInt16: return _uint16;
                case TypeCode.UInt32: return Convert.ToUInt16(_uint32);
                case TypeCode.UInt64: return Convert.ToUInt16(_uint64);
                case TypeCode.Decimal: return Convert.ToUInt16(_decimal);
                default: return default;
            }
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return Convert.ToUInt32(_byte);
                case TypeCode.Double: return Convert.ToUInt32(_double);
                case TypeCode.Int16: return Convert.ToUInt32(_int16);
                case TypeCode.Int32: return Convert.ToUInt32(_int32);
                case TypeCode.Int64: return Convert.ToUInt32(_int64);
                case TypeCode.SByte: return Convert.ToUInt32(_sbyte);
                case TypeCode.Single: return Convert.ToUInt32(_single);
                case TypeCode.UInt16: return Convert.ToUInt32(_uint16);
                case TypeCode.UInt32: return _uint32;
                case TypeCode.UInt64: return Convert.ToUInt32(_uint64);
                case TypeCode.Decimal: return Convert.ToUInt32(_decimal);
                default: return default;
            }
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return Convert.ToUInt64(_byte);
                case TypeCode.Double: return Convert.ToUInt64(_double);
                case TypeCode.Int16: return Convert.ToUInt64(_int16);
                case TypeCode.Int32: return Convert.ToUInt64(_int32);
                case TypeCode.Int64: return Convert.ToUInt64(_int64);
                case TypeCode.SByte: return Convert.ToUInt64(_sbyte);
                case TypeCode.Single: return Convert.ToUInt64(_single);
                case TypeCode.UInt16: return Convert.ToUInt64(_uint16);
                case TypeCode.UInt32: return Convert.ToUInt64(_uint32);
                case TypeCode.UInt64: return _uint64;
                case TypeCode.Decimal: return Convert.ToUInt64(_decimal);
                default: return default;
            }
        }
    }

#pragma warning restore CA2225 // Operator overloads have named alternates
}
