using System;
using System.Collections.Generic;

namespace Zireael.Net;

/// <summary>
/// Safe reader for packed event-batch payloads returned by <see cref="ZireaelEngine.PollEvents(int, byte[])" />.
/// </summary>
public sealed class ZrEventBatchReader
{
    private const int BatchHeaderSize = 24;
    private const int RecordHeaderSize = 16;

    private readonly byte[] _buffer;
    private readonly int[] _recordOffsets;
    private readonly int[] _recordSizes;

    private ZrEventBatchReader(byte[] buffer, in ZrEvBatchHeader header, int[] recordOffsets, int[] recordSizes)
    {
        _buffer = buffer;
        Header = header;
        _recordOffsets = recordOffsets;
        _recordSizes = recordSizes;
    }

    /// <summary>
    /// Parsed event-batch header.
    /// </summary>
    public ZrEvBatchHeader Header { get; }

    /// <summary>
    /// Number of validated records in the batch.
    /// </summary>
    public int RecordCount => _recordOffsets.Length;

    /// <summary>
    /// Attempts to parse and validate a full event batch.
    /// </summary>
    /// <param name="batchBytes">Raw event-batch payload.</param>
    /// <param name="reader">Parsed reader instance when successful.</param>
    /// <returns><see langword="true" /> when the payload is a valid v1 event batch.</returns>
    public static bool TryCreate(byte[]? batchBytes, out ZrEventBatchReader? reader)
    {
        reader = null;
        if (!TryReadHeader(batchBytes, out var header))
        {
            return false;
        }

        var bytes = batchBytes!;
        var totalSize = checked((int)header.TotalSize);
        var expectedCount = checked((int)header.EventCount);
        var recordOffsets = new int[expectedCount];
        var recordSizes = new int[expectedCount];

        var offset = BatchHeaderSize;
        for (var i = 0; i < expectedCount; i++)
        {
            if (offset + RecordHeaderSize > totalSize)
            {
                return false;
            }

            var size = checked((int)ReadU32(bytes, offset + 4));
            if (size < RecordHeaderSize || (size & 0x3) != 0)
            {
                return false;
            }

            if (offset + size > totalSize)
            {
                return false;
            }

            recordOffsets[i] = offset;
            recordSizes[i] = size;
            offset += size;
        }

        if (offset != totalSize)
        {
            return false;
        }

        reader = new ZrEventBatchReader(bytes, in header, recordOffsets, recordSizes);
        return true;
    }

    /// <summary>
    /// Attempts to read and validate only the event-batch header.
    /// </summary>
    /// <param name="batchBytes">Raw event-batch payload.</param>
    /// <param name="header">Parsed header when successful.</param>
    /// <returns><see langword="true" /> when the payload starts with a valid v1 batch header.</returns>
    public static bool TryReadHeader(byte[]? batchBytes, out ZrEvBatchHeader header)
    {
        header = default;
        if (batchBytes == null || batchBytes.Length < BatchHeaderSize)
        {
            return false;
        }

        header = new ZrEvBatchHeader
        {
            Magic = ReadU32(batchBytes, 0),
            Version = ReadU32(batchBytes, 4),
            TotalSize = ReadU32(batchBytes, 8),
            EventCount = ReadU32(batchBytes, 12),
            Flags = ReadU32(batchBytes, 16),
            Reserved0 = ReadU32(batchBytes, 20)
        };

        if (header.Magic != ZrEventConstants.Magic)
        {
            return false;
        }

        if (header.Version != ZrVersion.EventBatchVersionV1)
        {
            return false;
        }

        if (header.TotalSize < BatchHeaderSize || header.TotalSize > batchBytes.Length)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Returns the record at the specified index.
    /// </summary>
    /// <param name="index">Zero-based record index.</param>
    /// <returns>A typed record view.</returns>
    public ZrEventRecord GetRecord(int index)
    {
        if (index < 0 || index >= _recordOffsets.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        var offset = _recordOffsets[index];
        var size = _recordSizes[index];
        var header = ReadRecordHeader(_buffer, offset);
        return new ZrEventRecord(_buffer, offset, size, in header);
    }

    /// <summary>
    /// Enumerates all validated event records in the batch.
    /// </summary>
    /// <returns>Sequence of typed record views.</returns>
    public IEnumerable<ZrEventRecord> EnumerateRecords()
    {
        for (var i = 0; i < _recordOffsets.Length; i++)
        {
            yield return GetRecord(i);
        }
    }

    private static ZrEvRecordHeader ReadRecordHeader(byte[] source, int offset)
    {
        return new ZrEvRecordHeader
        {
            Type = (ZrEventType)ReadU32(source, offset),
            Size = ReadU32(source, offset + 4),
            TimeMs = ReadU32(source, offset + 8),
            Flags = ReadU32(source, offset + 12)
        };
    }

    private static ushort ReadU16(byte[] source, int offset)
    {
        return (ushort)(source[offset] | (source[offset + 1] << 8));
    }

    private static uint ReadU32(byte[] source, int offset)
    {
        return (uint)(
            source[offset] |
            (source[offset + 1] << 8) |
            (source[offset + 2] << 16) |
            (source[offset + 3] << 24));
    }

    private static int ReadI32(byte[] source, int offset) => unchecked((int)ReadU32(source, offset));

    /// <summary>
    /// Typed view over a single event record within a validated batch.
    /// </summary>
    public readonly struct ZrEventRecord
    {
        private readonly byte[] _buffer;
        private readonly int _offset;
        private readonly int _size;

        internal ZrEventRecord(byte[] buffer, int offset, int size, in ZrEvRecordHeader header)
        {
            _buffer = buffer;
            _offset = offset;
            _size = size;
            Header = header;
        }

        /// <summary>
        /// Parsed record header.
        /// </summary>
        public ZrEvRecordHeader Header { get; }

        /// <summary>
        /// Event type.
        /// </summary>
        public ZrEventType Type => Header.Type;

        /// <summary>
        /// Total encoded record size in bytes (header + payload).
        /// </summary>
        public int Size => _size;

        /// <summary>
        /// Attempts to decode the record as a keyboard event payload.
        /// </summary>
        public bool TryGetKey(out ZrEvKey value)
        {
            value = default;
            if (Type != ZrEventType.Key || !TryEnsureFixedPayloadSize(16))
            {
                return false;
            }

            var payloadOffset = _offset + RecordHeaderSize;
            value = new ZrEvKey
            {
                Key = (ZrKey)ReadU32(_buffer, payloadOffset),
                Mods = (ZrModifiers)ReadU32(_buffer, payloadOffset + 4),
                Action = (ZrKeyAction)ReadU32(_buffer, payloadOffset + 8),
                Reserved0 = ReadU32(_buffer, payloadOffset + 12)
            };

            return true;
        }

        /// <summary>
        /// Attempts to decode the record as a text event payload.
        /// </summary>
        public bool TryGetText(out ZrEvText value)
        {
            value = default;
            if (Type != ZrEventType.Text || !TryEnsureFixedPayloadSize(8))
            {
                return false;
            }

            var payloadOffset = _offset + RecordHeaderSize;
            value = new ZrEvText
            {
                Codepoint = ReadU32(_buffer, payloadOffset),
                Reserved0 = ReadU32(_buffer, payloadOffset + 4)
            };

            return true;
        }

        /// <summary>
        /// Attempts to decode the record as a paste payload and expose its raw byte segment.
        /// </summary>
        public bool TryGetPaste(out ZrEvPaste value, out ArraySegment<byte> bytes)
        {
            value = default;
            bytes = default;
            if (Type != ZrEventType.Paste || !TryEnsureMinimumPayloadSize(8))
            {
                return false;
            }

            var payloadOffset = _offset + RecordHeaderSize;
            var byteLen = checked((int)ReadU32(_buffer, payloadOffset));
            value = new ZrEvPaste
            {
                ByteLen = (uint)byteLen,
                Reserved0 = ReadU32(_buffer, payloadOffset + 4)
            };

            var bytesOffset = payloadOffset + 8;
            if (bytesOffset + byteLen > _offset + _size)
            {
                return false;
            }

            bytes = new ArraySegment<byte>(_buffer, bytesOffset, byteLen);
            return true;
        }

        /// <summary>
        /// Attempts to decode the record as a mouse event payload.
        /// </summary>
        public bool TryGetMouse(out ZrEvMouse value)
        {
            value = default;
            if (Type != ZrEventType.Mouse || !TryEnsureFixedPayloadSize(32))
            {
                return false;
            }

            var payloadOffset = _offset + RecordHeaderSize;
            value = new ZrEvMouse
            {
                X = ReadI32(_buffer, payloadOffset),
                Y = ReadI32(_buffer, payloadOffset + 4),
                Kind = (ZrMouseKind)ReadU32(_buffer, payloadOffset + 8),
                Mods = (ZrModifiers)ReadU32(_buffer, payloadOffset + 12),
                Buttons = ReadU32(_buffer, payloadOffset + 16),
                WheelX = ReadI32(_buffer, payloadOffset + 20),
                WheelY = ReadI32(_buffer, payloadOffset + 24),
                Reserved0 = ReadU32(_buffer, payloadOffset + 28)
            };

            return true;
        }

        /// <summary>
        /// Attempts to decode the record as a resize event payload.
        /// </summary>
        public bool TryGetResize(out ZrEvResize value)
        {
            value = default;
            if (Type != ZrEventType.Resize || !TryEnsureFixedPayloadSize(16))
            {
                return false;
            }

            var payloadOffset = _offset + RecordHeaderSize;
            value = new ZrEvResize
            {
                Cols = ReadU32(_buffer, payloadOffset),
                Rows = ReadU32(_buffer, payloadOffset + 4),
                Reserved0 = ReadU32(_buffer, payloadOffset + 8),
                Reserved1 = ReadU32(_buffer, payloadOffset + 12)
            };

            return true;
        }

        /// <summary>
        /// Attempts to decode the record as a tick event payload.
        /// </summary>
        public bool TryGetTick(out ZrEvTick value)
        {
            value = default;
            if (Type != ZrEventType.Tick || !TryEnsureFixedPayloadSize(16))
            {
                return false;
            }

            var payloadOffset = _offset + RecordHeaderSize;
            value = new ZrEvTick
            {
                DtMs = ReadU32(_buffer, payloadOffset),
                Reserved0 = ReadU32(_buffer, payloadOffset + 4),
                Reserved1 = ReadU32(_buffer, payloadOffset + 8),
                Reserved2 = ReadU32(_buffer, payloadOffset + 12)
            };

            return true;
        }

        /// <summary>
        /// Attempts to decode the record as a user payload and expose its raw byte segment.
        /// </summary>
        public bool TryGetUser(out ZrEvUser value, out ArraySegment<byte> bytes)
        {
            value = default;
            bytes = default;
            if (Type != ZrEventType.User || !TryEnsureMinimumPayloadSize(16))
            {
                return false;
            }

            var payloadOffset = _offset + RecordHeaderSize;
            var byteLen = checked((int)ReadU32(_buffer, payloadOffset + 4));
            value = new ZrEvUser
            {
                Tag = ReadU32(_buffer, payloadOffset),
                ByteLen = (uint)byteLen,
                Reserved0 = ReadU32(_buffer, payloadOffset + 8),
                Reserved1 = ReadU32(_buffer, payloadOffset + 12)
            };

            var bytesOffset = payloadOffset + 16;
            if (bytesOffset + byteLen > _offset + _size)
            {
                return false;
            }

            bytes = new ArraySegment<byte>(_buffer, bytesOffset, byteLen);
            return true;
        }

        private bool TryEnsureFixedPayloadSize(int expectedPayloadSize)
        {
            return _size == RecordHeaderSize + expectedPayloadSize;
        }

        private bool TryEnsureMinimumPayloadSize(int minimumPayloadSize)
        {
            return _size >= RecordHeaderSize + minimumPayloadSize;
        }
    }
}
