using System.Text;
using TidyHPC.LiteJson;

namespace TidyWin32;

public partial class Win32
{
    public class ComboBoxInterface(Json target) :WindowInterface(target)
    {
        public static implicit operator ComboBoxInterface(Json target) => new ComboBoxInterface(target);

        public static implicit operator Json(ComboBoxInterface comboBox) => comboBox.Target;

        public static implicit operator ComboBoxInterface(IntPtr hWnd) => New(hWnd);

        public new static ComboBoxInterface New()
        {
            return Json.NewObject();
        }

        public new static ComboBoxInterface New(IntPtr hWnd)
        {
            ComboBoxInterface result = Json.NewObject();
            result.hWnd = hWnd;
            return result;
        }

        public int Count
        {
            get
            {
                var result = GetCount();
                Target.Set(nameof(Count), result);
                return result;
            }
        }

        public int SelectedIndex
        {
            get
            {
                var result = (int)SendMessage(hWnd, CB_GETCURSEL, 0, 0);
                Target.Set(nameof(SelectedIndex), result);
                return result;
            }
            set
            {
                SendMessage(hWnd, CB_SETCURSEL, value, 0);
                Target.Set(nameof(SelectedIndex), value);
            }
        }

        public string SelectedText
        {
            get
            {
                string result;
                int index = SelectedIndex;
                if (index == -1)
                {
                    result = string.Empty;
                }
                else
                {
                    result = GetText(index);
                }
                Target.Set(nameof(SelectedText), result);
                return result;
            }
        }

        public string[] Items
        {
            get
            {
                string[] result=new string[Count];
                for (int i = 0; i < Count; i++)
                {
                    result[i] = GetText(i);
                }
                Target.Set(nameof(Items), result);
                return result;
            }
        }

        public int GetCount() => (int)SendMessage(hWnd, CB_GETCOUNT, 0, 0);

        public string GetText(int index)
        {
            int length = (int)SendMessage(hWnd, CB_GETLBTEXTLEN, index, 0);
            StringBuilder sb = new(length + 1);
            SendMessage(hWnd, CB_GETLBTEXT, index, sb);
            return sb.ToString();
        }

        public void Add(string text)
        {
            SendMessage(hWnd, CB_ADDSTRING, 0, text);
        }
    }
}