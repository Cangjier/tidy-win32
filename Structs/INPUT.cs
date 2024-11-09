using System.Runtime.InteropServices;

namespace TidyWin32;
public partial class Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        public uint type;
        public InputUnion U;
        public static int Size => Marshal.SizeOf(typeof(INPUT));
    }
}