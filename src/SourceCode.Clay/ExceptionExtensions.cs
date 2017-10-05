using System;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay
{
    /// <summary>
    ///   Represents extensions for <see cref="Exception"/>.
    /// </summary>
    public static class ExceptionExtensions
    {
#pragma warning disable RECS0154 // Parameter is never used

        #region Methods

        /// <summary>
        ///   Does nothing.
        /// </summary>
        /// <remarks>Used to indicate that an exception has been intentionally suppressed.</remarks>
        /// <param name="exception">The exception to suppress.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Exception Suppress(this Exception exception)
        {
            // Do nothing by design
            return exception;
        }

        #endregion Methods

#pragma warning restore RECS0154 // Parameter is never used
    }
}
