using System.Runtime.InteropServices;

namespace TidyWin32;

public partial class Win32
{
    /// <summary>
    /// <para>
    /// The <c>PAINTSTRUCT</c> structure contains information for an application. This information can be used to paint the client area
    /// of a window owned by that application.
    /// </para>
    /// </summary>
    // https://docs.microsoft.com/en-us/windows/desktop/api/winuser/ns-winuser-tagpaintstruct typedef struct tagPAINTSTRUCT { HDC hdc;
    // BOOL fErase; RECT rcPaint; BOOL fRestore; BOOL fIncUpdate; BYTE rgbReserved[32]; } PAINTSTRUCT, *PPAINTSTRUCT, *NPPAINTSTRUCT, *LPPAINTSTRUCT;
    [StructLayout(LayoutKind.Sequential)]
    public struct PAINTSTRUCT
    {
        /// <summary>
        /// <para>A handle to the display DC to be used for painting.</para>
        /// </summary>
        public IntPtr hdc;

        /// <summary>
        /// <para>
        /// Indicates whether the background must be erased. This value is nonzero if the application should erase the background. The
        /// application is responsible for erasing the background if a window class is created without a background brush. For more
        /// information, see the description of the <c>hbrBackground</c> member of the WNDCLASS structure.
        /// </para>
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool fErase;

        /// <summary>
        /// <para>
        /// A RECT structure that specifies the upper left and lower right corners of the rectangle in which the painting is requested,
        /// in device units relative to the upper-left corner of the client area.
        /// </para>
        /// </summary>
        public RECT rcPaint;

        /// <summary>
        /// <para>Reserved; used internally by the system.</para>
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool fRestore;

        /// <summary>
        /// <para>Reserved; used internally by the system.</para>
        /// </summary>
        [MarshalAs(UnmanagedType.Bool)]
        public bool fIncUpdate;

        /// <summary>
        /// <para>Reserved; used internally by the system.</para>
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] rgbReserved;
    }
}