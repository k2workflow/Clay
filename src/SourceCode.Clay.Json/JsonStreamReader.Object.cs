using System;

namespace SourceCode.Clay.Json
{
    public sealed partial class JsonStreamReader // .Object
    {
        /// <summary>
        /// Reads the current token value as a Json object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertySwitch">The property switch.</param>
        /// <param name="objectFactory">The object factory.</param>
        /// <returns>The value.</returns>
        public T ReadObject<T>(Action<string> propertySwitch, Func<T> objectFactory)
        {
            SkipExpected(JsonToken.ObjectOpen); // '{'

            for (;;)
            {
                ReadNext();

                switch (_token)
                {
                    case JsonToken.Comma: // ','
                        _token = JsonToken.None;
                        continue;

                    case JsonToken.ObjectClose: // '}'
                        _token = JsonToken.None;
                        var obj = objectFactory == null ? default(T) : objectFactory();
                        return obj;

                    default:
                        var name = ReadName(); // name
                        SkipExpected(JsonToken.Colon); // ':'
                        propertySwitch(name); // value
                        continue;
                }
            }
        }

        /// <summary>
        /// Process the current token value as a Json object.
        /// </summary>
        /// <param name="propertySwitch">The property switch.</param>
        /// <param name="objectFactory">The object factory.</param>
        public void ReadObject(Action<string> propertySwitch, Action objectFactory)
        {
            SkipExpected(JsonToken.ObjectOpen); // '{'

            for (;;)
            {
                ReadNext();

                switch (_token)
                {
                    case JsonToken.Comma: // ','
                        _token = JsonToken.None;
                        continue;

                    case JsonToken.ObjectClose: // '}'
                        _token = JsonToken.None;
                        objectFactory?.Invoke();
                        return;

                    default:
                        var name = ReadName(); // name
                        SkipExpected(JsonToken.Colon); // ':'
                        propertySwitch(name); // value
                        continue;
                }
            }
        }

        /// <summary>
        /// Process the current token value as a Json object.
        /// </summary>
        /// <param name="propertySwitch">The property switch.</param>
        public void ReadObject(Action<string> propertySwitch)
        {
            SkipExpected(JsonToken.ObjectOpen); // '{'

            for (;;)
            {
                ReadNext();

                switch (_token)
                {
                    case JsonToken.Comma: // ','
                        _token = JsonToken.None;
                        continue;

                    case JsonToken.ObjectClose: // '}'
                        _token = JsonToken.None;
                        return;

                    default:
                        var name = ReadName(); // name
                        SkipExpected(JsonToken.Colon); // ':'
                        propertySwitch(name); // value
                        continue;
                }
            }
        }
    }
}
