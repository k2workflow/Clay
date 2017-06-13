using System;
using System.Collections.Generic;

namespace SourceCode.Clay.Collections.Generic
{
    public static partial class DictionaryExtensions
    {
        /// <summary>
        /// Builds a dynamic switch with <see cref="System.String"/> keys.
        /// Uses ordinal string comparison semantics.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="cases">The items to convert into a dynamic switch statement.</param>
        /// <param name="ignoreCase">Invariant lowercase (ordinal) comparisons should be used.</param>
        /// <returns>The compiled switch statement.</returns>
        public static IDynamicSwitch<string, T> ToOrdinalSwitch<T>(IReadOnlyDictionary<string, T> cases, bool ignoreCase)
        {
            var impl = new OrdinalStringSwitchImpl<T>(cases, ignoreCase);
            return impl;
        }

        #region Helpers

        internal sealed class OrdinalStringSwitchImpl<T> : BaseSwitchImpl<string, T>
        {
            //private readonly bool _ignoreCase;

            public OrdinalStringSwitchImpl(IReadOnlyDictionary<string, T> cases, bool ignoreCase)
                : base(cases, ignoreCase ? k => k.ToLowerInvariant() : (Func<string, string>)null)
            {
                //_ignoreCase = ignoreCase;
            }

            /*
            protected override Func<string, int> Build(IReadOnlyDictionary<string, int> cases)
            {
                // Return -1 if item is not found (per standard convention for IndexOf())
                var notFound = Expression.Constant(-1);

                // Exit early if no items
                var count = cases?.Count ?? 0;
                if (count == 0)
                {
                    var noItems = Expression.Lambda<Func<string, int>>(notFound);
                    return noItems.Compile();
                }

                // Define formal parameter
                var formalParam = Expression.Parameter(typeof(string), "key");

                // Format MUST match #1 below
                Expression switchValue = formalParam;
                if (_ignoreCase)
                {
                    switchValue = Expression.Call(formalParam, nameof(string.ToLowerInvariant), null);
                }

                // Create <Key, SwitchCase>[] list
                var i = 0;
                var switchCases = new SwitchCase[count];
                foreach (var @case in cases)
                {
                    // Get Key
                    var key = @case.Key;

                    // Format MUST match #1 above
                    if (_ignoreCase)
                    {
                        key = key.ToLowerInvariant();
                    }

                    // Create Case Expression
                    var body = Expression.Constant(@case.Value, typeof(T));
                    switchCases[i++] = Expression.SwitchCase(body, Expression.Constant(key));
                }

                // Create Switch Expression
                var switchExpr = Expression.Switch(switchValue, notFound, switchCases);

                // Compile Lambda
                var lambda = Expression.Lambda<Func<string, int>>(switchExpr, formalParam);
                return lambda.Compile();
            }
            */
        }

        #endregion
    }
}
