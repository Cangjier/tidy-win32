using System.Drawing;
using TidyHPC.LiteJson;

namespace TidyWin32;
public partial class Win32
{
    public class TabControlInterface : WindowInterface
    {
        public static implicit operator TabControlInterface(Json target) => new TabControlInterface(target);

        public static implicit operator TabControlInterface(IntPtr target) => new TabControlInterface(target);

        public static implicit operator Json(TabControlInterface node) => node.Target;

        public TabControlInterface(Json target) : base(target)
        {
        }

        public TabControlInterface(IntPtr hWnd) : base(hWnd)
        {
        }

        public int Count
        {
            get
            {
                var result = (int)SendMessage(hWnd, TCM_GETITEMCOUNT, 0, IntPtr.Zero);
                Target.Set(nameof(Count), result);
                return result;
            }
        }

        public int Current
        {
            get
            {
                var result = (int)SendMessage(hWnd, TCM_GETCURSEL, 0, IntPtr.Zero);
                Target.Set(nameof(Current), result);
                return result;
            }
            set
            {
                SendMessage(hWnd, TCM_SETCURSEL, value, IntPtr.Zero);
            }
        }

        public void InitializeTabs()
        {
            using RemoteVirtual remoteVirtual = new(hWnd);
            RECT[] rectangles = new RECT[Count];
            for (int i = 0; i < Count; i++)
            {
                var rect = remoteVirtual.VirtualAllocEx<RECT>(new RECT());
                SendMessage(hWnd, TCM_GETITEMRECT, i, rect);
                rectangles[i] = remoteVirtual.Read<RECT>(rect);
            }
            string?[] tabTitles = new string?[Count];
            for (int i = 0; i < Count; i++)
            {
                var stringPtr = remoteVirtual.VirtualAllocString("", 256);
                var tCITEM = remoteVirtual.VirtualAllocEx<TCITEM>(new TCITEM()
                {
                    mask = 0x0001,
                    pszText = stringPtr,
                    cchTextMax = 256
                });
                SendMessage(hWnd, TCM_GETITEM, i, tCITEM);
                tabTitles[i] = remoteVirtual.ReadString(stringPtr);
            }
            var tabs = Target.GetOrCreateArray("Tabs");
            tabs.Clear();
            for(int i = 0; i < rectangles.Length; i++)
            {
                var rectangle = rectangles[i];

                var item = tabs.AddObject();

                item.Set("Text", tabTitles[i]);

                item.Set(nameof(rectangle.Left), rectangle.Left);
                item.Set(nameof(rectangle.Top), rectangle.Top);
                item.Set(nameof(rectangle.Right), rectangle.Right);
                item.Set(nameof(rectangle.Bottom), rectangle.Bottom);

                item.Set(nameof(rectangle.X), rectangle.X);
                item.Set(nameof(rectangle.Y), rectangle.Y);
                item.Set(nameof(rectangle.Width), rectangle.Width);
                item.Set(nameof(rectangle.Height), rectangle.Height);
            }
        }

        public void InitializeTabControlInfomation()
        {
            _ = Count;
            _ = Current;
            InitializeTabs();
        }
    }
}