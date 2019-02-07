#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;

namespace SourceCode.Clay
{
    partial class SemanticVersionComparer
    {
        /// <summary>
        /// Gets a <see cref="SemanticVersionComparer"/> that compares all fields of a <see cref="SemanticVersion"/>
        /// value.
        /// </summary>
        public static SemanticVersionComparer Strict { get; } = new StrictSemanticVersionComparer();

        private sealed class StrictSemanticVersionComparer : SemanticVersionComparer
        {
            public override int Compare(SemanticVersion x, SemanticVersion y)
            {
                int cmp = x.Major.CompareTo(y.Major);
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
#if !NETSTANDARD2_0
                var hash = new HashCode();
                hash.Add(obj.Major);
                hash.Add(obj.Minor);
                hash.Add(obj.Patch);

                if (!(obj.PreRelease is null))
                {
                    hash.Add(obj.PreRelease, StringComparer.Ordinal);
                }

                if (!(obj.BuildMetadata is null))
                {
                    hash.Add(obj.BuildMetadata, StringComparer.Ordinal);
                }

                var hc = hash.ToHashCode();
#else
                int hc = 11;

                unchecked
                {
                    hc = hc * 7 + obj.Major.GetHashCode();
                    hc = hc * 7 + obj.Minor.GetHashCode();
                    hc = hc * 7 + obj.Patch.GetHashCode();
                    hc = hc * 7 + (obj.PreRelease?.Length ?? 0);
                    hc = hc * 7 + (obj.BuildMetadata?.Length ?? 0);
                }
#endif

                return hc;
            }
        }
    }
}
