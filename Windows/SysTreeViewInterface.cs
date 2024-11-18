using System.Runtime.InteropServices;
using System.Text;
using TidyHPC.LiteJson;
using static TidyWin32.Win32;

namespace TidyWin32;
public partial class Win32
{
    public class SysTreeViewNodeInterface : WindowInterface
    {
        public static implicit operator SysTreeViewNodeInterface(Json target) => new SysTreeViewNodeInterface(target);

        public static implicit operator Json(SysTreeViewNodeInterface node) => node.Target;

        public SysTreeViewNodeInterface(Json target): base(target)
        {
        }

        public SysTreeViewNodeInterface(IntPtr treeViewHWnd,IntPtr parent, IntPtr hItem):base(Json.NewObject())
        {
            TreeViewHWnd = treeViewHWnd;
            hWnd = hItem;
            Parent = parent;
        }

        public IntPtr TreeViewHWnd
        {
            get => GetIntPtr(nameof(TreeViewHWnd));
            set => SetIntPtr(nameof(TreeViewHWnd), value);
        }

        public IntPtr Parent
        {
            get=> GetIntPtr(nameof(Parent));
            set => SetIntPtr(nameof(Parent), value);
        }

        public SysTreeViewNodeInterface[] Nodes
        {
            get
            {
                List<SysTreeViewNodeInterface> result = [];
                IntPtr hItem = SendMessage(TreeViewHWnd, TVM_GETNEXTITEM, TVGN_CHILD, hWnd);
                while (hItem != IntPtr.Zero)
                {
                    result.Add(new(TreeViewHWnd, hWnd, hItem));
                    hItem = SendMessage(TreeViewHWnd, TVM_GETNEXTITEM, TVGN_NEXT, hItem);
                }
                var nodes = Target.GetOrCreateArray("Nodes");
                nodes.Clear();
                foreach (var node in result)
                {
                    nodes.Add(node);
                }
                return [.. result];
            }
        }

        public new string Text
        {
            get
            {
                StringBuilder buffer = new (256);
                SendMessage(hWnd, TVM_GETITEMTEXT, hWnd, buffer);
                Target.Set("Text", buffer.ToString());
                return buffer.ToString();
            }
            set
            {
                Target.Set("Text", value);
                SendMessage(hWnd, TVM_SETITEMTEXT, hWnd, value);
            }
        }

        public void FullNodes()
        {
            _ = Text;
            _ = Rectangle;
            InitializeNodeInfomation();
            foreach (var node in Nodes)
            {
                node.FullNodes();
            }
        }

        public RECT Rectangle
        {
            get
            {
                using RemoteVirtual remote = new(TreeViewHWnd);
                //var tvitem = remote.VirtualAllocEx<TVITEM>(new TVITEM()
                //{
                //    hItem = hWnd,
                //    mask = TVIF_HANDLE
                //});
                //SendMessage(TreeViewHWnd, TVM_GETITEM, 0, tvitem);
                var rect = remote.VirtualAllocEx<RECT>(new RECT()
                {
                    left = (int)hWnd
                });
                SendMessage(TreeViewHWnd, TVM_GETITEMRECT, 1, rect);
                var result = remote.Read<RECT>(rect);
                var reactangle = Target.GetOrCreateObject("Rectangle");
                reactangle.Set("Left", result.Left);
                reactangle.Set("X", result.Left);
                reactangle.Set("Top", result.Top);
                reactangle.Set("Y", result.Top);
                reactangle.Set("Right", result.Right);
                reactangle.Set("Bottom", result.Bottom);
                reactangle.Set("Width", result.Width);
                reactangle.Set("Height", result.Height);
                return result;
            }
        }

        public void InitializeNodeInfomation()
        {
            const int bufferSize = 256;

            // 在目标进程中分配缓冲区
            var threadId = GetWindowThreadProcessId(TreeViewHWnd,out uint lpdwProcessId);
            IntPtr processHandle = OpenProcess(ProcessAccessFlags.All, false, lpdwProcessId);
            IntPtr remoteBuffer = VirtualAllocEx(processHandle, IntPtr.Zero, bufferSize * sizeof(char),AllocationType.Commit, MemoryProtection.ReadWrite);

            // 创建并初始化 TVITEM 结构
            TVITEM item = new TVITEM
            {
                mask = TVIF_TEXT | TVIF_STATE | TVIF_IMAGE | TVIF_PARAM,
                hItem = hWnd,
                stateMask = TVIS_SELECTED,
                pszText = remoteBuffer,
                cchTextMax = bufferSize
            };

            // 在目标进程中分配 TVITEM 结构
            IntPtr remoteItem = VirtualAllocEx(processHandle, IntPtr.Zero, (uint)Marshal.SizeOf<TVITEM>(),AllocationType.Commit, MemoryProtection.ReadWrite);

            try
            {
                // 将 TVITEM 写入目标进程
                var success = WriteProcessMemory(processHandle, remoteItem, ref item, (uint)Marshal.SizeOf<TVITEM>(), out _);

                SendMessage(TreeViewHWnd, TVM_GETITEM, IntPtr.Zero, remoteItem);

                // 从目标进程读取 TVITEM 和 pszText
                TVITEM retrievedItem = new TVITEM();
                ReadProcessMemory(processHandle, remoteItem, ref retrievedItem, (uint)Marshal.SizeOf<TVITEM>(), out _);

                byte[] textBuffer = new byte[bufferSize * sizeof(char)]; 
                ReadProcessMemory(processHandle, remoteBuffer, textBuffer, (uint)textBuffer.Length, out _);
                string nodeText = Encoding.Unicode.GetString(textBuffer).TrimEnd('\0');

                // 保存信息到 JSON
                Target.Set("mask", retrievedItem.mask);
                Target.Set("state", retrievedItem.state);
                Target.Set("stateMask", retrievedItem.stateMask);
                Target.Set("hItem", retrievedItem.hItem);
                Target.Set("pszText", nodeText);
                Target.Set("cchTextMax", retrievedItem.cchTextMax);
                Target.Set("iImage", retrievedItem.iImage);
                Target.Set("iSelectedImage", retrievedItem.iSelectedImage);
                Target.Set("cChildren", retrievedItem.cChildren);
                Target.Set("lParam", retrievedItem.lParam);
            }
            finally
            {
                // 释放分配的远程内存
                VirtualFreeEx(processHandle, remoteBuffer, 0, FreeType.Release);
                VirtualFreeEx(processHandle, remoteItem, 0, FreeType.Release);

                // 关闭进程句柄
                CloseHandle(processHandle);
            }
        }

    }

    public class SysTreeViewInterface(Json target) : WindowInterface(target)
    {
        public static implicit operator SysTreeViewInterface(Json target) => new SysTreeViewInterface(target);

        public static implicit operator Json(SysTreeViewInterface treeView) => treeView.Target;

        public static implicit operator SysTreeViewInterface(IntPtr hWnd)
        {
            WindowInterface result = hWnd;
            return result.Target;
        }

        public SysTreeViewNodeInterface[] Nodes
        {
            get
            {
                List<SysTreeViewNodeInterface> result = [];
                IntPtr hItem = SendMessage(hWnd, TVM_GETNEXTITEM, TVGN_ROOT, IntPtr.Zero);
                while (hItem != IntPtr.Zero)
                {
                    result.Add(new(hWnd, IntPtr.Zero, hItem));
                    hItem = SendMessage(hWnd, TVM_GETNEXTITEM, TVGN_NEXT, hItem);
                }
                var nodes = Target.GetOrCreateArray("Nodes");
                nodes.Clear();
                foreach (var node in result)
                {
                    nodes.Add(node);
                }
                return [.. result];
            }
        }

        public void FullNodes()
        {
            foreach (var node in Nodes)
            {
                node.FullNodes();
            }
        }

        public int Count
        {
            get
            {
                var result = (int)SendMessage(hWnd, TVM_GETCOUNT, 0, 0);
                Target.Set("Count", result);
                return result;
            }
        }
    }
}