using System;
using System.Buffers;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using crypt = System.Security.Cryptography;

namespace SourceCode.Clay
{
    public static class Sha1Extensions
    {
        private static readonly Sha1 s_empty1 = Sha1.Parse("da39a3ee-5e6b4b0d-3255bfef-95601890-afd80709"); // Well-known

        /// <summary>
        /// Hashes the specified bytes.
        /// </summary>
        /// <param name="alg">The SHA1 instance to use.</param>
        /// <param name="span">The bytes to hash.</param>
        /// <returns></returns>
        public static Sha1 HashData(this crypt.SHA1 alg, ReadOnlySpan<byte> span)
        {
            if (alg == null) throw new ArgumentNullException(nameof(alg));
            if (span.Length == 0) return s_empty1;

            Sha1 sha = HashImpl(alg, span);
            return sha;
        }

        /// <summary>
        /// Hashes the specified value using utf8 encoding.
        /// </summary>
        /// <param name="alg">The SHA1 instance to use.</param>
        /// <param name="value">The string to hash.</param>
        /// <returns></returns>
        public static Sha1 HashData(this crypt.SHA1 alg, string value)
        {
            if (alg == null) throw new ArgumentNullException(nameof(alg));
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (value.Length == 0) return s_empty1;

            int maxLen = Encoding.UTF8.GetMaxByteCount(value.Length); // Utf8 is 1-4 bpc

            byte[] rented = ArrayPool<byte>.Shared.Rent(maxLen);
            try
            {
                int count = Encoding.UTF8.GetBytes(value, 0, value.Length, rented, 0);

                var span = new ReadOnlySpan<byte>(rented, 0, count);

                Sha1 sha = HashImpl(alg, span);
                return sha;
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }

        /// <summary>
        /// Hashes the specified bytes.
        /// </summary>
        /// <param name="alg">The SHA1 instance to use.</param>
        /// <param name="bytes">The bytes to hash.</param>
        /// <returns></returns>
        public static Sha1 HashData(this crypt.SHA1 alg, byte[] bytes)
        {
            if (alg == null) throw new ArgumentNullException(nameof(alg));
            if (bytes is null) throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length == 0) return s_empty1;

            var span = new ReadOnlySpan<byte>(bytes);

            Sha1 sha = HashImpl(alg, span);
            return sha;
        }

        /// <summary>
        /// Hashes the specified <paramref name="bytes"/>, starting at the specified <paramref name="start"/> and <paramref name="length"/>.
        /// </summary>
        /// <param name="alg">The SHA1 instance to use.</param>
        /// <param name="bytes">The bytes to hash.</param>
        /// <param name="start">The offset.</param>
        /// <param name="length">The count.</param>
        /// <returns></returns>
        public static Sha1 HashData(this crypt.SHA1 alg, byte[] bytes, in int start, in int length)
        {
            if (alg == null) throw new ArgumentNullException(nameof(alg));
            if (bytes is null) throw new ArgumentNullException(nameof(bytes));

            var span = new ReadOnlySpan<byte>(bytes, start, length); // Check validity of start/length
            if (length == 0) return s_empty1;

            Sha1 sha = HashImpl(alg, span);
            return sha;
        }

        /// <summary>
        /// Hashes the specified stream.
        /// </summary>
        /// <param name="alg">The SHA1 instance to use.</param>
        /// <param name="stream">The stream to hash.</param>
        /// <returns></returns>
        public static Sha1 HashData(this crypt.SHA1 alg, in Stream stream)
        {
            if (alg == null) throw new ArgumentNullException(nameof(alg));
            if (stream is null) throw new ArgumentNullException(nameof(stream));

            // Note that length==0 should NOT short-circuit

            byte[] hash = alg.ComputeHash(stream);

            var sha = new Sha1(hash);
            return sha;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Sha1 HashImpl(crypt.SHA1 alg, ReadOnlySpan<byte> span)
        {
            Debug.Assert(alg != null);

            // Do NOT short-circuit here; rely on call-sites to do so
#if NETCOREAPP
            Span<byte> hash = stackalloc byte[Sha1.ByteLength];
            alg.TryComputeHash(span, hash, out _);
#else
            var hash = alg.ComputeHash(span.ToArray());
#endif
            var sha = new Sha1(hash);
            return sha;
        }
    }
}
