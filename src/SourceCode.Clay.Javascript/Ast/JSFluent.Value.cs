using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Javascript.Ast
{
    partial class JSFluent
    {
        private static readonly Type TypeofString = typeof(string);
        private static readonly Type TypeofObject = typeof(object);
        private static readonly Type TypeofJSIdentifier = typeof(JSIdentifier);
        private static readonly Type TypeofJSExpression = typeof(JSExpression);
        private static readonly Type TypeofJSObjectExpression = typeof(JSObjectExpression);
        private static readonly Type TypeofJSLiteral = typeof(JSLiteral);
        private static readonly Type TypeofProperty = typeof(Discriminated<JSLiteral, JSIdentifier>);
        private static readonly MethodInfo MethodOfJSObjectExpression_Property = TypeofJSObjectExpression.GetMethod(nameof(JSObjectExpression.Add), BindingFlags.Public | BindingFlags.Instance, null, new[] { TypeofProperty, TypeofJSExpression }, null);
        private static readonly ConstructorInfo ConstructorOfProperty_Identifier = TypeofProperty.GetConstructor(new[] { TypeofJSIdentifier });
        private static readonly ConstructorInfo ConstructorOfJSIdentifier = TypeofJSIdentifier.GetConstructor(new[] { TypeofString });
        private static readonly ConstructorInfo ConstructorOfJSLiteral = TypeofJSLiteral.GetConstructor(new[] { TypeofObject });

        private static readonly ConcurrentDictionary<RuntimeTypeHandle, Func<object, JSExpression>> _factories = new ConcurrentDictionary<RuntimeTypeHandle, Func<object, JSExpression>>();
        private static readonly Dictionary<RuntimeTypeHandle, Func<Type, Expression<Func<object, JSExpression>>>> _typeFactories = new Dictionary<RuntimeTypeHandle, Func<Type, Expression<Func<object, JSExpression>>>>()
        {
            { typeof(JSExpression).TypeHandle, CreateJSExpressionExpression },
            { typeof(string).TypeHandle, CreateJSLiteralExpression },
            { typeof(int).TypeHandle, CreateJSLiteralExpression },
            { typeof(long).TypeHandle, CreateJSLiteralExpression },
            { typeof(bool).TypeHandle, CreateJSLiteralExpression },
        };

        public static JSExpression JSValue<T>(T value)
        {
            if (Equals(value, default(T))) // TODO: ReferenceEquals
                return JSNull();
            if (value is JSExpression expression) return expression;
            var rth = typeof(T) == typeof(object)
                ? Type.GetTypeHandle(value)
                : typeof(T).TypeHandle;
            var factory = _factories.GetOrAdd(rth, CreateFactory);
            if (factory is null) throw new NotSupportedException();
            return factory(value);
        }

        private static Func<object, JSExpression> CreateFactory(RuntimeTypeHandle typeHandle)
        {
            var type = Type.GetTypeFromHandle(typeHandle);

            var factory = CreateAnyExpression(type);
            if (factory is null) return default;

            return factory.Compile();
        }

        private static Expression<Func<object, JSExpression>> CreateAnyExpression(Type propertyType)
        {
            return TypeofJSExpression.IsAssignableFrom(propertyType)
                ? CreateJSExpressionExpression(propertyType)
                : _typeFactories.TryGetValue(propertyType.TypeHandle, out var factoryFactory)
                    ? factoryFactory(propertyType)
                    : CreateFactoryExpression(propertyType);
        }

        private static Expression<Func<object, JSExpression>> CreateFactoryExpression(Type type)
        {
            if (type.GetCustomAttribute<CompilerGeneratedAttribute>(false) is null) return default;

            var param = Expression.Parameter(TypeofObject, "value");
            var convertedParam = Expression.Convert(param, type);

            var variable = Expression.Variable(TypeofJSObjectExpression);
            var result = (Expression)Expression.New(TypeofJSObjectExpression);

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var propertyType = prop.PropertyType;
                var accessor = Expression.Convert(Expression.Property(convertedParam, prop), TypeofObject);
                var identifier = Expression.Convert(Expression.New(ConstructorOfJSIdentifier, Expression.Constant(prop.Name)), TypeofJSExpression);
                var descrim = Expression.New(ConstructorOfProperty_Identifier, identifier);

                var factory = CreateAnyExpression(propertyType);
                if (factory is null) return null;

                var invoked = Expression.Invoke(factory, accessor);
                result = Expression.Call(result, MethodOfJSObjectExpression_Property, descrim, invoked);
            }

            result = Expression.Convert(result, TypeofJSExpression);
            return Expression.Lambda<Func<object, JSExpression>>(result, param);
        }

        private static Expression<Func<object, JSExpression>> CreateJSExpressionExpression(Type type)
        {
            var param = Expression.Parameter(TypeofObject, "value");
            var result = Expression.Convert(param, TypeofJSExpression);
            return Expression.Lambda<Func<object, JSExpression>>(result, param);
        }

        private static Expression<Func<object, JSExpression>> CreateJSLiteralExpression(Type type)
        {
            var param = Expression.Parameter(TypeofObject, "value");
            var constant = Expression.New(ConstructorOfJSLiteral, param);
            var result = Expression.Convert(constant, TypeofJSExpression);
            return Expression.Lambda<Func<object, JSExpression>>(result, param);
        }
    }
}
