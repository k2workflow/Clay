#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

namespace SourceCode.Clay.Distributed
{
    /// <summary>
    /// Represents a factory for <see cref="DistributedId"/> values.
    /// </summary>
    public interface IDistributedIdFactory
    {
        /// <summary>
        /// Creates a new <see cref="DistributedId"/>.
        /// </summary>
        /// <returns>The <see cref="DistributedId"/>.</returns>
        DistributedId Create();
    }
}
