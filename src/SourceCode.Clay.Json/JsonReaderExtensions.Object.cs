#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Threading;
using Newtonsoft.Json;

namespace SourceCode.Clay.Json
{
    partial class JsonReaderExtensions // .Object
    {
        /// <summary>
        /// Processes the current token value as a Json object.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        /// <param name="propertyHandler">The property switch.</param>
        public static void ReadObject(this JsonReader jr, Func<string, bool> propertyHandler)
        {
            if (jr is null) throw new ArgumentNullException(nameof(jr));
            if (propertyHandler is null) throw new ArgumentNullException(nameof(propertyHandler));

            if (jr.TokenType == JsonToken.None)
                jr.Read();

            // null
            if (jr.TokenType == JsonToken.Null)
                return;

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
                            string name = (string)jr.Value;
                            jr.Read();

                            // Value
                            bool handled = propertyHandler(name);
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
                        return;
                }
            }
        }

        /// <summary>
        /// Processes the current token value as a Json object but ignores all values.
        /// </summary>
        /// <param name="jr">The <see cref="JsonReader"/> instance.</param>
        public static int SkipCountObject(this JsonReader jr)
        {
            int count = 0;

            ReadObject(jr, CurryHandler);

            return count;

            // Curry delegate into local function
            bool CurryHandler(string name)
            {
                switch (jr.TokenType)
                {
                    case JsonToken.StartArray:
                    case JsonToken.StartObject:
                        jr.Skip(); // Skip the children of the current token
                        break;
                }

                count = Interlocked.Increment(ref count);

                return true;
            }
        }
    }
}
