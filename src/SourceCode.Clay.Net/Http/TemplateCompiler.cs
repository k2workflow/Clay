using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using static System.FormattableString;

namespace SourceCode.Clay.Net.Http
{
    internal static class TemplateCompiler
    {
        private readonly struct Context
        {
            public readonly Expression Target;
            public readonly List<Expression> Statements;
            public readonly List<ParameterExpression> Variables;

            public Context(Expression target, List<Expression> statements, List<ParameterExpression> variables)
            {
                Target = target;
                Statements = statements;
                Variables = variables;
            }

            public Context Add(Expression statement)
            {
                Statements.Add(statement);
                return this;
            }

            public ParameterExpression Add(Type type, string name)
            {
                ParameterExpression param = Expression.Parameter(type, name);
                Variables.Add(param);
                return param;
            }

            public Context With(List<Expression> statements) => new Context(Target, statements, Variables);

            public Context With(Expression target) => new Context(target, Statements, Variables);
        }

#pragma warning disable IDE1006 // Naming Styles
        #region Reflected
        private static readonly Type TypeofIDisposable = typeof(IDisposable);
        private static readonly MethodInfo MethodofIDisposable_Dispose = TypeofIDisposable.GetMethod(
            nameof(IDisposable.Dispose));

        private static readonly Type TypeofNullable = typeof(Nullable<>);

        private static readonly Type TypeofIEnumerableOfT = typeof(IEnumerable<>);

        private static readonly Type TypeofIEnumerator = typeof(System.Collections.IEnumerator);
        private static readonly MethodInfo MethodofIEnumerator_MoveNext = TypeofIEnumerator.GetMethod(
            nameof(System.Collections.IEnumerator.MoveNext));

        private static readonly Type TypeofEnum = typeof(Enum);
        private static readonly MethodInfo MethodofEnum_HasFlag = TypeofEnum.GetMethod(
            nameof(Enum.HasFlag));

        private static readonly Type TypeofUrlBuilder = typeof(UrlBuilder);
        private static readonly ConstructorInfo ConstructorOfUrlBuilder = TypeofUrlBuilder.GetConstructor(
            new[] { typeof(int) });
        private static readonly MethodInfo MethodofUrlBuilder_Append = TypeofUrlBuilder.GetMethod(
            nameof(UrlBuilder.Append), new[] { typeof(string) }, null);
        private static readonly MethodInfo MethodofUrlBuilder_StartParameter = TypeofUrlBuilder.GetMethod(
            nameof(UrlBuilder.StartParameter), Type.EmptyTypes, null);
        private static readonly MethodInfo MethodofUrlBuilder_StartValue = TypeofUrlBuilder.GetMethod(
            nameof(UrlBuilder.StartValue), Type.EmptyTypes, null);
        private static readonly MethodInfo MethodofUrlBuilder_ToString = TypeofUrlBuilder.GetMethod(
            nameof(StringBuilder.ToString), Type.EmptyTypes, null);

        private static readonly Type TypeofTemplateCompiler = typeof(TemplateCompiler);
        private static readonly MethodInfo MethodOfTemplateCompiler_ToStringFormattable = TypeofTemplateCompiler.GetMethods(
            BindingFlags.NonPublic | BindingFlags.Static).First(x => x.Name == nameof(ToString) && x.GetParameters().Length == 3);
        private static readonly MethodInfo MethodOfTemplateCompiler_ToString = TypeofTemplateCompiler.GetMethods(
            BindingFlags.NonPublic | BindingFlags.Static).First(x => x.Name == nameof(ToString) && x.GetParameters().Length == 2);
        private static readonly MethodInfo MethodOfTemplateCompiler_NullableToStringFormattable = TypeofTemplateCompiler.GetMethods(
            BindingFlags.NonPublic | BindingFlags.Static).First(x => x.Name == nameof(NullableToString) && x.GetParameters().Length == 3);
        private static readonly MethodInfo MethodOfTemplateCompiler_NullableToString = TypeofTemplateCompiler.GetMethods(
            BindingFlags.NonPublic | BindingFlags.Static).First(x => x.Name == nameof(NullableToString) && x.GetParameters().Length == 2);

        private static readonly Type TypeofIFormattable = typeof(IFormattable);
        #endregion

        #region Constants
        private static readonly ParameterExpression QueryBuilder = Expression.Variable(TypeofUrlBuilder, "q");
        private static readonly ConstantExpression Null = Expression.Constant(null);
        #endregion
#pragma warning restore IDE1006 // Naming Styles 

        private const string QueryStart = "?";
        private const string QueryAssign = "=";
        private const string QuerySeparator = "&";

        public static Func<object, string> Compile(RawUriTemplate raw, RuntimeTypeHandle typeHandle)
        {
            ParameterExpression param = Expression.Parameter(typeof(object), "value");
            BlockExpression body = Compile(raw, Expression.Convert(param, Type.GetTypeFromHandle(typeHandle)));
            var lambda = Expression.Lambda<Func<object, string>>(body, param);
            return lambda.Compile();
        }

        public static Func<T, string> Compile<T>(RawUriTemplate raw)
        {
            ParameterExpression param = Expression.Parameter(typeof(T), "value");
            BlockExpression body = Compile(raw, param);
            var lambda = Expression.Lambda<Func<T, string>>(body, param);
            return lambda.Compile();
        }

        private static BlockExpression Compile(RawUriTemplate raw, Expression value)
        {
            var context = new Context(
                target: value,
                statements: new List<Expression>
                {
                    Expression.Assign(QueryBuilder, Expression.New(ConstructorOfUrlBuilder, raw.LengthEstimateConstant))
                },
                variables: new List<ParameterExpression>
                {
                    QueryBuilder
                });

            for (var i = 0; i < raw.Path.Count; i++)
                AppendToken(context, raw.Path[i]);

            for (var i = 0; i < raw.Query.Count; i++)
                AppendQuery(context, raw.Query[i]);

            context.Add(Expression.Call(QueryBuilder, MethodofUrlBuilder_ToString));
            BlockExpression body = Expression.Block(context.Variables, context.Statements);
            return body;
        }

        private static void AppendQuery(Context context, UriQuery query)
        {
            if (query.Value.Count == 1 && query.Value[0].Type != UriTokenType.Literal)
            {
                UriToken value = query.Value[0];
                MemberExpression prop = Expression.PropertyOrField(context.Target, value.Name);
                Type propType = Nullable(prop.Type);

                if (value.Type == UriTokenType.Collection)
                {
                    MethodInfo getEnumerator = Enumerator(propType);
                    if (getEnumerator != null)
                        AppendCollection(context, prop, getEnumerator, query.Name, value);
                    else
                        throw new InvalidOperationException(Invariant($"An enumerable was expected for {value.Name}."));
                    return;
                }

                if (value.SubName != null)
                {
                    if (propType.IsEnum)
                        AppendEnum(context, prop, query.Name, value);
                    else
                        throw new InvalidOperationException(Invariant($"An enum was expected for {value.Name}."));
                    return;
                }

                if (value.Default == null)
                {
                    AppendNullable(context, prop, query.Name, value);
                    return;
                }
            }

            context.Add(Expression.Call(QueryBuilder, MethodofUrlBuilder_StartParameter));

            for (var i = 0; i < query.Name.Count; i++)
                AppendToken(context, query.Name[i]);

            context.Add(Expression.Call(QueryBuilder, MethodofUrlBuilder_StartValue));
            for (var i = 0; i < query.Value.Count; i++)
                AppendToken(context, query.Value[i]);
        }

        private static void AppendNullable(Context context, MemberExpression prop, IReadOnlyList<UriToken> name, UriToken value)
        {
            List<Expression> statements = StartQueryParameter(context, name);
            AppendValue(context.With(statements), prop, value);

            context.Add(IsNull(prop, Expression.Block(statements)));
        }

        private static void AppendEnum(Context context, MemberExpression prop, IReadOnlyList<UriToken> name, UriToken value)
        {
            Expression nonNull = Nullable(prop);
            var isFlags = nonNull.Type.GetCustomAttribute<FlagsAttribute>() != null;
            ConstantExpression enumMember = Expression.Constant(Enum.Parse(nonNull.Type, value.SubName));
            List<Expression> statements = StartQueryParameter(context, name);

            if (value.Default != null)
                statements.Add(Expression.Call(QueryBuilder, MethodofUrlBuilder_Append, value.DefaultConstant));

            Expression condition = isFlags
                ? (Expression)Expression.Call(enumMember, MethodofEnum_HasFlag, Expression.Convert(enumMember, TypeofEnum))
                : Expression.Equal(nonNull, enumMember);

            condition = Expression.IfThen(condition, Expression.Block(statements));

            context.Add(IsNull(prop, condition));
        }

        private static void AppendCollection(Context context, MemberExpression prop, MethodInfo getEnumerator, IReadOnlyList<UriToken> name, UriToken value)
        {
            Expression nonNull = Nullable(prop);
            MethodCallExpression createEnumerator = Expression.Call(nonNull, getEnumerator, null);
            ParameterExpression enumerator = context.Add(createEnumerator.Type, "e");
            MemberExpression current = Expression.Property(enumerator, nameof(System.Collections.IEnumerator.Current));
            MethodCallExpression moveNext = Expression.Call(enumerator, MethodofIEnumerator_MoveNext, null);
            List<Expression> statements = StartQueryParameter(context, name);

            AppendValue(context.With(statements), current, value);

            LabelTarget brk = Expression.Label("Exit " + prop.Member.Name);
            LoopExpression loop = Expression.Loop(
                Expression.IfThenElse(
                    moveNext,
                    Expression.Block(statements),
                    Expression.Break(brk)),
                brk
            );

            TryExpression t = Expression.TryFinally(
                Expression.Block(
                    Expression.Assign(enumerator, createEnumerator),
                    loop),
                Expression.Call(enumerator, MethodofIDisposable_Dispose)
            );

            context.Add(IsNull(prop, t));
        }

        private static List<Expression> StartQueryParameter(Context context, IReadOnlyList<UriToken> name)
        {
            var statements = new List<Expression>
            {
                Expression.Call(QueryBuilder, MethodofUrlBuilder_StartParameter)
            };

            for (var i = 0; i < name.Count; i++)
                AppendToken(context.With(statements), name[i]);

            statements.Add(Expression.Call(QueryBuilder, MethodofUrlBuilder_StartValue));
            return statements;
        }

        private static void AppendToken(Context context, UriToken token)
        {
            if (token.Type == UriTokenType.Literal)
                context.Add(Expression.Call(QueryBuilder, MethodofUrlBuilder_Append, token.DefaultConstant));
            else
            {
                Expression prop = Expression.PropertyOrField(context.Target, token.Name);
                AppendValue(context, prop, token);
            }
        }

        private static void AppendValue(Context context, Expression prop, UriToken token)
        {
            Type actualType = Nullable(prop.Type);
            MethodInfo formattableMethod = actualType == prop.Type
                ? MethodOfTemplateCompiler_ToStringFormattable
                : MethodOfTemplateCompiler_NullableToStringFormattable;
            MethodInfo toStringMethod = actualType == prop.Type
                ? MethodOfTemplateCompiler_ToString
                : MethodOfTemplateCompiler_NullableToString;

            Expression toString;
            if (TypeofIFormattable.IsAssignableFrom(actualType))
            {
                MethodInfo method = formattableMethod.MakeGenericMethod(actualType);
                toString = Expression.Call(null, method, prop, token.FormatConstant, token.DefaultConstant);
            }
            else
            {
                MethodInfo method = toStringMethod.MakeGenericMethod(actualType);
                toString = Expression.Call(null, method, prop, token.DefaultConstant);
            }
            toString = Expression.Call(QueryBuilder, MethodofUrlBuilder_Append, toString);
            context.Add(toString);
        }

        private static Expression IsNull(Expression value, Expression body)
        {
            if (value.Type.IsValueType && value.Type.IsGenericType && value.Type.GetGenericTypeDefinition() == TypeofNullable)
                return Expression.IfThen(Expression.Property(value, nameof(Nullable<int>.HasValue)), body);
            else if (!value.Type.IsValueType)
                return Expression.IfThen(Expression.ReferenceNotEqual(value, Null), body);
            return body;
        }

        private static Type Nullable(Type type)
        {
            if (type.IsValueType && type.IsGenericType && type.GetGenericTypeDefinition() == TypeofNullable)
                return type.GetGenericArguments()[0];
            return type;
        }

        private static Expression Nullable(Expression value)
        {
            if (value.Type.IsValueType && value.Type.IsGenericType && value.Type.GetGenericTypeDefinition() == TypeofNullable)
                return Expression.Property(value, nameof(Nullable<int>.Value));
            return value;
        }

        private static MethodInfo Enumerator(Type t)
        {
            MethodInfo mi = t.GetMethod(nameof(IEnumerable<int>.GetEnumerator), BindingFlags.Public | BindingFlags.Instance);
            if (mi == null || mi.ReturnType == typeof(System.Collections.IEnumerator))
            {
                Type iface = t.GetInterfaces().FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == TypeofIEnumerableOfT);
                if (iface != null)
                    mi = iface.GetMethod(nameof(IEnumerable<int>.GetEnumerator), BindingFlags.Public | BindingFlags.Instance);
            }
            return mi;
        }

        private static string NullableToString<T>(T? value, string formatString, string @default)
            where T : struct, IFormattable
            => value.HasValue
            ? Uri.EscapeDataString(value.Value.ToString(formatString, CultureInfo.InvariantCulture))
            : @default;

        private static string NullableToString<T>(T? value, string @default)
            where T : struct
            => value.HasValue
            ? Uri.EscapeDataString(value.Value.ToString())
            : @default;

        private static string ToString<T>(T value, string formatString, string @default)
            where T : IFormattable
            => !(value is null)
            ? Uri.EscapeDataString(value.ToString(formatString, CultureInfo.InvariantCulture))
            : @default;

        private static string ToString<T>(T value, string @default)
            => !(value is null)
            ? Uri.EscapeDataString(value.ToString())
            : @default;
    }
}
