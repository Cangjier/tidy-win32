namespace TidyWin32;

public partial class Win32
{
    public class MouseInterface
    {
        public static void Move(int deltaX, int deltaY)
        {
            INPUT mouseInput = new INPUT();
            mouseInput.type = INPUT_MOUSE;
            mouseInput.u.mi.dx = deltaX;
            mouseInput.u.mi.dy = deltaY;
            mouseInput.u.mi.mouseData = 0;
            mouseInput.u.mi.dwFlags = MOUSEEVENTF_MOVE;
            mouseInput.u.mi.time = 0;
            mouseInput.u.mi.dwExtraInfo = IntPtr.Zero;

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
            mouseInput.u.mi.dx = x;
            mouseInput.u.mi.dy = y;
            mouseInput.u.mi.mouseData = 0;
            mouseInput.u.mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;
            mouseInput.u.mi.time = 0;
            mouseInput.u.mi.dwExtraInfo = IntPtr.Zero;

            SendInput(1, [mouseInput], INPUT.Size);
        }

        public static void Click()
        {
            INPUT[] inputs = new INPUT[2];

            inputs[0].type = INPUT_MOUSE;
            inputs[0].u.mi.dx = 0;
            inputs[0].u.mi.dy = 0;
            inputs[0].u.mi.mouseData = 0;
            inputs[0].u.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
            inputs[0].u.mi.time = 0;
            inputs[0].u.mi.dwExtraInfo = IntPtr.Zero;

            inputs[1].type = INPUT_MOUSE;
            inputs[1].u.mi.dx = 0;
            inputs[1].u.mi.dy = 0;
            inputs[1].u.mi.mouseData = 0;
            inputs[1].u.mi.dwFlags = MOUSEEVENTF_LEFTUP;
            inputs[1].u.mi.time = 0;
            inputs[1].u.mi.dwExtraInfo = IntPtr.Zero;

            SendInput(2, inputs, INPUT.Size);
        }

        public static void Click2()
        {
            // 鼠标左键按下
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);

            // 鼠标左键弹起
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        public static void RightClick()
        {
            INPUT[] inputs = new INPUT[2];

            inputs[0].type = INPUT_MOUSE;
            inputs[0].u.mi.dx = 0;
            inputs[0].u.mi.dy = 0;
            inputs[0].u.mi.mouseData = 0;
            inputs[0].u.mi.dwFlags = MOUSEEVENTF_RIGHTDOWN;
            inputs[0].u.mi.time = 0;
            inputs[0].u.mi.dwExtraInfo = IntPtr.Zero;

            inputs[1].type = INPUT_MOUSE;
            inputs[1].u.mi.dx = 0;
            inputs[1].u.mi.dy = 0;
            inputs[1].u.mi.mouseData = 0;
            inputs[1].u.mi.dwFlags = MOUSEEVENTF_RIGHTUP;
            inputs[1].u.mi.time = 0;
            inputs[1].u.mi.dwExtraInfo = IntPtr.Zero;

            SendInput(2, inputs, INPUT.Size);
        }
    }

}