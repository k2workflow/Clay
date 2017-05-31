using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents a way to compare binary buffers.
    /// </summary>
    public sealed class BufferComparer :
        IEqualityComparer<byte[]>, IEqualityComparer<ArraySegment<byte>>, IEqualityComparer<IReadOnlyList<byte>>, IEqualityComparer<IList<byte>>, IEqualityComparer<IEnumerable<byte>>, IEqualityComparer<ReadOnlySpan<byte>>, IEqualityComparer<Span<byte>>,
        IComparer<byte[]>, IComparer<ArraySegment<byte>>, IComparer<IReadOnlyList<byte>>, IComparer<IList<byte>>, IComparer<IEnumerable<byte>>, IComparer<ReadOnlySpan<byte>>, IComparer<Span<byte>>
    {
        /// <summary>
        /// Gets the number of octets that will be processed when calculating a hashcode.
        /// </summary>
        public const int DefaultHashCodeFidelity = 512;

        /// <summary>
        /// Gets the default instance of the buffer comparer that uses FNV.
        /// </summary>
        /// <value>
        /// The default instance of the buffer comparer that uses FNV.
        /// </value>
        public static BufferComparer Default { get; } = new BufferComparer(DefaultHashCodeFidelity);

        private readonly int _hashCodeFidelity;

        /// <summary>
        /// Creates a new instance of the <see cref="BufferComparer"/> class, that considers the full
        /// buffer when calculating the hashcode.
        /// </summary>
        public BufferComparer()
            : this(-1)
        {

        }

        /// <summary>
        /// Creates a new instance of the <see cref="BufferComparer"/> class.
        /// </summary>
        /// <param name="hashCodeFidelity">
        /// The maximum number of octets that processed when calculating a hashcode. Pass zero or a negative value to
        /// disable the limit.
        /// </param>
        public BufferComparer(int hashCodeFidelity)
        {
            _hashCodeFidelity = hashCodeFidelity;
        }

        #region Native

        private static unsafe class NativeMethods
        {
            [DllImport("msvcrt.dll", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
            [SecurityCritical]
            public static extern int MemCompare(byte* x, byte* y, int count);
        }

        #endregion

        #region Compare

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        [SecuritySafeCritical]
        public int Compare(Span<byte> x, Span<byte> y) =>
            Compare((ReadOnlySpan<byte>)x, y);

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        [SecuritySafeCritical]
        public int Compare(ReadOnlySpan<byte> x, ReadOnlySpan<byte> y)
        {
            var cmp = x.Length.CompareTo(y.Length);
            if (cmp != 0) return cmp; // (m, n)

            if (x.Length == 0) return 0; // (0, 0)
            unsafe
            {
                fixed (byte* xp = &x.DangerousGetPinnableReference(), yp = &y.DangerousGetPinnableReference())
                {
                    return NativeMethods.MemCompare(xp, yp, x.Length);
                }
            }
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        [SecuritySafeCritical]
        public int Compare(byte[] x, byte[] y)
        {
            if (x == null)
            {
                if (y == null) return 0; // (null, null)
                return -1; // (null, y)
            }
            if (y == null) return 1; // (x, null)

            return Compare(new ReadOnlySpan<byte>(x), new ReadOnlySpan<byte>(y));
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        [SecuritySafeCritical]
        public int Compare(ArraySegment<byte> x, ArraySegment<byte> y)
        {
            if (x.Array == null)
            {
                if (y.Array == null) return 0; // (null, null)
                return -1; // (null, y)s
            }
            if (y.Array == null) return 1; // (x, null)

            return Compare(new ReadOnlySpan<byte>(x.Array, x.Offset, x.Count), new ReadOnlySpan<byte>(y.Array, y.Offset, y.Count));
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        [SecuritySafeCritical]
        public int Compare(IReadOnlyList<byte> x, IReadOnlyList<byte> y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            var cmp = x.Count.CompareTo(y.Count);
            if (cmp != 0) return cmp;

            var bax = x as byte[];
            var bay = y as byte[];
            if (bax != null && bay != null) return Compare(bax, bay);

            if (x is ArraySegment<byte> && y is ArraySegment<byte>)
                return Compare((ArraySegment<byte>)x, (ArraySegment<byte>)y);

            for (var i = 0; i < x.Count; i++)
            {
                cmp = x[i].CompareTo(y[i]);
                if (cmp != 0) return cmp;
            }

            return 0;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare(IList<byte> x, IList<byte> y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            var cmp = x.Count.CompareTo(y.Count);
            if (cmp != 0) return cmp;

            var bax = x as byte[];
            var bay = y as byte[];
            if (bax != null && bay != null) return Compare(bax, bay);

            if (x is ArraySegment<byte> && y is ArraySegment<byte>)
                return Compare((ArraySegment<byte>)x, (ArraySegment<byte>)y);

            for (var i = 0; i < x.Count; i++)
            {
                cmp = x[i].CompareTo(y[i]);
                if (cmp != 0) return cmp;
            }

            return 0;
        }

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare(IEnumerable<byte> x, IEnumerable<byte> y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (x == null) return -1;
            if (y == null) return 1;

            var bax = x as byte[];
            var bay = y as byte[];
            if (bax != null && bay != null) return Compare(bax, bay);

            if (x is ArraySegment<byte> && y is ArraySegment<byte>)
                return Compare((ArraySegment<byte>)x, (ArraySegment<byte>)y);

            using (var xe = x.GetEnumerator())
            using (var ye = y.GetEnumerator())
            {
                while (xe.MoveNext())
                {
                    if (!ye.MoveNext()) return 1; // x.Count > y.Count
                    var c = xe.Current.CompareTo(ye.Current);
                    if (c != 0) return c;
                }

                if (ye.MoveNext()) return -1; // y.Count > x.Count
                return 0;
            }
        }

        #endregion

        #region Equals

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        [SecuritySafeCritical]
        public bool Equals(ReadOnlySpan<byte> x, ReadOnlySpan<byte> y) => Compare(x, y) == 0;

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        [SecuritySafeCritical]
        public bool Equals(Span<byte> x, Span<byte> y) => Compare(x, y) == 0;

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        [SecuritySafeCritical]
        public bool Equals(byte[] x, byte[] y) => Compare(x, y) == 0;

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        [SecuritySafeCritical]
        public bool Equals(ArraySegment<byte> x, ArraySegment<byte> y) => Compare(x, y) == 0;

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        [SecuritySafeCritical]
        public bool Equals(IReadOnlyList<byte> x, IReadOnlyList<byte> y) => Compare(x, y) == 0;

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        [SecuritySafeCritical]
        public bool Equals(IList<byte> x, IList<byte> y) => Compare(x, y) == 0;

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        [SecuritySafeCritical]
        public bool Equals(IEnumerable<byte> x, IEnumerable<byte> y) => Compare(x, y) == 0;

        #endregion

        #region GetHashCode

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetHashCode(ReadOnlySpan<byte> obj)
        {
            if (_hashCodeFidelity <= 0 || obj.Length <= _hashCodeFidelity)
                return HashCode.Fnv(obj);

            return HashCode.Fnv(obj.Slice(0, _hashCodeFidelity));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        [SecuritySafeCritical]
        public int GetHashCode(Span<byte> obj)
        {
            if (_hashCodeFidelity <= 0 || obj.Length <= _hashCodeFidelity)
                return HashCode.Fnv(obj);

            return HashCode.Fnv(obj.Slice(0, _hashCodeFidelity));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        [SecuritySafeCritical]
        public int GetHashCode(byte[] obj)
        {
            if (_hashCodeFidelity <= 0 || obj.Length <= _hashCodeFidelity)
                return HashCode.Fnv(obj);

            return HashCode.Fnv(new ReadOnlySpan<byte>(obj, 0, _hashCodeFidelity));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        [SecuritySafeCritical]
        public int GetHashCode(ArraySegment<byte> obj)
        {
            if (_hashCodeFidelity <= 0 || obj.Count <= _hashCodeFidelity)
                return HashCode.Fnv(obj);

            return HashCode.Fnv(new ArraySegment<byte>(obj.Array, obj.Offset, _hashCodeFidelity));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        [SecuritySafeCritical]
        public int GetHashCode(IReadOnlyList<byte> obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (_hashCodeFidelity <= 0 || obj.Count <= _hashCodeFidelity)
                return HashCode.Fnv(obj);

            return HashCode.Fnv(obj.Take(_hashCodeFidelity));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        [SecuritySafeCritical]
        public int GetHashCode(IList<byte> obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (_hashCodeFidelity <= 0 || obj.Count <= _hashCodeFidelity)
                return HashCode.Fnv(obj);

            return HashCode.Fnv(obj.Take(_hashCodeFidelity));
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        [SecuritySafeCritical]
        public int GetHashCode(IEnumerable<byte> obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            if (_hashCodeFidelity <= 0)
                return HashCode.Fnv(obj);

            return HashCode.Fnv(obj.Take(_hashCodeFidelity));
        }

        #endregion
    }
}
