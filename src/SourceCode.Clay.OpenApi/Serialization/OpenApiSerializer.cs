#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Threading;

namespace SourceCode.Clay.OpenApi.Serialization
{
    /// <summary>
    /// Represents a serializer that can convert between OpenAPI objects and JSON.
    /// </summary>
    public class OpenApiSerializer : IOpenApiSerializer
    {
        #region Constants

        protected static class PropertyNames
        {
            #region Fields

            public const string Reference = "$ref";
            public const string Name = "name";
            public const string Tags = "tags";
            public const string ExternalDocs = "externalDocs";
            public const string Description = "description";
            public const string Type = "type";
            public const string Title = "title";
            public const string Format = "format";
            public const string MultipleOf = "multipleOf";
            public const string Maximum = "maximum";
            public const string ExclusiveMaximum = "exclusiveMaximum";
            public const string Minimum = "minimum";
            public const string ExclusiveMinimum = "exclusiveMinimum";
            public const string MinLength = "minLength";
            public const string MaxLength = "maxLength";
            public const string MinProperties = "minProperties";
            public const string MaxProperties = "maxProperties";
            public const string UniqueItems = "uniqueItems";
            public const string Required = "required";
            public const string EnumValue = "enum";
            public const string AllOf = "allOf";
            public const string AnyOf = "anyOf";
            public const string OneOf = "oneOf";
            public const string Not = "not";
            public const string Items = "items";
            public const string Properties = "properties";
            public const string AdditionalProperties = "additionalProperties";
            public const string Nullable = "nullable";
            public const string Deprecated = "deprecated";
            public const string Pattern = "pattern";
            public const string Schemas = "schemas";
            public const string Schema = "schema";
            public const string Responses = "responses";
            public const string Parameters = "parameters";
            public const string Examples = "examples";
            public const string RequestBodies = "requestBodies";
            public const string RequestBody = "requestBody";
            public const string Headers = "headers";
            public const string SecuritySchemes = "securitySchemes";
            public const string Links = "links";
            public const string Callbacks = "callbacks";
            public const string Url = "url";
            public const string Email = "email";
            public const string OpenApi = "openapi";
            public const string Info = "info";
            public const string Servers = "servers";
            public const string Server = "server";
            public const string Paths = "paths";
            public const string Components = "components";
            public const string Security = "security";
            public const string ContentType = "contentType";
            public const string Style = "style";
            public const string Explode = "explode";
            public const string AllowReserved = "allowReserved";
            public const string Summary = "summary";
            public const string Value = "value";
            public const string ExternalValue = "externalValue";
            public const string TermsOfService = "termsOfService";
            public const string Contact = "contact";
            public const string License = "license";
            public const string Version = "version";
            public const string OperationId = "operationId";
            public const string OperationRef = "operationRef";
            public const string Encoding = "encoding";
            public const string AllowEmptyValue = "allowEmptyValue";
            public const string Content = "content";
            public const string In = "in";
            public const string Delete = "delete";
            public const string Get = "get";
            public const string Head = "head";
            public const string Options = "options";
            public const string Patch = "patch";
            public const string Post = "post";
            public const string Put = "put";
            public const string Trace = "trace";
            public const string Default = "default";
            public const string Enum = "enum";
            public const string Variables = "variables";
            public const string Scheme = "scheme";
            public const string BearerFormat = "bearerFormat";
            public const string Flows = "flows";
            public const string Implicit = "implicit";
            public const string Password = "pass" + "word";
            public const string ClientCredentials = "clientCredentials";
            public const string AuthorizationCode = "authorizationCode";
            public const string AuthorizationUrl = "authorizationUrl";
            public const string TokenUrl = "tokenUrl";
            public const string RefreshUrl = "refreshUrl";
            public const string Scopes = "scopes";
            public const string OpenIdConnectUrl = "openIdConnectUrl";

            #endregion
        }

        protected static class EnumNames
        {
            #region Fields

            public const string Matrix = "matrix";
            public const string Label = "label";
            public const string Form = "form";
            public const string Simple = "simple";
            public const string SpaceDelimited = "spaceDelimited";
            public const string PipeDelimited = "pipeDelimited";
            public const string DeepObject = "deepObject";

            public const string Query = "query";
            public const string Header = "header";
            public const string Path = "path";
            public const string Cookie = "cookie";

            public const string Array = "array";
            public const string Boolean = "boolean";
            public const string Number = "number";
            public const string Object = "object";
            public const string String = "string";
            public const string Integer = "integer";

            public const string ApiKey = "apiKey";
            public const string Http = "http";
            public const string OAuth2 = "oauth2";
            public const string OpenIdConnect = "openIdConnect";

            #endregion
        }

        #endregion

        #region Fields

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, SerializerInfo> _serializers = new ConcurrentDictionary<RuntimeTypeHandle, SerializerInfo>();
        private SerializerInfo _mySerializers;

        #endregion

        #region Structs

        private struct SerializerKey : IEquatable<SerializerKey>
        {
            #region Properties

            public RuntimeTypeHandle GenericArgumentType { get; }
            public RuntimeTypeHandle InstanceType { get; }

            #endregion

            #region Constructors

            public SerializerKey(RuntimeTypeHandle genericArgumentType, RuntimeTypeHandle instanceType)
            {
                GenericArgumentType = genericArgumentType;
                InstanceType = instanceType;
            }

            #endregion

            #region Methods

            public override bool Equals(object obj) => obj is SerializerKey o && Equals(o);

            public bool Equals(SerializerKey other)
                => GenericArgumentType.Equals(other.GenericArgumentType)
                && InstanceType.Equals(other.InstanceType);

            public override int GetHashCode()
            {
                unchecked
                {
                    var hc = 17L;
                    hc = hc * 21 + GenericArgumentType.GetHashCode();
                    hc = hc * 21 + InstanceType.GetHashCode();
                    return ((int)(hc >> 32)) ^ (int)hc;
                }
            }

            #endregion
        }

        #endregion

        #region Classes

        private sealed class SerializerInfo
        {
            #region Fields

            private readonly ConcurrentDictionary<SerializerKey, Delegate> _delegates = new ConcurrentDictionary<SerializerKey, Delegate>();
            private readonly Type _serializerType;
            private readonly MethodInfo[] _methods;

            #endregion

            #region Constructors

            public SerializerInfo(Type serializerType)
            {
                _serializerType = serializerType;
                _methods = serializerType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(x => !x.IsAbstract && !x.IsGenericMethod && x.ReturnType == typeof(JsonValue) && x.GetParameters().Length == 1)
                    .ToArray();
            }

            #endregion

            #region Methods

            private Delegate CreateSerializer<T>(SerializerKey key)
            {
                var argType = typeof(T);
                var instType = Type.GetTypeFromHandle(key.InstanceType);

                MethodInfo method = default;
                for (; instType != typeof(ValueType) && instType != typeof(object); instType = instType.BaseType)
                {
                    method = _methods.FirstOrDefault(x => x.GetParameters()[0].ParameterType == instType);
                    if (method != null) break;
                }

                if (method == default)
                {
                    instType = Type.GetTypeFromHandle(key.InstanceType);
                    return new Func<OpenApiSerializer, T, JsonValue>(
                        (x, y) => throw new NotSupportedException($"Serializing the type {instType.FullName} is not supported."));
                }

                var serParam = Expression.Parameter(_serializerType, "serializer");
                var param = Expression.Parameter(argType, "value");
                var conv = argType == instType
                    ? (Expression)param
                    : Expression.Convert(param, instType);
                var call = Expression.Call(serParam, method, conv);
                return Expression.Lambda<Func<OpenApiSerializer, T, JsonValue>>(call, serParam, param).Compile();
            }

            public JsonValue Serialize<T>(OpenApiSerializer serializer, T value)
            {
                var key = new SerializerKey(typeof(T).TypeHandle, value.GetType().TypeHandle);
                var del = _delegates.GetOrAdd(key, CreateSerializer<T>);
                return ((Func<OpenApiSerializer, T, JsonValue>)del)(serializer, value);
            }

            #endregion
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of the <see cref="OpenApiSerializer"/> class.
        /// </summary>
        public OpenApiSerializer()
        {
        }

        #endregion

        #region Serialize

        /// <summary>
        /// Serializes a <see cref="Response"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Response"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        private JsonValue SerializeResponse(Response value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            var content = value.Content
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            SetJsonValue(json, PropertyNames.Description, value.Description, true);
            SetJsonMap(json, PropertyNames.Headers, value.Headers);
            SetJsonMap(json, PropertyNames.Content, content);
            SetJsonMap(json, PropertyNames.Links, value.Links);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="RequestBody"/> value.
        /// </summary>
        /// <param name="value">The <see cref="RequestBody"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        private JsonValue SerializeRequestBody(RequestBody value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            var content = value.Content
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonMap(json, PropertyNames.Content, content);
            SetJsonFlag(json, PropertyNames.RequestBodies, value.Options, RequestBodyOptions.Required);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="PropertyEncoding"/> value.
        /// </summary>
        /// <param name="value">The <see cref="PropertyEncoding"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        private JsonValue SerializePropertyEncoding(PropertyEncoding value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.ContentType, value.ContentType);
            SetJsonMap(json, PropertyNames.Headers, value.Headers);
            SetJsonValue(json, PropertyNames.Style, ToJsonValue(value.Style));
            SetJsonFlag(json, PropertyNames.Explode, value.Options, PropertyEncodingOptions.Explode);
            SetJsonFlag(json, PropertyNames.AllowReserved, value.Options, PropertyEncodingOptions.AllowReserved);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Path"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Path"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        private JsonValue SerializePath(Path value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Summary, value.Summary);
            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonObject(json, PropertyNames.Get, value.Get);
            SetJsonObject(json, PropertyNames.Put, value.Put);
            SetJsonObject(json, PropertyNames.Post, value.Post);
            SetJsonObject(json, PropertyNames.Delete, value.Delete);
            SetJsonObject(json, PropertyNames.Options, value.Options);
            SetJsonObject(json, PropertyNames.Head, value.Head);
            SetJsonObject(json, PropertyNames.Patch, value.Patch);
            SetJsonObject(json, PropertyNames.Trace, value.Trace);
            SetJsonArray(json, PropertyNames.Servers, value.Servers);
            SetJsonArray(json, PropertyNames.Parameters, value.Parameters);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="ParameterBody"/> value.
        /// </summary>
        /// <param name="value">The <see cref="ParameterBody"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        private JsonValue SerializeParameterBody(ParameterBody value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SerializeParameterBody(value, json);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="ParameterBody"/> value into an existing <see cref="JsonObject"/> instance.
        /// </summary>
        /// <param name="value">The <see cref="ParameterBody"/> value to serialize.</param>
        protected virtual void SerializeParameterBody(ParameterBody value, JsonObject json)
        {
            if (value == null) return;

            var examples = value.Examples
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            var content = value.Content
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonFlag(json, PropertyNames.Required, value.Options, ParameterOptions.Required);
            SetJsonFlag(json, PropertyNames.Deprecated, value.Options, ParameterOptions.Deprecated);
            SetJsonFlag(json, PropertyNames.AllowEmptyValue, value.Options, ParameterOptions.AllowEmptyValue);

            SetJsonValue(json, PropertyNames.Style, ToJsonValue(value.Style));
            SetJsonFlag(json, PropertyNames.Explode, value.Options, ParameterOptions.Explode);
            SetJsonFlag(json, PropertyNames.AllowReserved, value.Options, ParameterOptions.AllowReserved);
            SetJsonObject(json, PropertyNames.Schema, value.Schema);
            SetJsonMap(json, PropertyNames.Examples, examples);

            SetJsonMap(json, PropertyNames.Content, content);
        }

        /// <summary>
        /// Serializes a <see cref="Parameter"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Parameter"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeParameter(Parameter value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Name, value.Name, true);
            SetJsonValue(json, PropertyNames.In, ToJsonValue(value.Location));
            SerializeParameterBody(value, json);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Parameter"/> value according to its components.
        /// </summary>
        /// <param name="key">The <see cref="ParameterKey"/> component to serialize.</param>
        /// <param name="value">The referable <see cref="ParameterBody"/> component to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeParameter(ParameterKey key, Referable<ParameterBody> value)
        {
            if (value.IsReference) return SerializeReference(value.Reference);
            return SerializeParameter(key, value.Value);
        }

        /// <summary>
        /// Serializes a <see cref="Parameter"/> value according to its components.
        /// </summary>
        /// <param name="key">The <see cref="ParameterKey"/> component to serialize.</param>
        /// <param name="value">The <see cref="ParameterBody"/> component to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeParameter(ParameterKey key, ParameterBody value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Name, key.Name, true);
            SetJsonValue(json, PropertyNames.In, ToJsonValue(key.Location));
            SerializeParameterBody(value, json);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Operation"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Operation"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeOperation(Operation value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            var responses = value.Responses
                .Select(x => ValueTuple.Create(x.Key.ToString(), x.Value));

            SetJsonArray(json, PropertyNames.Tags, value.Tags);
            SetJsonValue(json, PropertyNames.Summary, value.Summary);
            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonObject(json, PropertyNames.ExternalDocs, value.ExternalDocumentation);
            SetJsonValue(json, PropertyNames.OperationId, value.OperationIdentifier);
            SetJsonArray(json, PropertyNames.Parameters, value.Parameters);
            SetJsonObject(json, PropertyNames.RequestBody, value.RequestBody);
            SetJsonMap(json, PropertyNames.Responses, responses);
            SetJsonMap(json, PropertyNames.Callbacks, value.Callbacks);
            SetJsonFlag(json, PropertyNames.Deprecated, value.Options, OperationOptions.Deprecated);
            SetJsonArray(json, PropertyNames.Security, value.Security);
            SetJsonArray(json, PropertyNames.Servers, value.Servers);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OpenIdConnectSecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OpenIdConnectSecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeOpenIdConnectSecurityScheme(OpenIdConnectSecurityScheme value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Type, EnumNames.OpenIdConnect);
            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonValue(json, PropertyNames.OpenIdConnectUrl, value.Url);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OAuthFlow"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OAuthFlow"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeOAuthFlow(OAuthFlow value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.AuthorizationUrl, value.AuthorizationUrl, true);
            SetJsonValue(json, PropertyNames.TokenUrl, value.TokenUrl, true);
            SetJsonValue(json, PropertyNames.RefreshUrl, value.RefreshUrl);
            SetJsonMap(json, PropertyNames.Scopes, value.Scopes);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OAuth2SecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OAuth2SecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeOAuth2SecurityScheme(OAuth2SecurityScheme value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Type, EnumNames.OAuth2);
            SetJsonValue(json, PropertyNames.Description, value.Description);

            var flows = new JsonObject();

            SetJsonObject(flows, PropertyNames.Implicit, value.ImplicitFlow);
            SetJsonObject(flows, PropertyNames.Password, value.PasswordFlow);
            SetJsonObject(flows, PropertyNames.ClientCredentials, value.ClientCredentialsFlow);
            SetJsonObject(flows, PropertyNames.AuthorizationCode, value.AuthorizationCodeFlow);

            json.Add(PropertyNames.Flows, flows);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="MediaType"/> value.
        /// </summary>
        /// <param name="value">The <see cref="MediaType"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeMediaType(MediaType value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonObject(json, PropertyNames.Schema, value.Schema);
            SetJsonMap(json, PropertyNames.Examples, value.Examples);
            SetJsonMap(json, PropertyNames.Encoding, value.Encoding);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Link"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Link"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeLink(Link value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.OperationRef, SerializeReference(value.OperationReference));
            SetJsonValue(json, PropertyNames.OperationId, value.OperationIdentifier);
            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonObject(json, PropertyNames.Server, value.Server);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="License"/> value.
        /// </summary>
        /// <param name="value">The <see cref="License"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeLicense(License value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Name, value.Name);
            SetJsonValue(json, PropertyNames.Url, value.Url);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Information"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Information"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeInformation(Information value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Title, value.Title);
            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonValue(json, PropertyNames.TermsOfService, value.TermsOfService);
            SetJsonObject(json, PropertyNames.Contact, value.Contact);
            SetJsonObject(json, PropertyNames.License, value.License);
            SetJsonValue(json, PropertyNames.Version, value.Version.ToString());

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="HttpSecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="HttpSecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeHttpSecurityScheme(HttpSecurityScheme value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Type, EnumNames.Http);
            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonValue(json, PropertyNames.Scheme, value.Scheme, true);
            SetJsonValue(json, PropertyNames.BearerFormat, value.BearerFormat);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Reference"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Reference"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeReference(Reference value)
        {
            if (!value.HasValue) return default;
            return value.ToUri()?.ToString();
        }

        /// <summary>
        /// Serializes a <see cref="Referable{T}"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Referable{T}"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeReferable(IReferable referable)
        {
            if (!referable.HasValue) return default;
            if (referable.IsReference)
            {
                var reference = Serialize(referable.Reference);
                return new JsonObject()
                {
                    [PropertyNames.Reference] = reference
                };
            }
            return SerializeUnknown(referable.Value);
        }

        /// <summary>
        /// Serializes a <see cref="Contact"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Contact"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeComponents(Components value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonMap(json, PropertyNames.Schemas, value.Schemas);
            SetJsonMap(json, PropertyNames.Responses, value.Responses);
            SetJsonMap(json, PropertyNames.Parameters, value.Parameters);
            SetJsonMap(json, PropertyNames.Examples, value.Examples);
            SetJsonMap(json, PropertyNames.RequestBodies, value.RequestBodies);
            SetJsonMap(json, PropertyNames.Headers, value.Headers);
            SetJsonMap(json, PropertyNames.SecuritySchemes, value.SecuritySchemes);
            SetJsonMap(json, PropertyNames.Links, value.Links);
            SetJsonMap(json, PropertyNames.Callbacks, value.Callbacks);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Contact"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Contact"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeContact(Contact value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Name, value.Name);
            SetJsonValue(json, PropertyNames.Url, value.Url);
            SetJsonValue(json, PropertyNames.Email, value.Email);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="ExternalDocumentation"/> value.
        /// </summary>
        /// <param name="value">The <see cref="ExternalDocumentation"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeExternalDocumentation(ExternalDocumentation value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonValue(json, PropertyNames.Url, value.Url, true);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Example"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Example"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeExample(Example value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Summary, value.Summary);
            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonValue(json, PropertyNames.ExternalValue, value.ExternalValue);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Document"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Document"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeDocument(Document value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.OpenApi, value.Version.ToString());
            SetJsonObject(json, PropertyNames.Info, value.Info, true);
            SetJsonArray(json, PropertyNames.Servers, value.Servers);
            SetJsonMap(json, PropertyNames.Paths, value.Paths, true);
            SetJsonObject(json, PropertyNames.Components, value.Components);
            SetJsonArray(json, PropertyNames.Security, value.Security);
            SetJsonArray(json, PropertyNames.Tags, value.Tags);
            SetJsonObject(json, PropertyNames.ExternalDocs, value.ExternalDocumentation);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Callback"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Callback"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeCallback(Callback value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            foreach (var item in value)
            {
                var k = item.Key.ToString();
                var v = Serialize(item.Value);
                json.Add(k, v);
            }

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="ApiKeySecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="ApiKeySecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeApiKeySecurityScheme(ApiKeySecurityScheme value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Type, EnumNames.ApiKey);
            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonValue(json, PropertyNames.Name, value.Name);
            SetJsonValue(json, PropertyNames.In, ToJsonValue(value.Location));

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Tag"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Tag"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeTag(Tag value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Name, value.Name);
            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonObject(json, PropertyNames.ExternalDocs, value.ExternalDocumentation);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="ServerVariable"/> value.
        /// </summary>
        /// <param name="value">The <see cref="ServerVariable"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeServerVariable(ServerVariable value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonArray(json, PropertyNames.Enum, value.Enum);
            SetJsonValue(json, PropertyNames.Default, value.Default);
            SetJsonValue(json, PropertyNames.Description, value.Description);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Server"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Server"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeServer(Server value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Url, value.Url);
            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonMap(json, PropertyNames.Variables, value.Variables);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="SecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="SecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeSecurityScheme(SecurityScheme value) =>
            SerializeUnknown(value);

        /// <summary>
        /// Serializes a <see cref="Schema"/> value.
        /// </summary>
        /// <param name="value">The schema value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeSchema(Schema value)
        {
            if (ReferenceEquals(value, null)) return default;

            var json = new JsonObject();

            SetJsonValue(json, PropertyNames.Title, value.Title);
            SetJsonValue(json, PropertyNames.Description, value.Description);
            SetJsonValue(json, PropertyNames.Type, ToJsonValue(value.Type));
            SetJsonValue(json, PropertyNames.Format, value.Format);

            SetJsonValue(json, PropertyNames.Pattern, value.Pattern);
            SetJsonArray(json, PropertyNames.EnumValue, value.Enum);

            SetJsonFlag(json, PropertyNames.Deprecated, value.Options, SchemaOptions.Deprecated);
            SetJsonFlag(json, PropertyNames.Nullable, value.Options, SchemaOptions.Nullable);
            SetJsonFlag(json, PropertyNames.Required, value.Options, SchemaOptions.Required);
            SetJsonFlag(json, PropertyNames.UniqueItems, value.Options, SchemaOptions.UniqueItems);

            SetJsonArray(json, PropertyNames.AllOf, value.AllOf);
            SetJsonArray(json, PropertyNames.AnyOf, value.AnyOf);
            SetJsonArray(json, PropertyNames.OneOf, value.OneOf);
            SetJsonArray(json, PropertyNames.Not, value.Not);

            SetJsonNumber(json, PropertyNames.Minimum, value.NumberRange.Minimum);
            if (value.NumberRange.Minimum.HasValue)
                SetJsonFlag(json, PropertyNames.ExclusiveMinimum, value.NumberRange.RangeOptions, RangeOptions.MinimumInclusive, invert: true);

            SetJsonNumber(json, PropertyNames.Maximum, value.NumberRange.Maximum);
            if (value.NumberRange.Maximum.HasValue)
                SetJsonFlag(json, PropertyNames.ExclusiveMaximum, value.NumberRange.RangeOptions, RangeOptions.MaximumInclusive, invert: true);

            SetJsonNumber(json, PropertyNames.MultipleOf, value.NumberRange.MultipleOf);

            SetJsonValue(json, PropertyNames.MinLength, value.LengthRange.Minimum);
            SetJsonValue(json, PropertyNames.MaxLength, value.LengthRange.Maximum);

            SetJsonValue(json, PropertyNames.MinProperties, value.PropertiesRange.Minimum);
            SetJsonValue(json, PropertyNames.MaxProperties, value.PropertiesRange.Maximum);

            SetJsonObject(json, PropertyNames.Items, value.Items);
            SetJsonMap(json, PropertyNames.Properties, value.Properties);
            SetJsonMap(json, PropertyNames.AdditionalProperties, value.AdditionalProperties);

            SetJsonObject(json, PropertyNames.ExternalDocs, value.ExternalDocumentation);

            return json;
        }

        /// <summary>
        /// Serializes an unknown object type.
        /// </summary>
        /// <typeparam name="T">The type of the object. This may not reflect the exact object type.</typeparam>
        /// <param name="value">The object value.</param>
        /// <returns>The serialized <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeUnknown<T>(T value)
        {
            if (ReferenceEquals(value, null)) return default;

            var mySerializers = _mySerializers;
            if (mySerializers == null)
            {
                Thread.MemoryBarrier();
                mySerializers = _serializers.GetOrAdd(GetType().TypeHandle, th => new SerializerInfo(Type.GetTypeFromHandle(th)));
                _mySerializers = mySerializers;
            }

            return _mySerializers.Serialize(this, value);
        }

        /// <summary>Serializes the specified OpenAPI object to a JSON value.</summary>
        /// <typeparam name="T">The type of the OpenAPI object.</typeparam>
        /// <param name="value">The instance of the OpenAPI object.</param>
        /// <returns>The serialized value.</returns>
        public virtual JsonValue Serialize<T>(T value)
        {
            if (ReferenceEquals(value, null)) return default;

            if (typeof(T) == typeof(ApiKeySecurityScheme)) return SerializeApiKeySecurityScheme((ApiKeySecurityScheme)(object)value);
            if (typeof(T) == typeof(Callback)) return SerializeCallback((Callback)(object)value);
            if (typeof(T) == typeof(Components)) return SerializeComponents((Components)(object)value);
            if (typeof(T) == typeof(Contact)) return SerializeContact((Contact)(object)value);
            if (typeof(T) == typeof(Document)) return SerializeDocument((Document)(object)value);
            if (typeof(T) == typeof(Example)) return SerializeExample((Example)(object)value);
            if (typeof(T) == typeof(ExternalDocumentation)) return SerializeExternalDocumentation((ExternalDocumentation)(object)value);
            if (typeof(T) == typeof(HttpSecurityScheme)) return SerializeHttpSecurityScheme((HttpSecurityScheme)(object)value);
            if (typeof(T) == typeof(Information)) return SerializeInformation((Information)(object)value);
            if (typeof(T) == typeof(License)) return SerializeLicense((License)(object)value);
            if (typeof(T) == typeof(Link)) return SerializeLink((Link)(object)value);
            if (typeof(T) == typeof(MediaType)) return SerializeMediaType((MediaType)(object)value);
            if (typeof(T) == typeof(OAuth2SecurityScheme)) return SerializeOAuth2SecurityScheme((OAuth2SecurityScheme)(object)value);
            if (typeof(T) == typeof(OAuthFlow)) return SerializeOAuthFlow((OAuthFlow)(object)value);
            if (typeof(T) == typeof(OpenIdConnectSecurityScheme)) return SerializeOpenIdConnectSecurityScheme((OpenIdConnectSecurityScheme)(object)value);
            if (typeof(T) == typeof(Operation)) return SerializeOperation((Operation)(object)value);
            if (typeof(T) == typeof(Parameter)) return SerializeParameter((Parameter)(object)value);
            if (typeof(T) == typeof(ParameterBody)) return SerializeParameterBody((ParameterBody)(object)value);
            if (typeof(T) == typeof(Path)) return SerializePath((Path)(object)value);
            if (typeof(T) == typeof(PropertyEncoding)) return SerializePropertyEncoding((PropertyEncoding)(object)value);
            if (typeof(T) == typeof(RequestBody)) return SerializeRequestBody((RequestBody)(object)value);
            if (typeof(T) == typeof(Response)) return SerializeResponse((Response)(object)value);
            if (typeof(T) == typeof(Schema)) return SerializeSchema((Schema)(object)value);
            if (typeof(T) == typeof(SecurityScheme)) return SerializeSecurityScheme((SecurityScheme)(object)value);
            if (typeof(T) == typeof(Server)) return SerializeServer((Server)(object)value);
            if (typeof(T) == typeof(ServerVariable)) return SerializeServerVariable((ServerVariable)(object)value);
            if (typeof(T) == typeof(Tag)) return SerializeTag((Tag)(object)value);
            if (typeof(T) == typeof(Reference)) return SerializeReference((Reference)(object)value);

            if (typeof(T) == typeof(string)) return (string)(object)value;

            if (value is IReferable referable) return SerializeReferable(referable);
            if (typeof(T) == typeof(KeyValuePair<ParameterKey, ParameterBody>))
            {
                var kvp = (KeyValuePair<ParameterKey, ParameterBody>)(object)value;
                return SerializeParameter(kvp.Key, kvp.Value);
            }
            if (typeof(T) == typeof(KeyValuePair<ParameterKey, Referable<ParameterBody>>))
            {
                var kvp = (KeyValuePair<ParameterKey, Referable<ParameterBody>>)(object)value;
                return SerializeParameter(kvp.Key, kvp.Value);
            }

            return SerializeUnknown(value);
        }

        #endregion

        #region Deserialize

        /// <summary>Deserializes the specified OpenAPI object from a JSON value.</summary>
        /// <typeparam name="T">The expected type of the OpenAPI object.</typeparam>
        /// <param name="value">The JSON value to deserialize.</param>
        /// <returns>The OpenAPI object.</returns>
        public T Deserialize<T>(JsonValue value)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region JsonHelpers

        /// <summary>
        /// Converts the specified dictionary into its object representation.
        /// </summary>
        /// <typeparam name="T">The type of value in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="required">A value indicating whether an empty object is required.</param>
        /// <returns>The <see cref="JsonObject"/> representing the dictionary.</returns>
        protected virtual JsonObject ToJsonMap<T>(IEnumerable<ValueTuple<string, T>> dictionary, bool required = false)
        {
            if (dictionary == null)
            {
                return required ? new JsonObject() : default;
            }

            using (var e = dictionary.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    return required ? new JsonObject() : default;
                }

                var result = new JsonObject();
                do
                {
                    result.Add(e.Current.Item1, Serialize(e.Current.Item2));
                } while (e.MoveNext());

                return result;
            }
        }

        /// <summary>
        /// Converts the specified dictionary into its object representation.
        /// </summary>
        /// <typeparam name="T">The type of value in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="required">A value indicating whether an empty object is required.</param>
        /// <returns>The <see cref="JsonObject"/> representing the dictionary.</returns>
        protected virtual JsonObject ToJsonMap<T>(IEnumerable<KeyValuePair<string, T>> dictionary, bool required = false)
        {
            if (dictionary == null)
            {
                return required ? new JsonObject() : default;
            }

            using (var e = dictionary.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    return required ? new JsonObject() : default;
                }

                var result = new JsonObject();
                do
                {
                    result.Add(e.Current.Key, Serialize(e.Current.Value));
                } while (e.MoveNext());

                return result;
            }
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of value in the dictionary.</typeparam>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="dictionary">The dictionary value.</param>
        /// <param name="required">A value indicating whether an empty object is required.</param>
        protected virtual void SetJsonMap<T>(JsonObject container, string key, IEnumerable<ValueTuple<string, T>> dictionary, bool required = false)
        {
            var result = ToJsonMap(dictionary, required);
            if (result != null) container.Add(key, result);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of value in the dictionary.</typeparam>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="dictionary">The dictionary value.</param>
        /// <param name="required">A value indicating whether an empty object is required.</param>
        protected virtual void SetJsonMap<T>(JsonObject container, string key, IEnumerable<KeyValuePair<string, T>> dictionary, bool required = false)
        {
            var result = ToJsonMap(dictionary, required);
            if (result != null) container.Add(key, result);
        }

        /// <summary>
        /// Converts the specified list into its array representation.
        /// </summary>
        /// <typeparam name="T">The type of value in the list.</typeparam>
        /// <param name="list">The type of elements in the list.</param>
        /// <param name="required">A value indicating whether an empty array is required.</param>
        /// <returns>The <see cref="JsonArray"/> containing the list.</returns>
        protected virtual JsonArray ToJsonArray<T>(IEnumerable<T> list, bool required = false)
        {
            if (list == null)
            {
                return required ? new JsonArray() : default;
            }

            using (var e = list.GetEnumerator())
            {
                if (!e.MoveNext())
                {
                    return required ? new JsonArray() : default;
                }

                var result = new JsonArray();
                do
                {
                    result.Add(Serialize(e.Current));
                } while (e.MoveNext());

                return result;
            }
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of value in the list.</typeparam>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="dictionary">The list value.</param>
        /// <param name="required">A value indicating whether an empty array is required.</param>
        protected virtual void SetJsonArray<T>(JsonObject container, string key, IEnumerable<T> list, bool required = false)
        {
            var result = ToJsonArray(list);
            if (result != null) container.Add(key, result);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum.</typeparam>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The enum flags value.</param>
        /// <param name="flag">The flag to search for.</param>
        /// <param name="defaultState">The default state of the flag.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        /// <param name="invert">A value indicating whether to invert the flag check.</param>
        protected virtual void SetJsonFlag<TEnum>(JsonObject container, string key, TEnum value, TEnum flag, bool defaultState = false, bool required = false, bool invert = false)
            where TEnum : struct, IComparable, IConvertible, IFormattable
        {
            var v = EnumConvert.ToUInt64(value);
            var f = EnumConvert.ToUInt64(flag);
            var present = (v & f) == f;
            if (invert) present = !present;

            if (present != defaultState || required)
                container.Add(key, present);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JsonObject container, string key, string value, bool required = false)
        {
            if (required && value == null)
                container.Add(key, null);
            else if (required || value != null)
                container.Add(key, value);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JsonObject container, string key, JsonValue value, bool required = false)
        {
            if (required || value != null)
                container.Add(key, value);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JsonObject container, string key, Uri value, bool required = false)
        {
            if (value == null && required)
                container.Add(key, null);
            else if (value != null)
                container.Add(key, value.ToString());
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JsonObject container, string key, ContentType value, bool required = false)
        {
            if (value == null && required)
                container.Add(key, null);
            else if (value != null)
                container.Add(key, value.ToString());
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonValue(JsonObject container, string key, MailAddress value, bool required = false)
        {
            if (value == null && required)
                container.Add(key, null);
            else if (value != null)
                container.Add(key, value.ToString());
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonNumber(JsonObject container, string key, Number? value, bool required = false)
        {
            if (value.HasValue)
                container.Add(key, value.Value.ToValue());
            else if (required)
                container.Add(key, null);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonObject<T>(JsonObject container, string key, T value, bool required = false)
        {
            var js = Serialize(value);
            if (js == null)
            {
                if (required) container.Add(key, new JsonObject());
                return;
            }

            container.Add(key, js);
        }

        /// <summary>
        /// Sets a property in the specified <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="container">The object to set the property on.</param>
        /// <param name="key">The property name in <paramref name="container"/>.</param>
        /// <param name="value">The value.</param>
        /// <param name="required">A value indicating whether the property is required.</param>
        protected virtual void SetJsonObject<T>(JsonObject container, string key, Referable<T> value, bool required = false)
            where T : class, IEquatable<T>
        {
            if (!value.HasValue)
            {
                if (required) container.Add(key, null);
            }
            else if (value.IsReference)
            {
                container.Add(key, SerializeReference(value.Reference));
            }
            else
            {
                container.Add(key, Serialize(value.Value));
            }
        }

        /// <summary>
        /// Converts the specified <see cref="ParameterStyle"/> to a <see cref="JsonValue"/>.
        /// </summary>
        /// <param name="parameterStyle">The <see cref="ParameterStyle"/> to convert.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue ToJsonValue(ParameterStyle parameterStyle)
        {
            switch (parameterStyle)
            {
                case ParameterStyle.Default: return default;
                case ParameterStyle.Matrix: return EnumNames.Matrix;
                case ParameterStyle.Label: return EnumNames.Label;
                case ParameterStyle.Form: return EnumNames.Form;
                case ParameterStyle.Simple: return EnumNames.Simple;
                case ParameterStyle.SpaceDelimited: return EnumNames.SpaceDelimited;
                case ParameterStyle.PipeDelimited: return EnumNames.PipeDelimited;
                case ParameterStyle.DeepObject: return EnumNames.DeepObject;
                default: throw new ArgumentOutOfRangeException(nameof(parameterStyle));
            }
        }

        /// <summary>
        /// Converts the specified <see cref="SchemaType"/> to a <see cref="JsonValue"/>.
        /// </summary>
        /// <param name="schemaType">The <see cref="SchemaType"/> to convert.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue ToJsonValue(SchemaType schemaType)
        {
            switch (schemaType)
            {
                case SchemaType.String: return EnumNames.String;
                case SchemaType.Number: return EnumNames.Number;
                case SchemaType.Object: return EnumNames.Object;
                case SchemaType.Array: return EnumNames.Array;
                case SchemaType.Boolean: return EnumNames.Boolean;
                case SchemaType.Integer: return EnumNames.Integer;
                default: throw new ArgumentOutOfRangeException(nameof(schemaType));
            }
        }

        /// <summary>
        /// Converts the specified <see cref="ParameterLocation"/> to a <see cref="JsonValue"/>.
        /// </summary>
        /// <param name="schemaType">The <see cref="ParameterLocation"/> to convert.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue ToJsonValue(ParameterLocation parameterLocation)
        {
            switch (parameterLocation)
            {
                case ParameterLocation.Query: return EnumNames.Query;
                case ParameterLocation.Header: return EnumNames.Header;
                case ParameterLocation.Path: return EnumNames.Path;
                case ParameterLocation.Cookie: return EnumNames.Cookie;
                default: throw new ArgumentOutOfRangeException(nameof(parameterLocation));
            }
        }

        #endregion
    }
}
