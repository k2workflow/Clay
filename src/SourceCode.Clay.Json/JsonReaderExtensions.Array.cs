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
        /// Reads the current token value as a Json <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="itemFactory">The item factory.</param>
        /// <returns>The value.</returns>
        public static List<T> ReadArray<T>(this JsonReader jr, Func<T> itemFactory)
        {
            if (jr == null) throw new ArgumentNullException(nameof(jr));
            if (itemFactory == null) throw new ArgumentNullException(nameof(itemFactory));

            // '['
            while (jr.TokenType == JsonToken.StartArray
                || jr.TokenType == JsonToken.None)
            {
                jr.Read();
            }

            var list = new List<T>();

            while (true)
            {
                switch (jr.TokenType)
                {
                    // Item
                    default:
                        var item = itemFactory();
                        list.Add(item);

                        jr.Read();
                        continue;

                    // ']'
                    case JsonToken.EndArray:
                        return list;
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

            // '['
            while (jr.TokenType == JsonToken.StartArray
                || jr.TokenType == JsonToken.None)
            {
                jr.Read();
            }

            while (true)
            {
                switch (jr.TokenType)
                {
                    // Item
                    default:
                        var item = itemFactory();

                        jr.Read();
                        yield return item;

                        continue;

                    // ']'
                    case JsonToken.EndArray:
                        yield break;
                }
            }
        }

        /// <summary>
        /// Processes the current token value as Json array.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="itemFactory">The item factory.</param>
        public static void ProcessArray(this JsonReader jr, Action itemFactory)
        {
            if (jr == null) throw new ArgumentNullException(nameof(jr));

            // '['
            while (jr.TokenType == JsonToken.StartArray
                || jr.TokenType == JsonToken.None)
            {
                jr.Read();
            }

            while (true)
            {
                switch (jr.TokenType)
                {
                    // Item
                    default:
                        itemFactory?.Invoke();
                        jr.Read();
                        continue;

                    // ']'
                    case JsonToken.EndArray:
                        return;
                }
            }
        }

        #endregion
    }
}
