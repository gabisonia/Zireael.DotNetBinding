using System;
using System.Collections.Generic;
using System.Text;

namespace Zireael.Net;

/// <summary>
/// High-level builder for serialized drawlist payloads.
/// </summary>
public sealed class ZrDrawlistBuilder
{
    private const uint HeaderSizeV1 = 64;
    private readonly List<byte[]> _commands = new();
    private readonly List<byte[]> _strings = new();

    /// <summary>
    /// Removes all queued commands and string data.
    /// </summary>
    public void Reset()
    {
        _commands.Clear();
        _strings.Clear();
    }

    /// <summary>
    /// Appends a clear command.
    /// </summary>
    /// <returns>The current builder instance.</returns>
    public ZrDrawlistBuilder Clear()
    {
        var bytes = new byte[8];
        WriteU16(bytes, 0, (ushort)ZrDlOpcode.Clear);
        WriteU16(bytes, 2, 0);
        WriteU32(bytes, 4, 8);
        _commands.Add(bytes);
        return this;
    }

    /// <summary>
    /// Appends a draw-text command using default white-on-black style.
    /// </summary>
    /// <param name="x">Destination X coordinate in cells.</param>
    /// <param name="y">Destination Y coordinate in cells.</param>
    /// <param name="text">UTF-8 text payload.</param>
    /// <returns>The current builder instance.</returns>
    public ZrDrawlistBuilder DrawText(int x, int y, string text)
    {
        var style = new ZrDlStyle
        {
            Fg = 0x00FF_FFFFu,
            Bg = 0x0000_0000u,
            Attrs = 0,
            Reserved0 = 0
        };

        return DrawText(x, y, text, in style);
    }

    /// <summary>
    /// Appends a draw-text command using an explicit style.
    /// </summary>
    /// <param name="x">Destination X coordinate in cells.</param>
    /// <param name="y">Destination Y coordinate in cells.</param>
    /// <param name="text">UTF-8 text payload.</param>
    /// <param name="style">Style fields for the draw-text command.</param>
    /// <returns>The current builder instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text" /> is <see langword="null" />.</exception>
    public ZrDrawlistBuilder DrawText(int x, int y, string text, in ZrDlStyle style)
    {
        if (text == null)
        {
            throw new ArgumentNullException(nameof(text));
        }

        var textBytes = Encoding.UTF8.GetBytes(text);
        var stringIndex = (uint)_strings.Count;
        _strings.Add(textBytes);

        var bytes = new byte[48];
        WriteU16(bytes, 0, (ushort)ZrDlOpcode.DrawText);
        WriteU16(bytes, 2, 0);
        WriteU32(bytes, 4, 48);
        WriteI32(bytes, 8, x);
        WriteI32(bytes, 12, y);
        WriteU32(bytes, 16, stringIndex);
        WriteU32(bytes, 20, 0); // byte offset inside the selected string
        WriteU32(bytes, 24, (uint)textBytes.Length);
        WriteU32(bytes, 28, style.Fg);
        WriteU32(bytes, 32, style.Bg);
        WriteU32(bytes, 36, style.Attrs);
        WriteU32(bytes, 40, style.Reserved0);
        WriteU32(bytes, 44, 0); // command reserved0
        _commands.Add(bytes);

        return this;
    }

    /// <summary>
    /// Builds the serialized drawlist byte payload.
    /// </summary>
    /// <param name="version">Requested drawlist version. Only v1 is supported by this builder.</param>
    /// <returns>Serialized drawlist bytes ready for <see cref="ZireaelEngine.SubmitDrawlist(byte[])" />.</returns>
    /// <exception cref="NotSupportedException">Thrown when <paramref name="version" /> is not supported.</exception>
    public byte[] Build(uint version = ZrVersion.DrawlistVersionV1)
    {
        if (version != ZrVersion.DrawlistVersionV1)
        {
            throw new NotSupportedException(
                $"ZrDrawlistBuilder currently supports only drawlist v1. Requested version: {version}.");
        }

        var cmdBytes = 0;
        for (var i = 0; i < _commands.Count; i++)
        {
            cmdBytes += _commands[i].Length;
        }

        var stringsSpanBytes = _strings.Count * 8;
        var stringsBytesRawLen = 0;
        for (var i = 0; i < _strings.Count; i++)
        {
            stringsBytesRawLen += _strings[i].Length;
        }

        var stringsBytesAlignedLen = Align4(stringsBytesRawLen);
        var cmdOffset = (int)HeaderSizeV1;
        var stringsSpanOffset = cmdOffset + cmdBytes;
        var stringsBytesOffset = stringsSpanOffset + stringsSpanBytes;
        var totalSize = stringsBytesOffset + stringsBytesAlignedLen;

        var output = new byte[totalSize];

        // Header
        WriteU32(output, 0, ZrDrawlistConstants.Magic);
        WriteU32(output, 4, version);
        WriteU32(output, 8, HeaderSizeV1);
        WriteU32(output, 12, (uint)totalSize);
        WriteU32(output, 16, (uint)cmdOffset);
        WriteU32(output, 20, (uint)cmdBytes);
        WriteU32(output, 24, (uint)_commands.Count);
        WriteU32(output, 28, (uint)stringsSpanOffset);
        WriteU32(output, 32, (uint)_strings.Count);
        WriteU32(output, 36, (uint)stringsBytesOffset);
        WriteU32(output, 40, (uint)stringsBytesAlignedLen);
        WriteU32(output, 44, 0); // blobs span offset
        WriteU32(output, 48, 0); // blobs count
        WriteU32(output, 52, 0); // blobs bytes offset
        WriteU32(output, 56, 0); // blobs bytes len
        WriteU32(output, 60, 0); // reserved0

        // Commands
        var cmdWriteOffset = cmdOffset;
        for (var i = 0; i < _commands.Count; i++)
        {
            var cmd = _commands[i];
            Buffer.BlockCopy(cmd, 0, output, cmdWriteOffset, cmd.Length);
            cmdWriteOffset += cmd.Length;
        }

        // String spans + string bytes
        var spanWriteOffset = stringsSpanOffset;
        var stringBytesWriteOffset = stringsBytesOffset;
        var stringRelativeOffset = 0;
        for (var i = 0; i < _strings.Count; i++)
        {
            var str = _strings[i];
            WriteU32(output, spanWriteOffset, (uint)stringRelativeOffset);
            WriteU32(output, spanWriteOffset + 4, (uint)str.Length);

            Buffer.BlockCopy(str, 0, output, stringBytesWriteOffset, str.Length);

            spanWriteOffset += 8;
            stringBytesWriteOffset += str.Length;
            stringRelativeOffset += str.Length;
        }

        return output;
    }

    private static int Align4(int value) => (value + 3) & ~3;

    private static void WriteU16(byte[] dest, int offset, ushort value)
    {
        dest[offset] = (byte)(value & 0xFF);
        dest[offset + 1] = (byte)(value >> 8);
    }

    private static void WriteI32(byte[] dest, int offset, int value) => WriteU32(dest, offset, unchecked((uint)value));

    private static void WriteU32(byte[] dest, int offset, uint value)
    {
        dest[offset] = (byte)(value & 0xFF);
        dest[offset + 1] = (byte)((value >> 8) & 0xFF);
        dest[offset + 2] = (byte)((value >> 16) & 0xFF);
        dest[offset + 3] = (byte)((value >> 24) & 0xFF);
    }
}
