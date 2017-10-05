#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System.Runtime.InteropServices;
using System.Security;

namespace SourceCode.Clay.Buffers
{
    internal static unsafe class NativeMethods
    {
        #region Methods

        [DllImport("msvcrt.dll", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        [SecurityCritical]
        public static extern int MemCompare(byte* x, byte* y, int count);

        #endregion
    }
}
