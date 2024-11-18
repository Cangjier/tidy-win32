using TidyHPC.LiteJson;

namespace TidyWin32;

public partial class Win32
{
    public class CheckBoxInterface(Json target) : WindowInterface(target)
    {
        public static implicit operator CheckBoxInterface(Json target) => new CheckBoxInterface(target);

        public static implicit operator Json(CheckBoxInterface comboBox) => comboBox.Target;

        public static implicit operator CheckBoxInterface(IntPtr hWnd) => New(hWnd);

        public new static CheckBoxInterface New()
        {
            return Json.NewObject();
        }

        public new static CheckBoxInterface New(IntPtr hWnd)
        {
            CheckBoxInterface result = Json.NewObject();
            result.hWnd = hWnd;
            return result;
        }

        public bool Checked
        {
            get
            {
                var result = SendMessage(hWnd, BM_GETCHECK, 0, 0) == BST_CHECKED;
                Target.Set(nameof(Checked), result);
                return result;
            }
            set
            {
                SendMessage(hWnd, BM_SETCHECK, value ? BST_CHECKED : BST_UNCHECKED, 0);
            }
        }
    }
}