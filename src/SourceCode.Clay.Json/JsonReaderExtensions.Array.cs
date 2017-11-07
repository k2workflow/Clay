#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Json
{
    partial class JsonReaderExtensions // .Array
    {
        #region Methods

        /// <summary>
        /// Reads the current token value as a Json <see cref="IList{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="itemFactory">The item factory.</param>
        /// <returns>The value.</returns>
        public static IReadOnlyList<T> ReadArray<T>(this JsonReader jr, Func<T> itemFactory)
        {
            if (jr == null) throw new ArgumentNullException(nameof(jr));
            if (itemFactory == null) throw new ArgumentNullException(nameof(itemFactory));

            if (jr.TokenType == JsonToken.None)
                jr.Read();

            // null
            if (jr.TokenType == JsonToken.Null)
            {
                jr.Read();
                return default;
            }

            // '['
            if (jr.TokenType == JsonToken.StartArray)
                jr.Read();

            List<T> list = null;
            while (true)
            {
                switch (jr.TokenType)
                {
                    // Item
                    default:
                        {
                            var item = itemFactory();

                            list = list ?? new List<T>();
                            list.Add(item);

                            jr.Read();
                        }
                        continue;

                    // ']'
                    case JsonToken.EndArray:
                        {
                            if (list == null) return Array.Empty<T>();
                            return list;
                        }
                }
            }
        }

        /// <summary>
        /// Reads the current token value as a Json <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="itemFactory">The item factory.</param>
        /// <returns>The value.</returns>
        public static IEnumerable<T> EnumerateArray<T>(this JsonReader jr, Func<T> itemFactory)
        {
            if (jr == null) throw new ArgumentNullException(nameof(jr));
            if (itemFactory == null) throw new ArgumentNullException(nameof(itemFactory));

            if (jr.TokenType == JsonToken.None)
                jr.Read();

            // null
            if (jr.TokenType == JsonToken.Null)
            {
                jr.Read();
                yield break;
            }

            // '['
            if (jr.TokenType == JsonToken.StartArray)
                jr.Read();

            while (true)
            {
                switch (jr.TokenType)
                {
                    // Item
                    default:
                        {
                            var item = itemFactory();

                            jr.Read();
                            yield return item;
                        }
                        continue;

                    // ']'
                    case JsonToken.EndArray:
                        yield break;
                }
            }
        }

        #endregion
    }
}
