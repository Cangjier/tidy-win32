namespace TidyWin32;


public partial class Win32
{
    public const int TV_FIRST = 0x1100;
    public const int TVM_INSERTITEMA = TV_FIRST + 0;
    public const int TVM_INSERTITEMW = TV_FIRST + 50;
    public const int TVM_DELETEITEM = TV_FIRST + 1;
    public const int TVM_EXPAND = TV_FIRST + 2;
    public const int TVM_GETITEMRECT = TV_FIRST + 4;
    public const int TVM_GETCOUNT = TV_FIRST + 5;
    public const int TVM_GETINDENT = TV_FIRST + 6;
    public const int TVM_SETINDENT = TV_FIRST + 7;
    public const int TVM_GETIMAGELIST = TV_FIRST + 8;
    public const int TVM_SETIMAGELIST = TV_FIRST + 9;
    public const int TVM_GETNEXTITEM = TV_FIRST + 10;
    public const int TVM_SELECTITEM = TV_FIRST + 11;
    public const int TVM_GETITEMA = TV_FIRST + 12;
    public const int TVM_GETITEMW = TV_FIRST + 62;
    public const int TVM_SETITEMA = TV_FIRST + 13;
    public const int TVM_SETITEMW = TV_FIRST + 63;
    public const int TVM_EDITLABELA = TV_FIRST + 14;
    public const int TVM_EDITLABELW = TV_FIRST + 65;
    public const int TVM_GETEDITCONTROL = TV_FIRST + 15;
    public const int TVM_GETVISIBLECOUNT = TV_FIRST + 16;
    public const int TVM_HITTEST = TV_FIRST + 17;
    public const int TVM_CREATEDRAGIMAGE = TV_FIRST + 18;
    public const int TVM_SORTCHILDREN = TV_FIRST + 19;
    public const int TVM_ENSUREVISIBLE = TV_FIRST + 20;
    public const int TVM_SORTCHILDRENCB = TV_FIRST + 21;
    public const int TVM_ENDEDITLABELNOW = TV_FIRST + 22;
    public const int TVM_GETISEARCHSTRINGA = TV_FIRST + 23;
    public const int TVM_GETISEARCHSTRINGW = TV_FIRST + 64;
    public const int TVM_SETTOOLTIPS = TV_FIRST + 24;
    public const int TVM_GETTOOLTIPS = TV_FIRST + 25;
    public const int TVM_SETINSERTMARK = TV_FIRST + 26;
    public const int TVM_SETITEMHEIGHT = TV_FIRST + 27;
    public const int TVM_GETITEMHEIGHT = TV_FIRST + 28;
    public const int TVM_SETBKCOLOR = TV_FIRST + 29;
    public const int TVM_SETTEXTCOLOR = TV_FIRST + 30;
    public const int TVM_GETBKCOLOR = TV_FIRST + 31;
    public const int TVM_GETTEXTCOLOR = TV_FIRST + 32;
    public const int TVM_SETSCROLLTIME = TV_FIRST + 33;
    public const int TVM_GETSCROLLTIME = TV_FIRST + 34;
    public const int TVM_SETINSERTMARKCOLOR = TV_FIRST + 37;
    public const int TVM_GETINSERTMARKCOLOR = TV_FIRST + 38;
    public const int TVM_GETITEMSTATE = TV_FIRST + 39;
    public const int TVM_SETLINECOLOR = TV_FIRST + 40;
    public const int TVM_GETLINECOLOR = TV_FIRST + 41;
    public const int TVM_MAPACCIDTOHTREEITEM = TV_FIRST + 42;
    public const int TVM_MAPHTREEITEMTOACCID = TV_FIRST + 43;
    public const int TVM_SETEXTENDEDSTYLE = TV_FIRST + 44;
    public const int TVM_GETEXTENDEDSTYLE = TV_FIRST + 45;
    public const int TVM_SETAUTOSCROLLINFO = TV_FIRST + 59;
    public const int TVM_GETSELECTEDCOUNT = TV_FIRST + 70;
    public const int TVM_SHOWINFOTIP = TV_FIRST + 71;
    public const int TVM_GETITEMPARTRECT = TV_FIRST + 72;

    public const int TVM_GETITEMTEXT = 0x1110;
    public const int TVM_SETITEMTEXT = 0x1111;
    public const int TVM_GETITEM = 0x110C;
    public const int TVM_SETITEM = 0x110D;

    public const int TVGN_ROOT = 0x0000;
    public const int TVGN_NEXT = 0x0001;
    public const int TVGN_PREVIOUS = 0x0002;
    public const int TVGN_PARENT = 0x0003;
    public const int TVGN_CHILD = 0x0004;
    public const int TVGN_FIRSTVISIBLE = 0x0005;
    public const int TVGN_NEXTVISIBLE = 0x0006;
    public const int TVGN_PREVIOUSVISIBLE = 0x0007;
    public const int TVGN_DROPHILITE = 0x0008;
    public const int TVGN_CARET = 0x0009;
    public const int TVGN_LASTVISIBLE = 0x000A;

    public const int TVIF_TEXT = 0x0001;
    public const int TVIF_IMAGE = 0x0002;
    public const int TVIF_PARAM = 0x0004;
    public const int TVIF_STATE = 0x0008;
    public const int TVIF_HANDLE = 0x0010;

    public const int TVIS_SELECTED = 0x0002;
    public const int TVIS_EXPANDED = 0x0020;
    public const int TVIS_EXPANDPARTIAL = 0x0080;
    public const int TVIS_OVERLAYMASK = 0x0F00;
    public const int TVIS_CUT = 0x0010;
    public const int TVIS_BOLD = 0x0040;
    public const int TVIS_EXPANDEDONCE = 0x0200;
}