using System.Runtime.InteropServices;

namespace TidyWin32;

public partial class Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public int Left
        {
            get=>left;
            set=>left=value;
        }

        public int Top
        {
            get=>top;
            set=>top=value;
        }

        public int Right
        {
            get=>right;
            set=>right=value;
        }

        public int Bottom
        {
            get=>bottom;
            set=>bottom=value;
        }

        public int Width => right - left;

        public int Height => bottom - top;
    }
}