#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents extensions for <see cref="Exception"/>.
    /// </summary>
    public static class ExceptionExtensions
    {
#pragma warning disable RECS0154 // Parameter is never used

        /// <summary>
        /// Does nothing.
        /// </summary>
        /// <remarks>
        /// Used to indicate that an exception has been intentionally suppressed.
        /// </remarks>
        /// <param name="exception">The exception to suppress.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Exception Suppress(this Exception exception)
        {
            // Do nothing by design
            return exception;
        }

#pragma warning restore RECS0154 // Parameter is never used
    }
}
