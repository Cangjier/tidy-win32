namespace TidyWin32;

public partial class Win32
{
    public class ProgressWidget : Widget
    {
        public ProgressWidget()
        {
            RemoveBorder();
            TopMost = true;
            Size = new Size(200, 30);
            CenterWindowOnScreen();
        }

        public double Value { get; set; }

        public override void OnPaint()
        {
            try
            {
                var hdc = GetDC(hWnd);
                Assert(hdc != IntPtr.Zero);

                RECT rect;
                Assert(GetClientRect(hWnd, out rect));
                int progressWidth = (int)((rect.right - rect.left) * Value);
                rect.right = rect.left + progressWidth;
                var hbrush = CreateSolidBrush(RGB(0, 128, 0));
                Assert(hbrush != IntPtr.Zero);
                var oldObject = SelectObject(hdc, hbrush);
                FillRect(hdc, ref rect, hbrush);
                SelectObject(hdc, oldObject);
                DeleteObject(hbrush);
                ReleaseDC(hWnd, hdc);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}