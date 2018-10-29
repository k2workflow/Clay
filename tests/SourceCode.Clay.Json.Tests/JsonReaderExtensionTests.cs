#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SourceCode.Clay.Json.Units
{
    public static class JsonReaderExtensionTests
    {
        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(When_read_empty_object))]
        [InlineData("{}")]
        [InlineData(" {\t\n   \t} \r\n")]
        public static void When_read_empty_object(string json)
        {
            // Read
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actual = jr.ReadObject(n => false, () => 0);

                Assert.Equal(0, actual);
            }

            // Process
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actual = 0;
                jr.ReadObject(n =>
                {
                    return false;
                },
                () => actual++);

                Assert.Equal(1, actual);
            }

            // Skip
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actualCount = jr.SkipCountObject();

                Assert.True(jr.TokenType == JsonToken.EndObject);
                Assert.Equal(0, actualCount);
            }
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(When_read_empty_array))]
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
                var actualCount = 0;
                jr.ReadArray(() => actualCount++);
                Assert.Equal(0, actualCount);
            }

            // Skip
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actualCount = jr.SkipCountArray();

                Assert.True(jr.TokenType == JsonToken.EndArray);
                Assert.Equal(0, actualCount);
            }
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(When_read_null_object))]
        [InlineData("{ \"a\": null }")]
        [InlineData("{ \t\n \"a\": \t\n null \t\n\t } \r\n")]
        public static void When_read_null_object(string json)
        {
            // Read
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                string actual = null;
                var model = jr.ReadObject(n =>
                {
                    switch (n)
                    {
                        case "a": actual = (string)jr.Value; return true;
                    }

                    return false;
                },
                () => actual);

                Assert.Null(model);
            }

            // Process
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
                },
                () => actual = actual is null ? null : "oops");

                Assert.Null(actual);
            }

            // Skip
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actualCount = jr.SkipCountObject();

                Assert.True(jr.TokenType == JsonToken.EndObject);
                Assert.Equal(1, actualCount);
            }
        }

        [Trait("Type", "Unit")]
        [Theory(DisplayName = nameof(When_read_null_array))]
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
                var actualCount = jr.SkipCountArray();

                Assert.True(jr.TokenType == JsonToken.EndArray);
                Assert.Equal(1, actualCount);
            }
        }

        private static readonly Guid s_guid1 = new Guid("82a7f48d-3b50-4b1e-b82e-3ada8210c358");

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
            ""guid1"": """ + s_guid1.ToString() + @""",
            ""object"": { ""foo"": 123 },
            ""array"": [ 123, ""abc"", null, { ""foo"": 123, ""bar"": [ false, ""a"", 123, null ] } ]
        }";

        private const string s_jsonArray = @"
        [
            ""joe"",
            null,
            """",
            // comment
            true,
            99
        ]";

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_read_simple_object))]
        public static void When_read_simple_object()
        {
            // Read
            using (var tr = new StringReader(s_jsonObject))
            using (var jr = new JsonTextReader(tr))
            {
                string name = null;
                var last = "smith";
                var middle = "a";
                var alive = false;
                var age = -1L;
                var type = new System.Data.SqlDbType?[3];
                Guid? guid = null;

                var text = jr.ReadObject(n =>
                {
                    switch (n)
                    {
                        case "name": name = (string)jr.Value; return true;
                        case "last": last = (string)jr.Value; return true;
                        case "middle": middle = (string)jr.Value; return true;
                        case "alive": alive = (bool)jr.Value; return true;
                        case "age": age = (long)jr.Value; return true;
                        case "type1": type[0] = jr.ParseEnum<System.Data.SqlDbType>(true); return true;
                        case "type2": type[1] = jr.ParseEnum<System.Data.SqlDbType>(true); return true;
                        case "type3": type[2] = jr.ParseEnum<System.Data.SqlDbType>(true); return true;
                        case "guid1": guid = jr.ParseGuid(); return true;
                        case "object": jr.SkipCountObject(); return true;
                        case "array": jr.SkipCountArray(); return true;
                    }

                    return false;
                },
                () => $"{name} {age}");

                Assert.Equal("joe 99", text);
                Assert.Equal("joe", name);
                Assert.Null(last);
                Assert.Equal(string.Empty, middle);
                Assert.True(alive);
                Assert.Equal(99, age);
                Assert.Equal(System.Data.SqlDbType.TinyInt, type[0]);
                Assert.Null(type[1]);
                Assert.Null(type[2]);
                Assert.Equal(s_guid1, guid.Value);
            }

            // Process
            using (var tr = new StringReader(s_jsonObject))
            using (var jr = new JsonTextReader(tr))
            {
                string name = null;
                var last = "smith";
                var middle = "a";
                var alive = false;
                var age = -1L;
                string text = null;
                var type = new System.Data.SqlDbType?[3];
                Guid? guid = null;

                jr.ReadObject(n =>
                {
                    switch (n)
                    {
                        case "name": name = (string)jr.Value; return true;
                        case "last": last = (string)jr.Value; return true;
                        case "middle": middle = (string)jr.Value; return true;
                        case "alive": alive = (bool)jr.Value; return true;
                        case "age": age = (long)jr.Value; return true;
                        case "type1": type[0] = jr.ParseEnum<System.Data.SqlDbType>(true); return true;
                        case "type2": type[1] = jr.ParseEnum<System.Data.SqlDbType>(true); return true;
                        case "type3": type[2] = jr.ParseEnum<System.Data.SqlDbType>(true); return true;
                        case "guid1": guid = jr.ParseGuidExact("D"); return true;
                        case "object": jr.SkipCountObject(); return true;
                        case "array": jr.SkipCountArray(); return true;
                    }

                    return false;
                },
                () => text = $"{name} {age}");

                Assert.Equal("joe 99", text);
                Assert.Equal("joe", name);
                Assert.Null(last);
                Assert.Equal(string.Empty, middle);
                Assert.True(alive);
                Assert.Equal(99, age);
                Assert.Equal(System.Data.SqlDbType.TinyInt, type[0]);
                Assert.Null(type[1]);
                Assert.Null(type[2]);
                Assert.Equal(s_guid1, guid.Value);
            }

            // Skip
            using (var tr = new StringReader(s_jsonObject))
            using (var jr = new JsonTextReader(tr))
            {
                var actualCount = jr.SkipCountObject();

                Assert.True(jr.TokenType == JsonToken.EndObject);
                Assert.Equal(11, actualCount);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_read_simple_array))]
        public static void When_read_simple_array()
        {
            // Read
            using (var tr = new StringReader(s_jsonArray))
            using (var jr = new JsonTextReader(tr))
            {
                IReadOnlyList<object> actual = jr.ReadArray(() => jr.Value);
                Assert.Collection(actual, n => Assert.Equal("joe", n), n => Assert.Null(n), n => Assert.Equal(string.Empty, n), n => Assert.True((bool)n), n => Assert.Equal(99L, n));
            }

            // Enumerate
            using (var tr = new StringReader(s_jsonArray))
            using (var jr = new JsonTextReader(tr))
            {
                IEnumerable<object> actual = jr.EnumerateArray(() => jr.Value);
                Assert.Collection(actual, n => Assert.Equal("joe", n), n => Assert.Null(n), n => Assert.Equal(string.Empty, n), n => Assert.True((bool)n), n => Assert.Equal(99L, n));
            }

            // Process
            using (var tr = new StringReader(s_jsonArray))
            using (var jr = new JsonTextReader(tr))
            {
                var actual = new List<object>();
                jr.ReadArray(() => actual.Add(jr.Value));
                Assert.Collection(actual, n => Assert.Equal("joe", n), n => Assert.Null(n), n => Assert.Equal(string.Empty, n), n => Assert.True((bool)n), n => Assert.Equal(99L, n));
            }

            // Skip
            using (var tr = new StringReader(s_jsonArray))
            using (var jr = new JsonTextReader(tr))
            {
                var actualCount = jr.SkipCountArray();

                Assert.True(jr.TokenType == JsonToken.EndArray);
                Assert.Equal(5, actualCount);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_read_simple_object_negative))]
        public static void When_read_simple_object_negative()
        {
            // Read
            using (var tr = new StringReader(s_jsonObject))
            using (var jr = new JsonTextReader(tr))
            {
                string name = null;
                var last = "smith";
                var middle = "a";
                var alive = false;

                Assert.Throws<JsonReaderException>
                (
                    () => jr.ReadObject(n =>
                    {
                        switch (n)
                        {
                            case "name": name = (string)jr.Value; return true;
                            case "last": last = (string)jr.Value; return true;
                            case "middle": middle = (string)jr.Value; return true;
                            case "alive": alive = (bool)jr.Value; return true;

                                // Neglect to process 'age'
                        }

                        return false;
                    },
                    () => name)
                );
            }

            // Process
            using (var tr = new StringReader(s_jsonObject))
            using (var jr = new JsonTextReader(tr))
            {
                string name = null;
                var last = "smith";
                var middle = "a";
                var alive = false;

                Assert.Throws<JsonReaderException>
                (
                    () => jr.ReadObject(n =>
                    {
                        switch (n)
                        {
                            case "name": name = (string)jr.Value; return true;
                            case "last": last = (string)jr.Value; return true;
                            case "middle": middle = (string)jr.Value; return true;
                            case "alive": alive = (bool)jr.Value; return true;

                                // Neglect to process 'age'
                        }

                        return false;
                    },
                    () => { })
                );
            }
        }
    }
}
