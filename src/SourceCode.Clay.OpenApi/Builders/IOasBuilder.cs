#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents a type that builds another type.
    /// </summary>
    /// <typeparam name="TBuilt">The type that will be built.</typeparam>
    public interface IOasBuilder<out TBuilt>
    {
        /// <summary>
        /// Creates the <typeparamref name="TBuilt"/> from this builder.
        /// </summary>
        /// <returns>The <typeparamref name="TBuilt"/>.</returns>
        TBuilt Build();
    }
}
