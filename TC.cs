namespace TidyWin32;

public partial class Win32
{
    public const int TCM_GETITEMCOUNT = 0x1304; // 获取选项卡数量
    public const int TCM_GETITEM = 0x1305;      // 获取选项卡内容
    public const int TCM_GETCURSEL = 0x130B;    // 获取当前选中的选项卡
    public const int TCM_SETCURSEL = 0x130C;    // 设置当前选中的选项卡
    public const int TCM_GETITEMRECT = 0x130A;

    public const int TCIF_TEXT = 0x0001;        // 标志：文本内容
    public const int TCIF_IMAGE = 0x0002;       // 标志：图像
    public const int TCIF_PARAM = 0x0008;       // 标志：参数



}
