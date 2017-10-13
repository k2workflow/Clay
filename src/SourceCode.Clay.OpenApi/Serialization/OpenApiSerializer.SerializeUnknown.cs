#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Collections.Concurrent;
using System.Json;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace SourceCode.Clay.OpenApi.Serialization
{
    partial class OpenApiSerializer
    {
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

        #region Methods

        /// <summary>
        /// Serializes an unknown object type.
        /// </summary>
        /// <typeparam name="T">The type of the object. This may not reflect the exact object type.</typeparam>
        /// <param name="value">The object value.</param>
        /// <returns>The serialized <see cref="JsonValue"/>.</returns>
        protected virtual JsonValue SerializeUnknown<T>(T value)
        {
#pragma warning disable S1168 // Empty arrays and collections should be returned instead of null
            // Null is significant in JSON.

            if (ReferenceEquals(value, null)) return null;
#pragma warning restore S1168 // Empty arrays and collections should be returned instead of null

            var mySerializers = _mySerializers;
            if (mySerializers == null)
            {
                Thread.MemoryBarrier();
                mySerializers = _serializers.GetOrAdd(GetType().TypeHandle, th => new SerializerInfo(Type.GetTypeFromHandle(th)));
                _mySerializers = mySerializers;
            }

            return _mySerializers.Serialize(this, value);
        }

        #endregion
    }
}
