using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace SourceCode.Clay.Json
{
    /// <summary>
    /// A lightweight stream reader for parsing Json.
    /// Allocations and state are kept to a minimum.
    /// </summary>
    /// <seealso cref="System.IO.StreamReader" />
    public sealed class JsonStreamReader
    {
        #region Fields

        private readonly TextReader _reader;
        private readonly char[] _buffer = new char[6]; // Longest atomic literal = unicode4 = "\u1234" = 6

        private char _numFirst;
        private JsonToken _token = JsonToken.None;
        private int _pos;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current Token.
        /// </summary>
        /// <value>
        /// The token.
        /// </value>
        public JsonToken Token
        {
            get
            {
                ReadNext();
                return _token;
            }
        }

        /// <summary>
        /// Gets the current position in the stream.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public int Position
        {
            get { return _pos; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonStreamReader"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public JsonStreamReader(TextReader reader)
        {
            //Contract.Requires(reader != null);

            _reader = reader;
            _pos = 0;
        }

        #endregion

        #region Helpers

        private void ReadNext()
        {
            if (_token != JsonToken.None)
                return;

            // Skip whitespace
            // https://msdn.microsoft.com/en-us/library/system.char.iswhitespace.aspx
            int ch;
            do
            {
                ch = _reader.Read();
                if (ch == -1) throw new EndOfStreamException();
                _pos++;
            } while (char.IsWhiteSpace((char)ch));

            // Categorize token
            switch (ch)
            {
                case '"': _token = JsonToken.String; return; // 34
                case ',': _token = JsonToken.Comma; return; // 44
                case ':': _token = JsonToken.Colon; return; // 58

                case '[': _token = JsonToken.ArrayOpen; return; // 91
                case ']': _token = JsonToken.ArrayClose; return; // 93

                case '{': _token = JsonToken.ObjectOpen; return; // 123
                case '}': _token = JsonToken.ObjectClose; return; // 125

                // f_alse (http://json.org - lower literals only)
                case 'f': // 102
                    {
                        var len = _reader.Read(_buffer, 0, 4);

                        if (len >= 4
                            && _buffer.Length >= 4
                            && _buffer[0] == 'a'
                            && _buffer[1] == 'l'
                            && _buffer[2] == 's'
                            && _buffer[3] == 'e')
                        {
                            _token = JsonToken.False;
                            _pos += len;
                            return;
                        }
                    }
                    break;

                // t_rue (http://json.org - lower literals only)
                case 't': // 116
                    {
                        var len = _reader.Read(_buffer, 0, 3);

                        if (len >= 3
                            && _buffer.Length >= 3
                            && _buffer[0] == 'r'
                            && _buffer[1] == 'u'
                            && _buffer[2] == 'e')
                        {
                            _token = JsonToken.True;
                            _pos += len;
                            return;
                        }
                    }
                    break;

                // n_ull (http://json.org - lower literals only)
                case 'n': // 110
                    {
                        var len = _reader.Read(_buffer, 0, 3);

                        if (len >= 3
                            && _buffer.Length >= 3
                            && _buffer[0] == 'u'
                            && _buffer[1] == 'l'
                            && _buffer[2] == 'l')
                        {
                            _token = JsonToken.Null;
                            _pos += len;
                            return;
                        }
                    }
                    break;

                // ['0'..'9', '-']
                case '-': // 45
                case '0': // 48
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9': // 57
                    {
                        _token = JsonToken.Number;
                        _numFirst = (char)ch; // Memoize first digit of Number
                        _pos += 1;
                        return;
                    }

                default: break;
            }

            throw new FormatException($"Invalid Json: Unexpected char \"{ch}\" at position {_pos}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ReadName()
        {
            var name = ParseString(false);
            return name;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SkipExpected(JsonToken expected)
        {
            ReadNext();

            if (_token == expected)
            {
                _token = JsonToken.None;
                return;
            }

            throw new FormatException($"Invalid Json: Expected token \"{expected}\" but got \"{_token}\" at position {_pos}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool SkipIfNull()
        {
            ReadNext();

            if (_token == JsonToken.Null)
            {
                _token = JsonToken.None;
                return true;
            }

            return false;
        }

        #endregion

        #region String

        /// <summary>
        /// Reads the current token value as a <see cref="System.String"/>.
        /// </summary>
        /// <returns>The value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadString()
        {
            if (SkipIfNull())
                return null;

            return ParseString(false);
        }

        /// <summary>
        /// Skips the current token value as a <see cref="System.String"/>.
        /// </summary>
        /// <returns>The value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SkipString()
        {
            if (SkipIfNull())
                return;

            ParseString(true);
        }

        private string ParseString(bool skip)
        {
            SkipExpected(JsonToken.String); // '"'

            StringBuilder sb = null;
            if (!skip)
                sb = new StringBuilder(40); // We need space for at least "{g-u-i-d}" = 2{} + 2"" + 32n + 4- = 40

            for (;;)
            {
                var c = _reader.Read();
                switch (c)
                {
                    case -1: throw new EndOfStreamException();

                    default:
                        {
                            _pos++;
                            if (!skip) sb.Append((char)c);
                            continue;
                        }

                    case '"':
                        {
                            _pos++;

                            if (skip) return null;

                            var s = sb.ToString();
                            return s;
                        }

                    case '\\':
                        {
                            _pos++;

                            var d = _reader.Read();
                            switch (d)
                            {
                                case -1: throw new EndOfStreamException();

                                case '"':
                                case '\\':
                                case '/': _pos++; if (!skip) sb.Append((char)d); continue;
                                case 'b': _pos++; if (!skip) sb.Append('\b'); continue;
                                case 'f': _pos++; if (!skip) sb.Append('\f'); continue;
                                case 'n': _pos++; if (!skip) sb.Append('\n'); continue;
                                case 'r': _pos++; if (!skip) sb.Append('\r'); continue;
                                case 't': _pos++; if (!skip) sb.Append('\t'); continue;
                                case 'u':
                                    _pos++;
                                    d = DecodeUnicode4();
                                    if (!skip) sb.Append((char)d);
                                    continue;

                                default: throw new FormatException($"Invalid Json: Unexpected escape string \"\\{(char)d}\" at position {_pos}");
                            }
                        }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private char DecodeUnicode4()
        {
            //Contract.Requires(_buffer.Length >= 4);

            var len = _reader.Read(_buffer, 0, 4);
            if (len != 4)
                throw new EndOfStreamException();
            _pos += len;

            uint ch = 0;
            byte shft = 16;
            for (var i = 0; i < 4; i++)
            {
                var c = _buffer[i];

                int n;
                if (c >= '0' && c <= '9')
                    n = c - '0';
                else if (c >= 'A' && c <= 'F')
                    n = c - 'A' + 10;
                else if (c >= 'a' && c <= 'f')
                    n = c - 'a' + 10;
                else
                    throw new FormatException($"Invalid Json: Unexpected token in Unicode \"\\u{_buffer[0]}{_buffer[1]}{_buffer[2]}{_buffer[3]}\" at position {_pos}");

                shft -= 4;
                ch += (uint)n << shft;
            }

            return (char)ch;
        }

        #endregion

        #region String[]

        /// <summary>
        /// Reads the current token value as a <see cref="System.String"/> array.
        /// </summary>
        /// <returns>The value.</returns>
        public IReadOnlyList<string> ReadStringArray()
        {
            //Contract.Ensures(Contract.Result<IReadOnlyList<string>>() != null);

            SkipExpected(JsonToken.ArrayOpen); // [

            var list = new List<string>();

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
                        var str = ReadString();
                        list.Add(str);
                        continue;
                }
            }
        }

        #endregion

        #region Boolean

        /// <summary>
        /// Reads the current token value as a <see cref="System.Boolean"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public bool ReadBoolean()
        {
            ReadNext();

            if (_token == JsonToken.False)
            {
                _token = JsonToken.None;
                return false;
            }

            if (_token == JsonToken.True)
            {
                _token = JsonToken.None;
                return true;
            }

            throw new FormatException($"Invalid Json: Expected token \"false\" or \"true\" but got \"{_token}\" at position {_pos}");
        }

        /// <summary>
        /// Reads the current token value as a nullable <see cref="System.Boolean"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public bool? ReadBooleanNullable()
        {
            ReadNext();

            if (_token == JsonToken.False)
            {
                _token = JsonToken.None;
                return false;
            }

            if (_token == JsonToken.True)
            {
                _token = JsonToken.None;
                return true;
            }

            if (_token == JsonToken.Null)
            {
                _token = JsonToken.None;
                return null;
            }

            throw new FormatException($"Invalid Json: Expected token \"false\", \"true\" or \"null\" but got \"{_token}\" at position {_pos}");
        }

        #endregion

        #region Guid

        /// <summary>
        /// Reads the current token value as a <see cref="System.Guid"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public Guid ReadGuid()
        {
            var str = ParseString(false);

            if (!string.IsNullOrWhiteSpace(str))
            {
                Guid guid;
                if (Guid.TryParseExact(str, "D", out guid))
                    return guid;
            }

            throw new FormatException($"Invalid Json: Unexpected {nameof(Guid)} string \"{str}\" at position {_pos}");
        }

        /// <summary>
        /// Reads the current token value as a nullable <see cref="System.Guid"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public Guid? ReadGuidNullable()
        {
            if (SkipIfNull())
                return null;

            return ReadGuid();
        }

        #endregion

        #region DateTime

        /// <summary>
        /// Reads the current token value as a <see cref="System.DateTime"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public DateTime ReadDateTime()
        {
            var str = ParseString(false);

            if (!string.IsNullOrWhiteSpace(str))
            {
                DateTime dt;
                if (DateTime.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dt))
                    return dt;
            }

            throw new FormatException($"Invalid Json: Unexpected {nameof(DateTime)} string \"{str}\" at position {_pos}");
        }

        /// <summary>
        /// Reads the current token value as a nullable <see cref="System.DateTime"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public DateTime? ReadDateTimeNullable()
        {
            if (SkipIfNull())
                return null;

            return ReadDateTime();
        }

        #endregion

        #region DateTimeOffset

        /// <summary>
        /// Reads the current token value as a <see cref="System.DateTimeOffset"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public DateTimeOffset ReadDateTimeOffset()
        {
            var str = ParseString(false);

            if (!string.IsNullOrWhiteSpace(str))
            {
                DateTimeOffset dt;
                if (DateTimeOffset.TryParse(str, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out dt))
                    return dt;
            }

            throw new FormatException($"Invalid Json: Unexpected {nameof(DateTimeOffset)} string \"{str}\" at position {_pos}");
        }

        /// <summary>
        /// Reads the current token value as a nullable <see cref="System.DateTimeOffset"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public DateTimeOffset? ReadDateTimeOffsetNullable()
        {
            if (SkipIfNull())
                return null;

            return ReadDateTimeOffset();
        }

        #endregion

        #region TimeSpan

        /// <summary>
        /// Reads the current token value as a <see cref="System.TimeSpan"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public TimeSpan ReadTimeSpan()
        {
            var str = ParseString(false);

            if (!string.IsNullOrWhiteSpace(str))
            {
                TimeSpan dt;
                if (TimeSpan.TryParse(str, CultureInfo.InvariantCulture, out dt))
                    return dt;
            }

            throw new FormatException($"Invalid Json: Unexpected {nameof(TimeSpan)} string \"{str}\" at position {_pos}");
        }

        /// <summary>
        /// Reads the current token value as a nullable <see cref="System.TimeSpan"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public TimeSpan? ReadTimeSpanNullable()
        {
            if (SkipIfNull())
                return null;

            return ReadTimeSpan();
        }

        #endregion

        #region Number

        /// <summary>
        /// Reads the current token value as a <see cref="System.String"/> representing a numeric value.
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadNumberString()
        {
            ReadNext();

            if (_token != JsonToken.Number)
                throw new FormatException($"Invalid Json: Expected token \"{JsonToken.Number}\" but got \"{_token}\" at position {_pos}");

            if (_numFirst == (char)0)
                throw new FormatException($"Invalid Json: Expected token \"{JsonToken.Number}\" with length 1 but got length 0 at position {_pos}");

            if (_numFirst != '-' && (_numFirst < '0' || _numFirst > '9'))
                throw new FormatException($"Invalid Json: Expected token \"{JsonToken.Number}\" with value '-', '.', '0'..'9' but got \'{_numFirst}\' at position {_pos}");

            try
            {
                var sb = new StringBuilder(20).Append(_numFirst);

                while (true)
                {
                    var ch = _reader.Read();
                    _pos++;

                    switch (ch)
                    {
                        // ['0'..'9']
                        case '0': // 48
                        case '1':
                        case '2':
                        case '3':
                        case '4':
                        case '5':
                        case '6':
                        case '7':
                        case '8':
                        case '9': // 57

                        case '+': // 43
                        case '-': // 45
                        case '.': // 46
                        case 'E': // 69
                        case 'e': // 101
                            sb.Append((char)ch);
                            continue;

                        case ',': _token = JsonToken.Comma; break; // 44
                        case ']': _token = JsonToken.ArrayClose; break; // 93
                        case '}': _token = JsonToken.ObjectClose; break; // 125

                        default:
                            if (!char.IsWhiteSpace((char)ch))
                                throw new FormatException($"Invalid Json: Unexpected {JsonToken.Number} token \'{(char)ch}\' at position {_pos}");

                            _token = JsonToken.None;
                            break;
                    }

                    break;
                }

                var numStr = sb.ToString();
                return numStr;
            }
            finally
            {
                _numFirst = (char)0;
            }
        }

        #endregion

        #region Decimal

        /// <summary>
        /// Reads the current token value as a <see cref="System.Decimal"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public decimal ReadDecimal()
        {
            var str = ReadNumberString();
            var num = decimal.Parse(str, NumberFormatInfo.InvariantInfo);
            return num;
        }

        /// <summary>
        /// Reads the current token value as a nullable <see cref="System.Decimal"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public decimal? ReadDecimalNullable()
        {
            if (SkipIfNull())
                return null;

            return ReadDecimal();
        }

        #endregion

        #region Double

        /// <summary>
        /// Reads the current token value as a <see cref="System.Double"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public double ReadDouble()
        {
            var str = ReadNumberString();
            var num = double.Parse(str, NumberFormatInfo.InvariantInfo);
            return num;
        }

        /// <summary>
        /// Reads the current token value as a nullable <see cref="System.Double"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public double? ReadDoubleNullable()
        {
            if (SkipIfNull())
                return null;

            return ReadDouble();
        }

        #endregion

        #region Int64

        /// <summary>
        /// Reads the current token value as a <see cref="System.Int64"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public long ReadInt64()
        {
            var str = ReadNumberString();
            var num = long.Parse(str, NumberFormatInfo.InvariantInfo);
            return num;
        }

        /// <summary>
        /// Reads the current token value as a nullable <see cref="System.Int64"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public long? ReadInt64Nullable()
        {
            if (SkipIfNull())
                return null;

            return ReadInt64();
        }

        #endregion

        #region UInt64

        /// <summary>
        /// Reads the current token value as a <see cref="System.UInt64"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public ulong ReadUInt64()
        {
            var str = ReadNumberString();
            var num = ulong.Parse(str, NumberFormatInfo.InvariantInfo);
            return num;
        }

        /// <summary>
        /// Reads the current token value as a nullable <see cref="System.UInt64"/>.
        /// </summary>
        /// <returns>The value.</returns>
        public ulong? ReadUInt64Nullable()
        {
            if (SkipIfNull())
                return null;

            return ReadUInt64();
        }

        #endregion

        #region Object

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

        #endregion

        #region Array

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

        #endregion

        #region Nested

        public enum JsonToken : byte
        {
            None = 0, // None

            ObjectOpen, // Object
            ObjectClose,

            ArrayOpen, // Array
            ArrayClose,

            Colon, // Seperator
            Comma,

            String, // Types
            Number,
            True,
            False,
            Null
        }

        #endregion
    }
}
