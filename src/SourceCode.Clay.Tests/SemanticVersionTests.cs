using Xunit;

namespace SourceCode.Clay.Tests
{
    public class SemanticVersionTests
    {
        [InlineData("123.4.56", 123, 4, 56, null, null)]
        [InlineData("123.4.56-pre-0", 123, 4, 56, "pre-0", null)]
        [InlineData("123.4.56+build-0.1", 123, 4, 56, null, "build-0.1")]
        [InlineData("123.4.56-pre-0+build-0.1", 123, 4, 56, "pre-0", "build-0.1")]
        [Theory(DisplayName = nameof(SemanticVersion_Parse))]
        public void SemanticVersion_Parse(string version, int major, int minor, int patch, string pre, string meta)
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
        public void SemanticVersion_ToString(string version, int major, int minor, int patch, string pre, string meta)
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
            SemanticVersionIncompatabilities.Identical
        )]
        [InlineData(
            0, 1, 2, null, null,
            0, 1, 3, null, null,
            SemanticVersionIncompatabilities.Incompatible | SemanticVersionIncompatabilities.NewerPatchVersion
        )]
        [InlineData(
            0, 1, 2, null, "build1",
            0, 1, 2, null, "build2",
            SemanticVersionIncompatabilities.DifferentBuildMetadata
        )]
        [InlineData(
            0, 1, 2, null, "build1",
            0, 1, 2, null, null,
            SemanticVersionIncompatabilities.BuildMetadataRemoved
        )]
        [InlineData(
            0, 1, 2, null, null,
            0, 1, 2, null, "build2",
            SemanticVersionIncompatabilities.BuildMetadataAdded
        )]

        // Minor version incompatability
        [InlineData(
            1, 2, 3, null, null,
            1, 3, 0, null, null,
            SemanticVersionIncompatabilities.NewerMinorVersion | SemanticVersionIncompatabilities.OlderPatchVersion
        )]
        [InlineData(
            1, 3, 3, null, null,
            1, 2, 0, null, null,
            SemanticVersionIncompatabilities.Incompatible | SemanticVersionIncompatabilities.OlderMinorVersion | SemanticVersionIncompatabilities.OlderPatchVersion
        )]

        // Patch version incompatability
        [InlineData(
            1, 2, 3, null, null,
            1, 2, 4, null, null,
            SemanticVersionIncompatabilities.NewerPatchVersion
        )]
        [InlineData(
            1, 2, 3, null, null,
            1, 2, 2, null, null,
            SemanticVersionIncompatabilities.OlderPatchVersion
        )]

        // Pre-release version incompatability
        [InlineData(
            1, 2, 3, "pre1", null,
            1, 2, 3, "pre1", null,
            SemanticVersionIncompatabilities.Identical
        )]
        [InlineData(
            1, 2, 3, "pre1", null,
            1, 2, 3, "pre2", null,
            SemanticVersionIncompatabilities.Incompatible | SemanticVersionIncompatabilities.NewerPreRelease
        )]
        [InlineData(
            1, 2, 3, "pre2", null,
            1, 2, 3, "pre1", null,
            SemanticVersionIncompatabilities.Incompatible | SemanticVersionIncompatabilities.OlderPreRelease
        )]
        [InlineData(
            1, 2, 3, null, null,
            1, 2, 3, "pre2", null,
            SemanticVersionIncompatabilities.Incompatible | SemanticVersionIncompatabilities.PreReleaseAdded
        )]
        [InlineData(
            1, 2, 3, "pre1", null,
            1, 2, 3, null, null,
            SemanticVersionIncompatabilities.Incompatible | SemanticVersionIncompatabilities.PreReleaseRemoved
        )]

        // Build metadata version incompatability
        [InlineData(
            1, 2, 3, null, "build1",
            1, 2, 3, null, "build1",
            SemanticVersionIncompatabilities.Identical
        )]
        [InlineData(
            1, 2, 3, null, "build1", 
            1, 2, 3, null, "build2", 
            SemanticVersionIncompatabilities.DifferentBuildMetadata
        )]
        [InlineData(
            1, 2, 3, null, null, 
            1, 2, 3, null, "build2", 
            SemanticVersionIncompatabilities.BuildMetadataAdded
        )]
        [InlineData(
            1, 2, 3, null, "build1", 
            1, 2, 3, null, null, 
            SemanticVersionIncompatabilities.BuildMetadataRemoved
        )]

        [Theory(DisplayName = nameof(SemanticVersion_GetIncompatabilities))]
        public void SemanticVersion_GetIncompatabilities(
            int major1, int minor1, int patch1, string pre1, string meta1,
            int major2, int minor2, int patch2, string pre2, string meta2,
            SemanticVersionIncompatabilities expected
            )
        {
            var sut1 = new SemanticVersion(major1, minor1, patch1, pre1, meta1);
            var sut2 = new SemanticVersion(major2, minor2, patch2, pre2, meta2);
            var compat = SemanticVersion.GetIncompatabilities(sut1, sut2);
            Assert.Equal(expected, compat);
        }
    }
}
