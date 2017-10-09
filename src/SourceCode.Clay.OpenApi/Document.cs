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
    public class Document : IEquatable<Document>
    {
        #region Properties

        /// <summary>
        /// Gets the semantic version number of the OpenAPI Specification version that the OpenAPI document uses.
        /// </summary>
        public SemanticVersion Version { get; }

        /// <summary>
        /// Gets the metadata about the API.
        /// </summary>
        public Information Info { get; }

        /// <summary>
        /// Gets the list of <see cref="Server"/> instances, which provide connectivity information to a target server.
        /// </summary>
        public IReadOnlyList<Server> Servers { get; }

        /// <summary>
        /// Gets the available paths and operations for the API.
        /// </summary>
        public IReadOnlyDictionary<string, Referable<Path>> Paths { get; }

        /// <summary>
        /// Gets the list that holds various schemas for the specification.
        /// </summary>
        public Components Components { get; }

        /// <summary>
        /// Gets the declaration of which security mechanisms can be used across the API.
        /// </summary>
        public IReadOnlyList<Referable<SecurityScheme>> Security { get; }

        /// <summary>
        /// Gets the list of tags used by the specification with additional metadata.
        /// </summary>
        public IReadOnlyList<Tag> Tags { get; }

        /// <summary>
        /// Gets the external documentation.
        /// </summary>
        public ExternalDocumentation ExternalDocumentation { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="Document"/> class.
        /// </summary>
        /// <param name="version">The semantic version number of the OpenAPI Specification version that the OpenAPI document uses. The default is 3.0.0.</param>
        /// <param name="info">The metadata about the API.</param>
        /// <param name="servers">The list of <see cref="Server"/> instances, which provide connectivity information to a target server.</param>
        /// <param name="paths">The available paths and operations for the API.</param>
        /// <param name="components">The list that holds various schemas for the specification.</param>
        /// <param name="security">The declaration of which security mechanisms can be used across the API.</param>
        /// <param name="tags">The list of tags used by the specification with additional metadata.</param>
        /// <param name="externalDocumentation">The external documentation.</param>
        public Document(
            SemanticVersion? version = default,
            Information info = default,
            IReadOnlyList<Server> servers = default,
            IReadOnlyDictionary<string, Referable<Path>> paths = default,
            Components components = default,
            IReadOnlyList<Referable<SecurityScheme>> security = default,
            IReadOnlyList<Tag> tags = default,
            ExternalDocumentation externalDocumentation = default)
        {
            Version = version ?? new SemanticVersion(3, 0, 0);
            Info = info;
            Servers = servers ?? Array.Empty<Server>();
            Paths = paths ?? ReadOnlyDictionary.Empty<string, Referable<Path>>();
            Components = components;
            Security = security ?? Array.Empty<Referable<SecurityScheme>>();
            Tags = tags ?? Array.Empty<Tag>();
            ExternalDocumentation = externalDocumentation;
        }

        #endregion

        #region IEquatable

        public static bool operator ==(Document document1, Document document2)
        {
            if (ReferenceEquals(document1, null) && ReferenceEquals(document2, null)) return true;
            if (ReferenceEquals(document1, null) || ReferenceEquals(document2, null)) return false;
            return document1.Equals((object)document2);
        }

        public static bool operator !=(Document document1, Document document2) => !(document1 == document2);

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) => Equals(obj as Document);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public virtual bool Equals(Document other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            if (Version != other.Version) return false;
            if (!Info.NullableEquals(other.Info)) return false;
            if (!Servers.ListEquals(other.Servers, true)) return false;
            if (!Paths.DictionaryEquals(other.Paths)) return false;
            if (!Components.NullableEquals(other.Components)) return false;
            if (!Security.ListEquals(other.Security, false)) return false;
            if (!Tags.ListEquals(other.Tags, false)) return false;
            if (!ExternalDocumentation.NullableEquals(other.ExternalDocumentation)) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                hc = (hc * 23) + Version.GetHashCode();
                if (Info != null)
                    hc = (hc * 23) + Info.GetHashCode();
                hc = (hc * 23) + Servers.Count;
                hc = (hc * 23) + Paths.Count;
                if (Components != null)
                    hc = (hc * 23) + Components.GetHashCode();
                hc = (hc * 23) + Security.Count;
                hc = (hc * 23) + Tags.Count;
                if (ExternalDocumentation != null)
                    hc = (hc * 23) + ExternalDocumentation.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion
    }
}
