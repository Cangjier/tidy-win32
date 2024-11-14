namespace TidyWin32;

public partial class Win32
{
    public class MouseInterface
    {
        public static void Move(int deltaX, int deltaY)
        {
            INPUT mouseInput = new INPUT();
            mouseInput.type = INPUT_MOUSE;
            mouseInput.U.mi.dx = deltaX;
            mouseInput.U.mi.dy = deltaY;
            mouseInput.U.mi.mouseData = 0;
            mouseInput.U.mi.dwFlags = MOUSEEVENTF_MOVE;
            mouseInput.U.mi.time = 0;
            mouseInput.U.mi.dwExtraInfo = IntPtr.Zero;

            SendInput(1, [mouseInput], INPUT.Size);
        }

        public static void MoveTo(int x, int y)
        {
            int screenWidth = GetSystemMetrics(SM_CXSCREEN);
            int screenHeight = GetSystemMetrics(SM_CYSCREEN);
            x = x * 65535 / screenWidth;
            y = y * 65535 / screenHeight;
            INPUT mouseInput = new INPUT();
            mouseInput.type = INPUT_MOUSE;
            mouseInput.U.mi.dx = x;
            mouseInput.U.mi.dy = y;
            mouseInput.U.mi.mouseData = 0;
            mouseInput.U.mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
            mouseInput.U.mi.time = 0;
            mouseInput.U.mi.dwExtraInfo = IntPtr.Zero;

            SendInput(1, [mouseInput], INPUT.Size);
        }

        public static void Click()
        {
            INPUT[] inputs = new INPUT[2];

            inputs[0].type = INPUT_MOUSE;
            inputs[0].U.mi.dx = 0;
            inputs[0].U.mi.dy = 0;
            inputs[0].U.mi.mouseData = 0;
            inputs[0].U.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
            inputs[0].U.mi.time = 0;
            inputs[0].U.mi.dwExtraInfo = IntPtr.Zero;

            inputs[1].type = INPUT_MOUSE;
            inputs[1].U.mi.dx = 0;
            inputs[1].U.mi.dy = 0;
            inputs[1].U.mi.mouseData = 0;
            inputs[1].U.mi.dwFlags = MOUSEEVENTF_LEFTUP;
            inputs[1].U.mi.time = 0;
            inputs[1].U.mi.dwExtraInfo = IntPtr.Zero;

            SendInput(2, inputs, INPUT.Size);
        }

        public static void RightClick()
        {
            INPUT[] inputs = new INPUT[2];

            inputs[0].type = INPUT_MOUSE;
            inputs[0].U.mi.dx = 0;
            inputs[0].U.mi.dy = 0;
            inputs[0].U.mi.mouseData = 0;
            inputs[0].U.mi.dwFlags = MOUSEEVENTF_RIGHTDOWN;
            inputs[0].U.mi.time = 0;
            inputs[0].U.mi.dwExtraInfo = IntPtr.Zero;

            inputs[1].type = INPUT_MOUSE;
            inputs[1].U.mi.dx = 0;
            inputs[1].U.mi.dy = 0;
            inputs[1].U.mi.mouseData = 0;
            inputs[1].U.mi.dwFlags = MOUSEEVENTF_RIGHTUP;
            inputs[1].U.mi.time = 0;
            inputs[1].U.mi.dwExtraInfo = IntPtr.Zero;

            SendInput(2, inputs, INPUT.Size);
        }
    }

}