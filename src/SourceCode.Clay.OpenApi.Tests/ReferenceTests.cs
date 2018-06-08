#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.Json.Pointers;
using System;
using Xunit;

namespace SourceCode.Clay.OpenApi.Tests
{
    public static class ReferenceTests
    {
        [Fact(DisplayName = nameof(Reference_ParseUrl_Absolute))]
        public static void Reference_ParseUrl_Absolute()
        {
            var sut = OasReference.ParseUrl(new Uri("http://example.com/openapi.json#/test/path"));
            Assert.Equal("http://example.com/openapi.json", sut.Url.ToString());
            Assert.Equal(JsonPointer.Parse("/test/path"), sut.Pointer);
            Assert.Equal("http://example.com/openapi.json#/test/path", sut.ToString());
        }

        [Fact(DisplayName = nameof(Reference_ParseUrl_AbsoluteDocument))]
        public static void Reference_ParseUrl_AbsoluteDocument()
        {
            var sut = OasReference.ParseUrl(new Uri("http://example.com/openapi.json"));
            Assert.Equal("http://example.com/openapi.json", sut.Url.ToString());
            Assert.Equal(default, sut.Pointer);
            Assert.Equal("http://example.com/openapi.json", sut.ToString());
        }

        [Fact(DisplayName = nameof(Reference_ParseUrl_Internal))]
        public static void Reference_ParseUrl_Internal()
        {
            var sut = OasReference.ParseUrl(new Uri("#/test/path", UriKind.Relative));
            Assert.Null(sut.Url);
            Assert.Equal(JsonPointer.Parse("/test/path"), sut.Pointer);
            Assert.Equal("#/test/path", sut.ToString());
        }

        [Fact(DisplayName = nameof(Reference_ParseUrl_Relative))]
        public static void Reference_ParseUrl_Relative()
        {
            var sut = OasReference.ParseUrl(new Uri("/openapi.json#/test/path", UriKind.Relative));
            Assert.Equal("/openapi.json", sut.Url.ToString());
            Assert.Equal(JsonPointer.Parse("/test/path"), sut.Pointer);
            Assert.Equal("/openapi.json#/test/path", sut.ToString());
        }
    }
}
