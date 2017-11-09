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
        /// <param name="propertySwitch">The property switch.</param>
        /// <param name="objectFactory">The object factory.</param>
        /// <returns>The value.</returns>
        public static T ReadObject<T>(this JsonReader jr, Action<string> propertySwitch, Func<T> objectFactory)
        {
            if (jr == null) throw new ArgumentNullException(nameof(jr));
            if (propertySwitch == null) throw new ArgumentNullException(nameof(propertySwitch));

            if (jr.TokenType == JsonToken.None)
                jr.Read();

            // null
            if (jr.TokenType == JsonToken.Null)
            {
                jr.Read();
                return default; // null for classes, default(T) for structs
            }

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
                            propertySwitch(name);
                            jr.Read();
                        }
                        continue;

                    case JsonToken.EndObject: // '}'
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
        /// <param name="propertySwitch">The property switch.</param>
        /// <param name="objectFactory">The object factory.</param>
        public static void ProcessObject(this JsonReader jr, Action<string> propertySwitch, Action objectFactory)
        {
            if (jr == null) throw new ArgumentNullException(nameof(jr));
            if (propertySwitch == null) throw new ArgumentNullException(nameof(propertySwitch));

            // '{'
            while (jr.TokenType == JsonToken.StartObject
                || jr.TokenType == JsonToken.None)
            {
                jr.Read();
            }

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
                            propertySwitch(name);
                            jr.Read();
                        }
                        continue;

                    // '}'
                    case JsonToken.EndObject:
                        objectFactory?.Invoke();
                        return;
                }
            }
        }

        #endregion
    }
}
