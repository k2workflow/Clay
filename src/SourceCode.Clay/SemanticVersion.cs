#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Globalization;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents a semantic version that conforms to the <a href="http://semver.org/spec/v2.0.0.html">v2.0.0 specification</a>.
    /// </summary>
    public readonly struct SemanticVersion : IComparable<SemanticVersion>, IEquatable<SemanticVersion>, IFormattable
    {
        private static readonly SemanticVersion s_empty;

        /// <summary>
        /// Gets the default value of <see cref="SemanticVersion"/>.
        /// </summary>
        public static ref readonly SemanticVersion Empty => ref s_empty;

        /// <summary>
        /// Gets the major version.
        /// </summary>
        public int Major { get; }

        /// <summary>
        /// Gets the minor version.
        /// </summary>
        public int Minor { get; }

        /// <summary>
        /// Gets the patch version.
        /// </summary>
        public int Patch { get; }

        /// <summary>
        /// Gets the pre-release version.
        /// </summary>
        public string PreRelease { get; }

        /// <summary>
        /// Gets the build metadata.
        /// </summary>
        public string BuildMetadata { get; }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/> with the specified
        /// <see cref="Major"/>, <see cref="Minor"/> and <see cref="Patch"/>
        /// values.
        /// </summary>
        /// <param name="major">The major version.</param>
        /// <param name="minor">The minor version.</param>
        /// <param name="patch">The patch version.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para>
        ///     Thrown in any of the following cases:
        ///     <list type="bullet">
        ///         <item><description><paramref name="major"/> is less than 0.</description></item>
        ///         <item><description><paramref name="minor"/> is less than 0.</description></item>
        ///         <item><description><paramref name="patch"/> is less than 0.</description></item>
        ///     </list>
        /// </para>
        /// </exception>
        public SemanticVersion(int major, int minor, int patch)
            : this(major, minor, patch, null, null)
        { }

        /// <summary>
        /// Creates a new <see cref="SemanticVersion"/> with the specified
        /// <see cref="Major"/>, <see cref="Minor"/>, <see cref="Patch"/>
        /// and <see cref="PreRelease"/> values.
        /// </summary>
        /// <param name="major">The major version.</param>
        /// <param name="minor">The minor version.</param>
        /// <param name="patch">The patch version.</param>
        /// <param name="preRelease">The prelease data.</param>
        /// <param name="buildMetadata">The build metadata.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para>
        ///     Thrown in any of the following cases:
        ///     <list type="bullet">
        ///         <item><description><paramref name="major"/> is less than 0.</description></item>
        ///         <item><description><paramref name="minor"/> is less than 0.</description></item>
        ///         <item><description><paramref name="patch"/> is less than 0.</description></item>
        ///         <item><description><paramref name="preRelease"/> contains characters that don't match the regular expression <c>[A-Za-z0-9\-]</c>.</description></item>
        ///         <item><description><paramref name="buildMetadata"/> contains characters that don't match the regular expression <c>[A-Za-z0-9\-\.]</c>.</description></item>
        ///     </list>
        /// </para>
        /// </exception>
        public SemanticVersion(int major, int minor, int patch, string preRelease, string buildMetadata)
        {
            if (major < 0) throw new ArgumentOutOfRangeException(nameof(major));
            if (minor < 0) throw new ArgumentOutOfRangeException(nameof(minor));
            if (patch < 0) throw new ArgumentOutOfRangeException(nameof(patch));
            if (!(preRelease is null) && !ValidatePreRelease(preRelease, 0, -1)) throw new ArgumentOutOfRangeException(nameof(preRelease));
            if (!(buildMetadata is null) && !ValidateBuildMetadata(buildMetadata, 0, -1)) throw new ArgumentOutOfRangeException(nameof(buildMetadata));

            Major = major;
            Minor = minor;
            Patch = patch;
            PreRelease = string.IsNullOrEmpty(preRelease) ? null : preRelease;
            BuildMetadata = string.IsNullOrEmpty(buildMetadata) ? null : buildMetadata;
        }

        private static bool TryParseInt(string value, int startIndex, int endIndex, out int result)
        {
            if (endIndex < 0) endIndex = value.Length;

            result = 0;
            for (var i = startIndex; i < endIndex; i++)
            {
                var c = value[i];
                if (c < '0' || c > '9') return false;
                result = unchecked((result * 10) + (c - '0'));
                if (result < 0) return false;
            }

            return true;
        }

        private static bool ValidateBuildMetadata(string value, int startIndex, int endIndex)
        {
            if (value is null) return true;
            if (endIndex < 0) endIndex = value.Length - 1;

            for (var i = startIndex; i <= endIndex; i++)
            {
                var c = value[i];
                if (
                    (c < '0' || c > '9') &&
                    (c < 'a' || c > 'z') &&
                    (c < 'A' || c > 'Z') &&
                    (c != '-') &&
                    (c != '.'))
                    return false;
            }
            return true;
        }

        private static bool ValidatePreRelease(string value, int startIndex, int endIndex)
        {
            if (value is null) return true;
            if (endIndex < 0) endIndex = value.Length - 1;

            for (var i = startIndex; i <= endIndex; i++)
            {
                var c = value[i];
                if (
                    (c < '0' || c > '9') &&
                    (c < 'a' || c > 'z') &&
                    (c < 'A' || c > 'Z') &&
                    (c != '-'))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Converts the <see cref="string"/> representation of a semantic version to its structured equivalent.
        /// </summary>
        /// <param name="s">A <see cref="string"/> containing a number to convert.</param>
        /// <returns>A <see cref="SemanticVersion"/> equivalent to the semantic version contained in <paramref name="s"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="s"/> is null or <see cref="string.Empty"/>.</exception>
        /// <exception cref="FormatException"><paramref name="s"/> is not in the correct format.</exception>
        public static SemanticVersion Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentNullException(nameof(s));
            if (!TryParse(s, out SemanticVersion result))
                throw new FormatException();
            return result;
        }

        /// <summary>
        /// Converts the <see cref="string"/> representation of a semantic version to its structured equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="s">A <see cref="string"/> containing a number to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the structured equivalent of the semantic version contained in
        /// <paramref name="s"/>, if the conversion succeeded, or <see cref="Empty"/> if the conversion failed. The
        /// conversion fails if the <paramref name="s"/> parameter is null or <see cref="string.Empty"/>, does not conform
        /// to the semantic version specification, or contains version components less than 0 or greater than
        /// <see cref="int.MaxValue"/>. This parameter is passed uninitialized.
        /// </param>
        /// <returns><c>true</c> if <paramref name="s"/> was converted successfully; otherwise, false.</returns>
        public static bool TryParse(string s, out SemanticVersion result)
        {
            if (string.IsNullOrEmpty(s))
            {
                result = default;
                return false;
            }

            var minorIndex = s.IndexOf('.', StringComparison.Ordinal);
            if (minorIndex <= 0 || minorIndex == s.Length - 1)
            {
                result = default;
                return false;
            }

            var patchIndex = s.IndexOf('.', minorIndex + 1);
            if (patchIndex <= minorIndex + 1 || minorIndex == s.Length - 1)
            {
                result = default;
                return false;
            }

            var buildMetadataIndex = s.IndexOf('+', patchIndex + 1);
            if (buildMetadataIndex == patchIndex + 1 || patchIndex == s.Length - 1)
            {
                result = default;
                return false;
            }

            var end = buildMetadataIndex < 0 ? s.Length - 1 : buildMetadataIndex;
            var preReleaseIndex = s.IndexOf('-', patchIndex + 1, end - patchIndex - 1);
            if (preReleaseIndex == patchIndex + 1 || preReleaseIndex == s.Length - 1)
            {
                result = default;
                return false;
            }

            end = preReleaseIndex < 0 ? buildMetadataIndex : preReleaseIndex;
            if (!TryParseInt(s, 0, minorIndex, out var major) ||
                !TryParseInt(s, minorIndex + 1, patchIndex, out var minor) ||
                !TryParseInt(s, patchIndex + 1, end, out var patch))
            {
                result = default;
                return false;
            }

            if (preReleaseIndex >= 0 && !ValidatePreRelease(s, preReleaseIndex + 1, buildMetadataIndex - 1))
            {
                result = default;
                return false;
            }

            if (buildMetadataIndex >= 0 && !ValidateBuildMetadata(s, buildMetadataIndex + 1, -1))
            {
                result = default;
                return false;
            }

            var buildMetadata = buildMetadataIndex < 0
                ? null
                : s.Substring(buildMetadataIndex + 1, s.Length - buildMetadataIndex - 1);

            var preRelease = preReleaseIndex < 0
                ? null
                : buildMetadataIndex < 0
                    ? s.Substring(preReleaseIndex + 1, s.Length - preReleaseIndex - 1)
                    : s.Substring(preReleaseIndex + 1, buildMetadataIndex - preReleaseIndex - 1);

            result = new SemanticVersion(major, minor, patch, preRelease, buildMetadata);
            return true;
        }

        /// <summary>
        /// Determines how the specified <paramref name="comparand"/> is compatible with the specified <paramref name="baseline"/>.
        /// </summary>
        /// <param name="baseline">The baseline <see cref="SemanticVersion"/>.</param>
        /// <param name="comparand">The <see cref="SemanticVersion"/> to test for compatability.</param>
        /// <remarks>
        /// The presence of the <see cref="SemanticVersionCompatabilities.Incompatible"/> flag indicates that
        /// <paramref name="comparand"/> is incompatible with <paramref name="baseline"/>. Any other value
        /// indicates compatability. <see cref="BuildMetadata"/> is completely ignored.
        /// </remarks>
        /// <returns>
        /// A value that contains information on how the <see cref="SemanticVersion"/> values are incompatible.
        /// </returns>
        public static SemanticVersionCompatabilities GetCompatabilities(SemanticVersion baseline, SemanticVersion comparand)
        {
            SemanticVersionCompatabilities compatability = SemanticVersionCompatabilities.Identical;

            // Major versions must match.
            if (baseline.Major > comparand.Major)
                compatability |= SemanticVersionCompatabilities.Incompatible | SemanticVersionCompatabilities.OlderMajorVersion;
            else if (baseline.Major < comparand.Major)
                compatability |= SemanticVersionCompatabilities.Incompatible | SemanticVersionCompatabilities.NewerMajorVersion;

            // If functionality is added, the second version must be newer.
            if (baseline.Minor > comparand.Minor)
                compatability |= SemanticVersionCompatabilities.Incompatible | SemanticVersionCompatabilities.OlderMinorVersion;
            else if (baseline.Minor < comparand.Minor)
                compatability |= SemanticVersionCompatabilities.NewerMinorVersion;

            // Major version 0 is used for initial development. Everything must match.
            if (
                    (baseline.Major == 0 || comparand.Major == 0) &&
                    (
                        baseline.Major != comparand.Major ||
                        baseline.Minor != comparand.Minor ||
                        baseline.Patch != comparand.Patch ||
                        !StringComparer.Ordinal.Equals(baseline.PreRelease, comparand.PreRelease)
                    )
                )
                compatability |= SemanticVersionCompatabilities.Incompatible;

            // Technically these are compatible.
            if (baseline.Patch > comparand.Patch)
                compatability |= SemanticVersionCompatabilities.OlderPatchVersion;
            else if (baseline.Patch < comparand.Patch)
                compatability |= SemanticVersionCompatabilities.NewerPatchVersion;

            // Different pre-releases are always incompatible.
            if (baseline.PreRelease is null && !(comparand.PreRelease is null))
                compatability |= SemanticVersionCompatabilities.Incompatible | SemanticVersionCompatabilities.PreReleaseAdded;
            else if (!(baseline.PreRelease is null) && comparand.PreRelease is null)
                compatability |= SemanticVersionCompatabilities.Incompatible | SemanticVersionCompatabilities.PreReleaseRemoved;
            else if (!(baseline.PreRelease is null) && !(comparand.PreRelease is null))
            {
                var cmp = string.CompareOrdinal(baseline.PreRelease, comparand.PreRelease);
                if (cmp == -1) compatability |= SemanticVersionCompatabilities.Incompatible | SemanticVersionCompatabilities.NewerPreRelease;
                else if (cmp == 1) compatability |= SemanticVersionCompatabilities.Incompatible | SemanticVersionCompatabilities.OlderPreRelease;
            }

            // Build metadata is irrelevant.
            if (baseline.BuildMetadata is null && !(comparand.BuildMetadata is null))
                compatability |= SemanticVersionCompatabilities.BuildMetadataAdded;
            else if (!(baseline.BuildMetadata is null) && comparand.BuildMetadata is null)
                compatability |= SemanticVersionCompatabilities.BuildMetadataRemoved;
            else if (!(baseline.BuildMetadata is null) && !(comparand.BuildMetadata is null) && !StringComparer.Ordinal.Equals(baseline.BuildMetadata, comparand.BuildMetadata))
                compatability |= SemanticVersionCompatabilities.DifferentBuildMetadata;

            return compatability;
        }

        /// <inheritdoc/>
        public int CompareTo(SemanticVersion other) => SemanticVersionComparer.Strict.Compare(this, other);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => obj is SemanticVersion other
            && Equals(other);

        /// <inheritdoc/>
        public bool Equals(SemanticVersion other) => SemanticVersionComparer.Strict.Equals(this, other);

        /// <inheritdoc/>
        public override int GetHashCode() => SemanticVersionComparer.Strict.GetHashCode(this);

        /// <summary>
        /// Determines if <paramref name="a"/> is a smaller version than <paramref name="b"/>.
        /// </summary>
        /// <param name="a">The first <see cref="SemanticVersion"/> to compare.</param>
        /// <param name="b">The second <see cref="SemanticVersion"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="SemanticVersion"/> is smaller than the <see cref="SemanticVersion"/>.
        /// </returns>
        public static bool operator <(SemanticVersion a, SemanticVersion b) => a.CompareTo(b) < 0;

        /// <summary>
        /// Determines if <paramref name="a"/> is a greater version than <paramref name="b"/>.
        /// </summary>
        /// <param name="a">The first <see cref="SemanticVersion"/> to compare.</param>
        /// <param name="b">The second <see cref="SemanticVersion"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="SemanticVersion"/> is smaller than the <see cref="SemanticVersion"/>.
        /// </returns>
        public static bool operator >(SemanticVersion a, SemanticVersion b) => a.CompareTo(b) > 0;

        /// <summary>
        /// Determines if <paramref name="a"/> is a smaller or similar version than <paramref name="b"/>.
        /// </summary>
        /// <param name="a">The first <see cref="SemanticVersion"/> to compare.</param>
        /// <param name="b">The second <see cref="SemanticVersion"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="SemanticVersion"/> is smaller or similar than the <see cref="SemanticVersion"/>.
        /// </returns>
        public static bool operator <=(SemanticVersion a, SemanticVersion b) => a.CompareTo(b) <= 0;

        /// <summary>
        /// Determines if <paramref name="a"/> is a greater or similar version than <paramref name="b"/>.
        /// </summary>
        /// <param name="a">The first <see cref="SemanticVersion"/> to compare.</param>
        /// <param name="b">The second <see cref="SemanticVersion"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="SemanticVersion"/> is greater or similar than the <see cref="SemanticVersion"/>.
        /// </returns>
        public static bool operator >=(SemanticVersion a, SemanticVersion b) => a.CompareTo(b) >= 0;

        /// <summary>
        /// Determines if <paramref name="a"/> is a similar version to <paramref name="b"/>.
        /// </summary>
        /// <param name="a">The first <see cref="SemanticVersion"/> to compare.</param>
        /// <param name="b">The second <see cref="SemanticVersion"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="SemanticVersion"/> is similar to <see cref="SemanticVersion"/>.
        /// </returns>
        public static bool operator ==(SemanticVersion a, SemanticVersion b) => a.CompareTo(b) == 0;

        /// <summary>
        /// Determines if <paramref name="a"/> is not a similar version to <paramref name="b"/>.
        /// </summary>
        /// <param name="a">The first <see cref="SemanticVersion"/> to compare.</param>
        /// <param name="b">The second <see cref="SemanticVersion"/> to compare.</param>
        /// <returns>
        /// A value indicating whether the first <see cref="SemanticVersion"/> is not similar to <see cref="SemanticVersion"/>.
        /// </returns>
        public static bool operator !=(SemanticVersion a, SemanticVersion b) => a.CompareTo(b) != 0;

        private string ToDefinedFormatString(char format, IFormatProvider formatProvider)
        {
            FormattableString str;
            switch (format)
            {
                case 'v':
                case 'V':
                    str = $"{Major}.{Minor}.{Patch}";
                    break;

                case 'p':
                case 'P':
                    if (PreRelease is null)
                        str = $"{Major}.{Minor}.{Patch}";
                    else
                        str = $"{Major}.{Minor}.{Patch}-{PreRelease}";
                    break;

                case 'm':
                case 'M':
                    if (BuildMetadata is null)
                        str = $"{Major}.{Minor}.{Patch}";
                    else
                        str = $"{Major}.{Minor}.{Patch}+{BuildMetadata}";
                    break;

                case 'f':
                case 'F':
                    if (PreRelease is null && BuildMetadata is null)
                        str = $"{Major}.{Minor}.{Patch}";
                    else if (PreRelease is null)
                        str = $"{Major}.{Minor}.{Patch}+{BuildMetadata}";
                    else if (BuildMetadata is null)
                        str = $"{Major}.{Minor}.{Patch}-{PreRelease}";
                    else
                        str = $"{Major}.{Minor}.{Patch}-{PreRelease}+{BuildMetadata}";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(format));
            }

            return str.ToString(formatProvider ?? CultureInfo.InvariantCulture);
        }

        /// <inheritdoc/>
        public override string ToString() => ToDefinedFormatString('F', CultureInfo.InvariantCulture);

        /// <inheritdoc/>
        public string ToString(string format) => ToString(format, CultureInfo.InvariantCulture);

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
                return ToDefinedFormatString('F', formatProvider);

            if (format.Length != 1) throw new ArgumentOutOfRangeException(nameof(format));

            return ToDefinedFormatString(format[0], formatProvider);
        }
    }
}
