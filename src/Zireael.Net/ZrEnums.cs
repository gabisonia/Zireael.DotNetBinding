using System;

namespace Zireael.Net;

/// <summary>
/// Policy for rendering ambiguous-width glyphs such as emoji.
/// </summary>
public enum ZrWidthPolicy : uint
{
    EmojiNarrow = 0,
    EmojiWide = 1
}

/// <summary>
/// Terminal color capability mode.
/// </summary>
public enum ZrPlatformColorMode : byte
{
    Unknown = 0,
    Color16 = 1,
    Color256 = 2,
    Rgb = 3
}

/// <summary>
/// Known terminal identifiers.
/// </summary>
public enum ZrTerminalId : int
{
    Unknown = 0,
    Kitty = 1,
    Ghostty = 2,
    WezTerm = 3,
    Foot = 4,
    ITerm2 = 5,
    Vte = 6,
    Konsole = 7,
    Contour = 8,
    WindowsTerminal = 9,
    Alacritty = 10,
    Xterm = 11,
    Mintty = 12,
    Tmux = 13,
    Screen = 14,
    Count = 15
}

/// <summary>
/// Terminal capability flags.
/// </summary>
[Flags]
public enum ZrTerminalCapFlags : uint
{
    None = 0,
    Sixel = 1u << 0,
    KittyGraphics = 1u << 1,
    ITerm2Images = 1u << 2,
    UnderlineStyles = 1u << 3,
    ColoredUnderlines = 1u << 4,
    Hyperlinks = 1u << 5,
    GraphemeClusters = 1u << 6,
    Overline = 1u << 7,
    PixelMouse = 1u << 8,
    KittyKeyboard = 1u << 9,
    Mouse = 1u << 10,
    BracketedPaste = 1u << 11,
    FocusEvents = 1u << 12,
    Osc52 = 1u << 13,
    SyncUpdate = 1u << 14,
    ScrollRegion = 1u << 15,
    CursorShape = 1u << 16,
    OutputWaitWritable = 1u << 17,

    AllMask = Sixel | KittyGraphics | ITerm2Images | UnderlineStyles | ColoredUnderlines | Hyperlinks |
              GraphemeClusters | Overline | PixelMouse | KittyKeyboard | Mouse | BracketedPaste |
              FocusEvents | Osc52 | SyncUpdate | ScrollRegion | CursorShape | OutputWaitWritable
}

/// <summary>
/// Bit flags for keyboard modifiers.
/// </summary>
[Flags]
public enum ZrModifiers : uint
{
    None = 0,
    Shift = 1u << 0,
    Ctrl = 1u << 1,
    Alt = 1u << 2,
    Meta = 1u << 3
}

/// <summary>
/// Event types emitted by the engine.
/// </summary>
public enum ZrEventType : uint
{
    Invalid = 0,
    Key = 1,
    Text = 2,
    Paste = 3,
    Mouse = 4,
    Resize = 5,
    Tick = 6,
    User = 7
}

/// <summary>
/// Logical key identifiers for keyboard events.
/// </summary>
public enum ZrKey : uint
{
    Unknown = 0,
    Escape = 1,
    Enter = 2,
    Tab = 3,
    Backspace = 4,
    Insert = 10,
    Delete = 11,
    Home = 12,
    End = 13,
    PageUp = 14,
    PageDown = 15,
    Up = 20,
    Down = 21,
    Left = 22,
    Right = 23,
    FocusIn = 30,
    FocusOut = 31,
    F1 = 100,
    F2 = 101,
    F3 = 102,
    F4 = 103,
    F5 = 104,
    F6 = 105,
    F7 = 106,
    F8 = 107,
    F9 = 108,
    F10 = 109,
    F11 = 110,
    F12 = 111
}

/// <summary>
/// Key transition state.
/// </summary>
public enum ZrKeyAction : uint
{
    Invalid = 0,
    Down = 1,
    Up = 2,
    Repeat = 3
}

/// <summary>
/// Mouse event kinds.
/// </summary>
public enum ZrMouseKind : uint
{
    Invalid = 0,
    Move = 1,
    Drag = 2,
    Down = 3,
    Up = 4,
    Wheel = 5
}

/// <summary>
/// Debug record categories.
/// </summary>
public enum ZrDebugCategory : uint
{
    None = 0,
    Frame = 1,
    Event = 2,
    Drawlist = 3,
    Error = 4,
    State = 5,
    Perf = 6
}

/// <summary>
/// Debug record severity levels.
/// </summary>
public enum ZrDebugSeverity : uint
{
    Trace = 0,
    Info = 1,
    Warn = 2,
    Error = 3
}

/// <summary>
/// Drawlist command opcodes.
/// </summary>
public enum ZrDlOpcode : ushort
{
    Invalid = 0,
    Clear = 1,
    FillRect = 2,
    DrawText = 3,
    PushClip = 4,
    PopClip = 5,
    DrawTextRun = 6,
    SetCursor = 7,
    DrawCanvas = 8,
    DrawImage = 9
}

/// <summary>
/// Canvas blitting strategy.
/// </summary>
public enum ZrBlitter : byte
{
    Auto = 0,
    Pixel = 1,
    Braille = 2,
    Sextant = 3,
    Quadrant = 4,
    Halfblock = 5,
    Ascii = 6
}

/// <summary>
/// Cursor shape options for drawlist commands.
/// </summary>
public enum ZrDlCursorShape : byte
{
    Block = 0,
    Underline = 1,
    Bar = 2
}

/// <summary>
/// Supported image source formats for draw-image commands.
/// </summary>
public enum ZrDlDrawImageFormat : byte
{
    Rgba = 0,
    Png = 1
}

/// <summary>
/// Image rendering protocols for draw-image commands.
/// </summary>
public enum ZrDlDrawImageProtocol : byte
{
    Auto = 0,
    Kitty = 1,
    Sixel = 2,
    ITerm2 = 3
}

/// <summary>
/// Z-layer for draw-image commands.
/// </summary>
public enum ZrDlDrawImageZLayer : sbyte
{
    Back = -1,
    Normal = 0,
    Front = 1
}

/// <summary>
/// Fit mode used when scaling images to destination bounds.
/// </summary>
public enum ZrDlDrawImageFitMode : byte
{
    Fill = 0,
    Contain = 1,
    Cover = 2
}