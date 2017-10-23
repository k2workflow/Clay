#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.OpenApi.Serialization;
using System;
using System.Json;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Allows referencing an external resource for extended documentation.
    /// </summary>
    public sealed class ExternalDocumentation : IEquatable<ExternalDocumentation>
    {
        #region Fields

        private readonly JsonObject _json;

        #endregion

        #region Properties

        /// <summary>
        /// Get the short description of the target documentation.
        /// </summary>
        /// <remarks>
        /// CommonMark syntax MAY be used for rich text representation.
        /// </remarks>
        public string Description
        {
            get
            {
                if (_json.TryGetValue(OpenApiSerializer.PropertyConstants.Description, out var jv))
                    return jv;

                return null;
            }
        }

        /// <summary>
        /// Gets the URL for the target documentation.
        /// </summary>
        public Uri Url
        {
            get
            {
                if (_json.TryGetValue(OpenApiSerializer.PropertyConstants.Url, out var jv))
                    return new Uri(jv);

                return null;
            }
        }

        /// <summary>
        /// Gets custom properties.
        /// </summary>
        public JsonObject VendorExtension { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ExternalDocumentation"/> class.
        /// </summary>
        /// <param name="description">The short description of the target documentation.</param>
        /// <param name="url">The URL for the target documentation.</param>
        /// <param name="vendorExtensions">Custom vendor properties.</param>
        public ExternalDocumentation(
            string description = default,
            Uri url = default,
            JsonObject vendorExtensions = default)
        {
            _json = new JsonObject
            {
                [OpenApiSerializer.PropertyConstants.Description] = description,
                [OpenApiSerializer.PropertyConstants.Url] = url.ToString()
            };

            // Vendor extensions
            if (vendorExtensions != null && vendorExtensions.Count > 0)
            {
                VendorExtension = new JsonObject();

                foreach (var item in vendorExtensions)
                {
                    // Don't add well-known properties
                    if (!_json.ContainsKey(item.Key))
                        VendorExtension.Add(item.Key, item.Value); // Leverage error handling in Add()
                }
            }
        }

        #endregion

        #region IEquatable

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is ExternalDocumentation other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(ExternalDocumentation other)
        {
            if (this is null) return other is null; // (null, null) or (null, y)
            if (other is null) return false; // (x, null)
            if (ReferenceEquals(this, other)) return true; // (x, x)

            if (_json is null) return other._json is null; // (null, null) or (null, y)
            if (other._json is null) return false; // (x, null)

            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (Url != other.Url) return false;

            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Description != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Description);
                if (Url != null)
                    hc = (hc * 23) + Url.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="externalDocumentation1">The external documentation1.</param>
        /// <param name="externalDocumentation2">The external documentation2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(ExternalDocumentation externalDocumentation1, ExternalDocumentation externalDocumentation2)
        {
            if (externalDocumentation1 is null && externalDocumentation2 is null) return true;
            if (externalDocumentation1 is null || externalDocumentation2 is null) return false;
            return externalDocumentation1.Equals((object)externalDocumentation2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="externalDocumentation1">The external documentation1.</param>
        /// <param name="externalDocumentation2">The external documentation2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(ExternalDocumentation externalDocumentation1, ExternalDocumentation externalDocumentation2)
            => !(externalDocumentation1 == externalDocumentation2);

        #endregion
    }
}
