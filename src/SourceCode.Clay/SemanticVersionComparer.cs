using System;
using System.Collections.Generic;

namespace SourceCode.Clay
{
    /// <summary>
    ///   Represents a way to compare different <see cref="SemanticVersion"/> values.
    /// </summary>
    public abstract class SemanticVersionComparer : IEqualityComparer<SemanticVersion>, IComparer<SemanticVersion>
    {
        #region Constants

        /// <summary>
        ///   Gets a <see cref="SemanticVersionComparer"/> that only compares the <see
        ///   cref="SemanticVersion.Major"/>, <see cref="SemanticVersion.Minor"/>, <see
        ///   cref="SemanticVersion.Patch"/> and <see cref="SemanticVersion.PreRelease"/> values.
        /// </summary>
        public static SemanticVersionComparer Standard { get; } = new StandardSemanticVersionComparer();

        /// <summary>
        ///   Gets a <see cref="SemanticVersionComparer"/> that compares all fields of a <see
        ///   cref="SemanticVersion"/> value.
        /// </summary>
        public static SemanticVersionComparer Strict { get; } = new StrictSemanticVersionComparer();

        #endregion Constants

        #region Constructors

        protected SemanticVersionComparer()
        { }

        #endregion Constructors

        #region IComparer

        /// <inheritdoc/>
        public abstract int Compare(SemanticVersion x, SemanticVersion y);

        #endregion IComparer

        #region IEqualityComparer

        /// <inheritdoc/>
        public abstract bool Equals(SemanticVersion x, SemanticVersion y);

        /// <inheritdoc/>
        public abstract int GetHashCode(SemanticVersion obj);

        #endregion IEqualityComparer

        #region Concrete

        private sealed class StandardSemanticVersionComparer : SemanticVersionComparer
        {
            #region Constructors

            internal StandardSemanticVersionComparer()
            { }

            #endregion Constructors

            #region Methods

            public override int Compare(SemanticVersion x, SemanticVersion y)
            {
                var cmp = x.Major.CompareTo(y.Major);
                if (cmp != 0) return cmp;

                cmp = x.Minor.CompareTo(y.Minor);
                if (cmp != 0) return cmp;

                cmp = x.Patch.CompareTo(y.Patch);
                if (cmp != 0) return cmp;

                cmp = StringComparer.Ordinal.Compare(x.PreRelease, y.PreRelease);
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
                var hc = 17L;

                unchecked
                {
                    hc = hc * 23 + obj.Major;
                    hc = hc * 23 + obj.Minor;
                    hc = hc * 23 + obj.Patch;
                    hc = hc * 23 + StringComparer.Ordinal.GetHashCode(obj.PreRelease);
                }

                return ((int)(hc >> 32)) ^ (int)hc;
            }

            #endregion Methods
        }

        private sealed class StrictSemanticVersionComparer : SemanticVersionComparer
        {
            #region Constructors

            internal StrictSemanticVersionComparer()
            { }

            #endregion Constructors

            #region Methods

            public override int Compare(SemanticVersion x, SemanticVersion y)
            {
                var cmp = x.Major.CompareTo(y.Major);
                if (cmp != 0) return cmp;

                cmp = x.Minor.CompareTo(y.Minor);
                if (cmp != 0) return cmp;

                cmp = x.Patch.CompareTo(y.Patch);
                if (cmp != 0) return cmp;

                cmp = StringComparer.Ordinal.Compare(x.PreRelease, y.PreRelease);
                if (cmp != 0) return cmp;

                cmp = StringComparer.Ordinal.Compare(x.BuildMetadata, y.BuildMetadata);
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
                var hc = 17L;

                unchecked
                {
                    hc = hc * 23 + obj.Major;
                    hc = hc * 23 + obj.Minor;
                    hc = hc * 23 + obj.Patch;
                    hc = hc * 23 + StringComparer.Ordinal.GetHashCode(obj.PreRelease);
                    hc = hc * 23 + StringComparer.Ordinal.GetHashCode(obj.BuildMetadata);
                }

                return ((int)(hc >> 32)) ^ (int)hc;
            }

            #endregion Methods
        }

        #endregion Concrete
    }
}
