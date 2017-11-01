#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Collections.Generic;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// This is the root document object of the OpenAPI document.
    /// </summary>
    public class OasDocument : IEquatable<OasDocument>
    {
        #region Properties

        /// <summary>
        /// Gets the semantic version number of the OpenAPI Specification version that the OpenAPI document uses.
        /// </summary>
        public SemanticVersion Version { get; }

        /// <summary>
        /// Gets the metadata about the API.
        /// </summary>
        public OasInformation Info { get; }

        /// <summary>
        /// Gets the list of <see cref="OasServer"/> instances, which provide connectivity information to a target server.
        /// </summary>
        public IReadOnlyList<OasServer> Servers { get; }

        /// <summary>
        /// Gets the available paths and operations for the API.
        /// </summary>
        public IReadOnlyDictionary<string, OasReferable<OasPath>> Paths { get; }

        /// <summary>
        /// Gets the list that holds various schemas for the specification.
        /// </summary>
        public OasComponents Components { get; }

        /// <summary>
        /// Gets the declaration of which security mechanisms can be used across the API.
        /// </summary>
        public IReadOnlyList<OasReferable<OasSecurityScheme>> Security { get; }

        /// <summary>
        /// Gets the list of tags used by the specification with additional metadata.
        /// </summary>
        public IReadOnlyList<OasTag> Tags { get; }

        /// <summary>
        /// Gets the external documentation.
        /// </summary>
        public OasExternalDocumentation ExternalDocumentation { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OasDocument"/> class.
        /// </summary>
        /// <param name="version">The semantic version number of the OpenAPI Specification version that the OpenAPI document uses. The default is 3.0.0.</param>
        /// <param name="info">The metadata about the API.</param>
        /// <param name="servers">The list of <see cref="OasServer"/> instances, which provide connectivity information to a target server.</param>
        /// <param name="paths">The available paths and operations for the API.</param>
        /// <param name="components">The list that holds various schemas for the specification.</param>
        /// <param name="security">The declaration of which security mechanisms can be used across the API.</param>
        /// <param name="tags">The list of tags used by the specification with additional metadata.</param>
        /// <param name="externalDocumentation">The external documentation.</param>
        public OasDocument(
            SemanticVersion? version = default,
            OasInformation info = default,
            IReadOnlyList<OasServer> servers = default,
            IReadOnlyDictionary<string, OasReferable<OasPath>> paths = default,
            OasComponents components = default,
            IReadOnlyList<OasReferable<OasSecurityScheme>> security = default,
            IReadOnlyList<OasTag> tags = default,
            OasExternalDocumentation externalDocumentation = default)
        {
            Version = version ?? new SemanticVersion(3, 0, 0);
            Info = info;
            Servers = servers ?? Array.Empty<OasServer>();
            Paths = paths ?? ReadOnlyDictionary.Empty<string, OasReferable<OasPath>>();
            Components = components;
            Security = security ?? Array.Empty<OasReferable<OasSecurityScheme>>();
            Tags = tags ?? Array.Empty<OasTag>();
            ExternalDocumentation = externalDocumentation;
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="document1">The document1.</param>
        /// <param name="document2">The document2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasDocument document1, OasDocument document2)
        {
            if (document1 is null) return document2 is null;
            return document1.Equals((object)document2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="document1">The document1.</param>
        /// <param name="document2">The document2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasDocument document1, OasDocument document2)
            => !(document1 == document2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasDocument other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(OasDocument other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            if (Version != other.Version) return false;
            if (!Info.NullableEquals(other.Info)) return false;
            if (!Servers.NullableListEquals(other.Servers)) return false;
            if (!Paths.NullableDictionaryEquals(other.Paths)) return false;
            if (!Components.NullableEquals(other.Components)) return false;
            if (!Security.NullableSetEquals(other.Security)) return false;
            if (!Tags.NullableSetEquals(other.Tags)) return false;
            if (!ExternalDocumentation.NullableEquals(other.ExternalDocumentation)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => new HashCode()
            .Tally(Version)
            .Tally(Info)
            .TallyCount(Servers)
            .TallyCount(Paths)
            .Tally(Components)
            .TallyCount(Security)
            .TallyCount(Tags)
            .Tally(ExternalDocumentation)
            .ToHashCode();

        #endregion
    }
}
