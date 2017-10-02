using System;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents the different levels of <see cref="SemanticVersion"/> compatability.
    /// </summary>
    [Flags]
    public enum SemanticVersionCompatabilities
    {
#       pragma warning disable S2346 // Flags enumerations zero-value members should be named "None"
        /// <summary>
        /// The two versions are completely identical.
        /// </summary>
        Identical = 0,
#       pragma warning restore S2346 // Flags enumerations zero-value members should be named "None"

        /// <summary>
        /// The two versions are completely incompatible.
        /// </summary>
        Incompatible = 1,

        /// <summary>
        /// The second specified version has an older minor version.
        /// </summary>
        OlderMajorVersion = 2,

        /// <summary>
        /// The second specified version has a newer minor version.
        /// </summary>
        NewerMajorVersion = 4,

        /// <summary>
        /// The second specified version has an older minor version.
        /// </summary>
        OlderMinorVersion = 8,

        /// <summary>
        /// The second specified version has a newer minor version.
        /// </summary>
        NewerMinorVersion = 16,

        /// <summary>
        /// The second specified version has an older patch version.
        /// </summary>
        OlderPatchVersion = 32,

        /// <summary>
        /// The second specified version has a newer patch version.
        /// </summary>
        NewerPatchVersion = 64,

        /// <summary>
        /// The second specified version has a pre-release version removed.
        /// </summary>
        PreReleaseRemoved = 128,

        /// <summary>
        /// The second specified version has a pre-release version added.
        /// </summary>
        PreReleaseAdded = 256,

        /// <summary>
        /// The second specified version has an older pre-release version.
        /// </summary>
        OlderPreRelease = 512,

        /// <summary>
        /// The second specified version has a newer pre-release version.
        /// </summary>
        NewerPreRelease = 1024,

        /// <summary>
        /// The first specifid version has additional build metadata.
        /// </summary>
        BuildMetadataRemoved = 2048,

        /// <summary>
        /// The second specifid version has additional build metadata.
        /// </summary>
        BuildMetadataAdded = 4096,

        /// <summary>
        /// The two versions have different build metadata.
        /// </summary>
        DifferentBuildMetadata = 8192
    }
}
