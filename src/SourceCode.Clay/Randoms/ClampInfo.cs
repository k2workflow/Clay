#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SourceCode.Clay.Randoms
{
    internal sealed class ClampInfo
    {
        /// <summary>
        /// A clamp with range [0, 1].
        /// </summary>
        public static ClampInfo Default { get; } = new ClampInfo();

        public double Min { get; }

        public double Max { get; }

        public double Range { get; }

        public ClampInfo(double min, double max)
        {
            Debug.Assert(min <= max);
            Debug.Assert(!double.IsInfinity(max - min));

            Min = min;
            Max = max;
            Range = max - min;
        }

        public ClampInfo()
            : this(0, 1)
        { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double Constrain(double value)
        {
            value = Math.Max(Min, value); // Floor
            value = Math.Min(Max, value); // Ceiling

            return value;
        }
    }
}
