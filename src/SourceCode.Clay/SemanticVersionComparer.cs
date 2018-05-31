#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents a way to compare different <see cref="SemanticVersion"/> values.
    /// </summary>
    public abstract class SemanticVersionComparer : IEqualityComparer<SemanticVersion>, IComparer<SemanticVersion>
    {
        #region Constants

        /// <summary>
        /// Gets a <see cref="SemanticVersionComparer"/> that compares all fields of a <see cref="SemanticVersion"/>
        /// value.
        /// </summary>
        public static SemanticVersionComparer Strict { get; } = new StrictSemanticVersionComparer();

        /// <summary>
        /// Gets a <see cref="SemanticVersionComparer"/> that only compares the <see cref="SemanticVersion.Major"/>,
        /// <see cref="SemanticVersion.Minor"/>, <see cref="SemanticVersion.Patch"/> and <see cref="SemanticVersion.PreRelease"/>
        /// values.
        /// </summary>
        public static SemanticVersionComparer Standard { get; } = new StandardSemanticVersionComparer();

        #endregion

        #region Constructors

        private SemanticVersionComparer()
        { }

        #endregion

        #region IComparer

        /// <inheritdoc/>
        public abstract int Compare(SemanticVersion x, SemanticVersion y);

        #endregion

        #region IEqualityComparer

        /// <inheritdoc/>
        public abstract bool Equals(SemanticVersion x, SemanticVersion y);

        /// <inheritdoc/>
        public abstract int GetHashCode(SemanticVersion obj);

        #endregion

        #region Concrete

        private sealed class StrictSemanticVersionComparer : SemanticVersionComparer
        {
            #region Methods

            public override int Compare(SemanticVersion x, SemanticVersion y)
            {
                var cmp = x.Major.CompareTo(y.Major);
                if (cmp != 0) return cmp;

                cmp = x.Minor.CompareTo(y.Minor);
                if (cmp != 0) return cmp;

                cmp = x.Patch.CompareTo(y.Patch);
                if (cmp != 0) return cmp;

                cmp = string.CompareOrdinal(x.PreRelease, y.PreRelease);
                if (cmp != 0) return cmp;

                cmp = string.CompareOrdinal(x.BuildMetadata, y.BuildMetadata);
                return cmp;
            }

            public override bool Equals(SemanticVersion x, SemanticVersion y)
            {
                if (x.Major != y.Major) return false;
                if (x.Minor != y.Minor) return false;
                if (x.Patch != y.Patch) return false;
                if (!StringComparer.Ordinal.Equals(x.PreRelease, y.PreRelease)) return false;
                if (!StringComparer.Ordinal.Equals(x.BuildMetadata, y.BuildMetadata)) return false;

                return true;
            }

            public override int GetHashCode(SemanticVersion obj)
            {
                var hash = new HashCode();
                hash.Add(obj.Major);
                hash.Add(obj.Minor);
                hash.Add(obj.Patch);

                if (obj.PreRelease != null)
                {
                    hash.Add(obj.PreRelease, StringComparer.Ordinal);
                }

                if (obj.BuildMetadata != null)
                {
                    hash.Add(obj.BuildMetadata, StringComparer.Ordinal);
                }

                var hc = hash.ToHashCode();
                return hc;
            }

            #endregion
        }

        private sealed class StandardSemanticVersionComparer : SemanticVersionComparer
        {
            #region Methods

            public override int Compare(SemanticVersion x, SemanticVersion y)
            {
                var cmp = x.Major.CompareTo(y.Major);
                if (cmp != 0) return cmp;

                cmp = x.Minor.CompareTo(y.Minor);
                if (cmp != 0) return cmp;

                cmp = x.Patch.CompareTo(y.Patch);
                if (cmp != 0) return cmp;

                cmp = string.CompareOrdinal(x.PreRelease, y.PreRelease);
                return cmp;
            }

            public override bool Equals(SemanticVersion x, SemanticVersion y)
            {
                if (x.Major != y.Major) return false;
                if (x.Minor != y.Minor) return false;
                if (x.Patch != y.Patch) return false;
                if (!StringComparer.Ordinal.Equals(x.PreRelease, y.PreRelease)) return false;

                return true;
            }

            public override int GetHashCode(SemanticVersion obj)
            {
                var hash = new HashCode();
                hash.Add(obj.Major);
                hash.Add(obj.Minor);
                hash.Add(obj.Patch);

                if (obj.PreRelease != null)
                {
                    hash.Add(obj.PreRelease, StringComparer.Ordinal);
                }

                var hc = hash.ToHashCode();
                return hc;
            }

            #endregion
        }

        #endregion
    }
}
