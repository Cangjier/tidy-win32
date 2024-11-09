using System.Runtime.InteropServices;

namespace TidyWin32;
public partial class Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }
    }
}
