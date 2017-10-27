#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents either a reference object or a value.
    /// </summary>
    public interface IOasReferable
    {
        #region Properties

        /// <summary>
        /// Gets the contained value.
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Gets the contained reference.
        /// </summary>
        OasReference Reference { get; }

        /// <summary>
        /// Gets a value indicating whether the referable is a reference.
        /// </summary>
        bool IsReference { get; }

        /// <summary>
        /// Gets a value indicating whether the reference is a value.
        /// </summary>
        bool IsValue { get; }

        /// <summary>
        /// Gets a value indicating whether the reference is null.
        /// </summary>
        bool HasValue { get; }

        #endregion
    }
}
