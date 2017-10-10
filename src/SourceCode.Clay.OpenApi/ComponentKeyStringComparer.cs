#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace SourceCode.Clay.OpenApi
{
    /// <summary>
    /// Represents a comparer that validates values for component keys.
    /// </summary>
    public class ComponentKeyStringComparer : StringComparer
    {
        #region Properties

        /// <summary>
        /// Gets a <see cref="StringComparer"/> that validates values for component keys.
        /// </summary>
        public static ComponentKeyStringComparer ComponentKey { get; } = new ComponentKeyStringComparer();

        #endregion

        #region Fields

        private readonly HashSet<char> _validCharacters;

        /// <summary>
        /// Gets the list of valid characters.
        /// </summary>
        public virtual IEnumerable<char> ValidCharacters => "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.-_";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="ComponentKeyStringComparer"/> class.
        /// </summary>
        public ComponentKeyStringComparer()
        {
            _validCharacters = new HashSet<char>(ValidCharacters);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the specified character value to determine if it is a valid
        /// component name.
        /// </summary>
        /// <param name="value">The character value to validate.</param>
        /// <returns>A value indicating whether the character value is valid.</returns>
        public virtual bool Validate(char value) => _validCharacters.Contains(value);

        /// <summary>
        /// Validates the specified string value to determine if it is a valid
        /// component name.
        /// </summary>
        /// <param name="value">The string value to validate.</param>
        /// <returns>A value indicating whether the string value is valid.</returns>
        public virtual bool Validate(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            for (var i = 0; i < value.Length; i++)
            {
                if (!Validate(value[i])) return false;
            }
            return true;
        }

        /// <summary>
        /// Compares two string values and returns a value indicating sort order.
        /// </summary>
        /// <param name="x">The first value to compare.</param>
        /// <param name="y">The second value to compare.</param>
        /// <returns></returns>
        public override int Compare(string x, string y)
        {
            if (!Validate(x)) throw new ArgumentOutOfRangeException(nameof(x), "The value is not a valid component name.");
            if (!Validate(y)) throw new ArgumentOutOfRangeException(nameof(y), "The value is not a valid component name.");
            return Ordinal.Compare(x, y);
        }

        /// <summary>Indicates whether two strings are equal.</summary>
        /// <param name="x">A string to compare to y.</param>
        /// <param name="y">A string to compare to x.</param>
        /// <returns>true if <paramref name="x">x</paramref> and <paramref name="y">y</paramref> refer to the same object, or <paramref name="x">x</paramref> and <paramref name="y">y</paramref> are equal, or <paramref name="x">x</paramref> and <paramref name="y">y</paramref> are null; otherwise, false.</returns>
        public override bool Equals(string x, string y)
        {
            if (!Validate(x)) throw new ArgumentOutOfRangeException(nameof(x), "The value is not a valid component name.");
            if (!Validate(y)) throw new ArgumentOutOfRangeException(nameof(y), "The value is not a valid component name.");
            return Ordinal.Equals(x, y);
        }

        /// <summary>Gets the hash code for the specified string.</summary>
        /// <param name="obj">A string.</param>
        /// <returns>A 32-bit signed hash code calculated from the value of the <paramref name="obj">obj</paramref> parameter.</returns>
        /// <exception cref="T:System.ArgumentException">Not enough memory is available to allocate the buffer that is required to compute the hash code.</exception>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="obj">obj</paramref> is null.</exception>
        public override int GetHashCode(string obj)
        {
            if (!Validate(obj)) throw new ArgumentOutOfRangeException(nameof(obj), "The value is not a valid component name.");
            return Ordinal.GetHashCode(obj);
        }

        #endregion
    }
}
