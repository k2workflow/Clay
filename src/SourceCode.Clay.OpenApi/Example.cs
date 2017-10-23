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
    /// Contains an example of how to use an entity in an Open API document.
    /// </summary>
    /// <remarks>
    /// The <c>Value</c> property is implementation-specific. If it is required, create a new type
    /// that inherits from this one.
    /// </remarks>
    public sealed class Example : IEquatable<Example>
    {
        #region Fields

        private readonly JsonObject _json;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the short description for the example.
        /// </summary>
        public string Summary
        {
            get
            {
                if (_json.TryGetValue(OpenApiSerializer.PropertyConstants.Summary, out var jv))
                    return jv;

                return null;
            }
        }

        /// <summary>
        /// Gets the long description for the example.
        /// </summary>
        /// <remarks>CommonMark syntax MAY be used for rich text representation.</remarks>
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
        /// Gets the URL that points to the literal example.
        /// </summary>
        public Uri ExternalValue
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
        /// Creates a new instance of the <see cref="Example"/> class.
        /// </summary>
        /// <param name="summary">The short description for the example.</param>
        /// <param name="description">The long description for the example.</param>
        /// <param name="externalValue">The URL that points to the literal example.</param>
        /// <param name="vendorExtensions">Custom vendor properties.</param>
        public Example(
            string summary = default,
            string description = default,
            Uri externalValue = default,
            JsonObject vendorExtensions = default)
        {
            _json = new JsonObject
            {
                [OpenApiSerializer.PropertyConstants.Summary] = summary,
                [OpenApiSerializer.PropertyConstants.Description] = description,
                [OpenApiSerializer.PropertyConstants.ExternalValue] = externalValue.ToString()
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

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is Example other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(Example other)
        {
            if (this is null) return other is null; // (null, null) or (null, y)
            if (other is null) return false; // (x, null)
            if (ReferenceEquals(this, other)) return true; // (x, x)

            if (_json is null) return other._json is null; // (null, null) or (null, y)
            if (other._json is null) return false; // (x, null)

            if (!StringComparer.Ordinal.Equals(Summary, other.Summary)) return false;
            if (!StringComparer.Ordinal.Equals(Description, other.Description)) return false;
            if (ExternalValue != other.ExternalValue) return false;

            return true;
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (Summary != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Summary);
                if (Description != null)
                    hc = (hc * 23) + StringComparer.Ordinal.GetHashCode(Description);
                if (ExternalValue != null)
                    hc = (hc * 23) + ExternalValue.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion

        #region Operators

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="example1">The example1.</param>
        /// <param name="example2">The example2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Example example1, Example example2)
        {
            if (example1 is null && example2 is null) return true;
            if (example1 is null || example2 is null) return false;
            return example1.Equals((object)example2);
        }

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="example1">The example1.</param>
        /// <param name="example2">The example2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Example example1, Example example2) => !(example1 == example2);

        #endregion
    }
}
