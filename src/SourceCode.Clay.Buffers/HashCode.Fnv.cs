using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents a way to calculate hash codes.
    /// </summary>
    public static partial class HashCode // .Fnv
    {
        private const uint FnvOffsetBasis = 0x811C9DC5u;
        private const uint FnvPrime = 0x01000193u;

        #region Root Implementations

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">A pointer to the initial octet in the data.</param>
        /// <param name="count">The number of octets to include in the hash code.</param>
        /// <returns>The hash code.</returns>
        public static int Fnv(ReadOnlySpan<byte> buffer)
        {
            var h = FnvOffsetBasis;

            for (var i = 0; i < buffer.Length; i++)
            {
                var c = buffer[i];
                h = unchecked((h ^ c) * FnvPrime);
            }

            return unchecked((int)h);
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">A pointer to the initial octet in the data.</param>
        /// <param name="count">The number of octets to include in the hash code.</param>
        /// <returns>The hash code.</returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(byte* buffer, int count)
        {
            if (buffer == default(byte*)) throw new ArgumentNullException(nameof(buffer));
            return Fnv(new ReadOnlySpan<byte>(buffer, count));
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="fields">The fields to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(IReadOnlyList<int> fields)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));

            int* data = stackalloc int[1];
            var bdata = (byte*)data;

            var h = FnvOffsetBasis;

            for (var i = 0; i < fields.Count; i++)
            {
                data[0] = fields[i];

                h = unchecked((h ^ bdata[0]) * FnvPrime);
                h = unchecked((h ^ bdata[1]) * FnvPrime);
                h = unchecked((h ^ bdata[2]) * FnvPrime);
                h = unchecked((h ^ bdata[3]) * FnvPrime);
            }

            return unchecked((int)h);
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="fields">The fields to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(IEnumerable<int> fields)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));

            int* data = stackalloc int[1];
            var bdata = (byte*)data;

            var h = FnvOffsetBasis;

            foreach (var field in fields)
            {
                data[0] = field;

                h = unchecked((h ^ bdata[0]) * FnvPrime);
                h = unchecked((h ^ bdata[1]) * FnvPrime);
                h = unchecked((h ^ bdata[2]) * FnvPrime);
                h = unchecked((h ^ bdata[3]) * FnvPrime);
            }

            return unchecked((int)h);
        }

        #endregion

        #region Overloads

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The array containing the range of bytes to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        public static unsafe int Fnv(params byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (buffer.Length == 0) return unchecked((int)FnvOffsetBasis);

            fixed (byte* b = &buffer[0])
            {
                return Fnv(b, buffer.Length);
            }
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The array containing the range of bytes to include in the hash code.</param>
        /// <param name="offset">The zero-based index of the first element in the range.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(byte[] buffer, int offset, int count)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
            if (offset > buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));
            if (offset + count > buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));

            if (count == 0) return unchecked((int)FnvOffsetBasis);

            fixed (byte* b = &buffer[offset])
            {
                return Fnv(b, count);
            }
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The range of bytes to incldue in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(ArraySegment<byte> buffer)
        {
            if (buffer.Array == null) throw new ArgumentNullException(nameof(buffer));
            if (buffer.Count == 0) return unchecked((int)FnvOffsetBasis);

            fixed (byte* b = &buffer.Array[buffer.Offset])
            {
                return Fnv(b, buffer.Count);
            }
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The range of bytes to incldue in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(IReadOnlyList<byte> buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (buffer.Count == 0) return unchecked((int)FnvOffsetBasis);

            var h = FnvOffsetBasis;
            for (var i = 0; i < buffer.Count; i++)
            {
                var c = buffer[i];
                h = unchecked((h ^ c) * FnvPrime);
            }
            return unchecked((int)h);
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The range of bytes to incldue in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(IList<byte> buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (buffer.Count == 0) return unchecked((int)FnvOffsetBasis);

            var h = FnvOffsetBasis;
            for (var i = 0; i < buffer.Count; i++)
            {
                var c = buffer[i];
                h = unchecked((h ^ c) * FnvPrime);
            }
            return unchecked((int)h);
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="buffer">The range of bytes to incldue in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(IEnumerable<byte> buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            var h = FnvOffsetBasis;
            foreach (var c in buffer)
            {
                h = unchecked((h ^ c) * FnvPrime);
            }
            return unchecked((int)h);
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="a">The first hash code to include in the hash code.</param>
        /// <param name="b">The second hash code to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(int a, int b)
        {
            int* data = stackalloc int[2];
            data[0] = a;
            data[1] = b;

            return Fnv((byte*)data, 2 * sizeof(int));
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="a">The first hash code to include in the hash code.</param>
        /// <param name="b">The second hash code to include in the hash code.</param>
        /// <param name="c">The third hash code to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(int a, int b, int c)
        {
            int* data = stackalloc int[3];
            data[0] = a;
            data[1] = b;
            data[2] = c;

            return Fnv((byte*)data, 3 * sizeof(int));
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="a">The first hash code to include in the hash code.</param>
        /// <param name="b">The second hash code to include in the hash code.</param>
        /// <param name="c">The third hash code to include in the hash code.</param>
        /// <param name="d">The fourth hash code to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(int a, int b, int c, int d)
        {
            int* data = stackalloc int[4];
            data[0] = a;
            data[1] = b;
            data[2] = c;
            data[3] = d;

            return Fnv((byte*)data, 4 * sizeof(int));
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="a">The first hash code to include in the hash code.</param>
        /// <param name="b">The second hash code to include in the hash code.</param>
        /// <param name="c">The third hash code to include in the hash code.</param>
        /// <param name="d">The fourth hash code to include in the hash code.</param>
        /// <param name="e">The fifth hash code to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(int a, int b, int c, int d, int e)
        {
            int* data = stackalloc int[5];
            data[0] = a;
            data[1] = b;
            data[2] = c;
            data[3] = d;
            data[4] = e;

            return Fnv((byte*)data, 5 * sizeof(int));
        }

        /// <summary>
        /// Calculates a hash code from the specified data using the
        /// Fowler/Noll/Vo 1-a algorithm.
        /// </summary>
        /// <param name="fields">The fields to include in the hash code.</param>
        /// <returns>
        /// The hash code.
        /// </returns>
        [SecurityCritical]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Fnv(params int[] fields)
        {
            if (fields == null) throw new ArgumentNullException(nameof(fields));
            if (fields.Length == 0) return unchecked((int)FnvOffsetBasis);

            fixed (int* data = &fields[0])
            {
                return Fnv((byte*)data, fields.Length * sizeof(int));
            }
        }

        #endregion
    }
}
