using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace TidyWin32;

public partial class Win32
{
    public static bool Assert(bool condition)
    {
        if (!condition)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }
        return condition;
    }

    public static IntPtr HWND_TOPMOST = (IntPtr)(-1);
    public static IntPtr HWND_NOTOPMOST = (IntPtr)(-2);

    public static string GetConsoleOutput()
    {
        // 获取控制台输出的句柄  
        IntPtr hConsole = GetStdHandle(-11); // -11 是标准输出的句柄值 (STDOUT)  
        CONSOLE_SCREEN_BUFFER_INFO csbi;

        // 获取控制台缓冲区信息  
        if (GetConsoleScreenBufferInfo(hConsole, out csbi))
        {
            int bufferWidth = csbi.dwSize.X;
            int bufferHeight = csbi.dwSize.Y;
            uint charsRead = 0;

            // 创建一个足够大的StringBuilder来保存缓冲区内容  
            StringBuilder sb = new StringBuilder((int)(bufferWidth * bufferHeight));

            // 逐行读取控制台缓冲区内容  
            for (int y = 0; y < bufferHeight; y++)
            {
                COORD coord = new COORD(0, (short)y);
                bool success = ReadConsoleOutputCharacter(hConsole, sb, (uint)bufferWidth, coord, out charsRead);
                if (!success)
                {
                    int errorCode = Marshal.GetLastWin32Error();
                    Console.WriteLine($"Error reading console output: {errorCode}");
                    break;
                }
            }

            // 输出控制台缓冲区的内容  
            Console.WriteLine(sb.ToString());
            return sb.ToString();
        }
        else
        {
            int errorCode = Marshal.GetLastWin32Error();
            Console.WriteLine($"Error getting console screen buffer info: {errorCode}");
            return "";
        }
    }
}