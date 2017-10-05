#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Xunit;

namespace SourceCode.Clay.Tests
{
    public static class SemanticVersionTests
    {
        #region Methods

        [InlineData("123.4.56", 123, 4, 56, null, null)]
        [InlineData("123.4.56-pre-0", 123, 4, 56, "pre-0", null)]
        [InlineData("123.4.56+build-0.1", 123, 4, 56, null, "build-0.1")]
        [InlineData("123.4.56-pre-0+build-0.1", 123, 4, 56, "pre-0", "build-0.1")]
        [Theory(DisplayName = nameof(SemanticVersion_Parse))]
        public static void SemanticVersion_Parse(string version, int major, int minor, int patch, string pre, string meta)
        {
            var success = SemanticVersion.TryParse(version, out var sut);
            Assert.True(success);
            Assert.Equal(new SemanticVersion(major, minor, patch, pre, meta), sut);
        }

        [InlineData("123.4.56", 123, 4, 56, null, null)]
        [InlineData("123.4.56-pre-0", 123, 4, 56, "pre-0", null)]
        [InlineData("123.4.56+build-0.1", 123, 4, 56, null, "build-0.1")]
        [InlineData("123.4.56-pre-0+build-0.1", 123, 4, 56, "pre-0", "build-0.1")]
        [Theory(DisplayName = nameof(SemanticVersion_Parse))]
        public static void SemanticVersion_ToString(string version, int major, int minor, int patch, string pre, string meta)
        {
            var sut = new SemanticVersion(major, minor, patch, pre, meta);
            Assert.Equal(version, sut.ToString());

            Assert.Equal(new SemanticVersion(major, minor, patch).ToString(), sut.ToString("V"));
            Assert.Equal(new SemanticVersion(major, minor, patch, pre, null).ToString(), sut.ToString("P"));
            Assert.Equal(new SemanticVersion(major, minor, patch, null, meta).ToString(), sut.ToString("M"));
        }

        // Major version incompatability
        [InlineData(
            0, 1, 2, null, null,
            0, 1, 2, null, null,
            SemanticVersionCompatabilities.Identical
        )]
        [InlineData(
            0, 1, 2, null, null,
            0, 1, 3, null, null,
            SemanticVersionCompatabilities.Incompatible | SemanticVersionCompatabilities.NewerPatchVersion
        )]
        [InlineData(
            0, 1, 2, null, "build1",
            0, 1, 2, null, "build2",
            SemanticVersionCompatabilities.DifferentBuildMetadata
        )]
        [InlineData(
            0, 1, 2, null, "build1",
            0, 1, 2, null, null,
            SemanticVersionCompatabilities.BuildMetadataRemoved
        )]
        [InlineData(
            0, 1, 2, null, null,
            0, 1, 2, null, "build2",
            SemanticVersionCompatabilities.BuildMetadataAdded
        )]

        // Minor version incompatability
        [InlineData(
            1, 2, 3, null, null,
            1, 3, 0, null, null,
            SemanticVersionCompatabilities.NewerMinorVersion | SemanticVersionCompatabilities.OlderPatchVersion
        )]
        [InlineData(
            1, 3, 3, null, null,
            1, 2, 0, null, null,
            SemanticVersionCompatabilities.Incompatible | SemanticVersionCompatabilities.OlderMinorVersion | SemanticVersionCompatabilities.OlderPatchVersion
        )]

        // Patch version incompatability
        [InlineData(
            1, 2, 3, null, null,
            1, 2, 4, null, null,
            SemanticVersionCompatabilities.NewerPatchVersion
        )]
        [InlineData(
            1, 2, 3, null, null,
            1, 2, 2, null, null,
            SemanticVersionCompatabilities.OlderPatchVersion
        )]

        // Pre-release version incompatability
        [InlineData(
            1, 2, 3, "pre1", null,
            1, 2, 3, "pre1", null,
            SemanticVersionCompatabilities.Identical
        )]
        [InlineData(
            1, 2, 3, "pre1", null,
            1, 2, 3, "pre2", null,
            SemanticVersionCompatabilities.Incompatible | SemanticVersionCompatabilities.NewerPreRelease
        )]
        [InlineData(
            1, 2, 3, "pre2", null,
            1, 2, 3, "pre1", null,
            SemanticVersionCompatabilities.Incompatible | SemanticVersionCompatabilities.OlderPreRelease
        )]
        [InlineData(
            1, 2, 3, null, null,
            1, 2, 3, "pre2", null,
            SemanticVersionCompatabilities.Incompatible | SemanticVersionCompatabilities.PreReleaseAdded
        )]
        [InlineData(
            1, 2, 3, "pre1", null,
            1, 2, 3, null, null,
            SemanticVersionCompatabilities.Incompatible | SemanticVersionCompatabilities.PreReleaseRemoved
        )]

        // Build metadata version incompatability
        [InlineData(
            1, 2, 3, null, "build1",
            1, 2, 3, null, "build1",
            SemanticVersionCompatabilities.Identical
        )]
        [InlineData(
            1, 2, 3, null, "build1",
            1, 2, 3, null, "build2",
            SemanticVersionCompatabilities.DifferentBuildMetadata
        )]
        [InlineData(
            1, 2, 3, null, null,
            1, 2, 3, null, "build2",
            SemanticVersionCompatabilities.BuildMetadataAdded
        )]
        [InlineData(
            1, 2, 3, null, "build1",
            1, 2, 3, null, null,
            SemanticVersionCompatabilities.BuildMetadataRemoved
        )]
        [Theory(DisplayName = nameof(SemanticVersion_GetCompatabilities))]
        public static void SemanticVersion_GetCompatabilities(
            int major1, int minor1, int patch1, string pre1, string meta1,
            int major2, int minor2, int patch2, string pre2, string meta2,
            SemanticVersionCompatabilities expected
            )
        {
            var sut1 = new SemanticVersion(major1, minor1, patch1, pre1, meta1);
            var sut2 = new SemanticVersion(major2, minor2, patch2, pre2, meta2);
            var compat = SemanticVersion.GetCompatabilities(sut1, sut2);
            Assert.Equal(expected, compat);
        }

        #endregion
    }
}
