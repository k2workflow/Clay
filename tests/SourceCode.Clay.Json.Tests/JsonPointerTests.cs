#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using SourceCode.Clay.Json.Pointers;
using Xunit;

namespace SourceCode.Clay.Json.Tests
{
    public static class JsonPointerTests
    {
        [Fact]
        public static void JsonPointer_ToString()
        {
            var sut = new JsonPointer();
            var str = sut.ToString();
            Assert.Equal("", str);

            sut = new JsonPointer("");
            str = sut.ToString();
            Assert.Equal("/", str);

            sut = new JsonPointer("", "test");
            str = sut.ToString();
            Assert.Equal("//test", str);

            sut = new JsonPointer("test", "");
            str = sut.ToString();
            Assert.Equal("/test/", str);

            sut = new JsonPointer("test/", "");
            str = sut.ToString();
            Assert.Equal("/test~1/", str);

            sut = new JsonPointer("test~", "");
            str = sut.ToString();
            Assert.Equal("/test~0/", str);

            sut = new JsonPointer("test~/", "");
            str = sut.ToString();
            Assert.Equal("/test~0~1/", str);
        }

        [Fact]
        public static void JsonPointer_Parse()
        {
            var str = "";
            var sut = JsonPointer.Parse(str);
            Assert.Equal(new JsonPointer(), sut);

            str = "/";
            sut = JsonPointer.Parse(str);
            Assert.Equal(sut, new JsonPointer(""));

            str = "//test";
            sut = JsonPointer.Parse(str);
            Assert.Equal(new JsonPointer("", "test"), sut);

            str = "/test/";
            sut = JsonPointer.Parse(str);
            Assert.Equal(new JsonPointer("test", ""), sut);

            str = "/test~1/";
            sut = JsonPointer.Parse(str);
            Assert.Equal(new JsonPointer("test/", ""), sut);

            str = "/test~0/";
            sut = JsonPointer.Parse(str);
            Assert.Equal(new JsonPointer("test~", ""), sut);

            str = "/test~0~1/";
            sut = JsonPointer.Parse(str);
            Assert.Equal(new JsonPointer("test~/", ""), sut);
        }

        [Fact]
        public static void JsonPointer_Evaluate()
        {
            var json = JToken.Parse(@"
            {
                ""foo"": [""bar"", ""baz""],
                """": 0,
                ""a/b"": 1,
                ""c%d"": 2,
                ""e^f"": 3,
                ""g|h"": 4,
                ""i\\j"": 5,
                ""k\""l"": 6,
                "" "": 7,
                ""m~n"": 8
            }");

            JToken result = JsonPointer.Parse("").Evaluate(json);
            Assert.Equal(json, result);

            result = JsonPointer.Parse("/foo").Evaluate(json);
            Assert.Equal(new[] { "bar", "baz" }, ((JArray)result).Select(x => (string)x).ToArray());

            result = JsonPointer.Parse("/foo/0").Evaluate(json);
            Assert.Equal("bar", result);

            result = JsonPointer.Parse("/").Evaluate(json);
            Assert.Equal(0, (int)result);

            result = JsonPointer.Parse("/a~1b").Evaluate(json);
            Assert.Equal(1, (int)result);

            result = JsonPointer.Parse("/c%d").Evaluate(json);
            Assert.Equal(2, (int)result);

            result = JsonPointer.Parse("/e^f").Evaluate(json);
            Assert.Equal(3, (int)result);

            result = JsonPointer.Parse("/g|h").Evaluate(json);
            Assert.Equal(4, (int)result);

            result = JsonPointer.Parse("/i\\j").Evaluate(json);
            Assert.Equal(5, (int)result);

            result = JsonPointer.Parse("/k\"l").Evaluate(json);
            Assert.Equal(6, (int)result);

            result = JsonPointer.Parse("/ ").Evaluate(json);
            Assert.Equal(7, (int)result);

            result = JsonPointer.Parse("/m~0n").Evaluate(json);
            Assert.Equal(8, (int)result);
        }

        [Fact]
        public static void JsonPointer_Evaluate_Errors()
        {
            var json = JToken.Parse(@"
            {
                ""foo1"": 123,
                ""foo2"": true,
                ""foo3"": false,
                ""foo4"": ""value"",
                ""foo5"": null,
                ""foo6"": [""bar"", ""baz""],
                ""foo7"": {
                    ""foo1"": 123,
                    ""foo2"": true,
                    ""foo3"": false,
                    ""foo4"": ""value"",
                    ""foo5"": null,
                    ""foo6"": [""bar"", ""baz""],
                    ""foo7"": {
                        ""foo1"": 123,
                        ""foo2"": true,
                        ""foo3"": false,
                        ""foo4"": ""value"",
                        ""foo5"": null,
                        ""foo6"": [""bar"", ""baz""],
                    }
                }
            }");

            Assert.Throws<InvalidOperationException>(() => JsonPointer.Parse("/foo0").Evaluate(json));
            Assert.Throws<InvalidOperationException>(() => JsonPointer.Parse("/foo1/foo1").Evaluate(json));
            Assert.Throws<InvalidOperationException>(() => JsonPointer.Parse("/foo2/foo1").Evaluate(json));
            Assert.Throws<InvalidOperationException>(() => JsonPointer.Parse("/foo3/foo1").Evaluate(json));
            Assert.Throws<InvalidOperationException>(() => JsonPointer.Parse("/foo4/foo1").Evaluate(json));
            Assert.Throws<InvalidOperationException>(() => JsonPointer.Parse("/foo5/foo1").Evaluate(json));
            Assert.Throws<InvalidOperationException>(() => JsonPointer.Parse("/foo6/foo1").Evaluate(json));
            Assert.Throws<InvalidOperationException>(() => JsonPointer.Parse("/foo7/foo0").Evaluate(json));
            Assert.Throws<InvalidOperationException>(() => JsonPointer.Parse("/foo7/foo7/foo0").Evaluate(json));
            Assert.Throws<InvalidOperationException>(() => JsonPointer.Parse("/foo6/2").Evaluate(json));
            Assert.Throws<InvalidOperationException>(() => JsonPointer.Parse("/foo6/-").Evaluate(json));

            JsonPointer.Parse("/foo0").Evaluate(json, JsonPointerEvaluationOptions.MissingMembersAreNull);
            JsonPointer.Parse("/foo1/foo1").Evaluate(json, JsonPointerEvaluationOptions.PrimitiveMembersAndIndiciesAreNull);
            JsonPointer.Parse("/foo2/foo1").Evaluate(json, JsonPointerEvaluationOptions.PrimitiveMembersAndIndiciesAreNull);
            JsonPointer.Parse("/foo3/foo1").Evaluate(json, JsonPointerEvaluationOptions.PrimitiveMembersAndIndiciesAreNull);
            JsonPointer.Parse("/foo4/foo1").Evaluate(json, JsonPointerEvaluationOptions.PrimitiveMembersAndIndiciesAreNull);
            JsonPointer.Parse("/foo5/foo1").Evaluate(json, JsonPointerEvaluationOptions.NullCoalescing);
            JsonPointer.Parse("/foo6/foo1").Evaluate(json, JsonPointerEvaluationOptions.ArrayMembersAreNull);
            JsonPointer.Parse("/foo7/foo0").Evaluate(json, JsonPointerEvaluationOptions.MissingMembersAreNull);
            JsonPointer.Parse("/foo7/foo0/foo0").Evaluate(json, JsonPointerEvaluationOptions.MissingMembersAreNull | JsonPointerEvaluationOptions.NullCoalescing);
            JsonPointer.Parse("/foo6/2").Evaluate(json, JsonPointerEvaluationOptions.InvalidIndiciesAreNull);
            JsonPointer.Parse("/foo6/-").Evaluate(json, JsonPointerEvaluationOptions.InvalidIndiciesAreNull);
        }
    }
}
