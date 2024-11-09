using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace TidyWin32;

public partial class Win32
{
    public static ConcurrentDictionary<string, bool> RegisterBooleanMap { get; } = new();

    public static ConcurrentDictionary<IntPtr,Widget> WidgetMap { get; } = new();

    public static ConcurrentDictionary<IntPtr,Action> ActionMap { get; } = new();

    public static WndProc DefaultWndProc = (hWnd, msg, wParam, lParam) =>
    {
        if (WidgetMap.TryGetValue(hWnd, out var widget))
        {
            if (widget.WndProc(hWnd, msg, wParam, lParam))
            {
                return IntPtr.Zero;
            }
        }
        return DefWindowProc(hWnd, msg, wParam, lParam);
    };

    public static WNDCLASSEX RegisterClass(string className, WndProc onMessage)
    {
        var wc = new WNDCLASSEX
        {
            cbSize = 0,
            style = CS_VREDRAW| CS_HREDRAW,
            lpfnWndProc = onMessage,
            cbClsExtra = 0,
            cbWndExtra = 0,
            hInstance = GetModuleHandle(null),
            hIcon = LoadIcon(IntPtr.Zero, new(2)),
            hCursor = LoadCursor(IntPtr.Zero, IDC_ARROW),
            hbrBackground = GetStockObject(WHITE_BRUSH),
            lpszMenuName = "Menu",
            lpszClassName = className,
            hIconSm = IntPtr.Zero
        };
        wc.cbSize = Marshal.SizeOf(wc);
        Assert(RegisterClassEx(ref wc) != 0);
        return wc;
    }

    public static void TryRegisterClass(string className)
    {
        if(!RegisterBooleanMap.TryGetValue(className,out var isRegistered))
        {
            RegisterBooleanMap.TryAdd(className, true);
            RegisterClass(className, DefaultWndProc);
        }
    }

    public class Application
    {
        private static void MessageLoop()
        {
            MSG msg = new MSG();
            while (GetMessage(out msg, IntPtr.Zero, 0, 0) > 0)
            {
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        }

        public static void Start(Widget widget)
        {
            widget.Show();
            MessageLoop();
        }
    }

    public class Widget
    {
        public IntPtr hWnd { get; }

        public Widget()
        {
            var className = GetType().FullName ?? throw new Exception("GetType().FullName is null");
            TryRegisterClass(className);
            hWnd = CreateWindowEx(
                WS_EX_LEFT,
                className,
                "",
                WS_OVERLAPPEDWINDOW,
                CW_USEDEFAULT, CW_USEDEFAULT,
                400, 300,
                IntPtr.Zero,
                IntPtr.Zero,
                GetModuleHandle(null),
                IntPtr.Zero
            );
            Assert(hWnd != IntPtr.Zero);
            WidgetMap.TryAdd(hWnd, this);
        }

        public Widget(Widget parent)
        {
            var className = GetType().FullName ?? throw new Exception("GetType().FullName is null");
            TryRegisterClass(className);
            hWnd = CreateWindowEx(
                0,
                className,
                "",
                WS_CHILD | WS_VISIBLE,
                0, 0,
                300, 200,
                parent.hWnd,
                IntPtr.Zero,
                GetModuleHandle(null),
                IntPtr.Zero
            );
            Assert(hWnd != IntPtr.Zero);
            WidgetMap.TryAdd(hWnd, this);
        }

        public void Show()
        {
            ShowWindow(hWnd, SW_NORMAL);
            UpdateWindow(hWnd);
        }

        public void Close()
        {
            DestroyWindow(hWnd);
        }

        public void Maximize()
        {
            const int SW_MAXIMIZE = 3;
            ShowWindow(hWnd, SW_MAXIMIZE);
        }

        public void Minimize()
        {
            const int SW_MINIMIZE = 6;
            ShowWindow(hWnd, SW_MINIMIZE);
        }

        public string Text
        {
            get
            {
                StringBuilder sb = new StringBuilder(256);
                GetWindowText(hWnd, sb, sb.Capacity);
                return sb.ToString();
            }
            set
            {
                SetWindowText(hWnd, value);
            }
        }

        public virtual void OnLoad()
        {

        }

        public Action? OnLeftButtonDownProxy { get; set; } = null;

        public virtual void OnLeftButtonDown(Point point)
        {
            OnLeftButtonDownProxy?.Invoke();
        }

        public Action? OnLeftButtonUpProxy { get; set; } = null;

        public virtual void OnLeftButtonUp(Point point)
        {
            OnLeftButtonUpProxy?.Invoke();
        }

        public virtual void OnPaint()
        {

        }

        public virtual bool OnClose()
        {
            return true;
        }

        private static ConcurrentDictionary<IntPtr, Action> ActionMap { get; } = new();

        private static int InvokeIndex = 0;

        public void Invoke(Action action)
        {
            try
            {
                int index;
                lock (this)
                {
                    index = InvokeIndex;
                    InvokeIndex++;
                }
                IntPtr actionId = (IntPtr)index;
                ActionMap.TryAdd(actionId, action);
                SendMessage(hWnd, 0x0400 + 10086, actionId, IntPtr.Zero);
            }
            catch(Exception e)
            {
                Console.WriteLine($"Invoke Error: {e}");
            }
        }

        public bool WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (hWnd == this.hWnd)
            {
                if (msg == 0x0400 + 10086)
                {
                    if (ActionMap.TryGetValue(wParam, out var action))
                    {
                        action();
                        ActionMap.TryRemove(wParam, out _);
                    }
                    return true;
                }
                switch (msg)
                {
                    case WM_PAINT:
                        OnPaint();
                        break;

                    case WM_DESTROY:
                        PostQuitMessage(0);
                        break;
                    case WM_LBUTTONDOWN:
                        {
                            var x = lParam.ToInt32() & 0xFFFF;
                            var y = lParam.ToInt32() >> 16;
                            OnLeftButtonDown(new Point(x, y));
                            break;
                        }
                    case WM_LBUTTONUP:
                        {
                            var x = lParam.ToInt32() & 0xFFFF;
                            var y = lParam.ToInt32() >> 16;
                            OnLeftButtonUp(new Point(x, y));
                            break;
                        }
                    case WM_COMMAND:
                        break;
                    default:
                        return false;

                }
                return true;
                
            }
            return false;
        }

        public Size Size
        {
            get
            {
                RECT rect;
                if (GetWindowRect(hWnd, out rect))
                {
                    int width = rect.right - rect.left;
                    int height = rect.bottom - rect.top;
                    return new Size(width, height);
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            set
            {
                // 假设Location不变，只改变Size  
                Point location = Location;
                MoveWindow(hWnd, location.X, location.Y, value.Width, value.Height, true);
            }
        }

        public Point Location
        {
            get
            {
                RECT rect;
                if (GetWindowRect(hWnd, out rect))
                {
                    return new Point(rect.left, rect.top);
                }
                else
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            set
            {
                // 假设Size不变，只改变Location  
                Size size = Size;
                MoveWindow(hWnd, value.X, value.Y, size.Width, size.Height, true);
            }
        }

        public RECT ClientRect
        {
            get
            {
                Assert(GetClientRect(hWnd, out RECT rect));
                return rect;
            }
        }

        public bool TopMost
        {
            set
            {
                if (value)
                {
                    SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
                }
                else
                {
                    SetWindowPos(hWnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
                }
            }
        }

        public void PostPaintMessage()
        {
            PostMessage(hWnd, WM_PAINT, IntPtr.Zero, IntPtr.Zero);
        }

        public void RemoveBorder()
        {
            SetWindowLong(hWnd, GWL_STYLE, WS_POPUP);
        }

        public void CenterWindowOnScreen()
        {
            // 获取屏幕尺寸  
            RECT screenRect;
            GetWindowRect(GetDesktopWindow(), out screenRect);
            int screenWidth = screenRect.Right - screenRect.Left;
            int screenHeight = screenRect.Bottom - screenRect.Top;

            RECT windowRect;
            GetWindowRect(hWnd, out windowRect);

            // 假设你已经知道窗口的大小，或者你可以使用Form的Width和Height属性  
            int windowWidth = windowRect.Width;
            int windowHeight = windowRect.Height;

            // 计算窗口左上角的坐标以使其居中  
            int x = (screenWidth - windowWidth) / 2;
            int y = (screenHeight - windowHeight) / 2;

            // 使用Win32 API来移动窗口到计算出的位置  
            MoveWindow(hWnd, x, y, windowWidth, windowHeight, true);
        }
    }
}
