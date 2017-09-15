using System;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents an efficient discriminated union across all the primitive number types.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)] // 9 bytes
    public struct Number : IEquatable<Number>, IComparable, IComparable<Number>, IFormattable, IConvertible
    {
        #region Fields

        // Signed

        [FieldOffset(0)] // [0..0]
        private readonly sbyte _sbyte;

        [FieldOffset(0)] // [0..1]
        private readonly short _int16;

        [FieldOffset(0)] // [0..3]
        private readonly int _int32;

        [FieldOffset(0)] // [0..7]
        private readonly long _int64;

        // Unsigned

        [FieldOffset(0)] // [0..0]
        private readonly byte _byte;

        [FieldOffset(0)] // [0..1]
        private readonly ushort _uint16;

        [FieldOffset(0)] // [0..3]
        private readonly uint _uint32;

        [FieldOffset(0)] // [0..7]
        private readonly ulong _uint64;

        // Float

        [FieldOffset(0)] // [0..3]
        private readonly float _single;

        [FieldOffset(0)] // [0..7]
        private readonly double _double;

        // Discriminator

        [FieldOffset(8)] // [8..8]
        private readonly byte _typeCode;

        #endregion

        #region Properties

        public bool IsInteger
        {
            get
            {
                switch (ValueTypeCode)
                {
                    // Signed
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:

                    // Unsigned
                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return true;

                    // Float, Empty
                    default:
                        return false;
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

                    // Empty
                    default:
                        return false;
                }
            }
        }

        /// <summary>
        /// Gets the value as a <see cref="sbyte"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.SByte"/>.</exception>
        public sbyte? SByte => GetValue(_sbyte, TypeCode.SByte);

        /// <summary>
        /// Gets the value as a <see cref="byte"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.Byte"/>.</exception>
        public byte? Byte => GetValue(_byte, TypeCode.Byte);

        /// <summary>
        /// Gets the value as a <see cref="ushort"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.UInt16"/>.</exception>
        public ushort? UInt16 => GetValue(_uint16, TypeCode.UInt16);

        /// <summary>
        /// Gets the value as a <see cref="short"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.Int16"/>.</exception>
        public short? Int16 => GetValue(_int16, TypeCode.Int16);

        /// <summary>
        /// Gets the value as a <see cref="uint"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.UInt32"/>.</exception>
        public uint? UInt32 => GetValue(_uint32, TypeCode.UInt32);

        /// <summary>
        /// Gets the value as a <see cref="int"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.Int32"/>.</exception>
        public int? Int32 => GetValue(_int32, TypeCode.Int32);

        /// <summary>
        /// Gets the value as a <see cref="ulong"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.UInt64"/>.</exception>
        public ulong? UInt64 => GetValue(_uint64, TypeCode.UInt64);

        /// <summary>
        /// Gets the value as a <see cref="long"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.Int64"/>.</exception>
        public long? Int64 => GetValue(_int64, TypeCode.Int64);

        /// <summary>
        /// Gets the value as a <see cref="float"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.Single"/>.</exception>
        public float? Single => GetValue(_single, TypeCode.Single);

        /// <summary>
        /// Gets the value as a <see cref="double"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The value of <see cref="ValueTypeCode"/> is not <see cref="TypeCode.Double"/>.</exception>
        public double? Double => GetValue(_double, TypeCode.Double);

        /// <summary>
        /// Determines whether a value is stored.
        /// </summary>
        public bool HasValue => _typeCode != 0;

        /// <summary>
        /// Gets the value boxed in a <see cref="object"/>.
        /// </summary>
        public object Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
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

                    // Empty
                    default: return null;
                }
            }
        }

        /// <summary>
        /// Gets the <see cref="TypeCode"/> that indicates what the number type is.
        /// </summary>
        public TypeCode ValueTypeCode => (TypeCode)_typeCode;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(sbyte? value) : this()
        {
            if (value.HasValue)
            {
                _typeCode = (byte)TypeCode.SByte;
                _sbyte = value.Value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(sbyte value) : this()
        {
            _typeCode = (byte)TypeCode.SByte;
            _sbyte = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(byte? value) : this()
        {
            if (value.HasValue)
            {
                _typeCode = (byte)TypeCode.Byte;
                _byte = value.Value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(byte value) : this()
        {
            _typeCode = (byte)TypeCode.Byte;
            _byte = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(short? value) : this()
        {
            if (value.HasValue)
            {
                _typeCode = (byte)TypeCode.Int16;
                _int16 = value.Value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(short value) : this()
        {
            _typeCode = (byte)TypeCode.Int16;
            _int16 = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(ushort? value) : this()
        {
            if (value.HasValue)
            {
                _typeCode = (byte)TypeCode.UInt16;
                _uint16 = value.Value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(ushort value) : this()
        {
            _typeCode = (byte)TypeCode.UInt16;
            _uint16 = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(int? value) : this()
        {
            if (value.HasValue)
            {
                _typeCode = (byte)TypeCode.Int32;
                _int32 = value.Value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(int value) : this()
        {
            _typeCode = (byte)TypeCode.Int32;
            _int32 = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(uint? value) : this()
        {
            if (value.HasValue)
            {
                _typeCode = (byte)TypeCode.UInt32;
                _uint32 = value.Value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(uint value) : this()
        {
            _typeCode = (byte)TypeCode.UInt32;
            _uint32 = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(long? value) : this()
        {
            if (value.HasValue)
            {
                _typeCode = (byte)TypeCode.Int64;
                _int64 = value.Value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(long value) : this()
        {
            _typeCode = (byte)TypeCode.Int64;
            _int64 = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(ulong? value) : this()
        {
            if (value.HasValue)
            {
                _typeCode = (byte)TypeCode.UInt64;
                _uint64 = value.Value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(ulong value) : this()
        {
            _typeCode = (byte)TypeCode.UInt64;
            _uint64 = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(float? value) : this()
        {
            if (value.HasValue)
            {
                _typeCode = (byte)TypeCode.Single;
                _single = value.Value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(float value) : this()
        {
            _typeCode = (byte)TypeCode.Single;
            _single = value;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(double? value) : this()
        {
            if (value.HasValue)
            {
                _typeCode = (byte)TypeCode.Double;
                _double = value.Value;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct.
        /// </summary>
        /// <param name="value">The value to be contained by the number.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Number(double value) : this()
        {
            _typeCode = (byte)TypeCode.Double;
            _double = value;
        }

        #endregion

        #region Factory

        private static readonly Func<RuntimeTypeHandle, object, Number> _objectFactory = CreateObjectFactory();

        private static Func<RuntimeTypeHandle, object, Number> CreateObjectFactory()
        {
            var rthParam = Expression.Parameter(typeof(RuntimeTypeHandle), "rth");
            var objParam = Expression.Parameter(typeof(object), "value");

            var types = new[]
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
                typeof(double)
            };

            var cases = new SwitchCase[types.Length * 2];
            for (var i = 0; i < types.Length; i++)
            {
                var j = i * 2;
                var type = types[i];
                var ctor = typeof(Number).GetConstructor(new[] { type });
                var cast = Expression.Convert(objParam, type);
                var create = Expression.New(ctor, cast);
                cases[j] = Expression.SwitchCase(create, Expression.Constant(type.TypeHandle));

                type = typeof(Nullable<>).MakeGenericType(type);
                ctor = typeof(Number).GetConstructor(new[] { type });
                cast = Expression.Convert(objParam, type);
                create = Expression.New(ctor, cast);
                cases[j + 1] = Expression.SwitchCase(create, Expression.Constant(type.TypeHandle));
            }

            var thrwCtor = typeof(ArgumentOutOfRangeException).GetConstructor(new[] { typeof(string) });
            var thrw = Expression.Throw(Expression.New(thrwCtor, Expression.Constant("value")), typeof(Number));

            var comp = typeof(Number).GetMethod(nameof(TypeHandleEquals), BindingFlags.NonPublic | BindingFlags.Static);

            var swtch = Expression.Switch(
                rthParam,
                thrw,
                comp,
                cases
            );

            var lambda = Expression.Lambda<Func<RuntimeTypeHandle, object, Number>>(swtch, rthParam, objParam).Compile();
            return lambda;
        }

        private static bool TypeHandleEquals(RuntimeTypeHandle a, RuntimeTypeHandle b) => a.Equals(b);

        /// <summary>
        /// Creates a new instance of the <see cref="Number"/> struct, given an unknown
        /// value type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The <see cref="Number"/> instance.</returns>
        public static Number CreateFromObject(object value)
            => ReferenceEquals(value, null)
            ? default
            : _objectFactory(value.GetType().TypeHandle, value);

        #endregion

        #region Methods

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T? GetValue<T>(T field, TypeCode typeCode)
            where T : struct
        {
            if (_typeCode == 0) return default;
            if (_typeCode != (byte)typeCode) throw new InvalidOperationException();
            return field;
        }

        /// <summary>Returns a string representation of the <see cref="Number"/> value.</summary>
        /// <param name="format">The format of the number to use.</param>
        /// <returns>A string representation of the number.</returns>
        public string ToString(string format)
            => ToString(format, CultureInfo.CurrentCulture);

        /// <summary>Returns a string representation of the <see cref="Number"/> value.</summary>
        /// <param name="provider">The format provider to use.</param>
        /// <returns>A string representation of the number.</returns>
        public string ToString(IFormatProvider provider)
            => ToString(null, provider);

        /// <summary>Returns a string representation of the <see cref="Number"/> value.</summary>
        /// <returns>A <see cref="T:System.String" /> containing the number.</returns>
        public override string ToString()
            => ToString(null, CultureInfo.CurrentCulture);

        /// <summary>Returns a string representation of the <see cref="Number"/> value.</summary>
        /// <param name="format">The format of the number to use.</param>
        /// <param name="provider">The format provider to use.</param>
        /// <returns>A string representation of the number.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            switch (ValueTypeCode)
            {
                // Signed
                case TypeCode.SByte: return _sbyte.ToString(format, formatProvider);
                case TypeCode.Int16: return _int16.ToString(format, formatProvider);
                case TypeCode.Int32: return _int32.ToString(format, formatProvider);
                case TypeCode.Int64: return _int64.ToString(format, formatProvider);

                // Unsigned
                case TypeCode.Byte: return _byte.ToString(format, formatProvider);
                case TypeCode.UInt16: return _uint16.ToString(format, formatProvider);
                case TypeCode.UInt32: return _uint32.ToString(format, formatProvider);
                case TypeCode.UInt64: return _uint64.ToString(format, formatProvider);

                // Float
                case TypeCode.Single: return _single.ToString(format, formatProvider);
                case TypeCode.Double: return _double.ToString(format, formatProvider);

                default: return string.Empty;
            }
        }

        #endregion

        #region IEquatable

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current instance.</param>
        public override bool Equals(object obj)
            => (ReferenceEquals(obj, null) && _typeCode == 0)
            || (obj is Number o && Equals(o));

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Number other)
        {
            if (_typeCode != other._typeCode) return false;
            if (_uint64 != other._uint64) return false;
            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            var hc = 17;

            unchecked
            {
                hc = hc * 21 + _typeCode;
                hc = hc * 21 + _uint64.GetHashCode();
            }

            return hc;
        }

        public static bool operator ==(Number x, Number y) => x.Equals(y);

        public static bool operator !=(Number x, Number y) => !(x == y);

        #endregion

        #region IComparable

        /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="obj" /> in the sort order. Zero This instance occurs in the same position in the sort order as <paramref name="obj" />. Greater than zero This instance follows <paramref name="obj" /> in the sort order.</returns>
        /// <param name="obj">An object to compare with this instance.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="obj" /> is not the same type as this instance.</exception>
        public int CompareTo(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                if (_typeCode == 0) return 0;
                return 1;
            }

            if (!(obj is Number n)) throw new ArgumentOutOfRangeException(nameof(obj));

            return CompareTo(n);
        }

        /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
        /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings: Value Meaning Less than zero This instance precedes <paramref name="other" /> in the sort order.  Zero This instance occurs in the same position in the sort order as <paramref name="other" />. Greater than zero This instance follows <paramref name="other" /> in the sort order.</returns>
        /// <param name="other">An object to compare with this instance.</param>
        public int CompareTo(Number other)
        {
            if (_typeCode == 0 && other._typeCode == 0) return 0;
            if (_typeCode == 0) return -1;
            if (other._typeCode == 0) return 1;

            switch (((uint)_typeCode << 5) | other._typeCode)
            {
                case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.SByte: return _sbyte.CompareTo(other._sbyte);
                case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.Byte: return _sbyte < 0 ? -1 : -other._byte.CompareTo((byte)_sbyte);
                case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.Int16: return -other._int16.CompareTo(_sbyte);
                case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.UInt16: return _sbyte < 0 ? -1 : -other._uint16.CompareTo((ushort)_sbyte);
                case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.Int32: return -other._int32.CompareTo(_sbyte);
                case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.UInt32: return _sbyte < 0 ? -1 : -other._uint32.CompareTo((uint)_sbyte);
                case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.Int64: return -other._int64.CompareTo(_sbyte);
                case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.UInt64: return _sbyte < 0 ? -1 : -other._uint64.CompareTo((ulong)_sbyte);
                case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.Single: return -other._single.CompareTo(_sbyte);
                case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.Double: return -other._double.CompareTo(_sbyte);
                case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.SByte: return other._sbyte < 0 ? 1 : _byte.CompareTo((byte)other._sbyte);
                case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.Byte: return _byte.CompareTo(other._byte);
                case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.Int16: return other._int16 < 0 ? 1 : -other._int16.CompareTo(_byte);
                case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.UInt16: return -other._uint16.CompareTo(_byte);
                case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.Int32: return other._int32 < 0 ? 1 : -other._int32.CompareTo(_byte);
                case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.UInt32: return -other._uint32.CompareTo(_byte);
                case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.Int64: return other._int64 < 0 ? 1 : -other._int64.CompareTo(_byte);
                case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.UInt64: return -other._uint64.CompareTo(_byte);
                case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.Single: return other._single < 0 ? 1 : -other._single.CompareTo(_byte);
                case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.Double: return other._double < 0 ? 1 : -other._double.CompareTo(_byte);
                case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.SByte: return _int16.CompareTo(other._sbyte);
                case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.Byte: return _int16 < 0 ? -1 : _int16.CompareTo(other._byte);
                case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.Int16: return _int16.CompareTo(other._int16);
                case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.UInt16: return _int16 < 0 ? -1 : -other._uint16.CompareTo((ushort)_int16);
                case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.Int32: return -other._int32.CompareTo(_int16);
                case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.UInt32: return _int16 < 0 ? -1 : -other._uint32.CompareTo((uint)_int16);
                case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.Int64: return -other._int64.CompareTo(_int16);
                case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.UInt64: return _int16 < 0 ? -1 : -other._uint64.CompareTo((ulong)_int16);
                case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.Single: return -other._single.CompareTo(_int16);
                case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.Double: return -other._double.CompareTo(_int16);
                case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.SByte: return other._sbyte < 0 ? 1 : _uint16.CompareTo((ushort)other._sbyte);
                case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.Byte: return _uint16.CompareTo(other._byte);
                case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.Int16: return other._int16 < 0 ? 1 : _uint16.CompareTo((ushort)other._int16);
                case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.UInt16: return _uint16.CompareTo(other._uint16);
                case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.Int32: return other._int32 < 0 ? 1 : -other._int32.CompareTo(_uint16);
                case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.UInt32: return -other._uint32.CompareTo(_uint16);
                case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.Int64: return other._int64 < 0 ? 1 : -other._int64.CompareTo(_uint16);
                case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.UInt64: return -other._uint64.CompareTo(_uint16);
                case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.Single: return other._single < 0 ? 1 : -other._single.CompareTo(_uint16);
                case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.Double: return other._double < 0 ? 1 : -other._double.CompareTo(_uint16);
                case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.SByte: return _int32.CompareTo(other._sbyte);
                case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.Byte: return _int32 < 0 ? -1 : _int32.CompareTo(other._byte);
                case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.Int16: return _int32.CompareTo(other._int16);
                case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.UInt16: return _int32 < 0 ? -1 : _int32.CompareTo(other._uint16);
                case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.Int32: return _int32.CompareTo(other._int32);
                case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.UInt32: return _int32 < 0 ? -1 : -other._uint32.CompareTo((uint)_int32);
                case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.Int64: return -other._int64.CompareTo(_int32);
                case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.UInt64: return _int32 < 0 ? -1 : -other._uint64.CompareTo((ulong)_int32);
                case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.Single: return -other._single.CompareTo(_int32);
                case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.Double: return -other._double.CompareTo(_int32);
                case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.SByte: return other._sbyte < 0 ? 1 : _uint32.CompareTo((uint)other._sbyte);
                case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.Byte: return _uint32.CompareTo(other._byte);
                case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.Int16: return other._int16 < 0 ? 1 : _uint32.CompareTo((uint)other._int16);
                case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.UInt16: return _uint32.CompareTo(other._uint16);
                case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.Int32: return other._int32 < 0 ? 1 : _uint32.CompareTo((uint)other._int32);
                case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.UInt32: return _uint32.CompareTo(other._uint32);
                case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.Int64: return other._int64 < 0 ? 1 : -other._int64.CompareTo(_uint32);
                case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.UInt64: return -other._uint64.CompareTo(_uint32);
                case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.Single: return other._single < 0 ? 1 : -other._single.CompareTo(_uint32);
                case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.Double: return other._double < 0 ? 1 : -other._double.CompareTo(_uint32);
                case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.SByte: return _int64.CompareTo(other._sbyte);
                case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.Byte: return _int64 < 0 ? -1 : _int64.CompareTo(other._byte);
                case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.Int16: return _int64.CompareTo(other._int16);
                case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.UInt16: return _int64 < 0 ? -1 : _int64.CompareTo(other._uint16);
                case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.Int32: return _int64.CompareTo(other._int32);
                case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.UInt32: return _int64 < 0 ? -1 : _int64.CompareTo(other._uint32);
                case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.Int64: return _int64.CompareTo(other._int64);
                case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.UInt64: return _int64 < 0 ? -1 : -other._uint64.CompareTo((ulong)_int64);
                case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.Single: return -((double)other._single).CompareTo((double)_int64);
                case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.Double: return -other._double.CompareTo(_int64);
                case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.SByte: return other._sbyte < 0 ? 1 : _uint64.CompareTo((ulong)other._sbyte);
                case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.Byte: return _uint64.CompareTo(other._byte);
                case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.Int16: return other._int16 < 0 ? 1 : _uint64.CompareTo((ulong)other._int16);
                case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.UInt16: return _uint64.CompareTo(other._uint16);
                case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.Int32: return other._int32 < 0 ? 1 : _uint64.CompareTo((ulong)other._int32);
                case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.UInt32: return _uint64.CompareTo(other._uint32);
                case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.Int64: return other._int64 < 0 ? 1 : _uint64.CompareTo((ulong)other._int64);
                case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.UInt64: return _uint64.CompareTo(other._uint64);
                case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.Single: return other._single < 0 ? 1 : -((double)other._single).CompareTo((double)_uint64);
                case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.Double: return other._double < 0 ? 1 : -other._double.CompareTo(_uint64);
                case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.SByte: return _single.CompareTo(other._sbyte);
                case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.Byte: return _single < 0 ? -1 : _single.CompareTo(other._byte);
                case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.Int16: return _single.CompareTo(other._int16);
                case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.UInt16: return _single < 0 ? -1 : _single.CompareTo(other._uint16);
                case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.Int32: return _single.CompareTo(other._int32);
                case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.UInt32: return _single < 0 ? -1 : _single.CompareTo(other._uint32);
                case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.Int64: return ((double)_single).CompareTo((double)other._int64);
                case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.UInt64: return _single < 0 ? -1 : ((double)_single).CompareTo((double)other._uint64);
                case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.Single: return _single.CompareTo(other._single);
                case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.Double: return -other._double.CompareTo(_single);
                case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.SByte: return _double.CompareTo(other._sbyte);
                case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.Byte: return _double < 0 ? -1 : _double.CompareTo(other._byte);
                case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.Int16: return _double.CompareTo(other._int16);
                case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.UInt16: return _double < 0 ? -1 : _double.CompareTo(other._uint16);
                case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.Int32: return _double.CompareTo(other._int32);
                case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.UInt32: return _double < 0 ? -1 : _double.CompareTo(other._uint32);
                case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.Int64: return _double.CompareTo(other._int64);
                case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.UInt64: return _double < 0 ? -1 : _double.CompareTo(other._uint64);
                case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.Single: return _double.CompareTo(other._single);
                case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.Double: return _double.CompareTo(other._double);
            }

            return 0;
        }

        public static bool operator <(Number x, Number y) => x.CompareTo(y) < 0;

        public static bool operator >(Number x, Number y) => x.CompareTo(y) > 0;

        public static bool operator <=(Number x, Number y) => x.CompareTo(y) <= 0;

        public static bool operator >=(Number x, Number y) => x.CompareTo(y) >= 0;

        #endregion

        #region IConvertible

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(sbyte? value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(sbyte value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(byte? value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(byte value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(short? value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(short value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(ushort? value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(ushort value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(int? value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(int value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(uint? value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(uint value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(long? value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(long value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(ulong? value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(ulong value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(float? value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(float value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(double? value) => new Number(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Number(double value) => new Number(value);

        TypeCode IConvertible.GetTypeCode() => TypeCode.Object;

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToBoolean(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToBoolean(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToBoolean(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToBoolean(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToBoolean(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToBoolean(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToBoolean(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToBoolean(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToBoolean(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToBoolean(provider);
                default: return default;
            }
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToByte(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToByte(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToByte(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToByte(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToByte(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToByte(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToByte(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToByte(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToByte(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToByte(provider);
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
                default: return default;
            }
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToDecimal(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToDecimal(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToDecimal(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToDecimal(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToDecimal(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToDecimal(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToDecimal(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToDecimal(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToDecimal(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToDecimal(provider);
                default: return default;
            }
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToDouble(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToDouble(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToDouble(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToDouble(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToDouble(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToDouble(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToDouble(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToDouble(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToDouble(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToDouble(provider);
                default: return default;
            }
        }

        short IConvertible.ToInt16(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToInt16(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToInt16(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToInt16(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToInt16(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToInt16(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToInt16(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToInt16(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToInt16(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToInt16(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToInt16(provider);
                default: return default;
            }
        }

        int IConvertible.ToInt32(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToInt32(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToInt32(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToInt32(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToInt32(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToInt32(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToInt32(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToInt32(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToInt32(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToInt32(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToInt32(provider);
                default: return default;
            }
        }

        long IConvertible.ToInt64(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToInt64(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToInt64(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToInt64(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToInt64(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToInt64(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToInt64(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToInt64(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToInt64(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToInt64(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToInt64(provider);
                default: return default;
            }
        }

        sbyte IConvertible.ToSByte(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToSByte(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToSByte(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToSByte(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToSByte(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToSByte(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToSByte(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToSByte(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToSByte(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToSByte(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToSByte(provider);
                default: return default;
            }
        }

        float IConvertible.ToSingle(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToSingle(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToSingle(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToSingle(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToSingle(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToSingle(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToSingle(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToSingle(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToSingle(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToSingle(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToSingle(provider);
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
                default: return default;
            }
        }

        ushort IConvertible.ToUInt16(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToUInt16(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToUInt16(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToUInt16(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToUInt16(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToUInt16(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToUInt16(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToUInt16(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToUInt16(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToUInt16(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToUInt16(provider);
                default: return default;
            }
        }

        uint IConvertible.ToUInt32(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToUInt32(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToUInt32(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToUInt32(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToUInt32(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToUInt32(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToUInt32(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToUInt32(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToUInt32(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToUInt32(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToUInt32(provider);
                default: return default;
            }
        }

        ulong IConvertible.ToUInt64(IFormatProvider provider)
        {
            switch (ValueTypeCode)
            {
                case TypeCode.Byte: return ((IConvertible)_byte).ToUInt64(provider);
                case TypeCode.Double: return ((IConvertible)_double).ToUInt64(provider);
                case TypeCode.Int16: return ((IConvertible)_int16).ToUInt64(provider);
                case TypeCode.Int32: return ((IConvertible)_int32).ToUInt64(provider);
                case TypeCode.Int64: return ((IConvertible)_int64).ToUInt64(provider);
                case TypeCode.SByte: return ((IConvertible)_sbyte).ToUInt64(provider);
                case TypeCode.Single: return ((IConvertible)_single).ToUInt64(provider);
                case TypeCode.UInt16: return ((IConvertible)_uint16).ToUInt64(provider);
                case TypeCode.UInt32: return ((IConvertible)_uint32).ToUInt64(provider);
                case TypeCode.UInt64: return ((IConvertible)_uint64).ToUInt64(provider);
                default: return default;
            }
        }

        #endregion
    }
}
