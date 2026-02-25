using System.Runtime.InteropServices;

namespace Zireael.Net;

[StructLayout(LayoutKind.Sequential)]
public struct ZrPlatformSize
{
    public uint Cols;
    public uint Rows;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrPlatformCaps
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
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrPlatformConfig
{
    public ZrPlatformColorMode RequestedColorMode;
    public byte EnableMouse;
    public byte EnableBracketedPaste;
    public byte EnableFocusEvents;
    public byte EnableOsc52;
    public byte Pad0;
    public byte Pad1;
    public byte Pad2;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrLimits
{
    public uint ArenaMaxTotalBytes;
    public uint ArenaInitialBytes;
    public uint OutMaxBytesPerFrame;
    public uint DlMaxTotalBytes;
    public uint DlMaxCmds;
    public uint DlMaxStrings;
    public uint DlMaxBlobs;
    public uint DlMaxClipDepth;
    public uint DlMaxTextRunSegments;
    public uint DiffMaxDamageRects;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrEngineConfig
{
    public uint RequestedEngineAbiMajor;
    public uint RequestedEngineAbiMinor;
    public uint RequestedEngineAbiPatch;

    public uint RequestedDrawlistVersion;
    public uint RequestedEventBatchVersion;

    public ZrLimits Limits;
    public ZrPlatformConfig Platform;

    public uint TabWidth;
    public ZrWidthPolicy WidthPolicy;
    public uint TargetFps;

    public byte EnableScrollOptimizations;
    public byte EnableDebugOverlay;
    public byte EnableReplayRecording;
    public byte WaitForOutputDrain;

    public ZrTerminalCapFlags CapForceFlags;
    public ZrTerminalCapFlags CapSuppressFlags;
}

[StructLayout(LayoutKind.Sequential)]
public struct ZrEngineRuntimeConfig
{
    public ZrLimits Limits;
    public ZrPlatformConfig Platform;

    public uint TabWidth;
    public ZrWidthPolicy WidthPolicy;
    public uint TargetFps;

    public byte EnableScrollOptimizations;
    public byte EnableDebugOverlay;
    public byte EnableReplayRecording;
    public byte WaitForOutputDrain;

    public ZrTerminalCapFlags CapForceFlags;
    public ZrTerminalCapFlags CapSuppressFlags;
}