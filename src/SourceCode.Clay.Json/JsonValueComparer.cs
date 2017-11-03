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
            public override bool Equals(ReadOnlyJsonObject x, ReadOnlyJsonObject y) => EqualsImpl(x?._json, (JsonValue)y?._json);

            /// <inheritdoc/>
            public override int GetHashCode(JsonValue obj) => GetHashCodeImpl(obj);

            /// <inheritdoc/>
            public override int GetHashCode(ReadOnlyJsonObject obj) => GetHashCodeImpl(obj?._json);

            #endregion

            #region Helpers

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

            private static bool EqualsImpl(JsonValue json1, JsonValue json2)
            {
                if (json1 is null) return json2 is null; // (null, null) or (null, y)
                if (json2 is null) return false; // (x, null)
                if (ReferenceEquals(json1, json2)) return true; // (x, x)

                // Heuristic: We compare in order of most-to-least likely/abundant

                if (json1 is JsonPrimitive jp1)
                    return (json2 is JsonPrimitive jp2) && EqualsImpl(jp1, jp2);

                if (json1 is JsonObject jo1)
                    return (json2 is JsonObject jo2) && EqualsImpl(jo1, jo2);

                if (json1 is JsonArray ja1)
                    return (json2 is JsonArray ja2) && EqualsImpl(ja1, ja2);

                // Fallback for unknown subclasses of JsonValue
                return json1.Equals(json2);
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool EqualsImpl(JsonPrimitive jp1, JsonPrimitive jp2)
            {
                if (jp1.JsonType != jp2.JsonType) return false;

                // We could also compare the ToString() values here, though that incurs at
                // least 1 string alloc, the size of which will depend on the specific primitive.
                //
                // On the other hand Linq expressions are slower than native code and the
                // implementation requires runtime resolution of Equals(obj, obj).
                // Might be where the extra allocs are coming from.
                //
                // TODO: May be worth benchmarking (if not a micro-optimization)

                var jv1 = JsonExtensions.GetValueFromPrimitive(jp1);
                var jv2 = JsonExtensions.GetValueFromPrimitive(jp2);

                // Runtime native object comparison
                if (!jv1.Equals(jv2)) return false;

                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool EqualsImpl(JsonObject jo1, JsonObject jo2)
            {
                if (jo1.Count != jo2.Count) return false; // (n, m)
                if (jo1.Count == 0) return true; // (0, 0)

                using (var je1 = jo1.GetEnumerator())
                using (var je2 = jo2.GetEnumerator())
                {
                    while (je1.MoveNext())
                    {
                        if (!je2.MoveNext()) return false;

                        if (!StringComparer.Ordinal.Equals(je1.Current.Key, je2.Current.Key)) return false;

                        // Recurse
                        if (!EqualsImpl(je1.Current.Value, je2.Current.Value)) return false;
                    }

                    return !je2.MoveNext();
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static bool EqualsImpl(JsonArray ja1, JsonArray ja2)
            {
                var j1Count = ja1.Count;
                if (j1Count != ja2.Count) return false; // (n, m)
                if (j1Count == 0) return true; // (0, 0)

                for (var i = 0; i < j1Count; i++)
                {
                    // Recurse
                    if (!EqualsImpl(ja1[i], ja2[i])) return false;
                }

                return true;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static int GetHashCodeImpl(JsonValue json)
                => json == null ? -42 : json.GetHashCode(); // Thanks for the phish

            #endregion
        }

        #endregion
    }
}
