using System.Runtime.InteropServices;

namespace Zireael.Net;

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

[StructLayout(LayoutKind.Sequential)]
public struct ZrDlSpan
{
    public uint Off;
    public uint Len;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdHeader
{
    public ZrDlOpcode Opcode;
    public ushort Flags;
    public uint Size;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrDlStyle
{
    public uint Fg;
    public uint Bg;
    public uint Attrs;
    public uint Reserved0;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrDlStyleV3Ext
{
    public uint UnderlineRgb;
    public uint LinkUriRef;
    public uint LinkIdRef;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrDlStyleV3
{
    public ZrDlStyle Base;
    public ZrDlStyleV3Ext Ext;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdFillRect
{
    public int X;
    public int Y;
    public int W;
    public int H;
    public ZrDlStyle Style;
}

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

[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdFillRectV3
{
    public int X;
    public int Y;
    public int W;
    public int H;
    public ZrDlStyleV3 Style;
}

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

[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdPushClip
{
    public int X;
    public int Y;
    public int W;
    public int H;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrDlCmdDrawTextRun
{
    public int X;
    public int Y;
    public uint BlobIndex;
    public uint Reserved0;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrDlTextRunSegmentV3
{
    public ZrDlStyleV3 Style;
    public uint StringIndex;
    public uint ByteOff;
    public uint ByteLen;
}

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