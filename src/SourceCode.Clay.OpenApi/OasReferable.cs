#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json.Pointers;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents either a reference object or a value.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    public readonly struct OasReferable<T> : IEquatable<OasReferable<T>>, IOasReferable
        where T : class, IEquatable<T>
    {
        #region Properties

        /// <summary>
        /// Gets the contained value.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the contained reference.
        /// </summary>
        public OasReference Reference { get; }

        /// <summary>
        /// Gets a value indicating whether the referable is a reference.
        /// </summary>
        public bool IsReference => Reference.HasValue;

        /// <summary>
        /// Gets a value indicating whether the reference is a value.
        /// </summary>
        public bool IsValue => Value != null;

        /// <summary>
        /// Gets a value indicating whether the reference is null.
        /// </summary>
        public bool HasValue => IsReference || IsValue;

        /// <summary>Gets the contained value.</summary>
        object IOasReferable.Value => Value;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new value <see cref="OasReferable{T}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        public OasReferable(T value)
        {
            Value = value;
            Reference = default;
        }

        /// <summary>
        /// Creates a new internal reference <see cref="OasReferable{T}"/>.
        /// </summary>
        /// <param name="reference">The reference.</param>
        public OasReferable(OasReference reference)
        {
            if (!reference.HasValue) throw new ArgumentOutOfRangeException(nameof(reference));
            Value = default;
            Reference = reference;
        }

        /// <summary>
        /// Creates a new internal reference <see cref="OasReferable{T}"/>.
        /// </summary>
        /// <param name="internalReference">The internal reference.</param>
        public OasReferable(JsonPointer internalReference)
        {
            if (internalReference.Count == 0) throw new ArgumentOutOfRangeException(nameof(internalReference));
            Value = default;
            Reference = internalReference;
        }

        /// <summary>
        /// Creates a new external reference <see cref="OasReferable{T}"/>.
        /// </summary>
        /// <param name="url">The external reference URL.</param>
        /// <param name="reference">The external reference pointer.</param>
        public OasReferable(Uri url, JsonPointer reference)
        {
            if (url == null && reference.Count == 0) throw new ArgumentNullException(nameof(url));
            Value = default;
            Reference = new OasReference(url, reference);
        }

        #endregion

        #region IEquatable

        /// <summary>
        /// Implements the operator == operator.
        /// </summary>
        /// <param name="referable1">The referable1.</param>
        /// <param name="referable2">The referable2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(OasReferable<T> referable1, OasReferable<T> referable2) => referable1.Equals(referable2);

        /// <summary>
        /// Implements the operator != operator.
        /// </summary>
        /// <param name="referable1">The referable1.</param>
        /// <param name="referable2">The referable2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(OasReferable<T> referable1, OasReferable<T> referable2) => !(referable1 == referable2);

        /// <summary>
        /// Performs an implicit conversion from <typeparamref name="T" /> to <see cref="OpenApi.OasReferable{T}" />.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<T>(T value) => new OasReferable<T>(value);

        /// <summary>
        /// Performs an implicit conversion from <see cref="SourceCode.Clay.Json.Pointers.JsonPointer" /> to <see cref="SourceCode.Clay.OpenApi.OasReferable{T}" />.
        /// </summary>
        /// <param name="internalReference">The internal reference.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<T>(JsonPointer internalReference) => new OasReferable<T>(internalReference);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Uri" /> to <see cref="SourceCode.Clay.OpenApi.OasReferable{T}" />.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<T>(Uri reference) => new OasReferable<T>(reference);

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String" /> to <see cref="SourceCode.Clay.OpenApi.OasReferable{T}" />.
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator OasReferable<T>(string reference) => new OasReferable<T>(reference);

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj)
            => obj is OasReferable<T> other
            && Equals(other);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(OasReferable<T> other)
        {
            if (IsReference != other.IsReference) return false;
            if (IsReference) return Reference.Equals(other.Reference);
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode() => HashCode.Combine(
            Reference,
            Value
        );

        /// <summary>Returns the string representation of the reference.</summary>
        /// <returns>The string representation of the reference.</returns>
        public override string ToString()
        {
            if (IsReference) return Reference.ToString();
            else if (IsValue) return Value.ToString();
            else return string.Empty;
        }

        #endregion
    }
}
