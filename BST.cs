namespace TidyWin32;
public partial class Win32
{
    /// <summary>
    /// 复选框未选中状态
    /// </summary>
    public const int BST_UNCHECKED = 0x0000;

    /// <summary>
    /// 复选框选中状态
    /// </summary>
    public const int BST_CHECKED = 0x0001;

    /// <summary>
    /// 复选框三态模式下的不确定状态
    /// （仅适用于具有 BS_AUTO3STATE 或 BS_3STATE 样式的复选框）
    /// </summary>
    public const int BST_INDETERMINATE = 0x0002;

    /// <summary>
    /// 当复选框处于未选中状态时，表示该状态获得焦点
    /// （通常与键盘交互相关）
    /// </summary>
    public const int BST_FOCUS = 0x0008;

    /// <summary>
    /// 当复选框处于选中状态时，表示该状态获得焦点
    /// </summary>
    public const int BST_CHECKED_FOCUS = BST_CHECKED | BST_FOCUS;

    /// <summary>
    /// 当复选框处于不确定状态时，表示该状态获得焦点
    /// </summary>
    public const int BST_INDETERMINATE_FOCUS = BST_INDETERMINATE | BST_FOCUS;
}