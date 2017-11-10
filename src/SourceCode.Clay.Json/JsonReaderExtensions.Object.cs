#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json;
using System;

namespace SourceCode.Clay.Json
{
    partial class JsonReaderExtensions // .Object
    {
        #region Methods

        /// <summary>
        /// Reads the current token value as a Json object.
        /// </summary>
        /// <typeparam name="T">The type of item to return.</typeparam>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="propertyHandler">The property switch.</param>
        /// <param name="objectFactory">The object factory.</param>
        /// <returns>The value.</returns>
        public static T ReadObject<T>(this JsonReader jr, Func<string, bool> propertyHandler, Func<T> objectFactory)
        {
            if (jr == null) throw new ArgumentNullException(nameof(jr));
            if (propertyHandler == null) throw new ArgumentNullException(nameof(propertyHandler));

            if (jr.TokenType == JsonToken.None)
                jr.Read();

            // null
            if (jr.TokenType == JsonToken.Null)
                return default; // null for classes, default(T) for structs

            // '{'
            if (jr.TokenType == JsonToken.StartObject)
                jr.Read();

            while (true)
            {
                switch (jr.TokenType)
                {
                    // Property
                    case JsonToken.PropertyName:
                        {
                            // Name
                            var name = (string)jr.Value;
                            jr.Read();

                            // Value
                            var handled = propertyHandler(name);
                            if (!handled)
                                throw new JsonReaderException($"Json property {name} found but not processed");

                            jr.Read();
                        }
                        continue;

                    // Skip
                    case JsonToken.Comment:
                        jr.Read();
                        continue;

                    // '}'
                    case JsonToken.EndObject:
                        {
                            if (objectFactory == null) return default;

                            var obj = objectFactory();
                            return obj;
                        }
                }
            }
        }

        /// <summary>
        /// Process the current token value as a Json object.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="propertyHandler">The property switch.</param>
        /// <param name="objectFactory">The object factory.</param>
        public static void ProcessObject(this JsonReader jr, Func<string, bool> propertyHandler, Action objectFactory)
        {
            // Leverage shared logic, ignoring sentinel return <int> value
            ReadObject(jr, propertyHandler, Curry);

            // Curry delegate into local function
            int Curry()
            {
                objectFactory?.Invoke();
                return 0;
            }
        }

        #endregion
    }
}
