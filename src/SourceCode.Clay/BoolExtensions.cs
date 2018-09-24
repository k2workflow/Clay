#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Runtime.CompilerServices;

namespace SourceCode.Clay
{
    /// <summary>
    /// Represents <see cref="bool"/> extensions.
    /// </summary>
    public static class BoolExtensions
    {
        /// <summary>
        /// Converts a bool to a byte value without branching
        /// Uses unsafe code.
        /// </summary>
        /// <param name="on">The value to convert.</param>
        /// <returns>Returns 1 if True, else returns 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(this bool on)
        {
            /*
                Method |     Mean |     Error |    StdDev | Scaled | ScaledSD |
               ------- |---------:|----------:|----------:|-------:|---------:|
                Unsafe | 1.574 ns | 0.0118 ns | 0.0092 ns |   1.10 |     0.05 |
                  Safe | 1.575 ns | 0.0310 ns | 0.0380 ns |   1.10 |     0.05 |
                Branch | 1.435 ns | 0.0285 ns | 0.0632 ns |   1.00 |     0.00 |
            */
            unsafe
            {
                return *(byte*)&on;
            }
        }
    }
}
