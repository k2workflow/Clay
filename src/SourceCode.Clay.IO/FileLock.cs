#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SourceCode.Clay.IO
{
    /// <summary>
    /// Represents methods for file locking.
    /// </summary>
    public static class FileLock
    {
        #region Methods

        /// <summary>
        /// Asynchronously waits for a lock on a file.
        /// </summary>
        /// <param name="path">The path to the file to lock.</param>
        /// <param name="mode">The mode for opening the file.</param>
        /// <param name="access">The access to request on the file.</param>
        /// <param name="share">The lock sharing option.</param>
        /// <param name="bufferSize">The size of the buffer. The default is 81920.</param>
        /// <param name="pollInterval">The interval in milliseconds at which the filesystem is polled. The default is the timer resolution on the system.</param>
        /// <param name="timeout">The maximum interval in milliseconds that the method will wait.</param>
        /// <param name="cancellationToken">The cancellation token. The default is <see cref="CancellationToken.None" />.</param>
        /// <returns>A <see cref="Task{TResult}" /> that represents the pending file. The value of <see cref="Task{TResult}" /> will contain
        /// the locked file when the operation completes.</returns>
        public static async ValueTask<FileStream> WaitForFileAsync(
            string path,
            FileMode mode,
            FileAccess access = FileAccess.ReadWrite,
            FileShare share = FileShare.None,
            int bufferSize = 81920,
            int pollInterval = 1,
            int timeout = 1000,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            if (pollInterval < 1) throw new ArgumentOutOfRangeException(nameof(pollInterval));
            if (timeout < pollInterval) throw new ArgumentOutOfRangeException(nameof(timeout));

#           pragma warning disable S1854 // Dead stores should be removed
            // Not a dead store

            var tickEnd = Environment.TickCount + timeout;
#           pragma warning restore S1854 // Dead stores should be removed

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                FileStream fs = null;
                try
                {
                    fs = new FileStream(path, mode, access, share, bufferSize);
                    return fs;
                }
                catch (IOException) when (
                    !cancellationToken.IsCancellationRequested &&
                    Environment.TickCount < tickEnd)
                {
                    fs?.Dispose();
                    await Task.Delay(pollInterval, cancellationToken).ConfigureAwait(false);
                }
            }
        }

        #endregion
    }
}
