namespace TidyWin32;
public partial class Win32
{
    /// <summary>Justifies the text to the top of the rectangle.</summary>
    public const int DT_TOP = 0x00000000;

    /// <summary>Aligns text to the left.</summary>
    public const int DT_LEFT = 0x00000000;

    /// <summary>Centers text horizontally in the rectangle.</summary>
    public const int DT_CENTER = 0x00000001;

    /// <summary>Aligns text to the right.</summary>
    public const int DT_RIGHT = 0x00000002;

    /// <summary>Centers text vertically. This value is used only with the DT_SINGLELINE value.</summary>
    public const int DT_VCENTER = 0x00000004;

    /// <summary>Justifies the text to the bottom of the rectangle. This value is used only with the DT_SINGLELINE value.</summary>
    public const int DT_BOTTOM = 0x00000008;

    /// <summary>
    /// Breaks words. Lines are automatically broken between words if a word extends past the edge of the rectangle specified by the lprc
    /// parameter. A carriage return-line feed sequence also breaks the line.
    /// </summary>
    public const int DT_WORDBREAK = 0x00000010;

    /// <summary>Displays text on a single line only. Carriage returns and line feeds do not break the line.</summary>
    public const int DT_SINGLELINE = 0x00000020;

    /// <summary>Expands tab characters. The default number of characters per tab is eight.</summary>
    public const int DT_EXPANDTABS = 0x00000040;

    /// <summary>
    /// Sets tab stops. The DRAWTEXTPARAMS structure pointed to by the lpDTParams parameter specifies the number of average character
    /// widths per tab stop.
    /// </summary>
    public const int DT_TABSTOP = 0x00000080;

    /// <summary>Draws without clipping. DrawTextEx is somewhat faster when DT_NOCLIP is used.</summary>
    public const int DT_NOCLIP = 0x00000100;

    /// <summary>
    /// Includes the font external leading in line height. Normally, external leading is not included in the height of a line of text.
    /// </summary>
    public const int DT_EXTERNALLEADING = 0x00000200;

    /// <summary>
    /// Determines the width and height of the rectangle. If there are multiple lines of text, DrawTextEx uses the width of the rectangle
    /// pointed to by the lprc parameter and extends the base of the rectangle to bound the last line of text. If there is only one line
    /// of text, DrawTextEx modifies the right side of the rectangle so that it bounds the last character in the line. In either case,
    /// DrawTextEx returns the height of the formatted text, but does not draw the text.
    /// </summary>
    public const int DT_CALCRECT = 0x00000400;

    /// <summary>
    /// Turns off processing of prefix characters. Normally, DrawTextEx interprets the ampersand (&amp;) mnemonic-prefix character as a
    /// directive to underscore the character that follows, and the double-ampersand (&amp;&amp;) mnemonic-prefix characters as a
    /// directive to print a single ampersand. By specifying DT_NOPREFIX, this processing is turned off. Compare with DT_HIDEPREFIX and DT_PREFIXONLY
    /// </summary>
    public const int DT_NOPREFIX = 0x00000800;

    /// <summary>Uses the system font to calculate text metrics.</summary>
    public const int DT_INTERNAL = 0x00001000;

    /// <summary>
    /// Duplicates the text-displaying characteristics of a multiline edit control. Specifically, the average character width is
    /// calculated in the same manner as for an edit control, and the function does not display a partially visible last line.
    /// </summary>
    public const int DT_EDITCONTROL = 0x00002000;

    /// <summary>
    /// For displayed text, replaces characters in the middle of the string with ellipses so that the result fits in the specified
    /// rectangle. If the string contains backslash (\) characters, DT_PATH_ELLIPSIS preserves as much as possible of the text after the
    /// last backslash. The string is not modified unless the DT_MODIFYSTRING flag is specified.
    /// </summary>
    public const int DT_PATH_ELLIPSIS = 0x00004000;

    /// <summary>
    /// For displayed text, replaces the end of a string with ellipses so that the result fits in the specified rectangle. Any word (not
    /// at the end of the string) that goes beyond the limits of the rectangle is truncated without ellipses. The string is not modified
    /// unless the DT_MODIFYSTRING flag is specified.
    /// </summary>
    public const int DT_END_ELLIPSIS = 0x00008000;

    /// <summary>
    /// Modifies the specified string to match the displayed text. This value has no effect unless DT_END_ELLIPSIS or DT_PATH_ELLIPSIS is specified.
    /// </summary>
    public const int DT_MODIFYSTRING = 0x00010000;

    /// <summary>
    /// Layout in right-to-left reading order for bidirectional text when the font selected into the hdc is a Hebrew or Arabic font. The
    /// default reading order for all text is left-to-right.
    /// </summary>
    public const int DT_RTLREADING = 0x00020000;

    /// <summary>Truncates any word that does not fit in the rectangle and adds ellipses.</summary>
    public const int DT_WORD_ELLIPSIS = 0x00040000;

    /// <summary>
    /// Prevents a line break at a DBCS (double-wide character string), so that the line-breaking rule is equivalent to SBCS strings. For
    /// example, this can be used in Korean windows, for more readability of icon labels. This value has no effect unless DT_WORDBREAK is specified.
    /// </summary>
    public const int DT_NOFULLWIDTHCHARBREAK = 0x00080000;

    /// <summary>
    /// Ignores the ampersand (&amp;) prefix character in the text. The letter that follows will not be underlined, but other
    /// mnemonic-prefix characters are still processed.
    /// </summary>
    public const int DT_HIDEPREFIX = 0x00100000;

    /// <summary>
    /// Draws only an underline at the position of the character following the ampersand (&amp;) prefix character. Does not draw any
    /// character in the string.
    /// </summary>
    public const int DT_PREFIXONLY = 0x00200000;
}
