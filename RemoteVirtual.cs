using System.Runtime.InteropServices;
using static TidyWin32.Win32;

namespace TidyWin32;
public class RemoteVirtual : IDisposable
{
    private bool disposed = false;

    public RemoteVirtual(IntPtr hWnd)
    {
        ThreadId = GetWindowThreadProcessId(hWnd, out uint lpdwProcessId);
        ProcessId = lpdwProcessId;
        ProcessHandle = OpenProcess(ProcessAccessFlags.All, false, lpdwProcessId);
        if (ProcessHandle == IntPtr.Zero)
            throw new InvalidOperationException("Failed to open process.");
    }

    private List<IntPtr> VirtualAllocDisposables { get; } = new();

    public uint ThreadId { get; private set; }

    public uint ProcessId { get; private set; }

    public IntPtr ProcessHandle { get; private set; }

    public IntPtr VirtualAllocEx<T>(T value)
        where T : struct
    {
        var size = (uint)Marshal.SizeOf<T>();
        var result = Win32.VirtualAllocEx(ProcessHandle, IntPtr.Zero, size, AllocationType.Commit, MemoryProtection.ReadWrite);
        if (result == IntPtr.Zero)
            throw new InvalidOperationException("VirtualAllocEx failed.");

        var buffer = Marshal.AllocHGlobal((int)size);
        try
        {
            Marshal.StructureToPtr(value, buffer, false);
            if (!Win32.WriteProcessMemory(ProcessHandle, result, buffer, size, out _))
                throw new InvalidOperationException("WriteProcessMemory failed.");
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }

        lock (VirtualAllocDisposables)
        {
            VirtualAllocDisposables.Add(result);
        }

        return result;
    }

    public T Read<T>(IntPtr hWnd) where T : struct
    {
        var size = (uint)Marshal.SizeOf<T>();
        var buffer = Marshal.AllocHGlobal((int)size);
        try
        {
            if (!Win32.ReadProcessMemory(ProcessHandle, hWnd, buffer, size, out _))
                throw new InvalidOperationException("ReadProcessMemory failed.");
            return Marshal.PtrToStructure<T>(buffer);
        }
        finally
        {
            Marshal.FreeHGlobal(buffer);
        }
    }

    public void Dispose()
    {
        if (disposed) return;
        disposed = true;

        lock (VirtualAllocDisposables)
        {
            foreach (var item in VirtualAllocDisposables)
            {
                Win32.VirtualFreeEx(ProcessHandle, item, 0, FreeType.Release);
            }
            VirtualAllocDisposables.Clear();
        }

        if (ProcessHandle != IntPtr.Zero)
        {
            Win32.CloseHandle(ProcessHandle);
            ProcessHandle = IntPtr.Zero;
        }
    }
}

