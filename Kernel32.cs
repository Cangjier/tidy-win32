using System.Runtime.InteropServices;
using System.Text;

namespace TidyWin32;

public partial class Win32
{
    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr GetModuleHandle(string? lpModuleName);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr GetConsoleWindow();

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool ReadConsoleOutputCharacter(
       IntPtr hConsoleOutput,
       [Out] StringBuilder lpCharacter,
       uint nLength,
       COORD dwReadCoord,
       out uint lpNumberOfCharsRead);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern bool GetConsoleScreenBufferInfo(
        IntPtr hConsoleOutput,
        out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool AttachConsole(uint dwProcessId);

    // P/Invoke 方法声明
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, uint processId);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(IntPtr hObject);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr VirtualAllocEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        uint dwSize,
        AllocationType flAllocationType,
        MemoryProtection flProtect
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool VirtualFreeEx(
        IntPtr hProcess,
        IntPtr lpAddress,
        uint dwSize,
        FreeType dwFreeType
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        ref TVITEM lpBuffer,
        uint nSize,
        out IntPtr lpNumberOfBytesWritten
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        IntPtr lpBuffer,
        uint nSize,
        out IntPtr lpNumberOfBytesWritten
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool WriteProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        uint nSize,
        out IntPtr lpNumberOfBytesWritten
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        byte[] lpBuffer,
        uint nSize,
        out IntPtr lpNumberOfBytesRead
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        IntPtr lpBuffer,
        uint nSize,
        out IntPtr lpNumberOfBytesRead
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        ref TVITEM lpBuffer,
        uint nSize,
        out IntPtr lpNumberOfBytesRead
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool ReadProcessMemory(
        IntPtr hProcess,
        IntPtr lpBaseAddress,
        ref RECT lpBuffer,
        uint nSize,
        out IntPtr lpNumberOfBytesRead
    );
}