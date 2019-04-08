#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Xunit;

namespace SourceCode.Clay.Json.Units
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class JsonReaderExtensionTests
    {
        [Trait("Type", "Unit")]
        [Theory]
        [InlineData("{}")]
        [InlineData(" {\t\n   \t} \r\n")]
        public static void When_read_empty_object(string json)
        {
            // Read
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                int actual = 0;
                jr.ReadObject(n =>
                {
                    actual++;
                    return false;
                });

                Assert.Equal(0, actual);
            }

            // Skip
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                int actualCount = jr.SkipObject();

                Assert.True(jr.TokenType == JsonToken.EndObject);
                Assert.Equal(0, actualCount);
            }
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData("[]")]
        [InlineData(" [\t\n   \t] \r\n")]
        public static void When_read_empty_array(string json)
        {
            // Read
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                IReadOnlyList<int> actual = jr.ReadArray(() => (int)jr.Value);
                Assert.Empty(actual);
            }

            // Enumerate
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                IEnumerable<int> actual = jr.EnumerateArray(() => (int)jr.Value);
                Assert.Empty(actual);
            }

            // Process
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                int actualCount = 0;
                jr.ReadArray(() => actualCount++);
                Assert.Equal(0, actualCount);
            }

            // Skip
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                int actualCount = jr.SkipCountArray();

                Assert.True(jr.TokenType == JsonToken.EndArray);
                Assert.Equal(0, actualCount);
            }
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData("{ \"a\": null }")]
        [InlineData("{ \t\n \"a\": \t\n null \t\n\t } \r\n")]
        public static void When_read_null_object(string json)
        {
            // Read
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                string actual = null;

                jr.ReadObject(n =>
                {
                    switch (n)
                    {
                        case "a": actual = (string)jr.Value; return true;
                    }

                    return false;
                });

                Assert.Null(actual);
            }

            // Skip
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                int actualCount = jr.SkipObject();

                Assert.True(jr.TokenType == JsonToken.EndObject);
                Assert.Equal(1, actualCount);
            }
        }

        [Trait("Type", "Unit")]
        [Theory]
        [InlineData("[ null ]")]
        [InlineData("[ \t\n null \t\n\t ] \r\n")]
        public static void When_read_null_array(string json)
        {
            // Read
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                IReadOnlyList<string> actual = jr.ReadArray(() => (string)jr.Value);
                Assert.Equal(new string[] { null }, actual);
            }

            // Enumerate
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                IEnumerable<string> actual = jr.EnumerateArray(() => (string)jr.Value);
                Assert.Equal(new string[] { null }, actual);
            }

            // Process
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actual = new List<string>();
                jr.ReadArray(() => actual.Add((string)jr.Value));
                Assert.Equal(new string[] { null }, actual);
            }

            // Skip
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                int actualCount = jr.SkipCountArray();

                Assert.True(jr.TokenType == JsonToken.EndArray);
                Assert.Equal(1, actualCount);
            }
        }

        private static readonly Guid s_guid = new Guid("82a7f48d-3b50-4b1e-b82e-3ada8210c358");
        private static readonly Uri s_uri = new Uri("http://www.microsoft.com", UriKind.Absolute);

        private static readonly string s_jsonObject = @"
        {
            ""name"": ""joe"",
            ""last"": null,
            ""middle"": """",
            // comment
            ""alive"": true,
            ""age"": 99,
            ""type1"": ""tINyINt"",
            ""type2"": """",
            ""type3"": null,
            ""guid"": """ + s_guid.ToString() + @""",
            ""uri"": """ + s_uri.ToString() + @""",
            ""object"": { ""foo"": 123 },
            ""array"": [ 123, ""abc"", null, { ""foo"": 123, ""bar"": [ false, ""a"", 123, null ] } ],
            ""byte"": " + byte.MaxValue.ToString() + @",
            ""sbyte"": " + sbyte.MinValue.ToString() + @",
            ""ushort"": " + ushort.MaxValue.ToString() + @",
            ""short"": " + short.MinValue.ToString() + @",
            ""uint"": " + uint.MaxValue.ToString() + @",
            ""int"": " + int.MinValue.ToString() + @",
            ""long"": " + long.MinValue.ToString() + @",
        }";

        private const string JsonArray = @"
        [
            ""joe"",
            null,
            """",
            // comment
            true,
            99
        ]";

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_read_simple_object()
        {
            // Read
            using (var tr = new StringReader(s_jsonObject))
            using (var jr = new JsonTextReader(tr))
            {
                string name = null;
                string last = "smith";
                string middle = "a";
                bool alive = false;
                int age = -1;
                var type = new System.Data.SqlDbType?[3];
                Guid? guid = null;
                Uri uri = null;
                byte u8 = 0;
                sbyte s8 = 0;
                ushort u16 = 0;
                short s16 = 0;
                uint u32 = 0;
                int s32 = 0;
                long s64 = 0;

                jr.ReadObject(n =>
                {
                    switch (n)
                    {
                        case "name": name = (string)jr.Value; return true;
                        case "last": last = (string)jr.Value; return true;
                        case "middle": middle = (string)jr.Value; return true;
                        case "alive": alive = jr.AsBool(); alive = jr.AsBoolNullable().Value; return true;
                        case "age": age = jr.AsInt32(); return true;
                        case "type1": type[0] = jr.AsEnum<System.Data.SqlDbType>(true); return true;
                        case "type2": type[1] = jr.AsEnumNullable<System.Data.SqlDbType>(true); return true;
                        case "type3": type[2] = jr.AsEnumNullable<System.Data.SqlDbType>(true); return true;
                        case "object": jr.SkipObject(); return true;
                        case "array": jr.SkipCountArray(); return true;

                        case "guid": guid = jr.AsGuid(); guid = jr.AsGuidNullable(); return true;
                        case "uri": uri = jr.AsUri(UriKind.Absolute); uri = jr.AsUriNullable(UriKind.Absolute); return true;

                        case "byte": u8 = jr.AsByte(); u8 = jr.AsByteNullable().Value; return true;
                        case "sbyte": s8 = jr.AsSByte(); s8 = jr.AsSByteNullable().Value; return true;

                        case "ushort": u16 = jr.AsUInt16(); u16 = jr.AsUInt16Nullable().Value; return true;
                        case "short": s16 = jr.AsInt16(); s16 = jr.AsInt16Nullable().Value; return true;

                        case "uint": u32 = jr.AsUInt32(); u32 = jr.AsUInt32Nullable().Value; return true;
                        case "int": s32 = jr.AsInt32(); s32 = jr.AsInt32Nullable().Value; return true;

                        case "long": s64 = jr.AsInt64(); s64 = jr.AsInt64Nullable().Value; return true;
                    }

                    return false;
                });

                string text = $"{name} {age}";

                Assert.Equal("joe 99", text);
                Assert.Equal("joe", name);
                Assert.Null(last);
                Assert.Equal(string.Empty, middle);
                Assert.True(alive);
                Assert.Equal(99, age);
                Assert.Equal(System.Data.SqlDbType.TinyInt, type[0]);
                Assert.Null(type[1]);
                Assert.Null(type[2]);

                Assert.Equal(s_guid, guid.Value);
                Assert.Equal(s_uri.ToString(), uri?.ToString(), StringComparer.Ordinal);

                Assert.Equal(byte.MaxValue, u8);
                Assert.Equal(sbyte.MinValue, s8);

                Assert.Equal(ushort.MaxValue, u16);
                Assert.Equal(short.MinValue, s16);

                Assert.Equal(uint.MaxValue, u32);
                Assert.Equal(int.MinValue, s32);

                Assert.Equal(long.MinValue, s64);
            }

            // Skip
            using (var tr = new StringReader(s_jsonObject))
            using (var jr = new JsonTextReader(tr))
            {
                int actualCount = jr.SkipObject();

                Assert.True(jr.TokenType == JsonToken.EndObject);
                Assert.Equal(19, actualCount);
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_read_simple_array()
        {
            // Read
            using (var tr = new StringReader(JsonArray))
            using (var jr = new JsonTextReader(tr))
            {
                IReadOnlyList<object> actual = jr.ReadArray(() => jr.Value);
                Assert.Collection(actual, n => Assert.Equal("joe", n), n => Assert.Null(n), n => Assert.Equal(string.Empty, n), n => Assert.True((bool)n), n => Assert.Equal(99L, n));
            }

            // Enumerate
            using (var tr = new StringReader(JsonArray))
            using (var jr = new JsonTextReader(tr))
            {
                IEnumerable<object> actual = jr.EnumerateArray(() => jr.Value);
                Assert.Collection(actual, n => Assert.Equal("joe", n), n => Assert.Null(n), n => Assert.Equal(string.Empty, n), n => Assert.True((bool)n), n => Assert.Equal(99L, n));
            }

            // Process
            using (var tr = new StringReader(JsonArray))
            using (var jr = new JsonTextReader(tr))
            {
                var actual = new List<object>();
                jr.ReadArray(() => actual.Add(jr.Value));
                Assert.Collection(actual, n => Assert.Equal("joe", n), n => Assert.Null(n), n => Assert.Equal(string.Empty, n), n => Assert.True((bool)n), n => Assert.Equal(99L, n));
            }

            // Skip
            using (var tr = new StringReader(JsonArray))
            using (var jr = new JsonTextReader(tr))
            {
                int actualCount = jr.SkipCountArray();

                Assert.True(jr.TokenType == JsonToken.EndArray);
                Assert.Equal(5, actualCount);
            }
        }

        [Trait("Type", "Unit")]
        [Fact]
        public static void When_read_simple_object_negative()
        {
            // Read
            using (var tr = new StringReader(s_jsonObject))
            using (var jr = new JsonTextReader(tr))
            {
                string name = null;
                string last = "smith";
                string middle = "a";
                bool alive = false;

                Assert.Throws<JsonReaderException>
                (
                    () => jr.ReadObject(n =>
                    {
                        switch (n)
                        {
                            case "name": name = (string)jr.Value; return true;
                            case "last": last = (string)jr.Value; return true;
                            case "middle": middle = (string)jr.Value; return true;
                            case "alive": alive = jr.AsBool(); return true;

                                // Neglect to process all other properties
                        }

                        return false;
                    })
                );
            }
        }
    }
}
