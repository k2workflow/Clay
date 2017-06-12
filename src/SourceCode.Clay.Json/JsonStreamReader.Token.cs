namespace SourceCode.Clay.Json
{
    public sealed partial class JsonStreamReader // .Token
    {
        public enum JsonToken : byte
        {
            None = 0, // None

            ObjectOpen, // Object
            ObjectClose,

            ArrayOpen, // Array
            ArrayClose,

            Colon, // Delimiters
            Comma,

            String, // Types
            Number,
            True,
            False,
            Null
        }
    }
}
