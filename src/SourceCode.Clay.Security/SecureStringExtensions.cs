#region License

// Copyright (c) K2 Workflow (SourceCode Technology Holdings Inc.). All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.

#endregion

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace SourceCode.Clay.Security
{
    /// <summary>
    /// Utility functions for <see cref="SecureString"/>.
    /// </summary>
    public static class SecureStringExtensions
    {
        /// <summary>
        /// Safely converts a <see cref="SecureString"/> to a <see cref="String"/>.
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        public static string ToClearText(this SecureString ss)
        {
            if (ss is null) return null;

            // https://blogs.msdn.microsoft.com/fpintos/2009/06/12/how-to-properly-convert-securestring-to-string/
            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.SecureStringToGlobalAllocUnicode(ss);

                return Marshal.PtrToStringUni(ptr);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }

        public static SecureString ToSecureString(this string str)
        {
            if (str is null) return null;

            var ss = new SecureString();
            for (var i = 0; i < str.Length; i++)
                ss.AppendChar(str[i]);

            ss.MakeReadOnly();
            return ss;
        }
    }
}
