using System.Runtime.InteropServices;

namespace Zireael.Net;

/// <summary>
/// Header for a serialized drawlist payload.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlHeader
{
    public uint Magic;
    public uint Version;
    public uint HeaderSize;
    public uint TotalSize;
    public uint CmdOffset;
    public uint CmdBytes;
    public uint CmdCount;
    public uint StringsSpanOffset;
    public uint StringsCount;
    public uint StringsBytesOffset;
    public uint StringsBytesLen;
    public uint BlobsSpanOffset;
    public uint BlobsCount;
    public uint BlobsBytesOffset;
    public uint BlobsBytesLen;
    public uint Reserved0;
}

/// <summary>
/// Byte span descriptor for strings or blob segments in a drawlist.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlSpan
{
    public uint Off;
    public uint Len;
}

/// <summary>
/// Common header for a drawlist command.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdHeader
{
    public ZrDlOpcode Opcode;
    public ushort Flags;
    public uint Size;
}

/// <summary>
/// Base style information for text and fill commands.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlStyle
{
    public uint Fg;
    public uint Bg;
    public uint Attrs;
    public uint Reserved0;
}

/// <summary>
/// Extended style data introduced in drawlist v3.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlStyleV3Ext
{
    public uint UnderlineRgb;
    public uint LinkUriRef;
    public uint LinkIdRef;
}

/// <summary>
/// Composite style structure for drawlist v3 commands.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlStyleV3
{
    public ZrDlStyle Base;
    public ZrDlStyleV3Ext Ext;
}

/// <summary>
/// Fill-rectangle command payload.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdFillRect
{
    public int X;
    public int Y;
    public int W;
    public int H;
    public ZrDlStyle Style;
}

/// <summary>
/// Draw-text command payload.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdDrawText
{
    public int X;
    public int Y;
    public uint StringIndex;
    public uint ByteOff;
    public uint ByteLen;
    public ZrDlStyle Style;
    public uint Reserved0;
}

/// <summary>
/// Fill-rectangle command payload for drawlist v3 style layout.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdFillRectV3
{
    public int X;
    public int Y;
    public int W;
    public int H;
    public ZrDlStyleV3 Style;
}

/// <summary>
/// Draw-text command payload for drawlist v3 style layout.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdDrawTextV3
{
    public int X;
    public int Y;
    public uint StringIndex;
    public uint ByteOff;
    public uint ByteLen;
    public ZrDlStyleV3 Style;
    public uint Reserved0;
}

/// <summary>
/// Push-clip command payload.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdPushClip
{
    public int X;
    public int Y;
    public int W;
    public int H;
}

/// <summary>
/// Draw-text-run command payload.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdDrawTextRun
{
    public int X;
    public int Y;
    public uint BlobIndex;
    public uint Reserved0;
}

/// <summary>
/// Segment descriptor for a v3 text-run blob.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlTextRunSegmentV3
{
    public ZrDlStyleV3 Style;
    public uint StringIndex;
    public uint ByteOff;
    public uint ByteLen;
}

/// <summary>
/// Set-cursor command payload.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdSetCursor
{
    public int X;
    public int Y;
    public ZrDlCursorShape Shape;
    public byte Visible;
    public byte Blink;
    public byte Reserved0;
}

/// <summary>
/// Draw-canvas command payload.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdDrawCanvas
{
    public ushort DstCol;
    public ushort DstRow;
    public ushort DstCols;
    public ushort DstRows;
    public ushort PxWidth;
    public ushort PxHeight;
    public uint BlobOffset;
    public uint BlobLen;
    public ZrBlitter Blitter;
    public byte Flags;
    public ushort Reserved;
}

/// <summary>
/// Draw-image command payload.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdDrawImage
{
    public ushort DstCol;
    public ushort DstRow;
    public ushort DstCols;
    public ushort DstRows;
    public ushort PxWidth;
    public ushort PxHeight;
    public uint BlobOffset;
    public uint BlobLen;
    public uint ImageId;
    public ZrDlDrawImageFormat Format;
    public ZrDlDrawImageProtocol Protocol;
    public ZrDlDrawImageZLayer ZLayer;
    public ZrDlDrawImageFitMode FitMode;
    public byte Flags;
    public byte Reserved0;
    public ushort Reserved1;
}