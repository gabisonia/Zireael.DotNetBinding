using System.Runtime.InteropServices;
using Zireael.Net.Internal;

namespace Zireael.Net;

/// <summary>
/// Detected terminal profile information reported by the engine.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct ZrTerminalProfile
{
    public ZrTerminalId Id;
    public byte Pad0_0;
    public byte Pad0_1;
    public byte Pad0_2;

    public fixed byte VersionString[ZrTerminalConstants.VersionLength];

    public byte SupportsSixel;
    public byte SupportsKittyGraphics;
    public byte SupportsITerm2Images;
    public byte SupportsUnderlineStyles;
    public byte SupportsColoredUnderlines;
    public byte SupportsHyperlinks;
    public byte SupportsGraphemeClusters;
    public byte SupportsOverline;

    public byte SupportsPixelMouse;
    public byte SupportsKittyKeyboard;
    public byte SupportsMouse;
    public byte SupportsBracketedPaste;
    public byte SupportsFocusEvents;
    public byte SupportsOsc52;
    public byte SupportsSyncUpdate;
    public byte Pad1;

    public ushort CellWidthPx;
    public ushort CellHeightPx;
    public ushort ScreenWidthPx;
    public ushort ScreenHeightPx;

    public byte XtVersionResponded;
    public byte Da1Responded;
    public byte Da2Responded;
    public byte Pad2;

    /// <summary>
    /// Decodes the terminal version string from the fixed UTF-8 buffer.
    /// </summary>
    /// <returns>The terminal version string.</returns>
    public readonly string GetVersionString()
    {
        fixed (byte* ptr = VersionString)
        {
            return Utf8Interop.ReadNullTerminated(ptr, ZrTerminalConstants.VersionLength);
        }
    }
}

/// <summary>
/// Terminal capability flags and negotiated behavior switches.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrTerminalCaps
{
    public ZrPlatformColorMode ColorMode;
    public byte SupportsMouse;
    public byte SupportsBracketedPaste;
    public byte SupportsFocusEvents;
    public byte SupportsOsc52;
    public byte SupportsSyncUpdate;
    public byte SupportsScrollRegion;
    public byte SupportsCursorShape;
    public byte SupportsOutputWaitWritable;
    public byte SupportsUnderlineStyles;
    public byte SupportsColoredUnderlines;
    public byte SupportsHyperlinks;

    public uint SgrAttrsSupported;

    public ZrTerminalId TerminalId;
    public byte Pad1_0;
    public byte Pad1_1;
    public byte Pad1_2;

    public ZrTerminalCapFlags CapFlags;
    public ZrTerminalCapFlags CapForceFlags;
    public ZrTerminalCapFlags CapSuppressFlags;
}