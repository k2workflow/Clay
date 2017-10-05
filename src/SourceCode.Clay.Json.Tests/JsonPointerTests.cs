using SourceCode.Clay.Json.Pointers;
using System;
using System.Json;
using System.Linq;
using Xunit;

namespace SourceCode.Clay.Json.Tests
{
    public static class JsonPointerTests
    {
        #region Methods

        [Fact(DisplayName = nameof(JsonPointer_Evaluate))]
        public static void JsonPointer_Evaluate()
        {
            var json = JsonValue.Parse(@"
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

            var result = JsonPointer.Parse("").Evaluate(json);
            Assert.Equal(json, result);

            result = JsonPointer.Parse("/foo").Evaluate(json);
            Assert.Equal(new[] { "bar", "baz" }, ((JsonArray)result).Select(x => (string)x).ToArray());

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

        [Fact(DisplayName = nameof(JsonPointer_Evaluate_Errors))]
        public static void JsonPointer_Evaluate_Errors()
        {
            var json = JsonValue.Parse(@"
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

        [Fact(DisplayName = nameof(JsonPointer_Parse))]
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

        [Fact(DisplayName = nameof(JsonPointer_ToString))]
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

        #endregion Methods
    }
}
