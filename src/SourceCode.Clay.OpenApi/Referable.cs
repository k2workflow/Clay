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
    public struct Referable<T> : IEquatable<Referable<T>>
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
        public Reference Reference { get; }

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

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new value <see cref="Referable{T}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        public Referable(T value)
        {
            Value = value;
            Reference = default;
        }

        /// <summary>
        /// Creates a new internal reference <see cref="Referable{T}"/>.
        /// </summary>
        /// <param name="reference">The reference.</param>
        public Referable(Reference reference)
        {
            if (!reference.HasValue) throw new ArgumentOutOfRangeException(nameof(reference));
            Value = default;
            Reference = reference;
        }

        /// <summary>
        /// Creates a new internal reference <see cref="Referable{T}"/>.
        /// </summary>
        /// <param name="internalReference">The internal reference.</param>
        public Referable(JsonPointer internalReference)
        {
            if (internalReference.Count == 0) throw new ArgumentOutOfRangeException(nameof(internalReference));
            Value = default;
            Reference = internalReference;
        }

        /// <summary>
        /// Creates a new external reference <see cref="Referable{T}"/>.
        /// </summary>
        /// <param name="url">The external reference URL.</param>
        /// <param name="reference">The external reference pointer.</param>
        public Referable(Uri url, JsonPointer reference)
        {
            if (url == null && reference.Count == 0) throw new ArgumentNullException(nameof(url));
            Value = default;
            Reference = new Reference(url, reference);
        }

        #endregion

        #region Methods

        public static bool operator ==(Referable<T> referable1, Referable<T> referable2) => referable1.Equals(referable2);

        public static bool operator !=(Referable<T> referable1, Referable<T> referable2) => !(referable1 == referable2);

        public static implicit operator Referable<T>(T value) => new Referable<T>(value);

        public static implicit operator Referable<T>(JsonPointer internalReference) => new Referable<T>(internalReference);

        public static implicit operator Referable<T>(Uri reference) => new Referable<T>(reference);

        public static implicit operator Referable<T>(string reference) => new Referable<T>(reference);

        /// <summary>Indicates whether this instance and a specified object are equal.</summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>true if <paramref name="obj">obj</paramref> and this instance are the same type and represent the same value; otherwise, false.</returns>
        public override bool Equals(object obj) => obj is Referable<T> o && Equals(o);

        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
        public bool Equals(Referable<T> other)
        {
            if (IsReference != other.IsReference) return false;
            if (IsReference) return Reference.Equals(other.Reference);
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        /// <summary>Returns the hash code for this instance.</summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hc = 17L;

                if (IsReference) hc = hc * 21 + Reference.GetHashCode();
                if (IsValue) hc = hc * 21 + EqualityComparer<T>.Default.GetHashCode(Value);

                return ((int)(hc >> 32)) ^ (int)hc;
            }
        }

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
