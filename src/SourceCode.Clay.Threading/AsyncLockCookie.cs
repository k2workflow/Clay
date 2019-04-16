using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace SourceCode.Clay.Threading
{
#   pragma warning disable CA1815 // Override equals and operator equals on value types
    /// <summary>
    /// Represents an acquired or unacquired lock on <see cref="AsyncLock"/>. Dispose
    /// the token to free the lock.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public struct AsyncLockCookie : IDisposable
    {
        private volatile AsyncLock _lock;

        /// <summary>
        /// Gets a value indicating whether the lock was acquired.
        /// </summary>
        public bool IsAcquired => _lock != null;

        [ExcludeFromCodeCoverage] // Debugger display.
        private string DebuggerDisplay => IsAcquired ? "Acquired" : "Unacquired";

        internal AsyncLockCookie(AsyncLock @lock) => _lock = @lock;

        /// <summary>
        /// Releases the lock.
        /// </summary>
        public void Dispose() => Interlocked.Exchange(ref _lock, null)?.Release();

#       pragma warning disable CA2225 // Operator overloads have named alternates
        /// <summary>
        /// Determines whether the spceified <see cref="AsyncLockCookie"/> is acquired.
        /// </summary>
        /// <param name="cookie">The <see cref="AsyncLockCookie"/> to inspect.</param>
        public static implicit operator bool(AsyncLockCookie cookie) => cookie.IsAcquired;
#       pragma warning restore CA2225 // Operator overloads have named alternates
    }
#   pragma warning restore CA1815 // Override equals and operator equals on value types
}
