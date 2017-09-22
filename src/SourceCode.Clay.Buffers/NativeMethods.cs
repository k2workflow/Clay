using System.Runtime.InteropServices;
using System.Security;

namespace SourceCode.Clay.Buffers
{
    internal static unsafe class NativeMethods
    {
        [DllImport("msvcrt.dll", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        [SecurityCritical]
        public static extern int MemCompare(byte* x, byte* y, int count);
    }
}
