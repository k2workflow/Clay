#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Json;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Json
{
    /// <summary>
    /// Represents a way to compare <see cref="JsonValue"/> and <see cref="ReadOnlyJsonObject"/> values.
    /// </summary>
    public abstract class JsonComparer :
        IEqualityComparer<JsonArray>, // We use specific logic for each JsonValue subtype
        IEqualityComparer<JsonObject>,
        IEqualityComparer<JsonPrimitive>,
        IEqualityComparer<ReadOnlyJsonObject>
    {
        #region Constants

        /// <summary>
        /// Gets a <see cref="JsonComparer"/> that compares all fields of a <see cref="JsonValue"/>
        /// value in a strict manner (ordinal string comparisons, determinisitc ordering of members).
        /// </summary>
        public static JsonComparer Default { get; } = new JsonStrictComparer();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="JsonComparer"/> class.
        /// </summary>
        protected JsonComparer()
        { }

        #endregion

        #region IEqualityComparer

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public abstract bool Equals(JsonArray x, JsonArray y);

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public abstract bool Equals(JsonObject x, JsonObject y);

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public abstract bool Equals(JsonPrimitive x, JsonPrimitive y);

        /// <summary>
        /// Determines whether the specified objects are equal.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// true if the specified objects are equal; otherwise, false.
        /// </returns>
        public abstract bool Equals(ReadOnlyJsonObject x, ReadOnlyJsonObject y);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public abstract int GetHashCode(JsonArray obj);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public abstract int GetHashCode(JsonObject obj);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public abstract int GetHashCode(JsonPrimitive obj);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public abstract int GetHashCode(ReadOnlyJsonObject obj);

        #endregion

        #region Helpers

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool EqualsImpl(JsonValue x, JsonValue y)
        {
            if (x is null) return y is null; // (null, null) or (null, y)
            if (y is null) return false; // (x, null)
            if (ReferenceEquals(x, y)) return true; // (x, x)

            // We could also compare the ToString() values here, though that incurs at
            // least 1 string alloc, the size of which will depend on the specific type.
            // For example a JsonArray(n) would incur at least n allocs, while a
            // JsonPrimitive would incur at least 1.
            // So instead we walk the tree, comparing each item. Note that ToString()
            // walks the tree regardless, so we can be sure this is not significantly
            // more expensive than that method.

            // Heuristic: Check for most common first
            if (x is JsonPrimitive xp)
                return (y is JsonPrimitive yp) && PrimitiveEquals(xp, yp);

            if (x is JsonObject xo)
                return (y is JsonObject yo) && ObjectEquals(xo, yo);

            // Least common last
            if (x is JsonArray xa)
                return (y is JsonArray ya) && ArrayEquals(xa, ya);

            return true;

            // Local functions

            bool ArrayEquals(JsonArray a, JsonArray b)
            {
                if (a is null) return b is null; // (null, null) or (null, y)
                if (b is null) return false; // (x, null)
                if (ReferenceEquals(a, b)) return true; // (x, x)

                // Item count
                if (a.Count != b.Count) return false; // (n, m)
                if (a.Count == 0) return true; // (0, 0)

                // Values
                // Avoid string allocs by enumerating colocated array members
                for (var i = 0; i < a.Count; i++)
                {
                    if (!EqualsImpl(a[i], b[i])) return false; // Recurse
                }

                return true;
            }

            bool ObjectEquals(JsonObject a, JsonObject b)
            {
                if (a is null) return b is null; // (null, null) or (null, y)
                if (b is null) return false; // (x, null)
                if (ReferenceEquals(a, b)) return true; // (x, x)

                // Property count
                if (a.Count != b.Count) return false; // (n, m)
                if (a.Count == 0) return true; // (0, 0)

                // Value
                // Avoid string allocs by enumerating colocated properties
                using (var ae = a.GetEnumerator())
                using (var be = b.GetEnumerator())
                {
                    while (ae.MoveNext())
                    {
                        if (!be.MoveNext()) return false;

                        if (!KeyValueEquals(ae.Current, be.Current)) return false;
                    }

                    return !be.MoveNext();
                }
            }

            bool KeyValueEquals(KeyValuePair<string, JsonValue> a, KeyValuePair<string, JsonValue> b)
            {
                if (a.Equals(default)) return b.Equals(default); // (null, null) or (null, y)
                if (b.Equals(default)) return false; // (x, null)

                // Key
                if (!StringComparer.Ordinal.Equals(a.Key, b.Key)) return false;

                // Value
                if (!EqualsImpl(a.Value, b.Value)) return false; // Recurse

                return true;
            }

            bool PrimitiveEquals(JsonPrimitive a, JsonPrimitive b)
            {
                if (a is null) return b is null; // (null, null) or (null, y)
                if (b is null) return false; // (x, null)
                if (ReferenceEquals(a, b)) return true; // (x, x)

                // JsonType
                if (a.JsonType != b.JsonType) return false;

                // We could also compare the ToString() values here, though that incurs at
                // least 1 string alloc, the size of which will depend on the specific primitive.
                // On the other hand Linq expressions are slower than native code, and the
                // implementation requires runtime resolution of Equals(obj, obj).
                // TODO: May be worth benchmarking (if not a micro-optimization)

                // Value
                var av = JsonExtensions.GetValueFromPrimitive(a);
                var bv = JsonExtensions.GetValueFromPrimitive(b);

                if (!av.Equals(bv)) return false; // Runtime native Object comparison

                return true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetHashCodeImpl(JsonValue json)
            => json == null ? -42 : json.GetHashCode(); // Thanks for all the phish

        #endregion

        #region Concrete

        /// <summary>
        /// Represents a way to compare two <see cref="JsonValue"/> instances in an case and order-sensitive manner.
        /// </summary>
        private sealed class JsonStrictComparer : JsonComparer
        {
            #region Constructors

            /// <inheritdoc/>
            internal JsonStrictComparer()
            { }

            #endregion

            #region IEqualityComparer

            /// <inheritdoc/>
            public override bool Equals(JsonArray x, JsonArray y) => EqualsImpl(x, y);

            /// <inheritdoc/>
            public override bool Equals(JsonObject x, JsonObject y) => EqualsImpl(x, y);

            /// <inheritdoc/>
            public override bool Equals(JsonPrimitive x, JsonPrimitive y) => EqualsImpl(x, y);

            /// <inheritdoc/>
            public override bool Equals(ReadOnlyJsonObject x, ReadOnlyJsonObject y) => EqualsImpl(x?._json, y?._json);

            /// <inheritdoc/>
            public override int GetHashCode(JsonArray obj) => GetHashCodeImpl(obj);

            /// <inheritdoc/>
            public override int GetHashCode(JsonObject obj) => GetHashCodeImpl(obj);

            /// <inheritdoc/>
            public override int GetHashCode(JsonPrimitive obj) => GetHashCodeImpl(obj);

            /// <inheritdoc/>
            public override int GetHashCode(ReadOnlyJsonObject obj) => GetHashCodeImpl(obj?._json);

            #endregion
        }

        #endregion
    }
}
