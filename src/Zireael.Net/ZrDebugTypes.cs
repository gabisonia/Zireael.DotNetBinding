using System.Runtime.InteropServices;
using Zireael.Net.Internal;

namespace Zireael.Net;

/// <summary>
/// Header metadata for a debug record.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDebugRecordHeader
{
    public ulong RecordId;
    public ulong TimestampUs;
    public ulong FrameId;
    public ZrDebugCategory Category;
    public ZrDebugSeverity Severity;
    public uint Code;
    public uint PayloadSize;
}

/// <summary>
/// Debug record containing per-frame draw and diff metrics.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDebugFrameRecord
{
    public ulong FrameId;
    public uint Cols;
    public uint Rows;
    public uint DrawlistBytes;
    public uint DrawlistCmds;
    public uint DiffBytesEmitted;
    public uint DirtyLines;
    public uint DirtyCells;
    public uint DamageRects;
    public uint UsDrawlist;
    public uint UsDiff;
    public uint UsWrite;
    public uint Pad0;
}

/// <summary>
/// Debug record containing captured event parsing details.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDebugEventRecord
{
    public ulong FrameId;
    public ZrEventType EventType;
    public uint EventFlags;
    public uint TimeMs;
    public uint RawBytesLen;
    public uint ParseResult;
    public uint Pad0;
}

/// <summary>
/// Debug record containing an engine error and contextual text.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public unsafe struct ZrDebugErrorRecord
{
    public ulong FrameId;
    public ZrResult ErrorCode;
    public uint SourceLine;
    public uint OccurrenceCount;
    public uint Pad0;

    public fixed byte SourceFile[ZrDebugConstants.SourceFileLength];
    public fixed byte Message[ZrDebugConstants.MessageLength];

    /// <summary>
    /// Decodes the source file path from the fixed UTF-8 buffer.
    /// </summary>
    /// <returns>The decoded source file path.</returns>
    public readonly string GetSourceFile()
    {
        fixed (byte* ptr = SourceFile)
        {
            return Utf8Interop.ReadNullTerminated(ptr, ZrDebugConstants.SourceFileLength);
        }
    }

    /// <summary>
    /// Decodes the debug message from the fixed UTF-8 buffer.
    /// </summary>
    /// <returns>The decoded debug message.</returns>
    public readonly string GetMessage()
    {
        fixed (byte* ptr = Message)
        {
            return Utf8Interop.ReadNullTerminated(ptr, ZrDebugConstants.MessageLength);
        }
    }
}

/// <summary>
/// Debug record containing drawlist validation and execution statistics.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDebugDrawlistRecord
{
    public ulong FrameId;
    public uint TotalBytes;
    public uint CmdCount;
    public uint Version;
    public uint ValidationResult;
    public uint ExecutionResult;
    public uint ClipStackMaxDepth;
    public uint TextRuns;
    public uint FillRects;
    public uint Pad0;
    public uint Pad1;
}

/// <summary>
/// Debug record containing phase timing data.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDebugPerfRecord
{
    public ulong FrameId;
    public uint Phase;
    public uint UsElapsed;
    public uint BytesProcessed;
    public uint Pad0;
}

/// <summary>
/// Configuration for engine debug capture.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDebugConfig
{
    public uint Enabled;
    public uint RingCapacity;
    public uint MinSeverity;
    public uint CategoryMask;
    public uint CaptureRawEvents;
    public uint CaptureDrawlistBytes;
    public uint Pad0;
    public uint Pad1;
}

/// <summary>
/// Query parameters for selecting debug records from the ring.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDebugQuery
{
    public ulong MinRecordId;
    public ulong MaxRecordId;
    public ulong MinFrameId;
    public ulong MaxFrameId;
    public uint CategoryMask;
    public uint MinSeverity;
    public uint MaxRecords;
    public uint Pad0;
}

/// <summary>
/// Metadata returned from a debug-record query.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDebugQueryResult
{
    public uint RecordsReturned;
    public uint RecordsAvailable;
    public ulong OldestRecordId;
    public ulong NewestRecordId;
    public uint RecordsDropped;
    public uint Pad0;
}

/// <summary>
/// Aggregate debug counters and ring utilization stats.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public struct ZrDebugStats
{
    public ulong TotalRecords;
    public ulong TotalDropped;
    public uint ErrorCount;
    public uint WarnCount;
    public uint CurrentRingUsage;
    public uint RingCapacity;
}