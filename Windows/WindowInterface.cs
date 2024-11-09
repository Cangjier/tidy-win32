using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using TidyHPC.LiteJson;

namespace TidyWin32;

public partial class Win32
{
    public class WindowInterface(Json target)
    {
        public Json Target = target;

        public static implicit operator WindowInterface(Json target) => new WindowInterface(target);

        public static implicit operator Json(WindowInterface window) => window.Target;

        public static implicit operator WindowInterface(IntPtr hWnd) => New(hWnd);

        public WindowInterface Clone() => Target.Clone();

        public static WindowInterface New()
        {
            return Json.NewObject();
        }

        public static WindowInterface New(IntPtr hWnd)
        {
            WindowInterface result = Json.NewObject();
            result.hWnd = hWnd;
            return result;
        }

        protected IntPtr GetIntPtr(string key)
        {
            var hex = Target.Read(key, "0");
            return new IntPtr(Convert.ToInt64(hex, 16));
        }

        protected void SetIntPtr(string key, IntPtr value)
        {
            Target.Set(key, $"0x{value:X}");
        }

        public IntPtr hWnd
        {
            get => GetIntPtr(nameof(hWnd));
            set => SetIntPtr(nameof(hWnd), value);
        }

        public bool IsComboBox
        {
            get
            {
                if (hWnd == IntPtr.Zero)
                {
                    return false;
                }
                if (ClassName == "ComboBox")
                {
                    return true;
                }
                ComboBoxInterface comboBox = Target;
                if (comboBox.GetCount() != 0)
                {
                    return true;
                }
                return false;
            }
        }

        public static WindowInterface[] GetWindows()
        {
            List<WindowInterface> windows = new();
            EnumWindows((hWnd, lParam) =>
            {
                windows.Add(hWnd);
                return true;
            }, IntPtr.Zero);
            return [.. windows];
        }

        public static WindowInterface GetWindow(string className, string windowName)
        {
            IntPtr hWnd = FindWindow(className, windowName);
            if (hWnd == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            return hWnd;
        }

        public WindowInterface[] GetChildren()
        {
            List<WindowInterface> children = new();
            EnumChildWindows(hWnd, (hWnd, lParam) =>
            {
                children.Add(hWnd);
                return true;
            }, IntPtr.Zero);
            return [.. children];
        }

        public bool Enable
        {
            get
            {
                bool result = (GetWindowLong(hWnd, GWL_STYLE) & WS_DISABLED) == 0;
                Target.Set(nameof(Enable), result);
                return result;
            }
        }

        public bool IsUserWindow()
        {
            // 检查句柄是否为一个窗口  
            if (!IsWindow(hWnd))
                return false;
            var className = ClassName;
            string[] invalidClassNames = ["IME", "tooltips_class32"];
 
            if (invalidClassNames.Contains(className))
                return false;
            
            return true;
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
                string result;
                int length = (int)SendMessage(hWnd, WM_GETTEXTLENGTH, 0, 0);
                if (length == 0)
                {
                    result = string.Empty;
                }
                else
                {
                    StringBuilder sb = new(length + 1);
                    SendMessage(hWnd, WM_GETTEXT, length + 1, sb);
                    result = sb.ToString();
                }
                Target.Set(nameof(Text), result);
                return result;
            }
            set
            {
                SendMessage(hWnd, WM_SETTEXT, 0, value);
                Target.Set(nameof(Text), value);
            }
        }

        public string WindowText
        {
            get
            {
                StringBuilder sb = new StringBuilder(256);
                GetWindowText(hWnd, sb, sb.Capacity);
                var result = sb.ToString();
                Target.Set(nameof(WindowText), result);
                return result;
            }
            set
            {
                SetWindowText(hWnd, value);
                Target.Set(nameof(WindowText), value);
            }
        }

        public string ClassName
        {
            get
            {
                StringBuilder sb = new StringBuilder(256);
                GetClassName(hWnd, sb, sb.Capacity);
                var result = sb.ToString();
                Target.Set(nameof(ClassName), result);
                return result;
            }
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
                    Target.Set(nameof(Size), new Dictionary<string,object?>()
                    {
                        ["Width"] = width,
                        ["Height"] = height
                    });
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
                Target.Set(nameof(Size), new Dictionary<string, object?>()
                {
                    ["Width"] = value.Width,
                    ["Height"] = value.Height
                });
            }
        }

        public Point Location
        {
            get
            {
                RECT rect;
                if (GetWindowRect(hWnd, out rect))
                {
                    Target.Set(nameof(Location), new Dictionary<string, object?>()
                    {
                        ["X"] = rect.left,
                        ["Y"] = rect.top
                    });
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
                Target.Set(nameof(Location), new Dictionary<string, object?>()
                {
                    ["X"] = value.X,
                    ["Y"] = value.Y
                });
            }
        }

        public RECT ClientRect
        {
            get
            {
                Assert(GetClientRect(hWnd, out RECT rect));
                Target.Set(nameof(ClientRect), new Dictionary<string, object?>()
                {
                    ["Left"] = rect.left,
                    ["Top"] = rect.top,
                    ["Right"] = rect.right,
                    ["Bottom"] = rect.bottom
                });
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
                Target.Set(nameof(TopMost), value);
            }
        }

        public void WM_Click()
        {
            PostMessage(hWnd, WM_LBUTTONDOWN, IntPtr.Zero, IntPtr.Zero);
            PostMessage(hWnd, WM_LBUTTONUP, IntPtr.Zero, IntPtr.Zero);
        }

        public void WM_RightClick()
        {
            PostMessage(hWnd, WM_RBUTTONDOWN, IntPtr.Zero, IntPtr.Zero);
            PostMessage(hWnd, WM_RBUTTONUP, IntPtr.Zero, IntPtr.Zero);
        }

        public void BM_Click()
        {
            PostMessage(hWnd, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
        }

        public void WM_Click(int x, int y)
        {
            PostMessage(hWnd, WM_LBUTTONDOWN, IntPtr.Zero, new IntPtr((y << 16) | x));
            PostMessage(hWnd, WM_LBUTTONUP, IntPtr.Zero, new IntPtr((y << 16) | x));
        }

        public void MOUSEEVENTF_Click()
        {
            Point p = new();
            // 获取按钮的屏幕坐标  
            ClientToScreen(hWnd, ref p);
            // 模拟鼠标点击  
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)p.X, (uint)p.Y, 0, 0);
        }

        public static void MOUSEEVENTF_Click(int x,int y)
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);
        }

        public void SendInput_Click()
        {
            Point p = new(); 
            ClientToScreen(hWnd, ref p);

            var inputMouseDown = new INPUT
            {
                type = INPUT_MOUSE,
                U = new InputUnion
                {
                    mi = new MOUSEINPUT
                    {
                        dx = p.X, // 鼠标移动的X坐标（相对于当前位置）  
                        dy = p.Y, // 鼠标移动的Y坐标（相对于当前位置）  
                        mouseData = 0,
                        dwFlags = MOUSEEVENTF_LEFTDOWN|MOUSEEVENTF_ABSOLUTE,
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };

            var inputMouseUp = new INPUT
            {
                type = INPUT_MOUSE,
                U = new InputUnion
                {
                    mi = new MOUSEINPUT
                    {
                        dx = p.X,
                        dy = p.Y,
                        mouseData = 0,
                        dwFlags = MOUSEEVENTF_LEFTUP | MOUSEEVENTF_ABSOLUTE,
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };

            INPUT[] inputs = [inputMouseDown, inputMouseUp];
            Win32.SendInput((uint)inputs.Length, inputs, INPUT.Size);
        }

        public static void SendInput_Click(int x,int y)
        {
            var inputMouseDown = new INPUT
            {
                type = INPUT_MOUSE,
                U = new InputUnion
                {
                    mi = new MOUSEINPUT
                    {
                        dx = x, // 鼠标移动的X坐标（相对于当前位置）  
                        dy = y, // 鼠标移动的Y坐标（相对于当前位置）  
                        mouseData = 0,
                        dwFlags = MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_ABSOLUTE,
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };

            var inputMouseUp = new INPUT
            {
                type = INPUT_MOUSE,
                U = new InputUnion
                {
                    mi = new MOUSEINPUT
                    {
                        dx = x,
                        dy = y,
                        mouseData = 0,
                        dwFlags = MOUSEEVENTF_LEFTUP | MOUSEEVENTF_ABSOLUTE,
                        time = 0,
                        dwExtraInfo = IntPtr.Zero
                    }
                }
            };

            INPUT[] inputs = [inputMouseDown, inputMouseUp];
            Win32.SendInput((uint)inputs.Length, inputs, INPUT.Size);
        }

        public void SendInput(string value)
        {
            List<INPUT> inputs = [];
            foreach (char c in value)
            {
                var input = new INPUT
                {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = 0,
                            wScan = c,
                            dwFlags = KEYEVENTF_UNICODE,
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };
                inputs.Add(input);
            }
            Win32.SendInput((uint)inputs.Count, inputs.ToArray(), INPUT.Size);
        }

        public void SendKeys(string key)
        {
            foreach (char c in key)
            {
                PostMessage(hWnd, WM_CHAR, c, IntPtr.Zero);
            }
        }

        public static void SendText(string text)
        {
            var inputs = new INPUT[text.Length * 2]; // 每个字符一个按下和一个抬起  
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                ushort vk = VkKeyScan(c);

                inputs[i * 2] = new INPUT
                {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = vk,
                            wScan = 0,
                            dwFlags = 0,
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };

                inputs[i * 2 + 1] = new INPUT
                {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = vk,
                            wScan = 0,
                            dwFlags = KEYEVENTF_KEYUP,
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                };
            }

            Win32.SendInput((uint)inputs.Length, inputs, INPUT.Size);
        }

        public static void ActiveEnglishKeyboard()
        {
            IntPtr hkl = GetKeyboardLayout(0x0409); // 0x0409 是美式英文的语言代码  

            if (hkl != IntPtr.Zero)
            {
                // 激活英文输入法  
                ActivateKeyboardLayout(hkl, KLF_ACTIVATE);
            }
            else
            {
                Console.WriteLine("Failed to get English keyboard layout.");
            }
        }

#pragma warning disable CA1416 // 验证平台兼容性
        public Bitmap Capture()
        {
            // 判断是Windows系统
            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Rectangle rect;
                GetWindowRect(hWnd, out rect);
                Bitmap bmp = new(rect.Width, rect.Height);
                using Graphics gfxBmp = Graphics.FromImage(bmp);
                IntPtr hdcBitmap = gfxBmp.GetHdc();
                PrintWindow(hWnd, hdcBitmap, 0);
                gfxBmp.ReleaseHdc(hdcBitmap);
                return bmp;
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
#pragma warning restore CA1416 // 验证平台兼容性
        public void InitializeInfomation()
        {
            if (hWnd != IntPtr.Zero)
            {
                _ = ClassName;
                _ = WindowText;
                _ = Text;
                _ = Enable;
                _ = Size;
                _ = Location;
                _ = ClientRect;
                ComboBoxInterface comboBox = Target;
                if (comboBox.GetCount() != 0)
                {
                    _ = comboBox.Count;
                    _ = comboBox.SelectedIndex;
                    _ = comboBox.SelectedText;
                    _ = comboBox.Items;
                }
                if(ClassName== "SysTreeView32")
                {
                    SysTreeViewInterface treeView = Target;
                    treeView.FullNodes();
                    _ = treeView.Text;
                    _ = treeView.Count;
                }
            }
        }

        public override string ToString()
        {
            InitializeInfomation();
            return Target.ToString();
        }
    }
}
