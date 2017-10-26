#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.OpenApi.Expressions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Xunit;

namespace SourceCode.Clay.OpenApi.Tests
{
    public static class DtoTests
    {
        #region Methods

        private static void TestDtoEquality<T>(Expression<Func<T>> constructor)
            where T : IEquatable<T>
        {
            Assert.Equal(ExpressionType.New, constructor.Body.NodeType);
            var newExpression = (NewExpression)constructor.Body;

            var p1 = Expression.Parameter(typeof(T), "p1");
            var p2 = Expression.Parameter(typeof(T), "p2");
            var eq = Expression.Lambda<Func<T, T, bool>>(Expression.Equal(p1, p2), p1, p2).Compile();
            var neq = Expression.Lambda<Func<T, T, bool>>(Expression.NotEqual(p1, p2), p1, p2).Compile();

            var ctor = newExpression.Constructor;
            var constructorArguments = newExpression.Arguments;
            var ctorParams = ctor.GetParameters();

            var param = new Expression[ctorParams.Length];
            var allValues = constructor.Compile()();
            var hs = new HashSet<T>();

            for (var i = -1; i < constructorArguments.Count; i++)
            {
                for (var j = 0; j < constructorArguments.Count; j++)
                {
                    param[j] = i != j
                        ? ExtractActualArg(constructorArguments[j])
                        : Expression.Default(ctorParams[j].ParameterType);
                }

                var nw = Expression.New(ctor, param);
                var lambda = Expression.Lambda<Func<T>>(nw).Compile();

                var current = lambda();
                hs.Add(current);
                if (i == -1)
                {
                    Assert.Equal(allValues, current);
                    Assert.NotEqual(default, current);

                    Assert.True(eq(allValues, current));
                    Assert.True(neq(default, current));

                    Assert.False(neq(allValues, current));
                    Assert.False(eq(default, current));
                }
                else
                {
                    Assert.Equal(current, current);
                    Assert.NotEqual(default, current);
                    Assert.NotEqual(allValues, current);

                    Assert.True(eq(current, current));
                    Assert.True(neq(default, current));
                    Assert.True(neq(allValues, current));

                    Assert.False(neq(current, current));
                    Assert.False(eq(default, current));
                    Assert.False(eq(allValues, current));
                }
            }

            Assert.Equal(constructorArguments.Count + 1, hs.Count);
        }

        private static Expression ExtractActualArg(Expression arg)
        {
            while (arg.CanReduce) arg = arg.Reduce();

            if (arg is LambdaExpression l) arg = l.Body;
            if (arg is UnaryExpression u && u.NodeType == ExpressionType.Convert && u.Type == typeof(object)) arg = u.Operand;

            return arg;
        }

        #endregion

        #region Equals

        [Fact(DisplayName = nameof(ApiSecurityScheme_Equals))]
        public static void ApiSecurityScheme_Equals()
        {
            TestDtoEquality(() => new OasApiKeySecurityScheme(
                "Description",
                "Name",
                OasParameterLocation.Header
            ));
        }

        [Fact(DisplayName = nameof(Callback_Equals))]
        public static void Callback_Equals()
        {
            var cb1 = new OasCallback();
            var cb2 = new OasCallback(new Dictionary<OasExpression, OasReferable<OasPath>>()
            {
                [OasExpression.Parse("http://test/{$statusCode}")] = new OasReferable<OasPath>("#/test"),
                [OasExpression.Parse("http://test/1/{$statusCode}")] = new OasReferable<OasPath>("#/test/1"),
            });
            var cb3 = new OasCallback(new Dictionary<OasExpression, OasReferable<OasPath>>()
            {
                [OasExpression.Parse("http://test/{$statusCode}")] = new OasReferable<OasPath>("#/test"),
                [OasExpression.Parse("http://test/1/{$statusCode}")] = new OasReferable<OasPath>("#/test/1"),
            });
            var cb4 = new OasCallback(new Dictionary<OasExpression, OasReferable<OasPath>>()
            {
                [OasExpression.Parse("http://test/1/{$statusCode}")] = new OasReferable<OasPath>("#/test/1"),
            });

            Assert.False(cb1 == null);
            Assert.True(cb2 == cb3);
            Assert.False(cb1 == cb2);
            Assert.False(cb3 == cb4);
        }

        [Fact(DisplayName = nameof(Contact_Equals))]
        public static void Contact_Equals()
        {
            TestDtoEquality(() => new OasContact(
                "Jonathan",
                new Uri("http://example.org/jonathan"),
                new MailAddress("jonathan@example.org")
            ));
        }

        [Fact(DisplayName = nameof(Document_Equals))]
        public static void Document_Equals()
        {
            var paths = new Dictionary<string, OasReferable<OasPath>>()
            {
                ["/test"] = "#/components/path0"
            };

            var schemas = new Dictionary<string, OasReferable<OasSchema>>()
            {
                ["schema1"] = "http://example.org/schema1"
            };

            TestDtoEquality(() => new OasDocument(
                new SemanticVersion(1, 0, 0),
                new OasInformation("Example API", default, default, default, default, default),
                new[] { new OasServer(default, "Server 1", default) },
                paths,
                new OasComponents(schemas, default, default, default, default, default, default, default, default),
                new[] { new OasReferable<OasSecurityScheme>(new OasHttpSecurityScheme("Http", default, default)) },
                new[] { new OasTag("Tag1", default, default) },
                new OasExternalDocumentation("External docs", default)
            ));
        }

        [Fact(DisplayName = nameof(Example_Equals))]
        public static void Example_Equals()
        {
            TestDtoEquality(() => new OasExample(
                "Summary",
                "Description",
                new Uri("http://example.org/docs")
            ));
        }

        [Fact(DisplayName = nameof(ExternalDocumentation_Equals))]
        public static void ExternalDocumentation_Equals()
        {
            TestDtoEquality(() => new OasExternalDocumentation(
                "Description",
                new Uri("http://example.org/docs")
            ));
        }

        [Fact(DisplayName = nameof(HttpSecurityScheme_Equals))]
        public static void HttpSecurityScheme_Equals()
        {
            TestDtoEquality(() => new OasHttpSecurityScheme(
                "Description",
                "Scheme",
                "Bearer Format"
            ));
        }

        [Fact(DisplayName = nameof(Information_Equals))]
        public static void Information_Equals()
        {
            TestDtoEquality(() => new OasInformation(
                "Title",
                "Description",
                new Uri("http://example.org/tos"),
                new OasContact("Name", default, default),
                new OasLicense("MIT", default),
                new SemanticVersion(1, 0, 0)
            ));
        }

        [Fact(DisplayName = nameof(License_Equals))]
        public static void License_Equals()
        {
            TestDtoEquality(() => new OasLicense(
                "MIT",
                new Uri("https://opensource.org/licenses/MIT")
            ));
        }

        [Fact(DisplayName = nameof(Link_Equals))]
        public static void Link_Equals()
        {
            TestDtoEquality(() => new OasLink(
                "http://example.org/linkOp",
                "Operation ID",
                "Description",
                new OasServer(default, "Description", default)
            ));
        }

        [Fact(DisplayName = nameof(MediaType_Equals))]
        public static void MediaType_Equals()
        {
            var examples = new Dictionary<string, OasReferable<OasExample>>()
            {
                ["example1"] = "#/components/examples/example1"
            };

            var encoding = new Dictionary<string, OasPropertyEncoding>()
            {
                ["application/json"] = new OasPropertyEncoding(
                    new System.Net.Mime.ContentType("application/json"),
                    default,
                    default,
                    default
                )
            };

            TestDtoEquality(() => new OasMediaType(
                "#/components/schemas/schema1",
                examples,
                encoding
            ));
        }

        [Fact(DisplayName = nameof(OAuth2SecurityScheme_Equals))]
        public static void OAuth2SecurityScheme_Equals()
        {
            TestDtoEquality(() => new OasOAuth2SecurityScheme(
                "Description",
                new OasOAuthFlow(new Uri("http://example.org/implicit"), default, default, default),
                new OasOAuthFlow(new Uri("http://example.org/password"), default, default, default),
                new OasOAuthFlow(new Uri("http://example.org/credential"), default, default, default),
                new OasOAuthFlow(new Uri("http://example.org/authorization"), default, default, default)
            ));
        }

        [Fact(DisplayName = nameof(OAuth2Flow_Equals))]
        public static void OAuth2Flow_Equals()
        {
            var scopes = new Dictionary<string, string>()
            {
                ["user:name"] = "Get the user name"
            };

            TestDtoEquality(() => new OasOAuthFlow(
                new Uri("http://example.org/authorization"),
                new Uri("http://example.org/token"),
                new Uri("http://example.org/refresh"),
                scopes
            ));
        }

        [Fact(DisplayName = nameof(OpenIdConnectSecurityScheme_Equals))]
        public static void OpenIdConnectSecurityScheme_Equals()
        {
            TestDtoEquality(() => new OasOidcSecurityScheme(
                "Description",
                new Uri("http://example.org/openid")
            ));
        }

        [Fact(DisplayName = nameof(Operation_Equals))]
        public static void Operation_Equals()
        {
            var param = new Dictionary<OasParameterKey, OasReferable<OasParameterBody>>()
            {
                [new OasParameterKey("param1", OasParameterLocation.Header)] = "#/components/parameters/param1"
            };

            var responses = new Dictionary<OasResponseKey, OasReferable<OasResponse>>()
            {
                [HttpStatusCode.InternalServerError] = "#/components/responses/error"
            };

            var callbacks = new Dictionary<string, OasReferable<OasCallback>>()
            {
                ["result"] = "#/components/callbacks/cb1"
            };

            TestDtoEquality(() => new OasOperation(
                new[] { "Tag1" },
                "Summary",
                "Description",
                new OasExternalDocumentation("Description", default),
                "Operation 1",
                param,
                "#/components/requestBodies/rb1",
                responses,
                callbacks,
                OasOperationOptions.Deprecated,
                new[] { new OasHttpSecurityScheme("Description", default, default) },
                new[] { new OasServer(new Uri("http://example.org"), default, default) }
            ));
        }

        [Fact(DisplayName = nameof(Parameter_Equals))]
        public static void Parameter_Equals()
        {
            var examples = new Dictionary<ContentType, OasReferable<OasExample>>()
            {
                [new ContentType("application/json")] = "#/components/examples/example1"
            };

            var content = new Dictionary<ContentType, OasMediaType>()
            {
                [new ContentType("application/json")] = new OasMediaType(
                    "#/components/schemas/schema1",
                    default,
                    default
                )
            };

            TestDtoEquality(() => new OasParameter(
                "Name",
                OasParameterLocation.Header,
                "Description",
                OasParameterOptions.AllowReserved,
                OasParameterStyle.PipeDelimited,
                "#/components/schemas/schema1",
                examples,
                content
            ));
        }

        [Fact(DisplayName = nameof(ParameterBody_Equals))]
        public static void ParameterBody_Equals()
        {
            var examples = new Dictionary<ContentType, OasReferable<OasExample>>()
            {
                [new ContentType("application/json")] = "#/components/examples/example1"
            };

            var content = new Dictionary<ContentType, OasMediaType>()
            {
                [new ContentType("application/json")] = new OasMediaType(
                    "#/components/schemas/schema1",
                    default,
                    default
                )
            };

            TestDtoEquality(() => new OasParameterBody(
                "Description",
                OasParameterOptions.AllowReserved,
                OasParameterStyle.PipeDelimited,
                "#/components/schemas/schema1",
                examples,
                content
            ));
        }

        [Fact(DisplayName = nameof(Path_Equals))]
        public static void Path_Equals()
        {
            var param = new Dictionary<OasParameterKey, OasReferable<OasParameterBody>>()
            {
                [new OasParameterKey("Param1", default)] = "#/components/parameters/param1"
            };

            TestDtoEquality(() => new OasPath(
                "Summary",
                "Description",
                new OasOperation(default, "Get", default, default, default, default, default, default, default, default, default, default),
                new OasOperation(default, "Put", default, default, default, default, default, default, default, default, default, default),
                new OasOperation(default, "Post", default, default, default, default, default, default, default, default, default, default),
                new OasOperation(default, "Delete", default, default, default, default, default, default, default, default, default, default),
                new OasOperation(default, "Options", default, default, default, default, default, default, default, default, default, default),
                new OasOperation(default, "Head", default, default, default, default, default, default, default, default, default, default),
                new OasOperation(default, "Patch", default, default, default, default, default, default, default, default, default, default),
                new OasOperation(default, "Trace", default, default, default, default, default, default, default, default, default, default),
                new[] { new OasServer(new Uri("http://example.org"), default, default) },
                param
            ));
        }

        [Fact(DisplayName = nameof(PropertyEncoding_Equals))]
        public static void PropertyEncoding_Equals()
        {
            var headers = new Dictionary<string, OasReferable<OasParameterBody>>()
            {
                ["header1"] = "#/components/parameters/param1"
            };

            TestDtoEquality(() => new OasPropertyEncoding(
                new ContentType("application/json"),
                headers,
                OasParameterStyle.PipeDelimited,
                OasPropertyEncodingOptions.Explode
            ));
        }

        [Fact(DisplayName = nameof(RequestBody_Equals))]
        public static void RequestBody_Equals()
        {
            var content = new Dictionary<ContentType, OasMediaType>()
            {
                [new ContentType("application/json")] = new OasMediaType("#/components/schemas/schema1")
            };

            TestDtoEquality(() => new OasRequestBody(
                "Description",
                content,
                OasRequestBodyOptions.Required
            ));
        }

        [Fact(DisplayName = nameof(Response_Equals))]
        public static void Response_Equals()
        {
            var headers = new Dictionary<string, OasReferable<OasParameterBody>>()
            {
                ["header1"] = "#/components/parameters/param1"
            };

            var content = new Dictionary<ContentType, OasMediaType>()
            {
                [new ContentType("application/json")] = new OasMediaType("#/components/schemas/schema1")
            };

            var links = new Dictionary<string, OasReferable<OasLink>>()
            {
                ["link"] = "#/components/links/link1"
            };

            TestDtoEquality(() => new OasResponse(
                "Description",
                headers,
                content,
                links
            ));
        }

        [Fact(DisplayName = nameof(Schema_Equals))]
        public static void Schema_Equals()
        {
            var properties = new Dictionary<string, OasReferable<OasSchema>>()
            {
                ["prop1"] = "#/components/schemas/prop1"
            };

            var additionalProperties = new Dictionary<string, OasReferable<OasSchema>>()
            {
                ["prop1"] = "#/components/schemas/adprop1"
            };

            TestDtoEquality(() => new OasSchema(
                OasSchemaType.String,
                "uuid",
                "Unique Identifier",
                "Description",
                new OasNumberRange(1, 100),
                new OasCountRange(2, 200),
                new OasCountRange(3, 300),
                new OasCountRange(4, 400),
                OasSchemaOptions.Required,
                "[a-z]",
                new[] { new OasScalarValue(true) },
                new OasReferable<OasSchema>[] { "#/components/schemas/allOf" },
                new OasReferable<OasSchema>[] { "#/components/schemas/oneOf" },
                new OasReferable<OasSchema>[] { "#/components/schemas/anyOf" },
                new OasReferable<OasSchema>[] { "#/components/schemas/not" },
                "#/components/schemas/items",
                properties,
                additionalProperties,
                new OasExternalDocumentation("Description", default)
            ));
        }

        [Fact(DisplayName = nameof(Server_Equals))]
        public static void Server_Equals()
        {
            var variables = new Dictionary<string, OasServerVariable>()
            {
                ["var1"] = new OasServerVariable(description: "var1")
            };

            TestDtoEquality(() => new OasServer(
                new Uri("http://example.org"),
                "Description",
                variables
            ));
        }

        [Fact(DisplayName = nameof(ServerVariable_Equals))]
        public static void ServerVariable_Equals()
        {
            TestDtoEquality(() => new OasServerVariable(
                new[] { "Val1" },
                "Val1",
                "Variable1"
            ));
        }

        [Fact(DisplayName = nameof(Tag_Equals))]
        public static void Tag_Equals()
        {
            TestDtoEquality(() => new OasTag(
                "Name",
                "Description",
                new OasExternalDocumentation("Description", default)
            ));
        }

        #endregion
    }
}
