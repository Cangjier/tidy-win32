using System.Runtime.InteropServices;

namespace TidyWin32;

public partial class Win32
{
    [DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr GetStockObject(int fnObject);

    [DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr CreateSolidBrush(uint crColor);

    [DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr SelectObject(IntPtr hdc, IntPtr h);

    [DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

    [DllImport("gdi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool DeleteObject(IntPtr hObject);

    public static uint RGB(byte r, byte g, byte b)
    {
        return (uint)((r << 16) | (g << 8) | b);
    }
}