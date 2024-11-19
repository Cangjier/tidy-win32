using System.Runtime.InteropServices;

namespace TidyWin32;
public partial class Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct TCITEM
    {
        public uint mask;
        public int dwState;
        public int dwStateMask;
        public IntPtr pszText;
        public int cchTextMax;
        public int iImage;
        public IntPtr lParam;
    }
}