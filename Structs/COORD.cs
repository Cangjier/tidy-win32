using System.Runtime.InteropServices;

namespace TidyWin32;
public partial class Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
    {
        public short X;
        public short Y;

        public COORD(short x, short y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}
