﻿using System.Runtime.InteropServices;

namespace TidyWin32;
public partial class Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TVITEM
    {
        public uint mask;          // 指定需要的信息类型
        public IntPtr hItem;       // 节点句柄
        public uint state;         // 节点的状态
        public uint stateMask;     // 指定需要的状态
        [MarshalAs(UnmanagedType.LPTStr)]
        public string pszText;     // 节点文本（未使用）
        public int cchTextMax;     // 文本缓冲区大小（未使用）
        public int iImage;         // 图标索引（未使用）
        public int iSelectedImage; // 选中图标索引（未使用）
        public int cChildren;      // 子节点数（未使用）
        public IntPtr lParam;      // 应用程序数据（未使用）
    }
}