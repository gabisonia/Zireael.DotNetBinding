using System.Runtime.InteropServices;

namespace Zireael.Net;

[StructLayout(LayoutKind.Sequential)]
public struct ZrEvBatchHeader
{
    public uint Magic;
    public uint Version;
    public uint TotalSize;
    public uint EventCount;
    public uint Flags;
    public uint Reserved0;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrEvRecordHeader
{
    public ZrEventType Type;
    public uint Size;
    public uint TimeMs;
    public uint Flags;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrEvKey
{
    public ZrKey Key;
    public ZrModifiers Mods;
    public ZrKeyAction Action;
    public uint Reserved0;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrEvText
{
    public uint Codepoint;
    public uint Reserved0;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrEvPaste
{
    public uint ByteLen;
    public uint Reserved0;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrEvMouse
{
    public int X;
    public int Y;
    public ZrMouseKind Kind;
    public ZrModifiers Mods;
    public uint Buttons;
    public int WheelX;
    public int WheelY;
    public uint Reserved0;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrEvResize
{
    public uint Cols;
    public uint Rows;
    public uint Reserved0;
    public uint Reserved1;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrEvTick
{
    public uint DtMs;
    public uint Reserved0;
    public uint Reserved1;
    public uint Reserved2;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrEvUser
{
    public uint Tag;
    public uint ByteLen;
    public uint Reserved0;
    public uint Reserved1;
}