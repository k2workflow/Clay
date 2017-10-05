using System;

namespace SourceCode.Clay.Json.Pointers
{
    /// <summary>
    ///   Determines options for evaluating a <see cref="JsonPointer"/>.
    /// </summary>
    [Flags]
    public enum JsonPointerEvaluationOptions : byte
    {
        /// <summary>
        ///   The default options.
        /// </summary>
        None = 0,

        /// <summary>
        ///   Null values will be coalesced.
        /// </summary>
        NullCoalescing = 1,

        /// <summary>
        ///   Objects yield null when indexed by a missing member.
        /// </summary>
        MissingMembersAreNull = 2,

        /// <summary>
        ///   String and boolean values yield null when indexed.
        /// </summary>
        PrimitiveMembersAndIndiciesAreNull = 4,

        /// <summary>
        ///   Arrays yield null when indexed by name.
        /// </summary>
        ArrayMembersAreNull = 8,

        /// <summary>
        ///   Arrays yield null when index by the new index ('-') or when outside of the index range.
        /// </summary>
        InvalidIndiciesAreNull = 16,

        /// <summary>
        ///   Evaluation returns null when any error condition is encountered.
        /// </summary>
        NeverFail = 0xFF
    }
}
