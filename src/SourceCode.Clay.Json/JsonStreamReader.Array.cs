using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Json
{
    public sealed partial class JsonStreamReader // .Array
    {
        /// <summary>
        /// Reads the current token value as a Json array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemFactory">The item factory.</param>
        /// <returns>The value.</returns>
        public IReadOnlyList<T> ReadArray<T>(Func<T> itemFactory)
        {
            //Contract.Ensures(Contract.Result<IReadOnlyList<T>>() != null);

            SkipExpected(JsonToken.ArrayOpen); // '['

            var list = new List<T>();

            for (;;)
            {
                ReadNext();

                switch (_token)
                {
                    case JsonToken.Comma: // ','
                        _token = JsonToken.None;
                        continue;

                    case JsonToken.ArrayClose: // ']'
                        _token = JsonToken.None;
                        return list;

                    default:
                        var item = itemFactory();
                        list.Add(item);
                        continue;
                }
            }
        }

        /// <summary>
        /// Processes the current token value as Json array.
        /// </summary>
        /// <param name="itemFactory">The item factory.</param>
        public void ReadArray(Action itemFactory)
        {
            SkipExpected(JsonToken.ArrayOpen); // '['

            for (;;)
            {
                ReadNext();

                switch (_token)
                {
                    case JsonToken.Comma: // ','
                        _token = JsonToken.None;
                        continue;

                    case JsonToken.ArrayClose: // ']'
                        _token = JsonToken.None;
                        return;

                    default:
                        itemFactory();
                        continue;
                }
            }
        }
    }
}
