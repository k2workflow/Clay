using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace SourceCode.Clay.Buffers
{
    /// <summary>
    /// Represents a way to compare binary buffers.
    /// </summary>
    public sealed partial class BufferComparer : IEqualityComparer<ReadOnlySpan<byte>>, IComparer<ReadOnlySpan<byte>>
    {
        #region Constants

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

        #endregion

        #region Fields

        private readonly int _hashCodeFidelity;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="BufferComparer"/> class, that considers the full
        /// buffer when calculating the hashcode.
        /// </summary>
        public BufferComparer()
            : this(-1)
        { }

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

        #endregion

        #region Native

        private static unsafe class NativeMethods
        {
            [DllImport("msvcrt.dll", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
            [SecurityCritical]
            public static extern int MemCompare(byte* x, byte* y, int count);
        }

        #endregion

        #region IComparer

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// A signed integer that indicates the relative values of <paramref name="x" /> and <paramref name="y" />, as shown in the following table.Value Meaning Less than zero<paramref name="x" /> is less than <paramref name="y" />.Zero<paramref name="x" /> equals <paramref name="y" />.Greater than zero<paramref name="x" /> is greater than <paramref name="y" />.
        /// </returns>
        public int Compare(ReadOnlySpan<byte> x, ReadOnlySpan<byte> y)
            => CompareReadOnlySpan(x, y);

        [SecuritySafeCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CompareReadOnlySpan(ReadOnlySpan<byte> x, ReadOnlySpan<byte> y)
        {
            // From https://github.com/dotnet/corefx/blob/master/src/System.Memory/src/System/ReadOnlySpan.cs
            // public static bool operator ==
            // Returns true if left and right point at the same memory and have the same length.  Note that
            // this does *not* check to see if the *contents* are equal.
            if (x == y) return 0;

            if (x.IsEmpty)
            {
                if (y.IsEmpty) return 0; // (null, null)
                return -1; // (null, y)
            }
            if (y.IsEmpty) return 1; // (x, null)

            var cmp = x.Length.CompareTo(y.Length);
            if (cmp != 0) return cmp; // (m, n)

            switch (x.Length)
            {
                // (0, 0)
                case 0:
                    return 0;

                // (m[0], n[0])
                case 1:
                    cmp = x[0].CompareTo(y[0]);
                    return cmp;

                // (m[0..N], n[0..N])
                default:
                    unsafe
                    {
                        fixed (byte* xp = &x.DangerousGetPinnableReference())
                        fixed (byte* yp = &y.DangerousGetPinnableReference())
                        {
                            cmp = NativeMethods.MemCompare(xp, yp, x.Length);
                            return cmp;
                        }
                    }
            }
        }

        #endregion

        #region IEqualityComparer

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public bool Equals(ReadOnlySpan<byte> x, ReadOnlySpan<byte> y)
            => Compare(x, y) == 0;

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public int GetHashCode(ReadOnlySpan<byte> obj)
        {
            if (_hashCodeFidelity <= 0 || obj.Length <= _hashCodeFidelity)
                return HashCode.Fnv(obj);

            var obk = obj.Slice(0, _hashCodeFidelity);
            return HashCode.Fnv(obk);
        }

        #endregion
    }
}
