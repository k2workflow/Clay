#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;

namespace SourceCode.Clay.OpenApi.Serialization
{
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
    // Null is significant in JSON.

    partial class OpenApiSerializer
    {
        #region Serialize

        /// <summary>
        /// Serializes a <see cref="Response"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Response"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeResponse(Response value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            var content = value.Content
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            SetJsonValue(json, PropertyConstants.Description, value.Description, true);
            SetJsonMap(json, PropertyConstants.Headers, value.Headers);
            SetJsonMap(json, PropertyConstants.Content, content);
            SetJsonMap(json, PropertyConstants.Links, value.Links);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="RequestBody"/> value.
        /// </summary>
        /// <param name="value">The <see cref="RequestBody"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeRequestBody(RequestBody value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            var content = value.Content
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonMap(json, PropertyConstants.Content, content);
            SetJsonFlag(json, PropertyConstants.RequestBodies, value.Options, RequestBodyOptions.Required);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="PropertyEncoding"/> value.
        /// </summary>
        /// <param name="value">The <see cref="PropertyEncoding"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializePropertyEncoding(PropertyEncoding value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.ContentType, value.ContentType);
            SetJsonMap(json, PropertyConstants.Headers, value.Headers);
            SetJsonValue(json, PropertyConstants.Style, ToJsonValue(value.Style));
            SetJsonFlag(json, PropertyConstants.Explode, value.Options, PropertyEncodingOptions.Explode);
            SetJsonFlag(json, PropertyConstants.AllowReserved, value.Options, PropertyEncodingOptions.AllowReserved);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Path"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Path"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializePath(Path value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Summary, value.Summary);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonObject(json, PropertyConstants.Get, value.Get);
            SetJsonObject(json, PropertyConstants.Put, value.Put);
            SetJsonObject(json, PropertyConstants.Post, value.Post);
            SetJsonObject(json, PropertyConstants.Delete, value.Delete);
            SetJsonObject(json, PropertyConstants.Options, value.Options);
            SetJsonObject(json, PropertyConstants.Head, value.Head);
            SetJsonObject(json, PropertyConstants.Patch, value.Patch);
            SetJsonObject(json, PropertyConstants.Trace, value.Trace);
            SetJsonArray(json, PropertyConstants.Servers, value.Servers);
            SetJsonArray(json, PropertyConstants.Parameters, value.Parameters);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="ParameterBody"/> value.
        /// </summary>
        /// <param name="value">The <see cref="ParameterBody"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeParameterBody(ParameterBody value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SerializeParameterBody(value, json);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="ParameterBody"/> value into an existing <see cref="JsonObject"/> instance.
        /// </summary>
        /// <param name="value">The <see cref="ParameterBody"/> value to serialize.</param>
        /// <param name="json">The <see cref="JsonObject"/> to populate.</param>
        protected virtual void SerializeParameterBody(ParameterBody value, JsonObject json)
        {
            if (value == null) return;

            var examples = value.Examples
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            var content = value.Content
                .Select(x => ValueTuple.Create(x.Key?.ToString(), x.Value));

            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonFlag(json, PropertyConstants.Required, value.Options, ParameterOptions.Required);
            SetJsonFlag(json, PropertyConstants.Deprecated, value.Options, ParameterOptions.Deprecated);
            SetJsonFlag(json, PropertyConstants.AllowEmptyValue, value.Options, ParameterOptions.AllowEmptyValue);

            SetJsonValue(json, PropertyConstants.Style, ToJsonValue(value.Style));
            SetJsonFlag(json, PropertyConstants.Explode, value.Options, ParameterOptions.Explode);
            SetJsonFlag(json, PropertyConstants.AllowReserved, value.Options, ParameterOptions.AllowReserved);
            SetJsonObject(json, PropertyConstants.Schema, value.Schema);
            SetJsonMap(json, PropertyConstants.Examples, examples);

            SetJsonMap(json, PropertyConstants.Content, content);
        }

        /// <summary>
        /// Serializes a <see cref="Parameter"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Parameter"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeParameter(Parameter value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Name, value.Name, true);
            SetJsonValue(json, PropertyConstants.In, ToJsonValue(value.Location));
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
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Name, key.Name, true);
            SetJsonValue(json, PropertyConstants.In, ToJsonValue(key.Location));
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
            if (value is null) return null;

            var json = new JsonObject();

            var responses = value.Responses
                .Select(x => ValueTuple.Create(x.Key.ToString(), x.Value));

            SetJsonArray(json, PropertyConstants.Tags, value.Tags);
            SetJsonValue(json, PropertyConstants.Summary, value.Summary);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonObject(json, PropertyConstants.ExternalDocs, value.ExternalDocumentation);
            SetJsonValue(json, PropertyConstants.OperationId, value.OperationIdentifier);
            SetJsonArray(json, PropertyConstants.Parameters, value.Parameters);
            SetJsonObject(json, PropertyConstants.RequestBody, value.RequestBody);
            SetJsonMap(json, PropertyConstants.Responses, responses);
            SetJsonMap(json, PropertyConstants.Callbacks, value.Callbacks);
            SetJsonFlag(json, PropertyConstants.Deprecated, value.Options, OperationOptions.Deprecated);
            SetJsonArray(json, PropertyConstants.Security, value.Security);
            SetJsonArray(json, PropertyConstants.Servers, value.Servers);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OpenIdConnectSecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OpenIdConnectSecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeOpenIdConnectSecurityScheme(OpenIdConnectSecurityScheme value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Type, EnumConstants.OpenIdConnect);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.OpenIdConnectUrl, value.Url);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OAuthFlow"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OAuthFlow"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeOAuthFlow(OAuthFlow value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.AuthorizationUrl, value.AuthorizationUrl, true);
            SetJsonValue(json, PropertyConstants.TokenUrl, value.TokenUrl, true);
            SetJsonValue(json, PropertyConstants.RefreshUrl, value.RefreshUrl);
            SetJsonMap(json, PropertyConstants.Scopes, value.Scopes);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="OAuth2SecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="OAuth2SecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeOAuth2SecurityScheme(OAuth2SecurityScheme value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Type, EnumConstants.OAuth2);
            SetJsonValue(json, PropertyConstants.Description, value.Description);

            var flows = new JsonObject();

            SetJsonObject(flows, PropertyConstants.Implicit, value.ImplicitFlow);
            SetJsonObject(flows, PropertyConstants.Password, value.PasswordFlow);
            SetJsonObject(flows, PropertyConstants.ClientCredentials, value.ClientCredentialsFlow);
            SetJsonObject(flows, PropertyConstants.AuthorizationCode, value.AuthorizationCodeFlow);

            json.Add(PropertyConstants.Flows, flows);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="MediaType"/> value.
        /// </summary>
        /// <param name="value">The <see cref="MediaType"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeMediaType(MediaType value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonObject(json, PropertyConstants.Schema, value.Schema);
            SetJsonMap(json, PropertyConstants.Examples, value.Examples);
            SetJsonMap(json, PropertyConstants.Encoding, value.Encoding);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Link"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Link"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeLink(Link value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.OperationRef, SerializeReference(value.OperationReference));
            SetJsonValue(json, PropertyConstants.OperationId, value.OperationIdentifier);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonObject(json, PropertyConstants.Server, value.Server);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="License"/> value.
        /// </summary>
        /// <param name="value">The <see cref="License"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeLicense(License value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Name, value.Name);
            SetJsonValue(json, PropertyConstants.Url, value.Url);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Information"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Information"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeInformation(Information value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Title, value.Title);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.TermsOfService, value.TermsOfService);
            SetJsonObject(json, PropertyConstants.Contact, value.Contact);
            SetJsonObject(json, PropertyConstants.License, value.License);
            SetJsonValue(json, PropertyConstants.Version, value.Version.ToString());

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="HttpSecurityScheme"/> value.
        /// </summary>
        /// <param name="value">The <see cref="HttpSecurityScheme"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeHttpSecurityScheme(HttpSecurityScheme value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Type, EnumConstants.Http);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.Scheme, value.Scheme, true);
            SetJsonValue(json, PropertyConstants.BearerFormat, value.BearerFormat);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Reference"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Reference"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeReference(Reference value)
        {
            if (!value.HasValue) return null;
            return value.ToUri()?.ToString();
        }

        /// <summary>
        /// Serializes a <see cref="IReferable"/> value.
        /// </summary>
        /// <param name="value">The <see cref="IReferable"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeReferable(IReferable value)
        {
            if (!value.HasValue) return null;
            if (value.IsReference)
            {
                var reference = Serialize(value.Reference);
                return new JsonObject()
                {
                    [PropertyConstants.Reference] = reference
                };
            }
            return SerializeUnknown(value.Value);
        }

        /// <summary>
        /// Serializes a <see cref="Contact"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Contact"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeComponents(Components value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonMap(json, PropertyConstants.Schemas, value.Schemas);
            SetJsonMap(json, PropertyConstants.Responses, value.Responses);
            SetJsonMap(json, PropertyConstants.Parameters, value.Parameters);
            SetJsonMap(json, PropertyConstants.Examples, value.Examples);
            SetJsonMap(json, PropertyConstants.RequestBodies, value.RequestBodies);
            SetJsonMap(json, PropertyConstants.Headers, value.Headers);
            SetJsonMap(json, PropertyConstants.SecuritySchemes, value.SecuritySchemes);
            SetJsonMap(json, PropertyConstants.Links, value.Links);
            SetJsonMap(json, PropertyConstants.Callbacks, value.Callbacks);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Contact"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Contact"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeContact(Contact value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Name, value.Name);
            SetJsonValue(json, PropertyConstants.Url, value.Url);
            SetJsonValue(json, PropertyConstants.Email, value.Email);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="ExternalDocumentation"/> value.
        /// </summary>
        /// <param name="value">The <see cref="ExternalDocumentation"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeExternalDocumentation(ExternalDocumentation value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.Url, value.Url, true);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Example"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Example"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeExample(Example value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Summary, value.Summary);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.ExternalValue, value.ExternalValue);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Document"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Document"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeDocument(Document value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.OpenApi, value.Version.ToString());
            SetJsonObject(json, PropertyConstants.Info, value.Info, true);
            SetJsonArray(json, PropertyConstants.Servers, value.Servers);
            SetJsonMap(json, PropertyConstants.Paths, value.Paths, true);
            SetJsonObject(json, PropertyConstants.Components, value.Components);
            SetJsonArray(json, PropertyConstants.Security, value.Security);
            SetJsonArray(json, PropertyConstants.Tags, value.Tags);
            SetJsonObject(json, PropertyConstants.ExternalDocs, value.ExternalDocumentation);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Callback"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Callback"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeCallback(Callback value)
        {
            if (value is null) return null;

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
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Type, EnumConstants.ApiKey);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.Name, value.Name);
            SetJsonValue(json, PropertyConstants.In, ToJsonValue(value.Location));

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Tag"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Tag"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeTag(Tag value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Name, value.Name);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonObject(json, PropertyConstants.ExternalDocs, value.ExternalDocumentation);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="ServerVariable"/> value.
        /// </summary>
        /// <param name="value">The <see cref="ServerVariable"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeServerVariable(ServerVariable value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonArray(json, PropertyConstants.Enum, value.Enum);
            SetJsonValue(json, PropertyConstants.Default, value.Default);
            SetJsonValue(json, PropertyConstants.Description, value.Description);

            return json;
        }

        /// <summary>
        /// Serializes a <see cref="Server"/> value.
        /// </summary>
        /// <param name="value">The <see cref="Server"/> value to serialize.</param>
        /// <returns>The <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeServer(Server value)
        {
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Url, value.Url);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonMap(json, PropertyConstants.Variables, value.Variables);

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
            if (value is null) return null;

            var json = new JsonObject();

            SetJsonValue(json, PropertyConstants.Title, value.Title);
            SetJsonValue(json, PropertyConstants.Description, value.Description);
            SetJsonValue(json, PropertyConstants.Type, ToJsonValue(value.JsonType));
            SetJsonValue(json, PropertyConstants.Format, value.Format);

            SetJsonValue(json, PropertyConstants.Pattern, value.Pattern);
            SetJsonArray(json, PropertyConstants.EnumValue, value.Enum);

            SetJsonFlag(json, PropertyConstants.Deprecated, value.Options, SchemaOptions.Deprecated);
            SetJsonFlag(json, PropertyConstants.Nullable, value.Options, SchemaOptions.Nullable);
            SetJsonFlag(json, PropertyConstants.Required, value.Options, SchemaOptions.Required);
            SetJsonFlag(json, PropertyConstants.UniqueItems, value.Options, SchemaOptions.UniqueItems);

            SetJsonArray(json, PropertyConstants.AllOf, value.AllOf);
            SetJsonArray(json, PropertyConstants.AnyOf, value.AnyOf);
            SetJsonArray(json, PropertyConstants.OneOf, value.OneOf);
            SetJsonArray(json, PropertyConstants.Not, value.Not);

            SetJsonNumber(json, PropertyConstants.Minimum, value.NumberRange.Minimum);
            if (value.NumberRange.Minimum.HasValue)
                SetJsonFlag(json, PropertyConstants.ExclusiveMinimum, value.NumberRange.RangeOptions, RangeOptions.MinimumInclusive, invert: true);

            SetJsonNumber(json, PropertyConstants.Maximum, value.NumberRange.Maximum);
            if (value.NumberRange.Maximum.HasValue)
                SetJsonFlag(json, PropertyConstants.ExclusiveMaximum, value.NumberRange.RangeOptions, RangeOptions.MaximumInclusive, invert: true);

            SetJsonNumber(json, PropertyConstants.MultipleOf, value.NumberRange.MultipleOf);

            SetJsonValue(json, PropertyConstants.MinLength, value.LengthRange.Minimum);
            SetJsonValue(json, PropertyConstants.MaxLength, value.LengthRange.Maximum);

            SetJsonValue(json, PropertyConstants.MinProperties, value.PropertiesRange.Minimum);
            SetJsonValue(json, PropertyConstants.MaxProperties, value.PropertiesRange.Maximum);

            SetJsonObject(json, PropertyConstants.Items, value.Items);
            SetJsonMap(json, PropertyConstants.Properties, value.Properties);
            SetJsonMap(json, PropertyConstants.AdditionalProperties, value.AdditionalProperties);

            SetJsonObject(json, PropertyConstants.ExternalDocs, value.ExternalDocumentation);

            return json;
        }

        /// <summary>Serializes the specified OpenAPI object to a JSON value.</summary>
        /// <typeparam name="T">The type of the OpenAPI object.</typeparam>
        /// <param name="value">The instance of the OpenAPI object.</param>
        /// <returns>The serialized value.</returns>
        public virtual JsonValue Serialize<T>(T value)
        {
            if (ReferenceEquals(value, null)) return null;

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
    }

#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null
}
