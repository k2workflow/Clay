using System;
using System.Threading;
using System.Threading.Tasks;

namespace SourceCode.Clay.Threading
{
    /// <summary>
    /// Represents a lock that can operate under an asynchronous context.
    /// </summary>
    public sealed class AsyncLock : IDisposable
    {
        private readonly SemaphoreSlim _semaphore;

        /// <summary>
        /// Creates a new instance of the <see cref="AsyncLock"/> class.
        /// </summary>
        public AsyncLock()
        {
            _semaphore = new SemaphoreSlim(1);
        }

        internal void Release() => _semaphore.Release();

        private AsyncLockCookie Wait(bool result)
            => result
            ? new AsyncLockCookie(this)
            : new AsyncLockCookie();

        private ValueTask<AsyncLockCookie> WaitAsync(Task<bool> wait)
        {
            async ValueTask<AsyncLockCookie> ImplAsync(Task<bool> task)
                => Wait(await task);

            return wait.IsCompleted
                ? new ValueTask<AsyncLockCookie>(Wait(wait.Result))
                : ImplAsync(wait);
        }

        private bool TryAcquire(out ValueTask<AsyncLockCookie> result, bool ping, CancellationToken cancellationToken)
        {
            var acquired = _semaphore.Wait(0, cancellationToken);
            if (acquired || ping)
            {
                result = new ValueTask<AsyncLockCookie>(Wait(acquired));
                return true;
            }
            result = default;
            return false;
        }

        /// <summary>
        /// Asynchronously waits to acquire the <see cref="AsyncLock"/>, using a 32-bit signed integer to measure the
        /// time interval, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait, <see cref="Timeout.Infinite"/> (-1) is the default. Specify
        /// zero to test the state of the lock and return immediately.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe.
        /// </param>
        /// <returns>
        /// A task that will complete with a <see cref="AsyncLockCookie"/>.
        /// </returns>
        public ValueTask<AsyncLockCookie> AcquireAsync(int millisecondsTimeout = Timeout.Infinite, CancellationToken cancellationToken = default)
            => TryAcquire(out ValueTask<AsyncLockCookie> result, millisecondsTimeout == 0, cancellationToken)
            ? result
            : WaitAsync(_semaphore.WaitAsync(millisecondsTimeout, cancellationToken));

        /// <summary>
        /// Asynchronously waits to acquire the <see cref="AsyncLock"/>, using a 32-bit signed integer to measure the
        /// time interval, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait. Specify <see cref="Timeout.InfiniteTimeSpan"/> to wait indefinitely, or
        /// <see cref="TimeSpan.Zero"/> to test the state of the lock and return immediately.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe.
        /// </param>
        /// <returns>
        /// A task that will complete with a <see cref="AsyncLockCookie"/>.
        /// </returns>
        public ValueTask<AsyncLockCookie> AcquireAsync(TimeSpan timeout, CancellationToken cancellationToken = default)
            => TryAcquire(out ValueTask<AsyncLockCookie> result, timeout.Ticks == 0, cancellationToken)
            ? result
            : WaitAsync(_semaphore.WaitAsync(timeout, cancellationToken));

        /// <summary>
        /// Waits to acquire the <see cref="AsyncLock"/>, using a 32-bit signed integer to measure the
        /// time interval, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait, <see cref="Timeout.Infinite"/> (-1) is the default. Specify
        /// zero to test the state of the lock and return immediately.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe.
        /// </param>
        /// <returns>
        /// A <see cref="AsyncLockCookie"/>.
        /// </returns>
        public AsyncLockCookie Acquire(int millisecondsTimeout = Timeout.Infinite, CancellationToken cancellationToken = default)
            => Wait(_semaphore.Wait(millisecondsTimeout, cancellationToken));

        /// <summary>
        /// Waits to acquire the <see cref="AsyncLock"/>, using a 32-bit signed integer to measure the
        /// time interval, while observing a <see cref="CancellationToken"/>.
        /// </summary>
        /// <param name="millisecondsTimeout">
        /// The number of milliseconds to wait. Specify <see cref="Timeout.InfiniteTimeSpan"/> to wait indefinitely, or
        /// <see cref="TimeSpan.Zero"/> to test the state of the lock and return immediately.
        /// </param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe.
        /// </param>
        /// <returns>
        /// A <see cref="AsyncLockCookie"/>.
        /// </returns>
        public AsyncLockCookie Acquire(TimeSpan timeout, CancellationToken cancellationToken = default)
            => Wait(_semaphore.Wait(timeout, cancellationToken));
        
        /// <summary>
        /// Releases any resources used by the <see cref="AsyncLock"/>.
        /// </summary>
        public void Dispose() => _semaphore.Dispose();
    }
}
