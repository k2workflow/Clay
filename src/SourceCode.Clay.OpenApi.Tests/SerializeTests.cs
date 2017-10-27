#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using SourceCode.Clay.OpenApi.Expressions;
using SourceCode.Clay.OpenApi.Serialization;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Json;
using System.Net.Mail;
using System.Net.Mime;
using Xunit;

namespace SourceCode.Clay.OpenApi.Tests
{
    public static class SerializeTests
    {
        #region Helpers

        // Actually check that the values are correct when using this.

#if DEBUG

        private static string GenerateExpectedCode(JsonValue value)
        {
            using (var sw = new StringWriter())
            using (var iw = new IndentedTextWriter(sw))
            {
                GenerateExpectedCode(value, iw);
                return sw.ToString();
            }
        }

        private static void GenerateExpectedCode(JsonValue value, IndentedTextWriter writer)
        {
            if (value == null)
            {
                writer.Write("null");
            }
            else if (value is JsonPrimitive)
            {
                writer.Write(value.ToString());
            }
            else if (value is JsonArray a)
            {
                if (a.Count == 0)
                    writer.Write("new JsonArray()");
                else
                {
                    writer.WriteLine("new JsonArray() {");
                    writer.Indent++;

                    for (var i = 0; i < a.Count; i++)
                    {
                        GenerateExpectedCode(a[i], writer);
                        if (i == a.Count - 1) writer.WriteLine();
                        else writer.WriteLine(",");
                    }

                    writer.Indent--;
                    writer.Write("}");
                }
            }
            else if (value is JsonObject o)
            {
                if (o.Count == 0)
                    writer.Write("new JsonObject()");
                else
                {
                    writer.WriteLine("new JsonObject() {");
                    writer.Indent++;

                    var first = true;
                    foreach (var item in o)
                    {
                        if (!first) writer.WriteLine(",");
                        first = false;

                        writer.Write("[ ");
                        GenerateExpectedCode(item.Key, writer);
                        writer.Write(" ] = ");

                        GenerateExpectedCode(item.Value, writer);
                    }

                    writer.WriteLine();
                    writer.Indent--;
                    writer.Write("}");
                }
            }
        }

#endif

        #endregion

        #region Methods

        [Fact(DisplayName = nameof(OpenApiSerializer_Serialize))]
        public static void OpenApiSerializer_Serialize()
        {
            var sut = new OasSerializer();

            #region Graph

            var actualGraphBuilder = new OasDocumentBuilder()
            {
                Components = new OasComponentsBuilder()
                {
                    Callbacks =
                    {
                        [ "callback1" ] = new OasCallbackBuilder()
                        {
                            [ OasExpression.Parse("http://foo{$statusCode}") ] = new OasPathBuilder()
                            {
                                Description = "path1",
                                Delete = new OasOperationBuilder()
                                {
                                    Callbacks =
                                    {
                                        [ "callback1" ] = "#/components/callbacks/callback1"
                                    },
                                    Description = "Delete operation",
                                    ExternalDocumentation = new OasExternalDocumentationBuilder()
                                    {
                                        Description = "External docs 1",
                                        Url = new Uri("http://example.org")
                                    },
                                    OperationIdentifier = "Operation 1",
                                    Options = OasOperationOptions.Deprecated,
                                    Parameters =
                                    {
                                        [ new OasParameterKey("p1") ] = "#/components/parameters/parameter1"
                                    },
                                    RequestBody = "#/components/requestBodies/requestBody1",
                                    Responses =
                                    {
                                        [OasResponseKey.Default] = "#/components/responses/response1"
                                    },
                                    Summary = "Delete operation summary"
                                },
                                Get = new OasOperationBuilder()
                                {
                                    Description = "Get"
                                },
                                Head = new OasOperationBuilder()
                                {
                                    Description = "Head"
                                },
                                Options = new OasOperationBuilder()
                                {
                                    Description = "Options"
                                },
                                Parameters =
                                {
                                    [ new OasParameterKey("p1") ] = "#/components/parameters/parameter1"
                                },
                                Patch = new OasOperationBuilder()
                                {
                                    Description = "Patch"
                                },
                                Post = new OasOperationBuilder()
                                {
                                    Description = "Post"
                                },
                                Put = new OasOperationBuilder()
                                {
                                    Description = "Put"
                                },
                                Summary = "Summary",
                                Trace = new OasOperationBuilder()
                                {
                                    Description = "Trace"
                                }
                            }
                        }
                    },
                    Examples =
                    {
                        [ "example1" ] = new OasExampleBuilder()
                        {
                            Description = "Description",
                            ExternalValue = new Uri("http://example.org/example"),
                            Summary = "Summary"
                        }
                    },
                    Headers =
                    {
                        [ "header1" ] = new OasParameterBodyBuilder()
                        {
                            Content =
                            {
                                [ new ContentType("application/json") ] = new OasMediaTypeBuilder()
                                {
                                    Encoding =
                                    {
                                        [ "header1" ] = new OasPropertyEncodingBuilder()
                                        {
                                            ContentType = new ContentType("application/json"),
                                            Headers =
                                            {
                                                [ "header1" ] = "#/components/headers/header1"
                                            },
                                            Options = OasPropertyEncodingOptions.AllowReserved | OasPropertyEncodingOptions.Explode,
                                            Style = OasParameterStyle.DeepObject
                                        }
                                    },
                                    Examples =
                                    {
                                        [ "Main Example" ] = "#/components/examples/example1"
                                    },
                                    Schema = "#/components/schemas/schema1"
                                }
                            }
                        }
                    },
                    Links =
                    {
                        [ "link1" ] = new OasLinkBuilder()
                        {
                            Description = "Description",
                            OperationIdentifier = "operation1",
                            OperationReference = "http://example.org/#/operation1",
                            Server = new OasServerBuilder()
                            {
                                Description = "Description",
                                Url = new Uri("http://example.org"),
                                Variables =
                                {
                                    [ "variable1" ] = new OasServerVariableBuilder()
                                    {
                                        Default = "Value",
                                        Description = "Description",
                                        Enum =
                                        {
                                            "Value", "Value1"
                                        }
                                    }
                                }
                            }
                        }
                    },
                    Parameters =
                    {
                        [ "parameter1" ] = new OasParameterBuilder()
                        {
                            Content =
                            {
                                [ new ContentType("application/json")] = new OasMediaTypeBuilder()
                                {
                                    Schema = "#/components/schemas/schema1"
                                }
                            },
                            Description = "Description",
                            Examples =
                            {
                                [ new ContentType("application/json") ] = "#/components/examples/example1"
                            },
                            Location = OasParameterLocation.Header,
                            Name = "parameter1",
                            Options = OasParameterOptions.AllowEmptyValue | OasParameterOptions.AllowReserved | OasParameterOptions.Deprecated | OasParameterOptions.Explode | OasParameterOptions.Required,
                            Schema = "#/components/schemas/schema1",
                            Style = OasParameterStyle.Matrix
                        }
                    },
                    RequestBodies =
                    {
                        [ "requestbody1" ] = new OasRequestBodyBuilder()
                        {
                            Content =
                            {
                                [ new ContentType("application/json" )] = new OasMediaTypeBuilder()
                                {
                                    Schema = "#/components/schemas/schema1"
                                }
                            },
                            Description = "Description",
                            Options = OasRequestBodyOptions.Required
                        }
                    },
                    Responses =
                    {
                        [ "response1" ] = new OasResponseBuilder()
                        {
                            Content =
                            {
                                [ new ContentType("application/json") ] = new OasMediaTypeBuilder()
                                {
                                    Schema = "#/components/schemas/schema1"
                                }
                            },
                            Description = "Description",
                            Headers =
                            {
                                [ "header1" ] = "#/components/headers/header1"
                            },
                            Links =
                            {
                                [ "link1" ] = "#/components/links/link1"
                            }
                        }
                    },
                    Schemas =
                    {
                        [ "schema1" ] = new OasSchemaBuilder()
                        {
                            AdditionalProperties =
                            {
                                [ "prop1" ] = "#/components/schemas/schema1"
                            },
                            Description = "Description",
                            ExternalDocumentation = new OasExternalDocumentationBuilder()
                            {
                                Description = "Description",
                                Url = new Uri("http://example.org/docs")
                            },
                            Format = "Format",
                            Items = "#/components/schemas/schema1",
                            ItemsRange = new OasCountRange(100, 200, OasRangeOptions.Inclusive),
                            LengthRange = new OasCountRange(200, 300, OasRangeOptions.Exclusive),
                            NumberRange = new OasNumberRange(300, 400, OasRangeOptions.Exclusive),
                            Options = OasSchemaOptions.Deprecated | OasSchemaOptions.Nullable | OasSchemaOptions.Required | OasSchemaOptions.UniqueItems,
                            Pattern = "[a-z]",
                            Properties =
                            {
                                [ "prop1" ] = "#/components/schemas/schema1"
                            },
                            PropertiesRange = new OasCountRange(100, 200),
                            Title = "Schema1",
                            JsonType = OasSchemaType.Object
                        }
                    },
                    SecuritySchemes =
                    {
                        [ "sec1" ] = new OasHttpSecuritySchemeBuilder()
                        {
                            BearerFormat = "Bearer",
                            Description = "Description",
                            Scheme = "Schema"
                        },
                        [ "sec2" ] = new OasApiKeySecuritySchemeBuilder()
                        {
                            Description = "Description",
                            Location = OasParameterLocation.Cookie,
                            Name = "Name"
                        },
                        [ "sec3" ] = new OasOidcSecuritySchemeBuilder()
                        {
                            Description = "Description",
                            Url = new Uri("http://example.org/openid")
                        },
                        [ "sec4" ] = new OasOAuth2SecuritySchemeBuilder()
                        {
                            AuthorizationCodeFlow = new OasOAuthFlowBuilder()
                            {
                                AuthorizationUrl = new Uri("http://example.org/auth/auth"),
                                RefreshUrl = new Uri("http://example.org/auth/refresh"),
                                TokenUrl = new Uri("http://example.org/auth/token"),
                                Scopes =
                                {
                                    [ "user:details" ] = "Get the user details"
                                }
                            },
                            ClientCredentialsFlow = new OasOAuthFlowBuilder()
                            {
                                AuthorizationUrl = new Uri("http://example.org/cli/auth")
                            },
                            Description = "Description",
                            ImplicitFlow = new OasOAuthFlowBuilder()
                            {
                                AuthorizationUrl = new Uri("http://example.org/imp/auth")
                            },
                            PasswordFlow = new OasOAuthFlowBuilder()
                            {
                                AuthorizationUrl = new Uri("http://example.org/pwd/auth")
                            }
                        }
                    }
                },
                ExternalDocumentation = new OasExternalDocumentationBuilder()
                {
                    Description = "Description",
                    Url = new Uri("http://example.org/docs")
                },
                Info = new OasInformationBuilder()
                {
                    Contact = new OasContactBuilder()
                    {
                        Email = new MailAddress("jonathan@example.org"),
                        Name = "Jonathan",
                        Url = new Uri("http://example.org/jonathan")
                    },
                    Description = "Description",
                    License = new OasLicenseBuilder()
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    },
                    TermsOfService = new Uri("http://example.org/tos"),
                    Title = "Title",
                    Version = new SemanticVersion(1, 2, 3, "pre1", "build1")
                },
                Version = new SemanticVersion(3, 0, 1),
                Paths =
                {
                    [ "path1" ] = new OasPathBuilder()
                    {
                        Description = "Description"
                    }
                },
                Security =
                {
                    "#/components/security/sec1"
                }
            };

            var actualJson = sut.Serialize(actualGraphBuilder.Build());

            #endregion

            #region JSON

            var expectedJson = new JsonObject()
            {
                ["components"] = new JsonObject()
                {
                    ["callbacks"] = new JsonObject()
                    {
                        ["callback1"] = new JsonObject()
                        {
                            ["http://foo{$statusCode}"] = new JsonObject()
                            {
                                ["delete"] = new JsonObject()
                                {
                                    ["callbacks"] = new JsonObject()
                                    {
                                        ["callback1"] = new JsonObject()
                                        {
                                            ["$ref"] = "#/components/callbacks/callback1"
                                        }
                                    },
                                    ["deprecated"] = true,
                                    ["description"] = "Delete operation",
                                    ["externalDocs"] = new JsonObject()
                                    {
                                        ["description"] = "External docs 1",
                                        ["url"] = "http://example.org/"
                                    },
                                    ["operationId"] = "Operation 1",
                                    ["parameters"] = new JsonArray()
                                    {
                                        new JsonObject()
                                        {
                                            [ "$ref" ] = "#/components/parameters/parameter1"
                                        }
                                    },
                                    ["requestBody"] = new JsonObject()
                                    {
                                        ["$ref"] = "#/components/requestBodies/requestBody1"
                                    },
                                    ["responses"] = new JsonObject()
                                    {
                                        ["default"] = new JsonObject()
                                        {
                                            ["$ref"] = "#/components/responses/response1"
                                        }
                                    },
                                    ["summary"] = "Delete operation summary"
                                },
                                ["description"] = "path1",
                                ["get"] = new JsonObject()
                                {
                                    ["description"] = "Get"
                                },
                                ["head"] = new JsonObject()
                                {
                                    ["description"] = "Head"
                                },
                                ["options"] = new JsonObject()
                                {
                                    ["description"] = "Options"
                                },
                                ["parameters"] = new JsonArray()
                                {
                                    new JsonObject()
                                    {
                                        [ "$ref" ] = "#/components/parameters/parameter1"
                                    }
                                },
                                ["patch"] = new JsonObject()
                                {
                                    ["description"] = "Patch"
                                },
                                ["post"] = new JsonObject()
                                {
                                    ["description"] = "Post"
                                },
                                ["put"] = new JsonObject()
                                {
                                    ["description"] = "Put"
                                },
                                ["summary"] = "Summary",
                                ["trace"] = new JsonObject()
                                {
                                    ["description"] = "Trace"
                                }
                            }
                        }
                    },
                    ["examples"] = new JsonObject()
                    {
                        ["example1"] = new JsonObject()
                        {
                            ["description"] = "Description",
                            ["externalValue"] = "http://example.org/example",
                            ["summary"] = "Summary"
                        }
                    },
                    ["headers"] = new JsonObject()
                    {
                        ["header1"] = new JsonObject()
                        {
                            ["content"] = new JsonObject()
                            {
                                ["application/json"] = new JsonObject()
                                {
                                    ["encoding"] = new JsonObject()
                                    {
                                        ["header1"] = new JsonObject()
                                        {
                                            ["allowReserved"] = true,
                                            ["contentType"] = "application/json",
                                            ["explode"] = true,
                                            ["headers"] = new JsonObject()
                                            {
                                                ["header1"] = new JsonObject()
                                                {
                                                    ["$ref"] = "#/components/headers/header1"
                                                }
                                            },
                                            ["style"] = "deepObject"
                                        }
                                    },
                                    ["examples"] = new JsonObject()
                                    {
                                        ["Main Example"] = new JsonObject()
                                        {
                                            ["$ref"] = "#/components/examples/example1"
                                        }
                                    },
                                    ["schema"] = new JsonObject()
                                    {
                                        ["$ref"] = "#/components/schemas/schema1"
                                    }
                                }
                            }
                        }
                    },
                    ["links"] = new JsonObject()
                    {
                        ["link1"] = new JsonObject()
                        {
                            ["description"] = "Description",
                            ["operationId"] = "operation1",
                            ["operationRef"] = "http://example.org/#/operation1",
                            ["server"] = new JsonObject()
                            {
                                ["description"] = "Description",
                                ["url"] = "http://example.org/",
                                ["variables"] = new JsonObject()
                                {
                                    ["variable1"] = new JsonObject()
                                    {
                                        ["default"] = "Value",
                                        ["description"] = "Description",
                                        ["enum"] = new JsonArray()
                                        {
                                            "Value",
                                            "Value1"
                                        }
                                    }
                                }
                            }
                        }
                    },
                    ["parameters"] = new JsonObject()
                    {
                        ["parameter1"] = new JsonObject()
                        {
                            ["allowEmptyValue"] = true,
                            ["allowReserved"] = true,
                            ["content"] = new JsonObject()
                            {
                                ["application/json"] = new JsonObject()
                                {
                                    ["schema"] = new JsonObject()
                                    {
                                        ["$ref"] = "#/components/schemas/schema1"
                                    }
                                }
                            },
                            ["deprecated"] = true,
                            ["description"] = "Description",
                            ["examples"] = new JsonObject()
                            {
                                ["application/json"] = new JsonObject()
                                {
                                    ["$ref"] = "#/components/examples/example1"
                                }
                            },
                            ["explode"] = true,
                            ["in"] = "header",
                            ["name"] = "parameter1",
                            ["required"] = true,
                            ["schema"] = new JsonObject()
                            {
                                ["$ref"] = "#/components/schemas/schema1"
                            },
                            ["style"] = "matrix"
                        }
                    },
                    ["requestBodies"] = new JsonObject()
                    {
                        ["requestbody1"] = new JsonObject()
                        {
                            ["content"] = new JsonObject()
                            {
                                ["application/json"] = new JsonObject()
                                {
                                    ["schema"] = new JsonObject()
                                    {
                                        ["$ref"] = "#/components/schemas/schema1"
                                    }
                                }
                            },
                            ["description"] = "Description",
                            ["required"] = true
                        }
                    },
                    ["responses"] = new JsonObject()
                    {
                        ["response1"] = new JsonObject()
                        {
                            ["content"] = new JsonObject()
                            {
                                ["application/json"] = new JsonObject()
                                {
                                    ["schema"] = new JsonObject()
                                    {
                                        ["$ref"] = "#/components/schemas/schema1"
                                    }
                                }
                            },
                            ["description"] = "Description",
                            ["headers"] = new JsonObject()
                            {
                                ["header1"] = new JsonObject()
                                {
                                    ["$ref"] = "#/components/headers/header1"
                                }
                            },
                            ["links"] = new JsonObject()
                            {
                                ["link1"] = new JsonObject()
                                {
                                    ["$ref"] = "#/components/links/link1"
                                }
                            }
                        }
                    },
                    ["schemas"] = new JsonObject()
                    {
                        ["schema1"] = new JsonObject()
                        {
                            ["additionalProperties"] = new JsonObject()
                            {
                                ["prop1"] = new JsonObject()
                                {
                                    ["$ref"] = "#/components/schemas/schema1"
                                }
                            },
                            ["deprecated"] = true,
                            ["description"] = "Description",
                            ["exclusiveMaximum"] = true,
                            ["exclusiveMinimum"] = true,
                            ["externalDocs"] = new JsonObject()
                            {
                                ["description"] = "Description",
                                ["url"] = "http://example.org/docs"
                            },
                            ["format"] = "Format",
                            ["items"] = new JsonObject()
                            {
                                ["$ref"] = "#/components/schemas/schema1"
                            },
                            ["maxLength"] = 300,
                            ["maxProperties"] = 200,
                            ["maximum"] = 400,
                            ["minLength"] = 200,
                            ["minProperties"] = 100,
                            ["minimum"] = 300,
                            ["nullable"] = true,
                            ["pattern"] = "[a-z]",
                            ["properties"] = new JsonObject()
                            {
                                ["prop1"] = new JsonObject()
                                {
                                    ["$ref"] = "#/components/schemas/schema1"
                                }
                            },
                            ["required"] = true,
                            ["title"] = "Schema1",
                            ["type"] = "object",
                            ["uniqueItems"] = true
                        }
                    },
                    ["securitySchemes"] = new JsonObject()
                    {
                        ["sec1"] = new JsonObject()
                        {
                            ["bearerFormat"] = "Bearer",
                            ["description"] = "Description",
                            ["scheme"] = "Schema",
                            ["type"] = "http"
                        },
                        ["sec2"] = new JsonObject()
                        {
                            ["description"] = "Description",
                            ["in"] = "cookie",
                            ["name"] = "Name",
                            ["type"] = "apiKey"
                        },
                        ["sec3"] = new JsonObject()
                        {
                            ["description"] = "Description",
                            ["openIdConnectUrl"] = "http://example.org/openid",
                            ["type"] = "openIdConnect"
                        },
                        ["sec4"] = new JsonObject()
                        {
                            ["description"] = "Description",
                            ["flows"] = new JsonObject()
                            {
                                ["authorizationCode"] = new JsonObject()
                                {
                                    ["authorizationUrl"] = "http://example.org/auth/auth",
                                    ["refreshUrl"] = "http://example.org/auth/refresh",
                                    ["scopes"] = new JsonObject()
                                    {
                                        ["user:details"] = "Get the user details"
                                    },
                                    ["tokenUrl"] = "http://example.org/auth/token"
                                },
                                ["clientCredentials"] = new JsonObject()
                                {
                                    ["authorizationUrl"] = "http://example.org/cli/auth",
                                    ["tokenUrl"] = null
                                },
                                ["implicit"] = new JsonObject()
                                {
                                    ["authorizationUrl"] = "http://example.org/imp/auth",
                                    ["tokenUrl"] = null
                                },
                                ["password"] = new JsonObject()
                                {
                                    ["authorizationUrl"] = "http://example.org/pwd/auth",
                                    ["tokenUrl"] = null
                                }
                            },
                            ["type"] = "oauth2"
                        }
                    }
                },
                ["externalDocs"] = new JsonObject()
                {
                    ["description"] = "Description",
                    ["url"] = "http://example.org/docs"
                },
                ["info"] = new JsonObject()
                {
                    ["contact"] = new JsonObject()
                    {
                        ["email"] = "jonathan@example.org",
                        ["name"] = "Jonathan",
                        ["url"] = "http://example.org/jonathan"
                    },
                    ["description"] = "Description",
                    ["license"] = new JsonObject()
                    {
                        ["name"] = "MIT",
                        ["url"] = "https://opensource.org/licenses/MIT"
                    },
                    ["termsOfService"] = "http://example.org/tos",
                    ["title"] = "Title",
                    ["version"] = "1.2.3-pre1+build1"
                },
                ["openapi"] = "3.0.1",
                ["paths"] = new JsonObject()
                {
                    ["path1"] = new JsonObject()
                    {
                        ["description"] = "Description"
                    }
                },
                ["security"] = new JsonArray()
                {
                    new JsonObject()
                    {
                        [ "$ref" ] = "#/components/security/sec1"
                    }
                }
            };

            #endregion

            Assert.Equal(expectedJson.ToString(), actualJson.ToString());
        }

        #endregion
    }
}
