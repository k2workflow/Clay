#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json.Pointers;
using System;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents a reference to an internal or external resource.
    /// </summary>
    public struct Reference : IEquatable<Reference>
    {
        #region Properties

        /// <summary>
        /// Gets the URL portion of the reference.
        /// </summary>
        public Uri Url { get; }

        /// <summary>
        /// Gets the pointer portion of the reference.
        /// </summary>
        public JsonPointer Pointer { get; }

        /// <summary>
        /// Gets a value indicating whether the reference has a value.
        /// </summary>
        public bool HasValue => Url != null || Pointer.Count != 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new internal <see cref="Reference"/> value.
        /// </summary>
        /// <param name="pointer">The internal reference pointer.</param>
        public Reference(JsonPointer pointer)
        {
            if (pointer.Count == 0) throw new ArgumentOutOfRangeException(nameof(pointer));
            Url = default;
            Pointer = pointer;
        }

        /// <summary>
        /// Creates a new external <see cref="Reference"/> value.
        /// </summary>
        /// <param name="url">The external reference URL.</param>
        public Reference(Uri url)
            : this(url, default)
        {
        }

        /// <summary>
        /// Creates a new external <see cref="Reference"/> value.
        /// </summary>
        /// <param name="url">The external reference URL.</param>
        /// <param name="pointer">The external reference pointer.</param>
        public Reference(Uri url, JsonPointer pointer)
        {
            if (url == null && pointer.Count == 0) throw new ArgumentNullException(nameof(url));

            if (url != null)
            {
                if (url.IsAbsoluteUri && !string.IsNullOrEmpty(url.Fragment) && pointer.Count != 0)
                    throw new ArgumentOutOfRangeException(nameof(url), "The URL cannot have a fragment when a pointer is provided.");
                else if (!url.IsAbsoluteUri && url.OriginalString.IndexOf('#') >= 0)
                    throw new ArgumentOutOfRangeException(nameof(url), "The URL cannot have a fragment when a pointer is provided.");
            }
            else if (pointer.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(pointer), "A reference cannot internally refer to the whole document.");

            Url = url;
            Pointer = pointer;
        }

        #endregion

        #region Equatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="reference1">The reference1.</param>
        /// <param name="reference2">The reference2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(Reference reference1, Reference reference2) => reference1.Equals(reference2);

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="reference1">The reference1.</param>
        /// <param name="reference2">The reference2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(Reference reference1, Reference reference2) => !(reference1 == reference2);

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Reference o && Equals(o);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(Reference other)
        {
            if (Url != other.Url) return false;
            if (!Pointer.Equals(other.Pointer)) return false;
            return true;
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;
                if (Url != null)
                    hc = (hc * 23) + Url.GetHashCode();
                hc = (hc * 23) + Pointer.GetHashCode();

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Converts the URL representation of a reference to its structured equivalent.
        /// </summary>
        /// <param name="url">A URL containing a reference to convert.</param>
        /// <returns>The structured equivalent of the reference contained in <paramref name="url"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="url"/> is null.</exception>
        /// <exception cref="FormatException"><paramref name="url"/> is not in a format compliant with the Open API specification.</exception>
        public static Reference ParseUrl(Uri url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));
            if (!TryParseUrl(url, out var result)) throw new FormatException("The specified value is not a valid reference.");
            return result;
        }

        /// <summary>
        /// Converts the URL representation of a reference to its structured equivalent.
        /// A return value indicates whether the conversion succeeded.
        /// </summary>
        /// <param name="url">A URL containing a reference to convert.</param>
        /// <param name="result">
        /// When this method returns, contains the structured equivalent of the reference contained in <paramref name="url"/>,
        /// if the conversion succeeded, or default if the conversion failed. The conversion fails if the <paramref name="url"/> parameter
        /// is not in a format compliant with the Open API specification. This parameter is passed uninitialized;
        /// any value originally supplied in result will be overwritten.
        /// </param>
        /// <returns><c>true</c> if <paramref name="url"/> was converted successfully; otherwise, <c>false</c>.</returns>
        public static bool TryParseUrl(Uri url, out Reference result)
        {
            if (url == null)
            {
                result = default;
                return false;
            }

            // TODO: Handle escaped URL characters in pointer.

            string frag = null;
            if (url.IsAbsoluteUri && url.Fragment != null)
            {
                frag = url.GetComponents(UriComponents.Fragment, UriFormat.Unescaped);
                url = new UriBuilder(url) { Fragment = null }.Uri;
            }
            else if (!url.IsAbsoluteUri)
            {
                if (url.OriginalString == "#")
                {
                    result = default;
                    return false;
                }

                var fragIndex = url.OriginalString.IndexOf('#');
                if (fragIndex < 0)
                {
                    // Do nothing.
                }
                else if (fragIndex == 0)
                {
                    frag = url.OriginalString.Substring(fragIndex + 1);
                    url = null;
                }
                else
                {
                    frag = url.OriginalString.Substring(fragIndex + 1);
                    url = new Uri(url.OriginalString.Substring(0, fragIndex), UriKind.Relative);
                }
            }

            if (frag != null && !JsonPointer.TryParse(frag, out var pointer))
            {
                result = default;
                return false;
            }

            result = new Reference(url, pointer);
            return true;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="SourceCode.Clay.Json.Pointers.JsonPointer" /> to <see cref="SourceCode.Clay.OpenApi.Reference" />.
        /// </summary>
        /// <param name="pointer">The pointer.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Reference(JsonPointer pointer) => pointer.Count == 0 ? default : new Reference(pointer);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Uri" /> to <see cref="SourceCode.Clay.OpenApi.Reference" />.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Reference(Uri url) => url == null ? default : ParseUrl(url);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String" /> to <see cref="SourceCode.Clay.OpenApi.Reference" />.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Reference(string url)
        {
            if (string.IsNullOrEmpty(url)) return default;

            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                uri = new Uri(url, UriKind.Relative);

            return ParseUrl(uri);
        }

        /// <summary>
        /// Returns the reference formatted as a <see cref="Uri"/>.
        /// </summary>
        /// <returns>The reference formatted as a <see cref="Uri"/>.</returns>
        public Uri ToUri()
        {
            if (!HasValue) return null;

            var frag = "#" + Uri.EscapeUriString(Pointer.ToString());
            if (Url == null) return new Uri(frag, UriKind.Relative);
            else if (frag == "#") return Url;
            else if (Url.IsAbsoluteUri) return new UriBuilder(Url) { Fragment = frag }.Uri;
            else return new Uri(Url.OriginalString + frag, UriKind.Relative);
        }

        /// <summary>Returns reference formatted as a string.</summary>
        /// <returns>The reference formatted as a string.</returns>
        public override string ToString() => ToUri()?.ToString() ?? string.Empty;

        #endregion
    }
}
