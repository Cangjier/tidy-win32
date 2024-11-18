using System.Runtime.InteropServices;

namespace TidyWin32;

public partial class Win32
{
    public enum Keys : ushort
    {
        // 数字键
        D0 = 0x30,
        D1 = 0x31,
        D2 = 0x32,
        D3 = 0x33,
        D4 = 0x34,
        D5 = 0x35,
        D6 = 0x36,
        D7 = 0x37,
        D8 = 0x38,
        D9 = 0x39,

        // 字母键
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,

        // 功能键
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,

        // 控制键
        Escape = 0x1B,
        Tab = 0x09,
        CapsLock = 0x14,
        Shift = 0x10,
        Control = 0x11,
        Alt = 0x12,
        Space = 0x20,
        Enter = 0x0D,
        Backspace = 0x08,

        // 箭头键
        Left = 0x25,
        Up = 0x26,
        Right = 0x27,
        Down = 0x28,

        // 数字小键盘键
        NumPad0 = 0x60,
        NumPad1 = 0x61,
        NumPad2 = 0x62,
        NumPad3 = 0x63,
        NumPad4 = 0x64,
        NumPad5 = 0x65,
        NumPad6 = 0x66,
        NumPad7 = 0x67,
        NumPad8 = 0x68,
        NumPad9 = 0x69,
        Multiply = 0x6A,
        Add = 0x6B,
        Subtract = 0x6D,
        Decimal = 0x6E,
        Divide = 0x6F,

        // 特殊键
        Insert = 0x2D,
        Delete = 0x2E,
        Home = 0x24,
        End = 0x23,
        PageUp = 0x21,
        PageDown = 0x22,

        // 系统键
        LWin = 0x5B, // 左Win键
        RWin = 0x5C, // 右Win键
        Applications = 0x5D, // 应用键
        Sleep = 0x5F, // 睡眠键

        // 浏览器键
        BrowserBack = 0xA6,
        BrowserForward = 0xA7,
        BrowserRefresh = 0xA8,
        BrowserStop = 0xA9,
        BrowserSearch = 0xAA,
        BrowserFavorites = 0xAB,
        BrowserHome = 0xAC,

        // 媒体键
        VolumeMute = 0xAD,
        VolumeDown = 0xAE,
        VolumeUp = 0xAF,
        MediaNextTrack = 0xB0,
        MediaPrevTrack = 0xB1,
        MediaStop = 0xB2,
        MediaPlayPause = 0xB3,

        // 数字锁定
        NumLock = 0x90,
        ScrollLock = 0x91,

        // OEM键（区域特定）
        OEM1 = 0xBA, // ";" 键
        OEMPlus = 0xBB, // "+" 键
        OEMComma = 0xBC, // "," 键
        OEMMinus = 0xBD, // "-" 键
        OEMPeriod = 0xBE, // "." 键
        OEM2 = 0xBF, // "/?" 键
        OEM3 = 0xC0, // "`~" 键
        OEM4 = 0xDB, // "[{" 键
        OEM5 = 0xDC, // "\|" 键
        OEM6 = 0xDD, // "]}" 键
        OEM7 = 0xDE, // "'\"" 键
    }

    /// <summary>
    /// 键盘接口
    /// </summary>
    public class KeyboardInterface
    {
        public static bool SendKey(Keys key)
        {
            // 模拟按下方向下键
            INPUT[] inputs = new INPUT[2];

            // 按下方向下键
            inputs[0].type = INPUT_KEYBOARD;
            inputs[0].u.ki.wVk = (ushort)key;
            inputs[0].u.ki.wScan = 0;
            inputs[0].u.ki.dwFlags = 0;
            inputs[0].u.ki.time = 0;
            inputs[0].u.ki.dwExtraInfo = IntPtr.Zero;

            // 松开方向下键
            inputs[1].type = INPUT_KEYBOARD;
            inputs[1].u.ki.wVk = (ushort)key;
            inputs[1].u.ki.wScan = 0;
            inputs[1].u.ki.dwFlags = KEYEVENTF_KEYUP;
            inputs[1].u.ki.time = 0;
            inputs[1].u.ki.dwExtraInfo = IntPtr.Zero;

            // 发送输入
            uint result = SendInput((uint)inputs.Length, inputs, Marshal.SizeOf(typeof(INPUT)));
            if (result == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

}
