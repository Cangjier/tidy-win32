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
            foreach (var node in Nodes)
            {
                node.FullNodes();
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