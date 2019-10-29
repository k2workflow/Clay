using System;
using System.Collections.Generic;
using System.Text;

namespace SourceCode.Clay.AspNetCore.Middleware.Correlation
{
    internal static class Deconstructors
    {
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }
    }
}
