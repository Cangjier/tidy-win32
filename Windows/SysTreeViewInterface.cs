using System.Text;
using TidyHPC.LiteJson;

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
                // 获取树节点的矩形信息
                RECT rect = new RECT();

                // 将 wParam 设置为 TRUE（非零）以包含树节点的图标和标签区域
                IntPtr wParam = new IntPtr(1);

                // 发送消息获取节点的矩形
                IntPtr result = SendMessage(hWnd, TVM_GETITEMRECT, wParam, ref rect);

                // 返回矩形
                return rect;
            }
        }

        public bool Selected
        {
            get
            {
                TVITEM item = new TVITEM
                {
                    mask = TVIF_STATE,        // 请求状态信息
                    hItem = hWnd,   // 节点句柄
                    stateMask = TVIS_SELECTED // 检查选中状态
                };

                // 发送 TVM_GETITEM 消息
                IntPtr result = SendMessage(TreeViewHWnd, TVM_GETITEM, IntPtr.Zero, ref item);

                // 检查返回结果和选中状态
                var selected = result != IntPtr.Zero && (item.state & TVIS_SELECTED) != 0;
                Target.Set("Selected", selected);
                return selected;
            }

            set
            {
                // 设置节点选中状态
                TVITEM item = new TVITEM
                {
                    mask = TVIF_STATE,        // 请求状态信息
                    hItem = hWnd,   // 节点句柄
                    stateMask = TVIS_SELECTED, // 设置选中状态
                    state = (uint)(value ? TVIS_SELECTED : 0) // 设置选中状态
                };

                // 发送 TVM_SETITEM 消息
                IntPtr result = SendMessage(TreeViewHWnd, TVM_SETITEM, IntPtr.Zero, ref item);
            }
        }

        public void InitializeNodeInfomation()
        {
            TVITEM item = new TVITEM
            {
                mask = TVIF_STATE,        // 请求状态信息
                hItem = hWnd,   // 节点句柄
                stateMask = TVIS_SELECTED // 检查选中状态
            };

            // 发送 TVM_GETITEM 消息
            IntPtr result = SendMessage(TreeViewHWnd, TVM_GETITEM, IntPtr.Zero, ref item);
            Target.Set("mask", item.mask);
            Target.Set("state", item.state);
            Target.Set("stateMask", item.stateMask);
            Target.Set("hItem", item.hItem);
            Target.Set("pszText", item.pszText);
            Target.Set("cchTextMax", item.cchTextMax);
            Target.Set("iImage", item.iImage);
            Target.Set("iSelectedImage", item.iSelectedImage);
            Target.Set("cChildren", item.cChildren);
            Target.Set("lParam", item.lParam);

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