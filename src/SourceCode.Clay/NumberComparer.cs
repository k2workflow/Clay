#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents a way to compare different <see cref="Number"/> values.
    /// </summary>
    public abstract class NumberComparer : IEqualityComparer<Number>, IEqualityComparer<Number?>, IComparer<Number>, IComparer<Number?>
    {
        #region Constants

        private const double DoubleDecimalMinValue = (double)decimal.MinValue;
        private const double DoubleDecimalMaxValue = (double)decimal.MaxValue;

        private const float SingleDecimalMinValue = (float)decimal.MinValue;
        private const float SingleDecimalMaxValue = (float)decimal.MaxValue;

        /// <summary>
        /// Gets a <see cref="NumberComparer"/> that compares all fields of a <see cref="Number"/> value.
        /// </summary>
        public static NumberComparer Default { get; } = new DefaultComparer();

        #endregion

        #region Constructors

        protected NumberComparer()
        { }

        #endregion

        #region IComparer

        /// <inheritdoc/>
        public abstract int Compare(Number x, Number y);

        public int Compare(Number? x, Number? y)
        {
            if (!x.HasValue && !y.HasValue) return 0;
            if (!x.HasValue) return -1;
            if (!y.HasValue) return 1;

            var cmp = Compare(x.Value, y.Value);
            return cmp;
        }

        #endregion

        #region IEqualityComparer

        /// <inheritdoc/>
        public abstract bool Equals(Number x, Number y);

        /// <inheritdoc/>
        public bool Equals(Number? x, Number? y)
        {
            if (!x.HasValue) return !y.HasValue; // (null, null) or (null, y)
            if (!y.HasValue) return true; // (x, null)

            var equal = Equals(x.Value, y.Value);
            return equal;
        }

        /// <inheritdoc/>
        public abstract int GetHashCode(Number obj);

        public int GetHashCode(Number? obj)
        {
            if (!obj.HasValue) return -42;

            return GetHashCode(obj.Value);
        }

        #endregion

        #region Concrete

        private sealed class DefaultComparer : NumberComparer
        {
            #region Constructors

            internal DefaultComparer()
            { }

            #endregion

            #region IComparer

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int Compare(Number x, Number y)
            {
                switch (((uint)x.ValueTypeCode << 5) | (uint)y.ValueTypeCode)
                {
                    // this == SByte
                    case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.SByte: return x._sbyte.CompareTo(y._sbyte);
                    case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.Byte: return x._sbyte < 0 ? -1 : -y._byte.CompareTo((byte)x._sbyte);
                    case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.Int16: return -y._int16.CompareTo(x._sbyte);
                    case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.UInt16: return x._sbyte < 0 ? -1 : -y._uint16.CompareTo((ushort)x._sbyte);
                    case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.Int32: return -y._int32.CompareTo(x._sbyte);
                    case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.UInt32: return x._sbyte < 0 ? -1 : -y._uint32.CompareTo((uint)x._sbyte);
                    case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.Int64: return -y._int64.CompareTo(x._sbyte);
                    case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.UInt64: return x._sbyte < 0 ? -1 : -y._uint64.CompareTo((ulong)x._sbyte);
                    case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.Single: return -y._single.CompareTo(x._sbyte);
                    case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.Double: return -y._double.CompareTo(x._sbyte);
                    case (((uint)TypeCode.SByte) << 5) | (uint)TypeCode.Decimal: return -y._decimal.CompareTo(x._sbyte);

                    // this == Byte
                    case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.SByte: return y._sbyte < 0 ? 1 : x._byte.CompareTo((byte)y._sbyte);
                    case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.Byte: return x._byte.CompareTo(y._byte);
                    case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.Int16: return y._int16 < 0 ? 1 : -y._int16.CompareTo(x._byte);
                    case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.UInt16: return -y._uint16.CompareTo(x._byte);
                    case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.Int32: return y._int32 < 0 ? 1 : -y._int32.CompareTo(x._byte);
                    case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.UInt32: return -y._uint32.CompareTo(x._byte);
                    case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.Int64: return y._int64 < 0 ? 1 : -y._int64.CompareTo(x._byte);
                    case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.UInt64: return -y._uint64.CompareTo(x._byte);
                    case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.Single: return y._single < 0 ? 1 : -y._single.CompareTo(x._byte);
                    case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.Double: return y._double < 0 ? 1 : -y._double.CompareTo(x._byte);
                    case (((uint)TypeCode.Byte) << 5) | (uint)TypeCode.Decimal: return y._decimal < 0 ? 1 : -y._decimal.CompareTo(x._byte);

                    // this == Int16
                    case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.SByte: return x._int16.CompareTo(y._sbyte);
                    case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.Byte: return x._int16 < 0 ? -1 : x._int16.CompareTo(y._byte);
                    case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.Int16: return x._int16.CompareTo(y._int16);
                    case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.UInt16: return x._int16 < 0 ? -1 : -y._uint16.CompareTo((ushort)x._int16);
                    case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.Int32: return -y._int32.CompareTo(x._int16);
                    case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.UInt32: return x._int16 < 0 ? -1 : -y._uint32.CompareTo((uint)x._int16);
                    case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.Int64: return -y._int64.CompareTo(x._int16);
                    case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.UInt64: return x._int16 < 0 ? -1 : -y._uint64.CompareTo((ulong)x._int16);
                    case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.Single: return -y._single.CompareTo(x._int16);
                    case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.Double: return -y._double.CompareTo(x._int16);
                    case (((uint)TypeCode.Int16) << 5) | (uint)TypeCode.Decimal: return -y._decimal.CompareTo(x._int16);

                    // this == UInt16
                    case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.SByte: return y._sbyte < 0 ? 1 : x._uint16.CompareTo((ushort)y._sbyte);
                    case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.Byte: return x._uint16.CompareTo(y._byte);
                    case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.Int16: return y._int16 < 0 ? 1 : x._uint16.CompareTo((ushort)y._int16);
                    case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.UInt16: return x._uint16.CompareTo(y._uint16);
                    case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.Int32: return y._int32 < 0 ? 1 : -y._int32.CompareTo(x._uint16);
                    case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.UInt32: return -y._uint32.CompareTo(x._uint16);
                    case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.Int64: return y._int64 < 0 ? 1 : -y._int64.CompareTo(x._uint16);
                    case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.UInt64: return -y._uint64.CompareTo(x._uint16);
                    case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.Single: return y._single < 0 ? 1 : -y._single.CompareTo(x._uint16);
                    case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.Double: return y._double < 0 ? 1 : -y._double.CompareTo(x._uint16);
                    case (((uint)TypeCode.UInt16) << 5) | (uint)TypeCode.Decimal: return y._decimal < 0 ? 1 : -y._decimal.CompareTo(x._uint16);

                    // this == Int32
                    case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.SByte: return x._int32.CompareTo(y._sbyte);
                    case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.Byte: return x._int32 < 0 ? -1 : x._int32.CompareTo(y._byte);
                    case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.Int16: return x._int32.CompareTo(y._int16);
                    case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.UInt16: return x._int32 < 0 ? -1 : x._int32.CompareTo(y._uint16);
                    case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.Int32: return x._int32.CompareTo(y._int32);
                    case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.UInt32: return x._int32 < 0 ? -1 : -y._uint32.CompareTo((uint)x._int32);
                    case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.Int64: return -y._int64.CompareTo(x._int32);
                    case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.UInt64: return x._int32 < 0 ? -1 : -y._uint64.CompareTo((ulong)x._int32);
                    case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.Single: return -y._single.CompareTo(x._int32);
                    case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.Double: return -y._double.CompareTo(x._int32);
                    case (((uint)TypeCode.Int32) << 5) | (uint)TypeCode.Decimal: return -y._decimal.CompareTo(x._int32);

                    // this == UInt32
                    case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.SByte: return y._sbyte < 0 ? 1 : x._uint32.CompareTo((uint)y._sbyte);
                    case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.Byte: return x._uint32.CompareTo(y._byte);
                    case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.Int16: return y._int16 < 0 ? 1 : x._uint32.CompareTo((uint)y._int16);
                    case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.UInt16: return x._uint32.CompareTo(y._uint16);
                    case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.Int32: return y._int32 < 0 ? 1 : x._uint32.CompareTo((uint)y._int32);
                    case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.UInt32: return x._uint32.CompareTo(y._uint32);
                    case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.Int64: return y._int64 < 0 ? 1 : -y._int64.CompareTo(x._uint32);
                    case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.UInt64: return -y._uint64.CompareTo(x._uint32);
                    case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.Single: return y._single < 0 ? 1 : -y._single.CompareTo(x._uint32);
                    case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.Double: return y._double < 0 ? 1 : -y._double.CompareTo(x._uint32);
                    case (((uint)TypeCode.UInt32) << 5) | (uint)TypeCode.Decimal: return y._decimal < 0 ? 1 : -y._decimal.CompareTo(x._uint32);

                    // this == Int64
                    case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.SByte: return x._int64.CompareTo(y._sbyte);
                    case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.Byte: return x._int64 < 0 ? -1 : x._int64.CompareTo(y._byte);
                    case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.Int16: return x._int64.CompareTo(y._int16);
                    case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.UInt16: return x._int64 < 0 ? -1 : x._int64.CompareTo(y._uint16);
                    case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.Int32: return x._int64.CompareTo(y._int32);
                    case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.UInt32: return x._int64 < 0 ? -1 : x._int64.CompareTo(y._uint32);
                    case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.Int64: return x._int64.CompareTo(y._int64);
                    case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.UInt64: return x._int64 < 0 ? -1 : -y._uint64.CompareTo((ulong)x._int64);
                    case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.Single: return -((double)y._single).CompareTo(x._int64);
                    case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.Double: return -y._double.CompareTo(x._int64);
                    case (((uint)TypeCode.Int64) << 5) | (uint)TypeCode.Decimal: return -y._decimal.CompareTo(x._int64);

                    // this == UInt64
                    case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.SByte: return y._sbyte < 0 ? 1 : x._uint64.CompareTo((ulong)y._sbyte);
                    case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.Byte: return x._uint64.CompareTo(y._byte);
                    case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.Int16: return y._int16 < 0 ? 1 : x._uint64.CompareTo((ulong)y._int16);
                    case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.UInt16: return x._uint64.CompareTo(y._uint16);
                    case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.Int32: return y._int32 < 0 ? 1 : x._uint64.CompareTo((ulong)y._int32);
                    case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.UInt32: return x._uint64.CompareTo(y._uint32);
                    case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.Int64: return y._int64 < 0 ? 1 : x._uint64.CompareTo((ulong)y._int64);
                    case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.UInt64: return x._uint64.CompareTo(y._uint64);
                    case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.Single: return y._single < 0 ? 1 : -((double)y._single).CompareTo(x._uint64);
                    case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.Double: return y._double < 0 ? 1 : -y._double.CompareTo(x._uint64);
                    case (((uint)TypeCode.UInt64) << 5) | (uint)TypeCode.Decimal: return y._decimal < 0 ? 1 : -y._decimal.CompareTo(x._uint64);

                    // this == Single
                    case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.SByte: return x._single.CompareTo(y._sbyte);
                    case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.Byte: return x._single < 0 ? -1 : x._single.CompareTo(y._byte);
                    case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.Int16: return x._single.CompareTo(y._int16);
                    case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.UInt16: return x._single < 0 ? -1 : x._single.CompareTo(y._uint16);
                    case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.Int32: return x._single.CompareTo(y._int32);
                    case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.UInt32: return x._single < 0 ? -1 : x._single.CompareTo(y._uint32);
                    case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.Int64: return ((double)x._single).CompareTo(y._int64);
                    case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.UInt64: return x._single < 0 ? -1 : ((double)x._single).CompareTo(y._uint64);
                    case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.Single: return x._single.CompareTo(y._single);
                    case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.Double: return -y._double.CompareTo(x._single);
                    case (((uint)TypeCode.Single) << 5) | (uint)TypeCode.Decimal:
                        return x._single <= SingleDecimalMinValue || x._single >= SingleDecimalMaxValue
                            ? x._single.CompareTo((float)y._decimal)
                            : -y._decimal.CompareTo((decimal)x._single);

                    // this == Double
                    case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.SByte: return x._double.CompareTo(y._sbyte);
                    case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.Byte: return x._double < 0 ? -1 : x._double.CompareTo(y._byte);
                    case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.Int16: return x._double.CompareTo(y._int16);
                    case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.UInt16: return x._double < 0 ? -1 : x._double.CompareTo(y._uint16);
                    case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.Int32: return x._double.CompareTo(y._int32);
                    case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.UInt32: return x._double < 0 ? -1 : x._double.CompareTo(y._uint32);
                    case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.Int64: return x._double.CompareTo(y._int64);
                    case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.UInt64: return x._double < 0 ? -1 : x._double.CompareTo(y._uint64);
                    case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.Single: return x._double.CompareTo(y._single);
                    case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.Double: return x._double.CompareTo(y._double);
                    case (((uint)TypeCode.Double) << 5) | (uint)TypeCode.Decimal:
                        return x._double <= DoubleDecimalMinValue || x._double >= DoubleDecimalMaxValue
                            ? x._double.CompareTo((double)y._decimal)
                            : -y._decimal.CompareTo((decimal)x._double);

                    // this == Decimal
                    case (((uint)TypeCode.Decimal) << 5) | (uint)TypeCode.SByte: return x._decimal.CompareTo(y._sbyte);
                    case (((uint)TypeCode.Decimal) << 5) | (uint)TypeCode.Byte: return x._decimal < 0 ? -1 : x._decimal.CompareTo(y._byte);
                    case (((uint)TypeCode.Decimal) << 5) | (uint)TypeCode.Int16: return x._decimal.CompareTo(y._int16);
                    case (((uint)TypeCode.Decimal) << 5) | (uint)TypeCode.UInt16: return x._decimal < 0 ? -1 : x._decimal.CompareTo(y._uint16);
                    case (((uint)TypeCode.Decimal) << 5) | (uint)TypeCode.Int32: return x._decimal.CompareTo(y._int32);
                    case (((uint)TypeCode.Decimal) << 5) | (uint)TypeCode.UInt32: return x._decimal < 0 ? -1 : x._decimal.CompareTo(y._uint32);
                    case (((uint)TypeCode.Decimal) << 5) | (uint)TypeCode.Int64: return x._decimal.CompareTo(y._int64);
                    case (((uint)TypeCode.Decimal) << 5) | (uint)TypeCode.UInt64: return x._decimal < 0 ? -1 : x._decimal.CompareTo(y._uint64);
                    case (((uint)TypeCode.Decimal) << 5) | (uint)TypeCode.Single:
                        return y._single <= SingleDecimalMinValue || y._single >= SingleDecimalMaxValue
                            ? ((float)x._decimal).CompareTo(y._single)
                            : x._decimal.CompareTo((decimal)y._single);

                    case (((uint)TypeCode.Decimal) << 5) | (uint)TypeCode.Double:
                        return y._double <= DoubleDecimalMinValue || y._double >= DoubleDecimalMaxValue
                            ? ((double)x._decimal).CompareTo(y._double)
                            : x._decimal.CompareTo((decimal)y._double);

                    case (((uint)TypeCode.Decimal) << 5) | (uint)TypeCode.Decimal: return x._decimal.CompareTo(y._decimal);
                }

                return 0;
            }

            #endregion

            #region IEqualityComparer

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override bool Equals(Number x, Number y)
            {
                if (x.ValueTypeCode != y.ValueTypeCode) return false;

                if (x.ValueTypeCode == TypeCode.Decimal)
                    return (x._decimal == y._decimal);

                if ((x.Kind & NumberKinds.Real) > 0)
                    return (x._double == y._double);

                // Int
                return x._uint64 == y._uint64;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override int GetHashCode(Number obj)
            {
                unchecked
                {
                    var hc = 17L;

                    hc = (hc * 23) + (int)obj.ValueTypeCode;

                    if (obj.ValueTypeCode == TypeCode.Decimal)
                        hc = (hc * 23) + obj._decimal.GetHashCode();
                    else if ((obj.Kind & NumberKinds.Real) > 0)
                        hc = (hc * 23) + obj._double.GetHashCode();
                    else // Int
                        hc = (hc * 23) + obj._uint64.GetHashCode();

                    return ((int)(hc >> 32)) ^ (int)hc;
                }
            }

            #endregion
        }

        #endregion
    }
}
