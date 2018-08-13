#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.InteropServices;

// Idea derived from various discussions, not limited to:
// https://github.com/dotnet/corefx/issues/28015
// https://github.com/dotnet/csharplang/issues/696
// https://github.com/dotnet/corefx/issues/28025
// NetFx itself has a similar internally-used class, System.Void.

namespace SourceCode.Clay
{
    /// <summary>
    /// An empty struct, used to represent <see cref="void"/> in generic types.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Size = 1)] // Size MUST be > 0: https://github.com/dotnet/csharplang/issues/696#issuecomment-310525451
    public readonly struct VoidResult : IEquatable<VoidResult>
    {
        /// <inheritdoc/>
        public bool Equals(VoidResult other) => true; // Invariant homogeneous equality

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => obj is VoidResult other;

        /// <inheritdoc/>
        public override int GetHashCode() => 0;

        /// <inheritdoc/>
        public static bool operator ==(VoidResult x, VoidResult y) => true;

        /// <inheritdoc/>
        public static bool operator !=(VoidResult x, VoidResult y) => false;
    }
}