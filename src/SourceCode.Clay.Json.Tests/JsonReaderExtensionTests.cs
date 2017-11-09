#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace SourceCode.Clay.Json.Units
{
    public static class JsonReaderExtensionTests
    {
        #region Empty

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
                var actual = jr.ReadObject(n =>
                {
                    Assert.False(true);
                },
                () => 0);

                Assert.Equal(0, actual);
            }

            // Process
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actual = 0;
                jr.ProcessObject(n =>
                {
                    actual += 2;
                },
                () => actual++);

                Assert.Equal(1, actual);
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
                var actual = jr.ReadArray(() => (int)jr.Value);
                Assert.Empty(actual);
            }

            // Enumerate
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actual = jr.EnumerateArray(() => (int)jr.Value);
                Assert.Empty(actual);
            }

            // Process
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actualCount = 0;
                jr.ProcessArray(() => actualCount++);
                Assert.Equal(0, actualCount);
            }
        }

        #endregion

        #region Null

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
                        case "a": actual = (string)jr.Value; break;

                        default: Assert.True(false); break;
                    }
                },
                () => actual);

                Assert.Null(model);
            }

            // Process
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                string actual = null;
                jr.ProcessObject(n =>
                {
                    switch (n)
                    {
                        case "a": actual = (string)jr.Value; break;

                        default: Assert.True(false); break;
                    }
                },
                () => actual = actual == null ? null : "oops");

                Assert.Null(actual);
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
                var actual = jr.ReadArray(() => (string)jr.Value);
                Assert.Equal(new string[] { null }, actual);
            }

            // Enumerate
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actual = jr.EnumerateArray(() => (string)jr.Value);
                Assert.Equal(new string[] { null }, actual);
            }

            // Process
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actual = new List<string>();
                jr.ProcessArray(() => actual.Add((string)jr.Value));
                Assert.Equal(new string[] { null }, actual);
            }
        }

        #endregion

        #region Simple

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_read_simple_object))]
        public static void When_read_simple_object()
        {
            var json = @"
            {
                ""name"": ""joe"",
                ""last"": null,
                ""middle"": """",
                ""alive"": true,
                ""age"": 99
            }";

            // Read
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                string name = null;
                var last = "smith";
                var middle = "a";
                var alive = false;
                var age = -1L;

                var text = jr.ReadObject(n =>
                {
                    switch (n)
                    {
                        case "name": name = (string)jr.Value; break;
                        case "last": last = (string)jr.Value; break;
                        case "middle": middle = (string)jr.Value; break;
                        case "alive": alive = (bool)jr.Value; break;
                        case "age": age = (long)jr.Value; break;

                        default: Assert.True(false); break;
                    }
                },
                () => $"{name} {age}");

                Assert.Equal("joe 99", text);
                Assert.Equal("joe", name);
                Assert.Null(last);
                Assert.Equal(string.Empty, middle);
                Assert.True(alive);
                Assert.Equal(99, age);
            }

            // Process
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                string name = null;
                var last = "smith";
                var middle = "a";
                var alive = false;
                var age = -1L;
                string text = null;

                jr.ProcessObject(n =>
                {
                    switch (n)
                    {
                        case "name": name = (string)jr.Value; break;
                        case "last": last = (string)jr.Value; break;
                        case "middle": middle = (string)jr.Value; break;
                        case "alive": alive = (bool)jr.Value; break;
                        case "age": age = (long)jr.Value; break;

                        default: Assert.True(false); break;
                    }
                },
                () => text = $"{name} {age}");

                Assert.Equal("joe 99", text);
                Assert.Equal("joe", name);
                Assert.Null(last);
                Assert.Equal(string.Empty, middle);
                Assert.True(alive);
                Assert.Equal(99, age);
            }
        }

        [Trait("Type", "Unit")]
        [Fact(DisplayName = nameof(When_read_simple_array))]
        public static void When_read_simple_array()
        {
            var json = @"
            [
                ""joe"",
                null,
                """",
                true,
                99
            ]";

            // Read
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actual = jr.ReadArray(() => jr.Value);
                Assert.Collection(actual, n => Assert.Equal("joe", n), n => Assert.Null(n), n => Assert.Equal(string.Empty, n), n => Assert.True((bool)n), n => Assert.Equal(99L, n));
            }

            // Enumerate
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actual = jr.EnumerateArray(() => jr.Value);
                Assert.Collection(actual, n => Assert.Equal("joe", n), n => Assert.Null(n), n => Assert.Equal(string.Empty, n), n => Assert.True((bool)n), n => Assert.Equal(99L, n));
            }

            // Process
            using (var tr = new StringReader(json))
            using (var jr = new JsonTextReader(tr))
            {
                var actual = new List<object>();
                jr.ProcessArray(() => actual.Add(jr.Value));
                Assert.Collection(actual, n => Assert.Equal("joe", n), n => Assert.Null(n), n => Assert.Equal(string.Empty, n), n => Assert.True((bool)n), n => Assert.Equal(99L, n));
            }
        }

        #endregion
    }
}
