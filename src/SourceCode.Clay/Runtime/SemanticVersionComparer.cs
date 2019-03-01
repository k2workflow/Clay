#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Collections.Generic;

namespace SourceCode.Clay.Runtime
{
    /// <summary>
    /// Represents a way to compare different <see cref="SemanticVersion"/> values.
    /// </summary>
    public abstract partial class SemanticVersionComparer : IEqualityComparer<SemanticVersion>, IComparer<SemanticVersion>
    {
        private SemanticVersionComparer()
        { }

        /// <inheritdoc/>
        public abstract int Compare(SemanticVersion x, SemanticVersion y);

        /// <inheritdoc/>
        public abstract bool Equals(SemanticVersion x, SemanticVersion y);

        /// <inheritdoc/>
        public abstract int GetHashCode(SemanticVersion obj);
    }
}
