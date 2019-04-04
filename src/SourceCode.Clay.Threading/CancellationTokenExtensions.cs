#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Threading;

namespace SourceCode.Clay.Threading
{
    /// <summary>
    /// Represents extensions for <see cref="CancellationToken"/>.
    /// </summary>
    public static class CancellationTokenExtensions
    {
        /// <summary>
        /// Returns a <see cref="CancellationTokenSource"/> that is linked to the provided <see cref="CancellationToken"/>,
        /// and configured using the specified <paramref name="timeout"/>.
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> to add a timeout to.</param>
        public static CancellationTokenSource WithTimeout(this CancellationToken cancellationToken, TimeSpan timeout)
        {
            if (timeout == Timeout.InfiniteTimeSpan) throw new ArgumentOutOfRangeException(nameof(timeout));

            if (cancellationToken == CancellationToken.None)
                return new CancellationTokenSource(timeout);

            var cts = CancellationTokenSource
                .CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(timeout);

            return cts;
        }
    }
}
