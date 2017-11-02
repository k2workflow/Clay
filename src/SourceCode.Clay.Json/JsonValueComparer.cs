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
    public abstract class JsonValueComparer :
        IEqualityComparer<JsonValue>,
        IEqualityComparer<ReadOnlyJsonObject>
    {
        #region Constants

        /// <summary>
        /// Gets a <see cref="JsonValueComparer"/> that compares all fields of a <see cref="JsonValue"/>
        /// value in a strict manner (ordinal string comparisons, determinisitc ordering of members).
        /// </summary>
        public static JsonValueComparer Default { get; } = new JsonStrictComparer();

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="JsonValueComparer"/> class.
        /// </summary>
        protected JsonValueComparer()
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
        public abstract bool Equals(JsonValue x, JsonValue y);

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
        public abstract int GetHashCode(JsonValue obj);

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public abstract int GetHashCode(ReadOnlyJsonObject obj);

        #endregion

        #region Concrete

        /// <summary>
        /// Represents a way to compare two <see cref="JsonValue"/> or <see cref="ReadOnlyJsonObject"/> instances
        /// in a case-sensitive and order-sensitive manner.
        /// </summary>
        private sealed class JsonStrictComparer : JsonValueComparer
        {
            #region Constructors

            /// <inheritdoc/>
            internal JsonStrictComparer()
            { }

            #endregion

            #region Methods

            /// <inheritdoc/>
            public override bool Equals(JsonValue x, JsonValue y) => EqualsImpl(x, y);

            /// <inheritdoc/>
            public override bool Equals(ReadOnlyJsonObject x, ReadOnlyJsonObject y) => EqualsImpl(x?._json, y?._json);

            /// <inheritdoc/>
            public override int GetHashCode(JsonValue obj) => GetHashCodeImpl(obj);

            /// <inheritdoc/>
            public override int GetHashCode(ReadOnlyJsonObject obj) => GetHashCodeImpl(obj?._json);

            #endregion

            #region Helpers

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool EqualsImpl(JsonValue x, JsonValue y)
            {
                if (x is null) return y is null; // (null, null) or (null, y)
                if (y is null) return false; // (x, null)
                if (ReferenceEquals(x, y)) return true; // (x, x)

                /*
                We could also compare the ToString() values here, though that incurs at
                least 1 string alloc, the size of which will depend on the specific type.
                For example a JsonArray(n) would incur at least n allocs, while a
                JsonPrimitive would incur at least 1.

                So instead we walk the tree, comparing each item. Note that ToString()
                walks the tree regardless, so should not be significantly
                more expensive than the latter method.

                Benchmarks validate this approach: 2-3x improvement in both time & memory
                https://github.com/dotnet/corefx/issues/25021

                           Method |      Mean |     Error |    StdDev | Scaled |    Gen 0 |    Gen 1 |   Gen 2 | Allocated |
                ----------------- |----------:|----------:|----------:|-------:|---------:|---------:|--------:|----------:|
                   ToStringEquals | 13.818 ms | 0.3012 ms | 0.3347 ms |   1.00 | 808.4677 | 435.9879 | 89.7177 | 6267582 B |
                 NewtonDeepEquals |  3.028 ms | 0.0657 ms | 0.1854 ms |   0.22 |        - |        - |       - |       0 B |
                      SmartEquals |  5.940 ms | 0.1109 ms | 0.1187 ms |   0.43 | 531.2500 |        - |       - | 2258560 B |

                The allocations are likely due to reflection/expression gymnastics required for JsonPrimitive.Value.
                */

                // Heuristic: We assume primitives are most likely/abundant
                if (x is JsonPrimitive xp)
                    return (y is JsonPrimitive yp) && PrimitiveEquals(ref xp, ref yp);

                if (x is JsonObject xo)
                    return (y is JsonObject yo) && ObjectEquals(ref xo, ref yo);

                // Least likely last
                if (x is JsonArray xa)
                    return (y is JsonArray ya) && ArrayEquals(ref xa, ref ya);

                return true;

                // Local functions

                bool ArrayEquals(ref JsonArray a, ref JsonArray b)
                {
                    if (a is null) return b is null; // (null, null) or (null, y)
                    if (b is null) return false; // (x, null)
                    if (ReferenceEquals(a, b)) return true; // (x, x)

                    // Item count
                    var aCount = a.Count;
                    if (aCount != b.Count) return false; // (n, m)
                    if (aCount == 0) return true; // (0, 0)

                    // Values
                    // Avoid string allocs by enumerating colocated array members
                    for (var i = 0; i < a.Count; i++)
                    {
                        if (!EqualsImpl(a[i], b[i])) return false; // Recurse
                    }

                    return true;
                }

                bool ObjectEquals(ref JsonObject a, ref JsonObject b)
                {
                    if (a is null) return b is null; // (null, null) or (null, y)
                    if (b is null) return false; // (x, null)
                    if (ReferenceEquals(a, b)) return true; // (x, x)

                    // Property count
                    var aCount = a.Count;
                    if (aCount != b.Count) return false; // (n, m)
                    if (aCount == 0) return true; // (0, 0)

                    // Value
                    // Avoid string allocs by enumerating colocated properties
                    using (var ae = a.GetEnumerator())
                    using (var be = b.GetEnumerator())
                    {
                        while (ae.MoveNext())
                        {
                            if (!be.MoveNext()) return false;

                            var ac = ae.Current;
                            var bc = be.Current;

                            // Key
                            if (!StringComparer.Ordinal.Equals(ac.Key, bc.Key)) return false;

                            // Value
                            if (!EqualsImpl(ac.Value, bc.Value)) return false; // Recurse
                        }

                        return !be.MoveNext();
                    }
                }

                bool PrimitiveEquals(ref JsonPrimitive a, ref JsonPrimitive b)
                {
                    if (a is null) return b is null; // (null, null) or (null, y)
                    if (b is null) return false; // (x, null)
                    if (ReferenceEquals(a, b)) return true; // (x, x)

                    // JsonType
                    if (a.JsonType != b.JsonType) return false;

                    // We could also compare the ToString() values here, though that incurs at
                    // least 1 string alloc, the size of which will depend on the specific primitive.
                    //
                    // On the other hand Linq expressions are slower than native code and the
                    // implementation requires runtime resolution of Equals(obj, obj).
                    // Might be where the extra allocs are coming from.
                    //
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
                => json == null ? -42 : json.GetHashCode(); // Thanks for the phish

            #endregion
        }

        #endregion
    }
}
