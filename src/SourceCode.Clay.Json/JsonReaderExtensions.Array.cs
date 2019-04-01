#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Threading;
using Newtonsoft.Json;

namespace SourceCode.Clay.Json
{
    partial class JsonReaderExtensions // .Array
    {
        /// <summary>
        /// Processes the current token value as Json array.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="itemFactory">The item factory.</param>
        public static void ReadArray(this JsonReader jr, Action itemFactory)
        {
            if (jr is null) throw new ArgumentNullException(nameof(jr));

            if (jr.TokenType == JsonToken.None)
                jr.Read();

            // null
            if (jr.TokenType == JsonToken.Null)
                return;

            // '['
            if (jr.TokenType == JsonToken.StartArray)
                jr.Read();

            while (true)
            {
                switch (jr.TokenType)
                {
                    // Item
                    default:
                        itemFactory?.Invoke();

                        jr.Read();
                        continue;

                    // Skip
                    case JsonToken.Comment:
                        jr.Read();
                        continue;

                    // ']'
                    case JsonToken.EndArray:
                        return;
                }
            }
        }

        /// <summary>
        /// Reads the current token value as a Json <see cref="IReadOnlyList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="itemFactory">The item factory.</param>
        /// <returns>The value.</returns>
        public static IReadOnlyList<T> ReadArray<T>(this JsonReader jr, Func<T> itemFactory)
        {
            var list = new List<T>(0); // ctor allocates singleton T[0]

            ReadArray(jr, Curry);

            return list;

            // Curry delegate into local function
            void Curry()
            {
                T item = itemFactory();

                if (list.Count == 0)
                    list = new List<T>();

                list.Add(item);
            }
        }

        /// <summary>
        /// Reads the current token value as a Json <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="jsonReader">The <see cref="JsonReader"/> instance.</param>
        /// <param name="itemFactory">The item factory.</param>
        /// <returns>The value.</returns>
        public static IEnumerable<T> EnumerateArray<T>(this JsonReader jsonReader, Func<T> itemFactory)
        {
            if (jsonReader is null) throw new ArgumentNullException(nameof(jsonReader));
            if (itemFactory is null) throw new ArgumentNullException(nameof(itemFactory));

            if (jsonReader.TokenType == JsonToken.None)
                jsonReader.Read();

            // null
            if (jsonReader.TokenType == JsonToken.Null)
                return Array.Empty<T>();

            return Enumerate(jsonReader, itemFactory);

            IEnumerable<T> Enumerate(JsonReader r, Func<T> factory)
            {
                // '['
                if (r.TokenType == JsonToken.StartArray)
                    r.Read();

                while (true)
                {
                    switch (r.TokenType)
                    {
                        // Item
                        default:
                            {
                                T item = factory();

                                r.Read();
                                yield return item;
                            }
                            continue;

                        // Skip
                        case JsonToken.Comment:
                            r.Read();
                            continue;

                        // ']'
                        case JsonToken.EndArray:
                            yield break;
                    }
                }
            }
        }

        /// <summary>
        /// Skips all items in a Json array, but returns the count.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <returns>The number of items skipped.</returns>
        public static int SkipCountArray(this JsonReader jr)
        {
            if (jr is null) throw new ArgumentNullException(nameof(jr));

            var count = 0;

            ReadArray(jr, Curry);

            return count;

            // Curry delegate into local function
            void Curry()
            {
                switch (jr.TokenType)
                {
                    case JsonToken.StartArray:
                    case JsonToken.StartObject:
                        jr.Skip(); // Skip the children of the current token
                        break;
                }

                count = Interlocked.Increment(ref count);
            }
        }
    }
}
