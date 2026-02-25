using System.Runtime.InteropServices;

namespace Zireael.Net;

[StructLayout(LayoutKind.Sequential)]
public struct ZrMetrics
{
    public uint StructSize;

    public uint NegotiatedEngineAbiMajor;
    public uint NegotiatedEngineAbiMinor;
    public uint NegotiatedEngineAbiPatch;

    public uint NegotiatedDrawlistVersion;
    public uint NegotiatedEventBatchVersion;

    public ulong FrameIndex;
    public uint Fps;
    public uint Pad0;

    public ulong BytesEmittedTotal;
    public uint BytesEmittedLastFrame;
    public uint Pad1;

    public uint DirtyLinesLastFrame;
    public uint DirtyColsLastFrame;

    public uint UsInputLastFrame;
    public uint UsDrawlistLastFrame;
    public uint UsDiffLastFrame;
    public uint UsWriteLastFrame;

    public uint EventsOutLastPoll;
    public uint EventsDroppedTotal;

    public ulong ArenaFrameHighWaterBytes;
    public ulong ArenaPersistentHighWaterBytes;

    public uint DamageRectsLastFrame;
    public uint DamageCellsLastFrame;
    public byte DamageFullFrame;
    public byte Pad2_0;
    public byte Pad2_1;
    public byte Pad2_2;
}